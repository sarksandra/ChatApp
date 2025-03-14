using ChatService;
using System.Net;
using System.Net.Sockets;

namespace ChatApp
{
    class Program
    {
        static List<Client> _user;
        static TcpListener _listener;
        static void Main(string[] args)
        {
            _user = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
            _listener.Start();

            while(true)
            {
                var client = new Client(_listener.AcceptTcpClient());
                _user.Add(client);
            }

            

        }
    }
}
