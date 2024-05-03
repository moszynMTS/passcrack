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
        public string CharacterKeys; //todo
        private int Method;
        private int Hash;
        private int ClientNr;
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
            for (int i = 0; i < 100; i++)
            {
                found = ReceiveAndSolvePackage();
            }
            Stream.Close();
            Client.Close();
        }

        void InitConnection()
        {
            ReceivePasswords();
            ReceiveMethod();
            if (Method == 1)
                ReceiveDictionaryFile();
        }
        bool ReceiveAndSolvePackage()
        {
            var mess = "";
            string foundedPasswords = "";
            string response = ReceiveMessage();
            var tmp = response.Split(";").ToList();
            Console.WriteLine(response);
            if (tmp.Count > 0)
                switch (Method)
                {
                    case 1: //slownik
                        {
                            var solver = new DictionaryMethodHandler(tmp, Hash, CharacterKeys);
                            foundedPasswords = solver.Resolve();
                            break;
                        }
                    case 2://2 or bruteforce
                        {
                            var solver = new BruteForceHandler(ulong.Parse(tmp[0]), ulong.Parse(tmp[1]), Hash, CharacterKeys);
                            foundedPasswords = solver.Resolve();
                            break;
                        }
                    default:
                        Console.WriteLine("Blad metody");
                        break;
                }
            SendOkMessage(foundedPasswords!="", foundedPasswords);
            return foundedPasswords != "";
        }
        void ReceiveMethod()
        {
            string response = ReceiveMessage();
            var tmp = response.Split(";").ToList();
            Method = int.Parse(tmp[0]);
            Hash = int.Parse(tmp[1]);
            ClientNr = int.Parse(tmp[2]);
            CharacterKeys = tmp[3]; //for custom keys 
            SendOkMessage();
        }
        void ReceiveDictionaryFile()
        {
            string filePath = $"{Directory.GetCurrentDirectory()}\\user_{ClientNr}.txt";

            if (File.Exists(filePath))
            {
                File.WriteAllText(filePath, string.Empty);
                Console.WriteLine($"Zawartość pliku {filePath} została wyczyszczona.");
            }

            using (FileStream fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write))
            {
                string response;
                while ((response = ReceiveMessage()) != "EOF")
                {
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(response);

                    fileStream.Write(data, 0, data.Length);
                }
            }
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
        }

        void SendOkMessage(bool found = false, string foundedPasswords="")
        {
            var mess = "Ok";
            if (found)
                mess = "found;"+foundedPasswords;
            byte[] data = Encoding.ASCII.GetBytes(mess);
            Stream.Write(data, 0, data.Length);
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
