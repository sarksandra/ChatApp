using ConsoleApp.MvvM.ViewModel;
using ConsoleApp.Net;
using ConsoleApp.Net.IO;

namespace ConsoleApp
{
    class Program
    {


        static void Main(string[] args)
        {
            Server server = new Server();
            server.ConnecToServer("tere");
            server.SendMessageToServer("meow");
        }

    }
}