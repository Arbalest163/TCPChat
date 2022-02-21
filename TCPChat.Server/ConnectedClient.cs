using System.Net.Sockets;

namespace TCPChat.Server
{
    public class ConnectedClient
    {
        public TcpClient Client { get; set; }
        public string Name { get; set; }

        public ConnectedClient(TcpClient client, string name)
        {
            Client = client;
            Name = name;
        }
    }
}