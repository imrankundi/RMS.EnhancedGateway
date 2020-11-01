using DotNetty.Transport.Channels;
using RMS.Component.Communication.Tcp.Event;
using System;
using System.Collections.Concurrent;

namespace RMS.Component.Communication.Tcp.Server
{
    public class ClientChannelManager
    {
        private static ConcurrentDictionary<string, IChannelHandlerContext> ChannelKeyDictionary = new ConcurrentDictionary<string, IChannelHandlerContext>();
        public static ConcurrentDictionary<IChannelHandlerContext, ChannelInfo> Channels { get; } = new ConcurrentDictionary<IChannelHandlerContext, ChannelInfo>();
        public static ConcurrentDictionary<string, ChannelInfo> ChannelInfo { get; } = new ConcurrentDictionary<string, ChannelInfo>();
        public static ChannelInfo Add(IChannelHandlerContext context)
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
        public static string GenerateChannelId()
        {
            return Guid.NewGuid().ToString();
        }
        public static string FindChannelKey(IChannelHandlerContext context)
        {
            if (Channels.ContainsKey(context))
            {
                var info = Channels[context];
                return info.ChannelKey;

            }
            return string.Empty;
        }
        public static ChannelInfo FindChannelInfo(IChannelHandlerContext context)
        {
            if (Channels.ContainsKey(context))
            {
                var info = Channels[context];
                return info;

            }
            return null;
        }

        public static bool Remove(IChannelHandlerContext context)
        {
            if (Channels.ContainsKey(context))
            {
                ChannelInfo info;
                var result = Channels.TryRemove(context, out info);
                return result;

            }
            return false;
        }
        public static bool RemoveAll()
        {
            return false;
        }

        public static IChannelHandlerContext FindChannelByKey(string key)
        {
            if (!ChannelKeyDictionary.ContainsKey(key))
                return null;

            var channel = ChannelKeyDictionary[key];
            return channel;
        }

        public static bool RegisterChannelKey(IChannelHandlerContext context, string key)
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
                channelInfo.ChannelKey = key;
                channelInfo.RegisteredOn = DateTime.UtcNow;
                //channelInfo.ChannelStatus = Touchless.DataAccess.Enum.ChannelStatus.Connected;
                //add to DB.
            }
            //var res2 = UpdateChannelKey(context, key);

            return result;
        }

        public static bool UpdateChannelKey(IChannelHandlerContext context, string key)
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

        public static bool UnrigisterChannelKey(string key)
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
