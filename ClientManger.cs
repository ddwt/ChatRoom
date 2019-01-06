using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ClientManger : MonoBehaviour {

    private string ipAddr = "10.1.1.34";
    private int port = 8878;
    private byte[] data = new byte[1000];
    private string message = "";
    private Socket clientSocket;
    private Thread thread;

    public Text chat_text;
    public InputField chat_input;


	// Use this for initialization
	void Start () {
        ConnectToServer();
	}
	
	// Update is called once per frame
	void Update () {
		if (message != "" && message != null) {
            chat_text.text += "\n" + message;
            message = "";
        }
	}

    void OnDestroy() {
        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();
    }

    private void ConnectToServer() {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAddr), port);
        clientSocket.Connect(endPoint);
        thread = new Thread(ReceiveMessage);
    }

    private void ReceiveMessage() {
        while (true) {
            if (!clientSocket.Connected) {
                break;
            }
            int length = clientSocket.Receive(data);
            message = Encoding.UTF8.GetString(data, 0, length);
            Debug.Log(message);
        }
    }

    private void SendMessage(string message) {
        byte[] data = Encoding.UTF8.GetBytes(message);
        clientSocket.Send(data);
    }

    public void OnSendButtonDown() {
        string value = chat_input.text;
        SendMessage(value);
        chat_input.text = "";
    }
}
