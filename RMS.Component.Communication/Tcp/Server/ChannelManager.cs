using DotNetty.Transport.Channels;
using RMS.Component.Communication.Tcp.Event;
using RMS.Core.Common;
using RMS.Gateway;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RMS.Component.Communication.Tcp.Server
{
    public class ChannelManager
    {
        private static readonly ConcurrentDictionary<IChannelHandlerContext, ChannelInfo> channels =
            new ConcurrentDictionary<IChannelHandlerContext, ChannelInfo>();
        public static ConcurrentDictionary<IChannelHandlerContext, ChannelInfo> Channels => channels;

        private static readonly ConcurrentDictionary<string, IChannelHandlerContext> channelKeyDictionary =
            new ConcurrentDictionary<string, IChannelHandlerContext>();
        public static ConcurrentDictionary<string, IChannelHandlerContext> ChannelKeyDictionary => channelKeyDictionary;

        private static readonly ChannelManager instance = new ChannelManager();
        public static ChannelManager Instance => instance;
        private ChannelManager()
        {
        }

        public ICollection<string> ChannelKeys => ChannelKeyDictionary?.Keys;

        public ChannelInfo AddChannel(IChannelHandlerContext context)
        {
            if (context == null)
                return null;

            if (!Channels.ContainsKey(context))
            {
                ChannelInfo info = new ChannelInfo
                {
                    ChannelId = GenerateChannelId(),
                    ConnectedOn = DateTimeHelper.CurrentUniversalTime,
                    ChannelKey = TerminalHelper.DefaultTerminalId
                };
                var result = Channels.TryAdd(context, info);
                if (!result)
                    return null;

                return info;
            }

            return null;
        }
        public bool RemoveChannel(IChannelHandlerContext context)
        {
            if (Channels.ContainsKey(context))
            {
                var result = Channels.TryRemove(context, out ChannelInfo info);
                /*----------------------------------------------*/
                if (info != null)
                {
                    try
                    {
                        UnregisterChannelKey(info.ChannelKey);
                        ChannelKeyDictionary.TryRemove(info.ChannelKey, out IChannelHandlerContext ctx);
                        context.CloseAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                /*----------------------------------------------*/
                return result;

            }
            return false;
        }

        public void BroadCast(string message)
        {
            var channels = Channels.Keys.ToArray();
            try
            {
                foreach (var channel in channels)
                {
                    try
                    {
                        channel.WriteAndFlushAsync(message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SynchronizeTerminals()
        {

            var channels = Channels.Keys.ToArray();
            Console.WriteLine("SynchronizeTerminals. Channel Count: {0}", channels.Length);
            try
            {
                foreach (var channel in channels)
                {
                    try
                    {
                        bool result = Channels.TryGetValue(channel, out ChannelInfo info);
                        Console.WriteLine("Channel Key: {0}", info.ChannelKey);
                        if (result)
                        {
                            var kickInterval = DateTimeHelper.CurrentUniversalTime
                                .AddSeconds(-(ServerChannelConfigurationManager.Instance
                                .Configurations.KickIntervalInSeconds));

                            if (kickInterval > info.LastDataReceived && kickInterval > info.ConnectedOn)
                            {
                                RemoveChannel(channel);
                            }
                            else
                            {
                                var pingInterval = DateTimeHelper.CurrentUniversalTime
                                .AddSeconds(-(ServerChannelConfigurationManager.Instance
                                .Configurations.PingIntervalInSeconds));
                                if (pingInterval > info.LastSynchronized)
                                {
                                    channel.WriteAndFlushAsync(TerminalHelper.TimeSync());
                                    info.LastSynchronized = DateTimeHelper.CurrentUniversalTime;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void UpdateChannelInfo(IChannelHandlerContext context, string key)
        {
            var info = FindChannelInfo(context);
            info.ChannelKey = key;
            info.LastDataReceived = DateTimeHelper.CurrentUniversalTime;

            if (info.ChannelKey != TerminalHelper.DefaultTerminalId)
            {
                if (!ChannelKeyDictionary.ContainsKey(key))
                {
                    ChannelKeyDictionary.TryAdd(key, context);
                }
            }


        }

        public string GenerateChannelId()
        {
            return Guid.NewGuid().ToString();
        }
        public string FindChannelKey(IChannelHandlerContext context)
        {
            if (Channels.ContainsKey(context))
            {
                var info = Channels[context];
                return info.ChannelKey;

            }
            return string.Empty;
        }
        public ChannelInfo FindChannelInfo(IChannelHandlerContext context)
        {
            if (Channels.ContainsKey(context))
            {
                var info = Channels[context];
                return info;

            }
            return null;
        }


        public bool RemoveAll()
        {
            return false;
        }

        public IChannelHandlerContext FindChannelByKey(string key)
        {
            if (!ChannelKeyDictionary.ContainsKey(key))
                return null;

            var channel = ChannelKeyDictionary[key];
            return channel;
        }

        public bool RegisterChannelKey(IChannelHandlerContext context, string key)
        {
            if (context == null)
                return false;

            if (string.IsNullOrEmpty(key))
                return false;


            var result = ChannelKeyDictionary.TryAdd(key, context);

            if (result)
            {
                var channelHandlerContext = FindChannelByKey(key);
                var channelInfo = FindChannelInfo(channelHandlerContext);
                if (channelInfo != null)
                {
                    channelInfo.ChannelKey = key;
                    channelInfo.RegisteredOn = DateTime.UtcNow;
                }

                //channelInfo.ChannelStatus = Touchless.DataAccess.Enum.ChannelStatus.Connected;
                //add to DB.
            }
            //var res2 = UpdateChannelKey(context, key);

            return result;
        }

        public bool UpdateChannelKey(IChannelHandlerContext context, string key)
        {
            try
            {
                if (context == null)
                    return false;

                if (!Channels.ContainsKey(context))
                    return false;

                var info = Channels[context];

                if (info == null)
                    return false;

                info.ChannelKey = key;
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UnregisterChannelKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;

            if (!ChannelKeyDictionary.ContainsKey(key))
                return false;

            IChannelHandlerContext context;
            ChannelKeyDictionary.TryRemove(key, out context);

            if (context != null)
            {

                return true;
            }
            //

            return false;
        }
        /*-------------------------------------------------------------*/

    }
}
