﻿using ConsoleApp.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Net
{
    class Server
    {
        TcpClient _client;
        public PacketReader PacketReader;

        public event Action connectedEvent;
        public event Action msgRecivedEvent;
        public event Action disconnect;
        //PacketBuilder _packetBuilder;
        public Server()
        {
            _client = new TcpClient();

        }

        public void ConnecToServer(string username)
        {
            if (!_client.Connected)
            {
                _client.Connect("server", 7891);
                PacketReader = new PacketReader(_client.GetStream());

                if (!string.IsNullOrEmpty(username))
                {
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOpCode(0);
                    connectPacket.WriteMessage(username);
                    _client.Client.Send(connectPacket.GetPacketBytes());
                }
                ReadPackets();



            }


        }
        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var opcode = PacketReader.ReadByte();
                    switch (opcode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;
                        case 5:
                            msgRecivedEvent?.Invoke();
                            break;
                        case 10:
                            disconnect?.Invoke();
                            break;

                        default:
                            Console.WriteLine("ah yes..");
                            break;
                    }

                }
            });
        }
        public void SendMessageToServer(string message)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteMessage(message);
            _client.Client.Send(messagePacket.GetPacketBytes());
        }
    }
}
