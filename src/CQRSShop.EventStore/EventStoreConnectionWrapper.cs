using System;
using System.Configuration;
using System.Net;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace CQRSShop.EventStore
{
    public class EventStoreConnectionWrapper
    {
        private static IEventStoreConnection _connection;

        public static IEventStoreConnection Connect()
        {
            ConnectionSettings settings =
                ConnectionSettings.Create()
                    .UseConsoleLogger()
                    .KeepReconnecting()
                    .SetDefaultUserCredentials(new UserCredentials(EventStoreUser, EventStorePassword));
            var endPoint = new IPEndPoint(EventStoreIP, EventStorePort);
            _connection = EventStoreConnection.Create(settings, endPoint, null);
            _connection.ConnectAsync().Wait();
            return _connection;
        }

        private static string EventStoreUser
        {
            get { return GetConfigSetting("EventStoreUser", y => y, () => "admin"); }
        }

        private static string EventStorePassword
        {
            get { return GetConfigSetting("EventStorePassword", y => y, () => "changeit"); }
        }

        private static IPAddress EventStoreIP
        {
            get
            {
                return GetConfigSetting("EventStoreHostName", val => Dns.GetHostAddresses(val)[0], () => IPAddress.Loopback);
            }
        }

        private static int EventStorePort
        {
            get
            {
                return GetConfigSetting("EventStorePort", int.Parse, () => 1113);
            }
        }

        private static TType GetConfigSetting<TType>(string key, Func<string, TType> convertFunc, Func<TType> defaultValueFunc)
        {
            var valueString = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(valueString))
            {
                return defaultValueFunc();
            }
            return convertFunc(valueString);
        }
    }
}
