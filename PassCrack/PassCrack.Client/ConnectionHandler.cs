using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace PassCrack.Client
{
    public class ConnectionHandler
    {
        private NetworkStream Stream;
        private TcpClient Client;
        private string message = "";
        public ConnectionHandler(TcpClient client)
        {
            this.Client = client;
            this.Stream = client.GetStream();
        }
        public void HandleClient()
        {
                for (int i = 0; i < 10; i++)
                {
                    // Oczekiwanie na odpowiedź
                    string response = ReceiveMessage(Stream);
                    Console.WriteLine("Odebrano odpowiedź: " + response);
                    
                    if (message == "10")
                    {
                        Stream.Close();
                        Client.Close();
                        Console.WriteLine("Zakończono połączenie z serwerem.");
                        return;
                    }
                    string messageToSend = "Wiadomość " + (i + 1);
                    SendMessage(Stream, messageToSend);
                }
        }
        static void SendMessage(NetworkStream stream, string message)
        {
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
