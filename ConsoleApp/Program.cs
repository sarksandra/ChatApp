using ConsoleApp.MvvM.ViewModel;
using ConsoleApp.Net;

namespace ConsoleApp
{
    class Program
    {

        static void Main(string[] args)
        {
            
            MainViewModel mainViewModel = new MainViewModel();
            mainViewModel._server.ConnecToServer("tervist");
            mainViewModel._server.SendMessageToServer("tere");





        }
        
    }
    
}
