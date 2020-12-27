﻿using AutoMapper;
using RMS.AWS;
using RMS.Component.Communication.Tcp.Server;
using RMS.Component.DataAccess.SQLite.Entities;
using RMS.Server.DataTypes;
using RMS.Server.WebApi.Configuration;
using System.Collections.Generic;

namespace RMS.Component.Mappers
{
    public class ConfigurationMapper
    {
        public static MapperConfiguration Config => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TcpServerChannelConfig, ServerChannelConfiguration>();
            cfg.CreateMap<ListenerApiConfig, ServerInfo>();
            cfg.CreateMap<SiteConfig, SiteInfo>();
            cfg.CreateMap<TimeOffsetConfig, TimeOffset>();
            cfg.CreateMap<WebApiConfig, WebApiServerConfiguration>();
        });
        public static Mapper Mapper => new Mapper(Config);
        public static ServerChannelConfiguration Map(TcpServerChannelConfig entity)
        {
            var obj = Mapper.Map<ServerChannelConfiguration>(entity);
            return obj;
        }
        public static SiteInfo Map(SiteConfig entity)
        {
            var obj = Mapper.Map<SiteInfo>(entity);
            return obj;
        }
        public static WebApiServerConfiguration Map(WebApiConfig entity)
        {
            var obj = Mapper.Map<WebApiServerConfiguration>(entity);
            return obj;
        }
        public static IEnumerable<SiteInfo> Map(IEnumerable<SiteConfig> entities)
        {
            var obj = Mapper.Map<IEnumerable<SiteInfo>>(entities);
            return obj;
        }
    }
}
