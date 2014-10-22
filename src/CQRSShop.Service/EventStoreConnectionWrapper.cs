using System.Net;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace CQRSShop.Service
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
                    .SetDefaultUserCredentials(new UserCredentials("admin", "changeit"));
            var endPoint = new IPEndPoint(IPAddress.Loopback, 1113);
            _connection = EventStoreConnection.Create(settings, endPoint, null);
            _connection.ConnectAsync().Wait();
            return _connection;
        }
    }
}