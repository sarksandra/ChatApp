using ConsoleApp.Model;
using ConsoleApp.Net;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApp.MvvM.ViewModel
{
    class MainViewModel
    {
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<string> Messages { get; set; }


        public string Username { get; set; }

        public string Message { get; set; }

        public Server _server;

        public MainViewModel()
        {
            Users = new ObservableCollection<UserModel>();
            Messages = new ObservableCollection<string>();
            _server = new Server();
            _server.connectedEvent += UserConnected;
            _server.msgRecivedEvent += MessageRecived;
            _server.disconnect += RemoveUser;
           
        }

        private void UserConnected()
        {
            var user = new UserModel
            {
                UserName = _server.PacketReader.ReadMessage(),
                UID = _server.PacketReader.ReadMessage(),
            };
            if (!Users.Any(x => x.UID == user.UID))
            {
                Users.Add(user);


            }
        }
        private void MessageRecived()
        {
            var msg = _server.PacketReader.ReadMessage();
           Messages.Add(msg);

        }
        private void RemoveUser()
        {
            var uid = _server.PacketReader.ReadMessage();
            var user = Users.Where(x => x.UID == uid).FirstOrDefault();
            Users.Remove(user);
        }
    }
}
