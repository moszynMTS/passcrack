using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PassCrack.Host
{
    public class ConnectionHandler
    {
        private TcpClient Client;
        private readonly int ClientNr;
        public ConnectionHandler(TcpClient client, int clientNr)
        {
            this.Client = client;
            this.ClientNr = clientNr;
        }
        public void HandleClient()
        {
            try
            {
                Console.WriteLine("Obsługa klienta {0} rozpoczęta.", ClientNr);

                for (int i = 0; i < 10; i++)
                {
                    string message = (i + 1).ToString();
                    SendMessage(message);
                    Console.WriteLine("Obsługa klienta {0} {1}.", ClientNr, ReceiveMessage(Client.GetStream()));
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
            Console.WriteLine("Wysłano: " + message);
        }

        static string ReceiveMessage(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            return Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }
    }
}
