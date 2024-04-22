using System.Net.Sockets;
using System.Net;
using System.IO;

namespace PassCrack.Host
{

    public class Server
    {
        public int ClientCount;
        public int Method;
        public int Hash;
        public int Number;
        List<List<string>> WordList = new List<List<string>>();
        public Server(int _ClientCount, int _Method, int _Hash) 
        {
            ClientCount = _ClientCount;
            Method = _Method;
            Hash = _Hash;
        }

        public bool Start(List<string> passwords)
        {
            //BruteForce(0,18446744073709551615);
           /*var time = DateTime.Now;
            Console.WriteLine((DateTime.Now - time).TotalSeconds);
            return true;*/
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 5000);
                List<Thread> clientThreads = new List<Thread>();
                // Rozpocznij nasłuchiwanie
                GlobalData.SetNumber(0);
                Console.WriteLine("Serwer uruchomiony. Nasłuchiwanie na porcie 5000...");
                listener.Start();

                for (int i = 0; i < ClientCount; i++)
                {                                                                                  
                    // Akceptuj połączenie od klienta
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Klient połączony!");

                    // Tworzenie nowego wątku do obsługi klienta
                    ConnectionHandlerHost connectionHandler = new ConnectionHandlerHost(client, i, passwords, Method, Hash, null);
                    Thread clientThread = new Thread(connectionHandler.HandleClient);
                    clientThreads.Add(clientThread);
                    clientThread.Start();
                }
                foreach (Thread thread in clientThreads)
                {
                    thread.Join();
                }
                // Zamknij nasłuchiwanie po zakończeniu obsługi wszystkich klientów
                listener.Stop();
                Console.WriteLine("Nasłuchiwanie zakończone.");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd serwera: " + ex.Message);
                return false;
            }
        }
    }
}
