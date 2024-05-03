using System.Net.Sockets;

namespace PassCrack.Client
{
    internal class Client
    {
        public string IP = "127.0.0.1"; //needs to be the same as config.json on host
        public int Port = 5000;
        public Client() { }

        public void Start()
        {
            try
            {
                TcpClient client = new TcpClient(IP, Port);
                Console.WriteLine("Połączono z serwerem.");

                // Utwórz strumienie do komunikacji z serwerem
                ConnectionHandlerClient connectionHandler = new ConnectionHandlerClient(client);
                Thread clientThread = new Thread(connectionHandler.HandleClient);
                clientThread.Start();
                // Zamknij połączenie
                clientThread.Join();
                Console.WriteLine("Zakończono połączenie z serwerem.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd klienta: " + ex.Message);
            }
        }
    }
}
