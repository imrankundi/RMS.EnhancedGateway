using System;
using System.Collections.Concurrent;

namespace RMS.Parser.Configuration
{
    public class DeviceConfigurationManager
    {
        protected ConcurrentDictionary<string, ConfigurationPacket> configQueue { get; private set; }

        public static DeviceConfigurationManager Instance { get; set; }

        private DeviceConfigurationManager()
        {
            configQueue = new ConcurrentDictionary<string, ConfigurationPacket>();
        }

        public ConfigurationQueueResult Add(ConfigurationPacket packet)
        {
            try
            {
                if (packet == null)
                {
                    return new ConfigurationQueueResult
                    {
                        Message = "NULL Packet",
                        Packet = packet,
                        Status = ConfigurationQueueStatus.Error
                    };

                }

                if (configQueue.ContainsKey(packet.TerminalId))
                {
                    return new ConfigurationQueueResult
                    {
                        Message = string.Format("Configuration for Terminal {0} is already in progress. Send configuration after old configuration is performed", packet.TerminalId),
                        Packet = packet,
                        Status = ConfigurationQueueStatus.Error
                    };

                }

                bool result = configQueue.TryAdd(packet.TerminalId, packet);

                if (!result)
                {
                    return new ConfigurationQueueResult
                    {
                        Message = string.Format("Unable to add configuration for Terminal {0} in Configuration Queue", packet.TerminalId),
                        Packet = packet,
                        Status = ConfigurationQueueStatus.Error
                    };
                }
                else
                {
                    return new ConfigurationQueueResult
                    {
                        Message = string.Format("Configuration for Terminal {0} is successfully added in Configuration Queue", packet.TerminalId),
                        Packet = packet,
                        Status = ConfigurationQueueStatus.Added
                    };
                }
            }
            catch (Exception ex)
            {
                return new ConfigurationQueueResult
                {
                    Message = ex.Message,
                    Packet = packet,
                    Status = ConfigurationQueueStatus.Error
                };
            }
        }
        public ConfigurationQueueResult Read(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    return new ConfigurationQueueResult
                    {
                        Message = "Invalid key provided",
                        Packet = null,
                        Status = ConfigurationQueueStatus.Error
                    };
                }

                bool result = configQueue.TryGetValue(key, out ConfigurationPacket packet);

                if (!result)
                {
                    return new ConfigurationQueueResult
                    {
                        Message = "Invalid key provided",
                        Packet = null,
                        Status = ConfigurationQueueStatus.Error
                    };
                }
                else
                {
                    return new ConfigurationQueueResult
                    {
                        Message = "Successfully read",
                        Packet = packet,
                        Status = ConfigurationQueueStatus.Pending
                    };
                }
            }
            catch (Exception ex)
            {
                return new ConfigurationQueueResult
                {
                    Message = ex.Message,
                    Packet = null,
                    Status = ConfigurationQueueStatus.Error
                };
            }
        }
        public ConfigurationQueueResult Remove(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    return new ConfigurationQueueResult
                    {
                        Message = "Invalid key provided",
                        Packet = null,
                        Status = ConfigurationQueueStatus.Error
                    };
                }

                bool result = configQueue.TryRemove(key, out ConfigurationPacket packet);

                if (!result)
                {
                    return new ConfigurationQueueResult
                    {
                        Message = "Invalid key provided",
                        Packet = null,
                        Status = ConfigurationQueueStatus.Error
                    };
                }
                else
                {
                    return new ConfigurationQueueResult
                    {
                        Message = "Successfully read",
                        Packet = packet,
                        Status = ConfigurationQueueStatus.Removed
                    };
                }
            }
            catch (Exception ex)
            {
                return new ConfigurationQueueResult
                {
                    Message = ex.Message,
                    Packet = null,
                    Status = ConfigurationQueueStatus.Error
                };
            }
        }
    }
}
