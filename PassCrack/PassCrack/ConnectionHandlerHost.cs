using System;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace PassCrack.Host
{
    public class ConnectionHandlerHost
    {
        private TcpClient Client;
        private readonly int ClientNr;
        private readonly List<string> Passwords;
        private readonly string CharacterKeys;
        private List<string> WordList;
        private int Method;
        private int Hash;
        private int ClientCount;
        static bool Stop = false;
        static bool Pause = false;

        static ManualResetEvent pauseEvent = new ManualResetEvent(true);
        public ConnectionHandlerHost(TcpClient client, int clientNr, int clientsCount, List<string> passwords, int method, int hash, List<string> wordList, string characterKeys)//method 1 slownik 2 brute
        {
            Client = client;
            ClientNr = clientNr;
            ClientCount = clientsCount;
            Passwords = HashPasswords(passwords);
            Method = method;
            Hash = hash;
            WordList = wordList; //for slownik
            CharacterKeys = characterKeys;//for bruteforce
        }

        private List<string> HashPasswords(List<string> passwords)
        {
            var result = new List<string>();
            foreach (string password in passwords)
            {
                if (Hash == 1)
                    result.Add(CalculateMD5Hash(password));
                else
                    result.Add(CalculateSHA1Hash(password));
            }
            return result;
        }
        public void HandleClient()
        {
            bool isError = false;
            string received = "";
            try
            {
                Console.WriteLine("Obsługa klienta {0} rozpoczęta.", ClientNr);

                SendMessage(string.Join(";", Passwords));
                (isError, received) = ReceiveMessage(Client.GetStream());
                SendMessage($"{Method};{Hash};{ClientNr};{CharacterKeys}");
                (isError, received) = ReceiveMessage(Client.GetStream());

                if (Method == 1)
                {
                    SendDictionaryFile();
                    (isError, received) = ReceiveMessage(Client.GetStream());
                }

                bool found = false;

                // - key event for stop client
                Thread keyListenerThread = new Thread(ReadKey);
                keyListenerThread.Start();

                object sonThread = new object();
                int number = 0;
                int size = 20000;
                while (!Stop)//wszytkie hasla nie znalezie
                {
                    //if (Pause)
                        pauseEvent.WaitOne();
                    lock (sonThread)
                    {
                        number = GlobalData.GetNumber();
                        GlobalData.SetNumber(number + size);
                    }
                    SendMessage($"{number};{size}");
                    (isError, received) = ReceiveMessage(Client.GetStream());
 
                     //Console.WriteLine("Obsługa klienta {0} {1}.", ClientNr, "wysylanie kolejnej paczki");
                }

                keyListenerThread.Join();
                Console.WriteLine("Obsługa klienta {0} zakończona.", ClientNr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd obsługi klienta {0}: {1}", ClientNr, ex.Message);
            }
            finally
            {
                Client.Close();
            }
        }

        private void SendDictionaryFile()
        {
            string fileName = "wordlist.txt";
            string currentDirectory = Directory.GetCurrentDirectory();
            string sourceDirectory = Directory.GetParent(currentDirectory)?.Parent?.Parent?.FullName;
            string filePath = Path.Combine(sourceDirectory, fileName);
            if (File.Exists(filePath))
            {
                Console.WriteLine("ZNALEZIONI PLIK");
                FileInfo fileInfo = new FileInfo(filePath);
                long fileSize = fileInfo.Length;

                long partSize = fileSize / ClientCount;
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    long startPosition = (ClientNr) * partSize;

                    // - to ważne bo ustawia wskaźnik z pliku na tej części pliku którą ma dostać klient
                    reader.BaseStream.Seek(startPosition, SeekOrigin.Begin);

                    byte[] buffer = new byte[1024];
                    int bytesRead;
                    long totalBytesRead = 0;

                    while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0 && totalBytesRead < partSize)
                    {
                        string data = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.Write(data);

                        // - send pacakges of file to Client
                        SendMessage(data);

                        totalBytesRead += bytesRead;
                    }
                    SendMessage("EOF");

                }

            }
        }

        private void SendMessage(string message)
        {
            NetworkStream stream = Client.GetStream();
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Console.WriteLine("Obsługa klienta {0} wysłano {1}", ClientNr, message);
        }

        private (bool, string) ReceiveMessage(NetworkStream stream)//zwraca czy błąd i result
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            var result = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            if (result.StartsWith("found;"))
            {
                Console.WriteLine("Obsługa klienta {0} znaleziono hasłą {1}.", ClientNr, result.Substring(6));
                Console.WriteLine("--------------------------------------------------------");
            }
            //Console.WriteLine("Obsługa klienta {0} odebrano {1}.", ClientNr, result);
            if (result != "Error")
                return (false, result);
            return (true, result);
        }
        public static string CalculateSHA1Hash(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                // Konwertuj input na tablicę bajtów i oblicz skrót SHA-1
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha1.ComputeHash(inputBytes);

                // Konwertuj tablicę bajtów na ciąg szesnastkowy
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        public static string CalculateMD5Hash(string input)
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
        public static void ReadKey()
        {
            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.P)
                {
                    //Pause = !Pause;
                    //if (!Pause)
                    //{
                    //    pauseEvent.Dispose();
                    //}
                    if (pauseEvent.WaitOne(0))
                    {
                        Console.WriteLine("PAUZA");
                        pauseEvent.Reset();
                    }
                    else
                    {
                        Console.WriteLine("START");
                        pauseEvent.Set();
                    }
                }
            }
        }
    }
}
