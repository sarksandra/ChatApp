using ChatService;
using ChatService.Net.IO;
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

            while (true)
            {
                var client = new Client(_listener.AcceptTcpClient());
                _user.Add(client);

                BroadcastConnetion();
            }
        }
        static void BroadcastConnetion()
        {
            foreach (var user in _user)
            {
                foreach (var usr in _user)
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(1);
                    broadcastPacket.WriteMessage(usr.Username);
                    broadcastPacket.WriteMessage(usr.UID.ToString());
                    user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());

                }

            }
        }
        public static void BroadcastMessage(string message)
        {
            foreach (var user in _user)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOpCode(5);
                msgPacket.WriteMessage(message);
                user.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
            }
        }

        public static void BroadcastDisconnect(string uid)
        {
            var disconnectedUser = _user.Where(x => x.UID.ToString() == uid).FirstOrDefault();
            _user.Remove(disconnectedUser);
            foreach (var user in _user)
            {
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(10);
                broadcastPacket.WriteMessage(uid);
                user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
            }

            BroadcastMessage($"[{disconnectedUser.Username}]: Disconnected");
        }
    }
}
