using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace chat_room_server {
    class Program {

        static List<Client> clientList = new List<Client>();

        public static void BroadcastMessage(string message) {
            var offClient = new List<Client>();
            foreach (var client in clientList) {
                if (client.Connected()) {
                    client.SendMessage(message);
                } else {
                    offClient.Add(client);
                }
            }
            foreach (var temp in offClient) {
                clientList.Remove(temp);
            }
        }

        static void Main(string[] args) {
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("10.1.1.34"), 8878);
            server.Bind(ipEndPoint);
            Console.WriteLine("服务器开启--------");
            server.Listen(100);
            while (true) {
                Socket clientSocket = server.Accept();
                Client client = new Client(clientSocket);
                Console.WriteLine("客户端连接+1");
                clientList.Add(client);
            }
        }
    }
}
