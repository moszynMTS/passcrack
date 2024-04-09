using System.Net.Sockets;
using System.Text;

namespace PassCrack.Host
{
    public class ConnectionHandler
    {
        private TcpClient Client;
        private readonly int ClientNr;
        private readonly List<string> Passwords;
        public ConnectionHandler(TcpClient client, int clientNr, List<string> passwords)
        {
            this.Client = client;
            this.ClientNr = clientNr;
            this.Passwords = passwords;
        }
        public void HandleClient()
        {
            try
            {
                Console.WriteLine("Obsługa klienta {0} rozpoczęta.", ClientNr);

                SendMessage(string.Join(";", Passwords));
                bool found = false;
                while (!found)
                {
                    int number = 1000;// tutaj jakos trzeba synchronizowac wszystkie wątki tak, aby ten number był zawsze ostatni sprawdzany
                    int size = 20000;
                    //globalNr += size;
                    SendMessage($"{number},{size}");
                    var received = ReceiveMessage(Client.GetStream());
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

        private string ReceiveMessage(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            var result = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Obsługa klienta {0} odebrano {1}.", ClientNr, result);
            return result;
        }
    }
}
