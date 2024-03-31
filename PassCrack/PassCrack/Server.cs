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
        public string Keys = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789,./;'[]\\-=<>?:\"{}|_+!@#$%^&*() ";
        public List<string> Passwords = new List<string>();
        public Server(int _ClientCount) 
        {
            ClientCount = _ClientCount;
        }

        public string DecToString(ulong number)
        {
            string result = "";
            if (number == 0)
                return "a";
            while (number > 0)
            {
                int remainder = (int)(number % (ulong)Keys.Length);
                result = Keys[remainder] + result;
                number /= (ulong)Keys.Length;
            }
            return result;
        }

        static string CalculateMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                // Konwertuj input na tablicę bajtów i oblicz skrót MD5
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Konwertuj tablicę bajtów na ciąg szesnastkowy
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        public void BruteForce(ulong from, ulong to)
        {
            ulong i = from;
            while (i < to)
            {
                var tmp = DecToString(i);
                var hash = CalculateMD5Hash(tmp);
                if(hash == "6cd3556deb0da54bca060b4c39479839") Console.WriteLine("x");
                /* for (int j = 0; j < Passwords.Count; j++)
                 {
                 }*/
                i++;
            }
        }
        public bool Start()
        {
            //BruteForce(0,18446744073709551615);
            var time = DateTime.Now;
            BruteForce(0,20000000);
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
