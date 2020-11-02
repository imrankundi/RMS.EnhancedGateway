﻿using DotNetty.Transport.Channels;
using RMS.Component.Communication.Tcp.Event;
using RMS.Core.Common;
using RMS.Gateway;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RMS.Component.Communication.Tcp.Server
{
    public class ClientChannelManager
    {
        public static ConcurrentDictionary<string, IChannelHandlerContext> ChannelKeyDictionary { get; private set; }
        public static ConcurrentDictionary<IChannelHandlerContext, ChannelInfo> Channels { get; private set; }
        public IEnumerable<string> ChannelKeys => ChannelKeyDictionary?.Keys.ToArray();
        //public ConcurrentDictionary<string, ChannelInfo> ChannelInfo { get; private set; }
        public ClientChannelManager()
        {
            ChannelKeyDictionary = new ConcurrentDictionary<string, IChannelHandlerContext>();
            Channels = new ConcurrentDictionary<IChannelHandlerContext, ChannelInfo>();
            //ChannelInfo = new ConcurrentDictionary<string, ChannelInfo>();
        }
        public ChannelInfo AddChannel(IChannelHandlerContext context)
        {
            if (context == null)
                return null;

            if (!Channels.ContainsKey(context))
            {
                ChannelInfo info = new ChannelInfo
                {
                    ChannelId = GenerateChannelId(),
                    ConnectedOn = DateTime.UtcNow,
                    ChannelKey = "SPxxxxxx"
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
                    //ChannelKeyDictionary.TryRemove(info.ChannelKey, out IChannelHandlerContext ctx);
                    //ChannelInfo.TryRemove(info.ChannelKey, out ChannelInfo inf);

                    UnrigisterChannelKey(info.ChannelKey);
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
                foreach(var channel in channels)
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
            Console.WriteLine("SynchronizeTerminals");
            var channels = Channels.Keys.ToArray();
            try
            {
                foreach (var channel in channels)
                {
                    try
                    {
                        bool result = Channels.TryGetValue(channel, out ChannelInfo info);
                        if(result)
                        {
                            if(DateTimeHelper.CurrentUniversalTime
                                .AddSeconds(-(ServerChannelConfigurationManager.Instance
                                .Configurations.KickIntervalInSeconds)) > info.LastDataReceived)
                            {
                                RemoveChannel(channel);
                            }
                            else
                            {
                                if (DateTimeHelper.CurrentUniversalTime
                                .AddSeconds(-(ServerChannelConfigurationManager.Instance
                                .Configurations.PingIntervalInSeconds)) > info.LastSynchronized)
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
                if(channelInfo != null)
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

        public bool UnrigisterChannelKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;


            if (!ChannelKeyDictionary.ContainsKey(key))
                return false;

            IChannelHandlerContext context;
            ChannelKeyDictionary.TryRemove(key, out context);

            if (context != null)
                return true;

            return false;
        }
        /*-------------------------------------------------------------*/
       
    }
}
