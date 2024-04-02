using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace PassCrack.Host
{
    public class Server
    {
        public int ClientCount;
        public Server(int _ClientCount) 
        {
            ClientCount = _ClientCount;
        }

        public bool Start()
        {
            //BruteForce(0,18446744073709551615);
            var time = DateTime.Now;
            Console.WriteLine((DateTime.Now - time).TotalSeconds);
            return true;
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 5000);
                List<Thread> clientThreads = new List<Thread>();
                // Rozpocznij nasłuchiwanie
                Console.WriteLine("Serwer uruchomiony. Nasłuchiwanie na porcie 5000...");
                listener.Start();
                for (int i = 0; i < ClientCount; i++)
                {
                    // Akceptuj połączenie od klienta
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Klient połączony!");

                    // Tworzenie nowego wątku do obsługi klienta
                    ConnectionHandler connectionHandler = new ConnectionHandler(client, i);
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
