using ConsoleApp.MvvM.ViewModel;
using ConsoleApp.Net;
using ConsoleApp.Net.IO;

namespace ConsoleApp
{
    class Program
    {
        private static Server _server;

        static void Main(string[] args)
        {
            InitializeServer();

            Console.Write("Enter your username: ");
            string username = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Username cannot be empty.");
                return;
            }

            _server.ConnecToServer(username);
            StartChatLoop(username);
        }

        private static void InitializeServer()
        {
            _server = new Server();
            _server.msgRecivedEvent += OnMessageReceived;
        }

        private static void OnMessageReceived()
        {
            string receivedMessage = _server.PacketReader.ReadMessage();
            Console.WriteLine($"\n[Server]: {receivedMessage}");
        }

        private static void StartChatLoop(string username)
        {
            Console.WriteLine("You can start typing messages. Type 'exit' to quit.\n");

            while (true)
            {
                Console.Write("You: ");
                string message = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(message)) continue;

                if (message.ToLower() == "exit") break;

                _server.SendMessageToServer(message);
            }

            Console.WriteLine("Chat ended.");
        }
    }
}