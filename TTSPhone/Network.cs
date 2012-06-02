using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Windows;

namespace TTSPhone
{
    class Network
    {
        TcpListener listener;
        TcpClient client;
        NetworkStream stream;
        byte[] receivedByes;

        public Network()
        {
            receivedByes = new byte[4096];
        }

        public void StartListening(int port)
        {
            listener = new TcpListener(port);
            MainWindow.listenThread = new System.Threading.Thread(this.Listen);
        }

        private void Listen()
        {
            listener.Start();
            if (listener.Pending())
            {
                MessageBoxResult result = MessageBox.Show("Incoming call from     Accept", "Incoming call", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                {
                    this.listener.Server.Disconnect(true);
                }
                else if (result == MessageBoxResult.Yes)
                {
                    client = listener.AcceptTcpClient();
                    stream = client.GetStream();
                }
            }
        }

        public void Send(byte[] buffer)
        {
        }

        public void TryConnect(string IPAddress)
        {
            try
            {
                client = new TcpClient(IPAddress, 3301);
                stream = client.GetStream();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred: " + ex.Message.ToString());
            }
        }            
    }
}
