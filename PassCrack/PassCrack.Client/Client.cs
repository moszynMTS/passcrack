using System.Net.Sockets;

namespace PassCrack.Client
{
    internal class Client
    {

        public Client() { }

        public void Start()
        {
            try
            {
                // Połącz się z serwerem na porcie 5000 na localhost
                TcpClient client = new TcpClient("127.0.0.1", 5000);
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
