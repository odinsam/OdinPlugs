using System.Security.AccessControl;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using OdinPlugs.OdinCore.ConfigModel.ConsulModel;
using OdinPlugs.OdinCore.ConfigModel.RabbitMQConfigModel;

namespace OdinPlugs.OdinCore.ConfigModel
{
    public enum EnumEnvironment
    {
        // ~ 开发环境
        DEV,
        // ~ 生产环境
        PRO
    }
    public class OcelotModel
    {
        public bool Enable { get; set; }
    }
    public class SerilogModel
    {
        public bool WriteToMySql { get; set; }
    }
    public class FrameworkConfigModel
    {
        public bool ApiSecurity { get; set; }
        public SnowFlakeModel SnowFlake { get; set; }
        public ParamsSignModel ParamsSign { get; set; }
        public WebUIConfigModel WebUIConfig { get; set; }
        public bool ApiLink { get; set; }
        public CheckIpsModel CheckIps { get; set; }
    }
    public class ApiSecurityModel
    {
        public bool Enable { get; set; }
    }
    public class RsaModel
    {
        public string RsaPrivateKey { get; set; }
        public string RsaPublicKey { get; set; }
    }
    public class SecurityModel
    {
        public RsaModel Rsa { get; set; }
    }
    public class PageRecordModel
    {
        public int PageSize { get; set; }
    }
    public class DoMainModel
    {
        public string Protocol { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
    }
    public class SysConfigModel
    {
        public bool EnableAop { get; set; }
    }
    public class ApiVersionModel
    {
        public int MajorVersion { get; set; }
        public int MinorVersion { get; set; }
    }
    public class ApiAuthenModel
    {
        public bool Enable { get; set; }
    }
    public class EnableErrorNotifyMailModel
    {
        public bool Enable { get; set; }
    }
    public class DbEntityModel
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        public bool InitDb { get; set; }
    }
    public class MongoDbModel
    {
        public bool Enable { get; set; }
        public string MongoConnection { get; set; }
        public string Database { get; set; }
    }
    public class AopErrorNotifyMailModel
    {
        public string ApiName { get; set; }
    }
    public class AopSysErrorRecordModel
    {
        public string ApiName { get; set; }
    }
    public class ApiInvokerCatchModel
    {
        public string ApiName { get; set; }
    }
    public class ApiInvokerRecordModel
    {
        public string ApiName { get; set; }
    }
    public class AopSysErrorCodesModel
    {
        public string ApiName { get; set; }
    }
    public class GetDeveloperUsersModel
    {
        public string ApiName { get; set; }
    }
    public class SysApiModel
    {
        public ApiInvokerRecordModel ApiInvokerRecord { get; set; }
        public ApiInvokerCatchModel ApiInvokerCatch { get; set; }
        public AopSysErrorRecordModel AopSysErrorRecord { get; set; }
        public AopErrorNotifyMailModel AopErrorNotifyMail { get; set; }
        public AopSysErrorCodesModel AopSysErrorCodes { get; set; }
        public GetDeveloperUsersModel GetDeveloperUsers { get; set; }
    }



    public class GloabModel
    {
        public string Url { get; set; }
        public SysApiModel SysApi { get; set; }
        public bool EnableAop { get; set; }
        public bool EnableErrorNotifyMail { get; set; }
    }
    public class ApiLinkModel
    {
        public bool Enable { get; set; }
    }
    public class HostServerModel
    {
        public string Url { get; set; }

    }
    public class ParamsSignModel
    {
        public bool Enable { get; set; }
        public string signKey { get; set; }
    }
    public class ValidateTokenModel
    {
        public bool Enable { get; set; }
        public string Uri { get; set; }
        public int ExpireTime { get; set; }
        public string UserField { get; set; }
        public string TokenValidate { get; set; }
        public string ReNewToken { get; set; }
    }
    public class AccountModel
    {
        public string Login { get; set; }
        public string TokenValidate { get; set; }
        public string ReNewToken { get; set; }
        public string ChangePwd { get; set; }
        public string Register { get; set; }
    }
    public class AuthenServerModel
    {
        public string Url { get; set; }
        public AccountModel Account { get; set; }
    }
    public class LocalHostApiModel
    {
        public string TokenReNew { get; set; }
    }
    public class LocalHostModel
    {
        public string Url { get; set; }
        public LocalHostApiModel Apis { get; set; }
    }
    public class AllowOriginModel
    {
        public string PolicyName { get; set; }
        public string WithOrigins { get; set; }
    }
    public class CheckIpsModel
    {
        public bool Enable { get; set; }
        public string AllowIps { get; set; }
        public string DisallowIps { get; set; }
    }
    public class CrossDomainModel
    {
        public AllowOriginModel AllowOrigin { get; set; }

    }
    public class RedisConfigModel
    {
        public bool Enable { get; set; }
        public string RedisIp { get; set; }
        public int RedisPort { get; set; }
        public string RedisPassword { get; set; }
        public string Connection { get; set; }
        public int DefaultDatabase { get; set; } = 0;
        public string InstanceName { get; set; }
    }


    public class WebUIConfigModel
    {
        public PageRecordModel PageRecord { get; set; }
    }
    public class BackplaneModel
    {
        public string Key { get; set; }
        public string KnownType { get; set; }
        public string ChannelName { get; set; }
    }

    public class LoggerFactoryModel
    {
        public string KnownType { get; set; } = "Microsoft";
    }
    public class SerializerModel
    {
        public string KnownType { get; set; } = "Json";
    }
    public class HandlesModel
    {
        public string KnownType { get; set; }
        public bool EnablePerformanceCounters { get; set; } = true;
        public bool EnableStatistics { get; set; } = true;
        public string ExpirationMode { get; set; } = "Absolute";
        public int ExpirationTimeout { get; set; }
        public bool IsBackPlaneSource { get; set; }
        public string HandleName { get; set; }
    }
    public class CacheManagerModel
    {
        public string CacheName { get; set; }
        public int RetryTimeout { get; set; } = 100;
        public string UpdateMode { get; set; } = "Up";
        public int MaxRetries { get; set; } = 1000;
        public BackplaneModel BackPlane { get; set; }
        public LoggerFactoryModel LoggerFactory { get; set; }
        public SerializerModel Serializer { get; set; }
        public HandlesModel[] Handles { get; set; }
    }
    public class CacheManagersModel
    {
        public int MaxRetries { get; set; }
        public string CacheName { get; set; }
        public int RetryTimeout { get; set; }
        public string UpdateMode { get; set; }
        public BackplaneModel Backplane { get; set; }
        public LoggerFactoryModel LoggerFactory { get; set; }
        public SerializerModel Serializer { get; set; }
        public HandlesModel[] Handles { get; set; }
    }
    public class IdentityServerOptions
    {
        public bool Enable { get; set; }
    }
    public class CapOptions
    {
        public string AopRouteingKey { get; set; } = "cap.odinCore.Aop.RabbitMQ.{0}";
        public string GetAopRouteingKey(string key)
        {
            return string.Format(AopRouteingKey, key);
        }
    }
    public class SnowFlakeModel
    {
        public long DataCenterId { get; set; }
        public long WorkerId { get; set; }
    }

    public class ConfigOptions
    {
        public EnumEnvironment EnvironmentName { get; set; }
        public DoMainModel Domain { get; set; }
        public bool Debug { get; set; }

        public ApiVersionModel ApiVersion { get; set; }
        public FrameworkConfigModel FrameworkConfig { get; set; }

        public string Url { get; set; }
        public DbEntityModel DbEntity { get; set; }
        public MongoDbModel MongoDb { get; set; }

        public CacheManagerModel CacheManager { get; set; }
        [ObsoleteAttribute]
        public HostServerModel HostServer { get; set; }
        public SecurityModel Security { get; set; }
        public GloabModel Global { get; set; }


        public ValidateTokenModel ValidateToken { get; set; }
        public AuthenServerModel AuthenServer { get; set; }
        public LocalHostModel LocalHost { get; set; }
        public CrossDomainModel CrossDomain { get; set; }
        public RedisConfigModel Redis { get; set; }
        public ConsulOptions Consul { get; set; }
        public IdentityServerOptions IdentityServer { get; set; }
        public RabbitMQOptions RabbitMQ { get; set; }
        public CapOptions Cap { get; set; }
    }
}