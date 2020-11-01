

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net.Security;
using System.Runtime.ExceptionServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Common.Concurrency;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;

namespace ES.Communication.Touchless.Common
{
    public sealed class TlsHandshakeCompletionEvent
    {
        public static readonly TlsHandshakeCompletionEvent Success = new TlsHandshakeCompletionEvent();

        readonly Exception exception;

        /// <summary>
        ///  Creates a new event that indicates a successful handshake.
        /// </summary>
        TlsHandshakeCompletionEvent()
        {
            this.exception = null;
        }

        /// <summary>
        ///  Creates a new event that indicates an unsuccessful handshake.
        ///  Use {@link #SUCCESS} to indicate a successful handshake.
        /// </summary>
        public TlsHandshakeCompletionEvent(Exception exception)
        {
            Contract.Requires(exception != null);

            this.exception = exception;
        }

        /// <summary>
        ///  Return {@code true} if the handshake was successful
        /// </summary>
        public bool IsSuccessful
        {
            get { return this.exception == null; }
        }

        /// <summary>
        ///  Return the {@link Throwable} if {@link #isSuccess()} returns {@code false}
        ///  and so the handshake failed.
        /// </summary>
        public Exception Exception
        {
            get { return this.exception; }
        }
    }

    public sealed class CustomTlsHandler : ByteToMessageDecoder
    {
        const int ReadBufferSize = 4 * 1024; // todo: research perfect size

        static readonly Exception ChannelClosedException = new IOException("Channel is closed");
        static readonly Action<Task, object> AuthenticationCompletionCallback = new Action<Task, object>(HandleAuthenticationCompleted);
        static readonly AsyncCallback SslStreamReadCallback = new AsyncCallback(HandleSslStreamRead);

        readonly SslStream sslStream;
        State state;
        readonly MediationStream mediationStream;
        IByteBuffer sslStreamReadBuffer;
        volatile IChannelHandlerContext capturedContext;
        PendingWriteQueue pendingUnencryptedWrites;
        Task lastContextWriteTask;
        TaskCompletionSource closeFuture;
        readonly bool isServer;
        readonly X509Certificate2 certificate;
        readonly string targetHost;

        CustomTlsHandler(bool isServer, X509Certificate2 certificate, string targetHost, RemoteCertificateValidationCallback certificateValidationCallback)
        {
            Contract.Requires(!isServer || certificate != null);
            Contract.Requires(isServer || !string.IsNullOrEmpty(targetHost));

            this.closeFuture = new TaskCompletionSource();

            this.isServer = isServer;
            this.certificate = certificate;
            this.targetHost = targetHost;
            this.mediationStream = new MediationStream(this);
            this.sslStream = new SslStream(this.mediationStream, true, certificateValidationCallback);
        }

        public static CustomTlsHandler Client(string targetHost)
        {
            return new CustomTlsHandler(false, null, targetHost, null);
        }

        public static CustomTlsHandler Client(string targetHost, X509Certificate2 certificate)
        {
            return new CustomTlsHandler(false, certificate, targetHost, null);
        }

        public static CustomTlsHandler Client(string targetHost, X509Certificate2 certificate, RemoteCertificateValidationCallback certificateValidationCallback)
        {
            return new CustomTlsHandler(false, certificate, targetHost, certificateValidationCallback);
        }

        public static CustomTlsHandler Server(X509Certificate2 certificate)
        {
            return new CustomTlsHandler(true, certificate, null, null);
        }

        public X509Certificate LocalCertificate
        {
            get { return this.sslStream.LocalCertificate; }
        }

        public X509Certificate RemoteCertificate
        {
            get { return this.sslStream.RemoteCertificate; }
        }

        public void Dispose()
        {
            if (this.sslStream != null)
            {
                this.sslStream.Dispose();
            }
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            base.ChannelActive(context);

            if (!this.isServer)
            {
                this.EnsureAuthenticated();
            }
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            // Make sure to release SslStream,
            // and notify the handshake future if the connection has been closed during handshake.
            this.HandleFailure(ChannelClosedException);

            base.ChannelInactive(context);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            if (IgnoreException(exception))
            {
                // Close the connection explicitly just in case the transport
                // did not close the connection automatically.
                if (context.Channel.Active)
                {
                    context.CloseAsync();
                }
            }
            else
            {
                base.ExceptionCaught(context, exception);
            }
        }

        bool IgnoreException(Exception t)
        {
            if (t is ObjectDisposedException && this.closeFuture.Task.IsCompleted)
            {
                return true;
            }
            return false;
        }

        static void HandleAuthenticationCompleted(Task task, object state)
        {
            var self = (CustomTlsHandler)state;
            switch (task.Status)
            {
                case TaskStatus.RanToCompletion:
                    {
                        State oldState = self.state;
                        if ((oldState & State.AuthenticationCompleted) == 0)
                        {
                            self.state = (oldState | State.Authenticated) & ~(State.Authenticating | State.FlushedBeforeHandshake);

                            self.capturedContext.FireUserEventTriggered(TlsHandshakeCompletionEvent.Success);

                            if ((oldState & State.ReadRequestedBeforeAuthenticated) == State.ReadRequestedBeforeAuthenticated
                                && !self.capturedContext.Channel.Configuration.AutoRead)
                            {
                                self.capturedContext.Read();
                            }

                            if ((oldState & State.FlushedBeforeHandshake) != 0)
                            {
                                self.Wrap(self.capturedContext);
                                self.capturedContext.Flush();
                            }
                        }
                        break;
                    }
                case TaskStatus.Canceled:
                case TaskStatus.Faulted:
                    {
                        // ReSharper disable once AssignNullToNotNullAttribute -- task.Exception will be present as task is faulted
                        State oldState = self.state;
                        if ((oldState & State.AuthenticationCompleted) == 0)
                        {
                            self.state = (oldState | State.FailedAuthentication) & ~State.Authenticating;
                        }
                        self.HandleFailure(task.Exception);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException("task", "Unexpected task status: " + task.Status);
            }
        }

        public override void HandlerAdded(IChannelHandlerContext context)
        {
            base.HandlerAdded(context);
            this.capturedContext = context;
            this.pendingUnencryptedWrites = new PendingWriteQueue(context);
            if (context.Channel.Active && !this.isServer)
            {
                // todo: support delayed initialization on an existing/active channel if in client mode
                this.EnsureAuthenticated();
            }
        }

        protected override void HandlerRemovedInternal(IChannelHandlerContext context)
        {
            if (!this.pendingUnencryptedWrites.IsEmpty)
            {
                // Check if queue is not empty first because create a new ChannelException is expensive
                this.pendingUnencryptedWrites.RemoveAndFailAll(new ChannelException("Write has failed due to TlsHandler being removed from channel pipeline."));
            }
        }

        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            // pass bytes to SslStream through input -> trigger HandleSslStreamRead. After this call sslStreamReadBuffer may or may not have bytes to read
            this.mediationStream.AcceptBytes(input);

            if (!this.EnsureAuthenticated())
            {
                return;
            }

            IByteBuffer readBuffer = this.sslStreamReadBuffer;
            if (readBuffer == null)
            {
                this.sslStreamReadBuffer = readBuffer = context.Channel.Allocator.Buffer(ReadBufferSize);
                this.ScheduleSslStreamRead();
            }

            if (readBuffer.IsReadable())
            {
                // SslStream parsed at least one full frame and completed read request
                // Pass the buffer to a next handler in pipeline
                output.Add(readBuffer);
                this.sslStreamReadBuffer = null;
            }
        }

        public override void Read(IChannelHandlerContext context)
        {
            State oldState = this.state;
            if ((oldState & State.AuthenticationCompleted) == 0)
            {
                this.state = oldState | State.ReadRequestedBeforeAuthenticated;
            }

            context.Read();
        }

        bool EnsureAuthenticated()
        {
            State oldState = this.state;
            if ((oldState & State.AuthenticationStarted) == 0)
            {
                this.state = oldState | State.Authenticating;
                if (this.isServer)
                {
                    //try
                    //{
                    //    this.sslStream.AuthenticateAsServerAsync(this.certificate) // todo: change to begin/end
                    //    .ContinueWith(AuthenticationCompletionCallback, this, TaskContinuationOptions.ExecuteSynchronously);

                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    this.sslStream.AuthenticateAsServerAsync(this.certificate) // todo: change to begin/end
                        .ContinueWith(AuthenticationCompletionCallback, this, TaskContinuationOptions.ExecuteSynchronously);
                }
                else
                {
                    var certificateCollection = new X509Certificate2Collection();
                    if (this.certificate != null)
                    {
                        certificateCollection.Add(this.certificate);
                    }
                    this.sslStream.AuthenticateAsClientAsync(this.targetHost, certificateCollection, SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12, false) // todo: change to begin/end
                        .ContinueWith(AuthenticationCompletionCallback, this, TaskContinuationOptions.ExecuteSynchronously);
                }
                return false;
            }
            return (oldState & State.Authenticated) == State.Authenticated;
        }

        void ScheduleSslStreamRead()
        {
            try
            {
                IByteBuffer buf = this.sslStreamReadBuffer;
                this.sslStream.BeginRead(buf.Array, buf.ArrayOffset + buf.WriterIndex, buf.WritableBytes, SslStreamReadCallback, this);
            }
            catch (Exception ex)
            {
                this.HandleFailure(ex);
                throw;
            }
        }

        static void HandleSslStreamRead(IAsyncResult ar)
        {
            var self = (CustomTlsHandler)ar.AsyncState;
            int length = self.sslStream.EndRead(ar);
            self.sslStreamReadBuffer.SetWriterIndex(self.sslStreamReadBuffer.ReaderIndex + length); // adjust byte buffer's writer index to reflect read progress
        }

        public override Task WriteAsync(IChannelHandlerContext context, object message)
        {
            if (!(message is IByteBuffer))
            {
                return TaskEx.FromException(new UnsupportedMessageTypeException(message, typeof(IByteBuffer)));
            }
            return this.pendingUnencryptedWrites.Add(message);
        }

        public override void Flush(IChannelHandlerContext context)
        {
            if (this.pendingUnencryptedWrites.IsEmpty)
            {
                this.pendingUnencryptedWrites.Add(Unpooled.Empty);
            }

            if (!this.EnsureAuthenticated())
            {
                this.state |= State.FlushedBeforeHandshake;
                return;
            }

            this.Wrap(context);
            context.Flush();
        }

        void Wrap(IChannelHandlerContext context)
        {
            Contract.Assert(context == this.capturedContext);

            while (true)
            {
                object msg = this.pendingUnencryptedWrites.Current;
                if (msg == null)
                {
                    break;
                }

                var buf = (IByteBuffer)msg;
                buf.ReadBytes(this.sslStream, buf.ReadableBytes); // this leads to FinishWrap being called 0+ times

                TaskCompletionSource promise = this.pendingUnencryptedWrites.Remove();
                Task task = this.lastContextWriteTask;
                if (task != null)
                {
                    task.LinkOutcome(promise);
                    this.lastContextWriteTask = null;
                }
                else
                {
                    promise.TryComplete();
                }
            }
        }

        void FinishWrap(byte[] buffer, int offset, int count)
        {
            IByteBuffer output = this.capturedContext.Allocator.Buffer(count);
            output.WriteBytes(buffer, offset, count);

            if (!output.IsReadable())
            {
                output.Release();
                output = Unpooled.Empty;
            }

            this.lastContextWriteTask = this.capturedContext.WriteAsync(output);
        }

        public override Task CloseAsync(IChannelHandlerContext context)
        {
            this.sslStream.Close();
            this.closeFuture.TryComplete();
            return base.CloseAsync(context);
        }

        void HandleFailure(Exception cause)
        {
            // Release all resources such as internal buffers that SSLEngine
            // is managing.

            try
            {
                this.sslStream.Close();
            }
            catch (Exception ex)
            {
                // todo: evaluate following:
                // only log in Debug mode as it most likely harmless and latest chrome still trigger
                // this all the time.
                //
                // See https://github.com/netty/netty/issues/1340
                //string msg = ex.Message;
                //if (msg == null || !msg.contains("possible truncation attack"))
                //{
                //    logger.Debug("{} SSLEngine.closeInbound() raised an exception.", ctx.channel(), e);
                //}
            }
            this.NotifyHandshakeFailure(cause);
            this.pendingUnencryptedWrites.RemoveAndFailAll(cause);
        }

        void NotifyHandshakeFailure(Exception cause)
        {
            if ((this.state & State.AuthenticationCompleted) == 0)
            {
                // handshake was not completed yet => TlsHandler react to failure by closing the channel
                this.state = (this.state | State.FailedAuthentication) & ~State.Authenticating;
                this.capturedContext.FireUserEventTriggered(new TlsHandshakeCompletionEvent(cause));
                this.capturedContext.CloseAsync();
            }
        }

        [Flags]
        enum State
        {
            Authenticating = 1,
            Authenticated = 1 << 1,
            FailedAuthentication = 1 << 2,
            ReadRequestedBeforeAuthenticated = 1 << 3,
            FlushedBeforeHandshake = 1 << 4,
            AuthenticationStarted = Authenticating | Authenticated | FailedAuthentication,
            AuthenticationCompleted = Authenticated | FailedAuthentication
        }

        sealed class MediationStream : Stream
        {
            static readonly Action<Task, object> WriteCompleteCallback = HandleChannelWriteComplete;

            readonly CustomTlsHandler owner;
            IByteBuffer pendingReadBuffer;
            readonly SynchronousAsyncResult<int> syncReadResult;
            TaskCompletionSource<int> readCompletionSource;
            AsyncCallback readCallback;
            ArraySegment<byte> sslOwnedBuffer;
            TaskCompletionSource writeCompletion;
            AsyncCallback writeCallback;

            public MediationStream(CustomTlsHandler owner)
            {
                //Program.Trace.WriteLine($"[MediationStream] Ctor");
                this.syncReadResult = new SynchronousAsyncResult<int>();
                this.owner = owner;
            }

            public void AcceptBytes(IByteBuffer input)
            {
                TaskCompletionSource<int> tcs = this.readCompletionSource;
                if (tcs == null)
                {
                    // there is no pending read operation - keep for future
                    this.pendingReadBuffer = input;
                    return;
                }

                ArraySegment<byte> sslBuffer = this.sslOwnedBuffer;

                Contract.Assert(sslBuffer.Array != null);

                int readableBytes = input.ReadableBytes;
                //Program.Trace.WriteLine($"[MediationStream] AcceptBytes - Input Readable bytes {readableBytes}");
                int length = Math.Min(sslBuffer.Count, readableBytes);
                //Program.Trace.WriteLine($"[MediationStream] AcceptBytes - SslBuffer {length}");
                input.ReadBytes(sslBuffer.Array, sslBuffer.Offset, length);
                tcs.TrySetResult(length);
                if (length < readableBytes)
                {
                    //Program.Trace.WriteLine($"[MediationStream] AcceptBytes - length < readableBytes == true");
                    // set buffer for consecutive read to use
                    this.pendingReadBuffer = input;
                }

                AsyncCallback callback = this.readCallback;
                if (callback != null)
                {
                    //Program.Trace.WriteLine($"[MediationStream] AcceptBytes - before callback()");
                    callback(tcs.Task);
                    //Program.Trace.WriteLine($"[MediationStream] AcceptBytes - after callback()");
                }
            }

            public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
            {
                //Program.Trace.WriteLine($"[MediationStream] BeginRead ");
                IByteBuffer pendingBuf = this.pendingReadBuffer;
                if (pendingBuf != null)
                {
                    // we have the bytes available upfront - write out synchronously
                    int readableBytes = pendingBuf.ReadableBytes;
                    //Program.Trace.WriteLine($"[MediationStream] BeginRead - ReadableBytes {readableBytes}");
                    int length = Math.Min(count, readableBytes);
                    //Program.Trace.WriteLine($"[MediationStream] BeginRead - length {length}");
                    pendingBuf.ReadBytes(buffer, offset, length);
                    if (length == readableBytes)
                    {
                        //Program.Trace.WriteLine($"[MediationStream] BeginRead - length == readableBytes");
                        // buffer has been read out to the end
                        this.pendingReadBuffer = null;
                    }

                    //Program.Trace.WriteLine($"[MediationStream] BeginRead - return PrepareSyncReadResult");
                    //return this.PrepareSyncReadResult(length, state);
                    var syncResult = this.PrepareSyncReadResult(length, state);
                    if (callback != null)
                    {
                        callback(syncResult);
                    }
                    return syncResult;
                }


                //Program.Trace.WriteLine($"[MediationStream] BeginRead - return Task");
                // take note of buffer - we will pass bytes in here
                this.sslOwnedBuffer = new ArraySegment<byte>(buffer, offset, count);
                this.readCompletionSource = new TaskCompletionSource<int>(state);
                this.readCallback = callback;
                return this.readCompletionSource.Task;
            }

            public override int EndRead(IAsyncResult asyncResult)
            {
                SynchronousAsyncResult<int> syncResult = this.syncReadResult;
                if (ReferenceEquals(asyncResult, syncResult))
                {
                    //Program.Trace.WriteLine($"[MediationStream] EndRead - return syncResult.Result");
                    return syncResult.Result;
                }

                Contract.Assert(!((Task<int>)asyncResult).IsCanceled);

                try
                {
                    return ((Task<int>)asyncResult).Result;
                }
                catch (AggregateException ex)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                    throw; // unreachable
                }
                finally
                {
                    this.readCompletionSource = null;
                    this.readCallback = null;
                    this.sslOwnedBuffer = default(ArraySegment<byte>);
                }
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                this.owner.FinishWrap(buffer, offset, count);
            }

            public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
            {
                Task task = this.owner.capturedContext.WriteAndFlushAsync(Unpooled.WrappedBuffer(buffer, offset, count));
                switch (task.Status)
                {
                    case TaskStatus.RanToCompletion:
                        // write+flush completed synchronously (and successfully)
                        var result = new SynchronousAsyncResult<int>();
                        result.AsyncState = state;
                        callback(result);
                        return result;
                    default:
                        this.writeCallback = callback;
                        var tcs = new TaskCompletionSource(state);
                        this.writeCompletion = tcs;
                        task.ContinueWith(WriteCompleteCallback, this, TaskContinuationOptions.ExecuteSynchronously);
                        return tcs.Task;
                }
            }

            static void HandleChannelWriteComplete(Task writeTask, object state)
            {
                //Program.Trace.WriteLine($"[MediationStream] HandleChannelWriteComplete - status {writeTask.Status}");
                var self = (MediationStream)state;
                switch (writeTask.Status)
                {
                    case TaskStatus.RanToCompletion:
                        self.writeCompletion.TryComplete();
                        break;
                    case TaskStatus.Canceled:
                        self.writeCompletion.TrySetCanceled();
                        break;
                    case TaskStatus.Faulted:
                        self.writeCompletion.TrySetException(writeTask.Exception);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Unexpected task status: " + writeTask.Status);
                }

                if (self.writeCallback != null)
                {
                    //Program.Trace.WriteLine($"[MediationStream] HandleChannelWriteComplete - self.writeCallback != null");
                    self.writeCallback(self.writeCompletion.Task);
                }
            }

            public override void EndWrite(IAsyncResult asyncResult)
            {
                if (this.writeCallback == null) { return; }

                this.writeCallback = null;
                this.writeCompletion = null;

                if (asyncResult is SynchronousAsyncResult<int>)
                {
                    return;
                }

                try
                {
                    ((Task<int>)asyncResult).Wait();
                }
                catch (AggregateException ex)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                    throw;
                }
            }

            IAsyncResult PrepareSyncReadResult(int readBytes, object state)
            {
                //Program.Trace.WriteLine($"[MediationStream] PrepareSyncReadResult - readBytes {readBytes}");
                // it is safe to reuse sync result object as it can't lead to leak (no way to attach to it via handle)
                SynchronousAsyncResult<int> result = this.syncReadResult;
                result.Result = readBytes;
                result.AsyncState = state;
                return result;
            }

            public override void Flush()
            {
                // NOOP: called on SslStream.Close
            }

            #region plumbing

            public override long Seek(long offset, SeekOrigin origin)
            {
                //Program.Trace.WriteLine($"[MediationStream] Seek");
                throw new NotSupportedException();
            }

            public override void SetLength(long value)
            {
                //Program.Trace.WriteLine($"[MediationStream] SetLength");
                throw new NotSupportedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                //Program.Trace.WriteLine($"[MediationStream] Read ");
                IByteBuffer pendingBuf = this.pendingReadBuffer;
                if (pendingBuf != null)
                {
                    //Program.Trace.WriteLine($"[MediationStream] Read pendingBuf != null");
                    // we have the bytes available upfront - write out synchronously
                    int readableBytes = pendingBuf.ReadableBytes;
                    //Program.Trace.WriteLine($"[MediationStream] Read - readable bytes {readableBytes}");
                    int length = Math.Min(count, readableBytes);
                    //Program.Trace.WriteLine($"[MediationStream] Read - {length}");
                    pendingBuf.ReadBytes(buffer, offset, length);
                    if (length == readableBytes)
                    {
                        // buffer has been read out to the end
                        this.pendingReadBuffer = null;
                    }
                    return length;
                }
                else
                {
                    throw new InvalidOperationException("No readily available bytes to complete the call. Would need to block to complete this call.");
                }
            }

            public override bool CanRead
            {
                get { return true; }
            }

            public override bool CanSeek
            {
                get { return false; }
            }

            public override bool CanWrite
            {
                get { return true; }
            }

            public override long Length
            {
                get { throw new NotSupportedException(); }
            }

            public override long Position
            {
                get { throw new NotSupportedException(); }
                set { throw new NotSupportedException(); }
            }

            #endregion

            #region sync result

            sealed class SynchronousAsyncResult<T> : IAsyncResult
            {
                public T Result { get; set; }

                public bool IsCompleted
                {
                    get { return true; }
                }

                public WaitHandle AsyncWaitHandle
                {
                    get { throw new InvalidOperationException("Cannot wait on a synchronous result."); }
                }

                public object AsyncState { get; set; }

                public bool CompletedSynchronously
                {
                    get { return true; }
                }
            }

            #endregion
        }
    }
}


