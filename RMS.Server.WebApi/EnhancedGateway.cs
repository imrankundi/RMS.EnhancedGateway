using Newtonsoft.Json.Linq;
using RMS.AWS;
using RMS.Component.Communication.Tcp.Server;
using RMS.Core.Common;
using RMS.Server.DataTypes;
using RMS.Server.DataTypes.Requests;
using RMS.Server.WebApi.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace RMS.Server.WebApi
{
    public class EnhancedGateway : ITerminalCommandHandler
    {
        private ServerChannel server;
        private ServerInfo info;
        private ServerChannelConfiguration configurations;
        private Timer timer;

        public IEnumerable<string> ChannelKeys => server.ChannelKeys;
        public EnhancedGateway()
        {
            timer = new Timer();
            timer.Interval = ServerChannelConfigurationManager.Instance.Configurations.SyncIntervalInSeconds * 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Timer Tick...");
            timer.Enabled = false;
            try
            {
                if (!server.IsStarted)
                {
                    RetryEstablishServerChannel();
                }
                SynchronizeTerminals();
                SendNoChannelConnectedEmail();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                timer.Enabled = true;
            }

        }

        private bool noChannelConnectedEmailSent = false;
        private void SendNoChannelConnectedEmail()
        {
            try
            {
                if (server.ChannelKeys.Count == 0)
                {
                    if (!noChannelConnectedEmailSent)
                    {
                        Console.WriteLine("Sending No Channel Connected Email");
                        //noChannelConnectedEmailSent = Configuration.EmailManager.Instance.SendNoChannelConnectedEmail();
                    }
                }
                else
                {
                    noChannelConnectedEmailSent = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private bool retryingServerChannelConnection = false;
        private void RetryEstablishServerChannel()
        {
            if (retryingServerChannelConnection)
                return;

            retryingServerChannelConnection = true;
            //Log?.Verbose(className, "RetryEstablishRouterChannel", "Establishing Router Channel");
            Console.WriteLine("Retrying....");
            try
            {
                //if (cxpRouterChannel != null)
                //    cxpRouterChannel.CloseAsync();

                System.Threading.Thread.Sleep(5 * 1000);
                server.StartAsync();
                //ReEstablishCxpServerChannel();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


            retryingServerChannelConnection = false;
        }

        public async Task Start()
        {
            await EstablishServerChannel();
        }

        public async Task Stop()
        {
            await server.StopAsync();
        }

        public void Notify(object request)
        {
            var jsonObject = JObject.FromObject(request);
            TerminalCommandRequest command = jsonObject.ToObject<TerminalCommandRequest>();

            // parse and store command to a dictionary
            Console.WriteLine(command.ToFormattedJson());
            Task.Run(() => server.Send(command.TerminalId, command.Data));
        }

        private void SynchronizeTerminals()
        {
            //ClientChannelManager.BroadCast(TerminalHelper.TimeSync());
            server.SynchronizeTerminals();
            //terminalGateway.BroadCast(TerminalHelper.TimeSync());
        }
        private async Task EstablishServerChannel()
        {

            try
            {
                configurations = ServerChannelConfigurationManager.Instance.Configurations;
                server = new ServerChannel(configurations)
                {
                    ServerChannelHandler = this
                };
                await server.StartAsync();


            }
            catch (Exception)
            {
                server.ServerChannelHandler = null;
                server = null;
            }
        }
        public void TerminalCommandReceived(TerminalCommandReceivedEventArgs e)
        {
            Console.WriteLine("{0}: {1}", e.ChannelKey, e.Message);
            //var result = ParsingManager.FirstLevelParser(e.Message);
            var command = TerminalCommandHandler.Instance.Find(e.ChannelKey);
            if (command != null)
            {
                command.ResponseData = e.Message;
                command.ResponseReceivedOn = DateTimeHelper.CurrentUniversalTime;
                command.Status = CommandStatus.ResponseReceived;
            }
            //var result = ParsingManager.FirstLevelParser(e.Message);
            //UpdateClientInfo(e.Context, result);
            //var packet = ParsingManager.SecondLevelParser(result);
            //var json = JsonConvert.SerializeObject(packet, Formatting.Indented);
            //Console.WriteLine(json);
            //PushToServer(json);
        }

        private void PopulateChannelList()
        {
            //var keys = server.ClientChannelManager?.ChannelKeys;
            //if (keys == null)
            //    return;
            ////StringBuilder sb = new StringBuilder();
            //Console.WriteLine("------------------------------------");
            //foreach (var key in keys)
            //{
            //    Console.WriteLine(key);
            //}
            //Console.WriteLine("------------------------------------");

        }


    }
}
