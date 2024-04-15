using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace PassCrack.Host
{
    public class ConnectionHandler
    {
        private TcpClient Client;
        private readonly int ClientNr;
        private readonly List<string> Passwords;
        private int Method;
        public ConnectionHandler(TcpClient client, int clientNr, List<string> passwords, int method)//1 slownik 2 brute
        {
            Client = client;
            ClientNr = clientNr;
            Passwords = Hash(passwords);
            Method = method;
        }

        private List<string> Hash(List<string> passwords) 
        {
            var result = new List<string>();
            foreach(string password in passwords)
            {
                result.Add(CalculateMD5Hash(password));
            }
            return result;
        }
        public void HandleClient()
        {
            bool isError=false;
            string received="";
            try
            {
                Console.WriteLine("Obsługa klienta {0} rozpoczęta.", ClientNr);

                SendMessage(string.Join(";", Passwords));
                (isError, received) = ReceiveMessage(Client.GetStream());
                SendMessage(Method.ToString());
                (isError, received) = ReceiveMessage(Client.GetStream());
                bool found = false;

                object sonThread = new object();
                int number=0;
                int size = 20000;
                while (!found)
                {
                    lock(sonThread)
                    {
                        number += size;// tutaj jakos trzeba synchronizowac wszystkie wątki tak, aby ten number był zawsze ostatni sprawdzany
                    }
                    //globalNr += size;
                    SendMessage($"{number};{size};");
                    (isError, received) = ReceiveMessage(Client.GetStream());
                    if(received == "found")
                    {
                        found = true;
                        Console.WriteLine("Obsługa klienta {0} {1}.", ClientNr, "znaleziono haslo");
                    } else
                        Console.WriteLine("Obsługa klienta {0} {1}.", ClientNr, "wysylanie kolejnej paczki");
                }
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

        private void SendMessage(string message)
        {
            NetworkStream stream = Client.GetStream();
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Console.WriteLine("Obsługa klienta {0} wysłano {1}", ClientNr, message);
        }

        private (bool,string) ReceiveMessage(NetworkStream stream)//zwraca czy błąd i result
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            var result = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Obsługa klienta {0} odebrano {1}.", ClientNr, result);
            if(result !="Error")
                return (false, result);
            return (true,result);
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
    }
}
