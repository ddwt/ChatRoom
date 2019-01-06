using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chat_room_server {
    class Client {

        private Socket clientSocket;
        private byte[] data = new byte[1024];

        public Client(Socket socket) {
            clientSocket = socket;
            Thread thread = new Thread(ReceiveMessage);
            thread.Start();
        }

        private void ReceiveMessage() {
            while (true) {
                if (clientSocket.Poll(10, SelectMode.SelectRead)) {
                    clientSocket.Close();
                    break;
                }
                int length = clientSocket.Receive(data);
                string message = Encoding.UTF8.GetString(data, 0, length);

                Console.WriteLine(message);
            }
        }

        public void SendMessage(string message) {
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(data);
        }

        public bool Connected() {
            return clientSocket.Connected;
        }
    }
}
