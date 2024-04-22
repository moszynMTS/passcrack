using System.Net.Sockets;
using System.Text;

namespace PassCrack.Client
{
    public class ConnectionHandlerClient
    {
        private NetworkStream Stream;
        private TcpClient Client;
        private string message = "";
        private List<string> Passwords;
        private int Method;
        private int Hash;
        private ulong From;
        private ulong To;

        public ConnectionHandlerClient(TcpClient client)
        {
            Client = client;
            Stream = client.GetStream();
        }
        public void HandleClient()
        {
            InitConnection();
            var found = false;
            for (int i =0; i<100;i++)
            {
                found=ReceiveAndSolvePackage();
            }
            Stream.Close();
            Client.Close();
        }

        void InitConnection()
        {
            ReceivePasswords();
            ReceiveMethod();
        }
        bool ReceiveAndSolvePackage()
        {
            var mess = "";
            var found = false;
            switch (Method)
            {
                case 1: //slownik
                    {
                        string response = ReceiveMessage();
                        var tmp = response.Split(";").ToList();
                        var solver = new DictionaryMethodHandler(tmp, Hash);
                        found=solver.Resolve();
                        break;
                    }
                case 2://2 or bruteforce
                    {
                        string response = ReceiveMessage();
                        var tmp = response.Split(";").ToList();
                        var solver = new BruteForceHandler(ulong.Parse(tmp[0]), ulong.Parse(tmp[1]), Hash);
                        found= solver.Resolve();
                        break;
                    }
                default:
                    Console.WriteLine("Blad metody");
                    break;
            }
            SendOkMessage(found);
            return found;
        }
        void ReceiveMethod()
        {
            string response = ReceiveMessage();
            var tmp = response.Split(";").ToList();
            Method = int.Parse(tmp[0]);
            Hash = int.Parse(tmp[1]);
            SendOkMessage();
        }
        void ReceivePasswords()
        {
            string response = ReceiveMessage();
            Passwords = response.Split(";").ToList();
            var mess = Passwords.Count() + " haseł.";
            SendMessage(mess);
        }
        void SendMessage(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            Stream.Write(data, 0, data.Length);
            Console.WriteLine("Wysłano: " + message);
        }

        void SendOkMessage(bool found=false)
        {
            var mess = "Ok";
            if (found)
                mess = "Found";
            byte[] data = Encoding.ASCII.GetBytes(mess);
            Stream.Write(data, 0, data.Length);
            Console.WriteLine("Wysłano: " + message);
        }

        void SendErrorMessage()
        {
            byte[] data = Encoding.ASCII.GetBytes("Error");
            Stream.Write(data, 0, data.Length);
            Console.WriteLine("Wysłano: " + message);
        }

        string ReceiveMessage()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = Stream.Read(buffer, 0, buffer.Length);
            return Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }
    }
}
