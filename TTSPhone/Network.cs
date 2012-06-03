using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Runtime.InteropServices;

namespace TTSPhone
{
    [StructLayout(LayoutKind.Explicit)]
    public struct PacketHeader
    {
        [FieldOffset(0)]
        public byte byte1;

        [FieldOffset(1)]
        public byte byte2;

        [FieldOffset(2)]
        public byte byte3;

        [FieldOffset(3)]
        public byte byte4;

        [FieldOffset(0)]
        public int length;
    }
    public class Network
    {
        TcpListener listener;
        TcpClient client;
        NetworkStream receiveStream;
        NetworkStream sendStream;
        public bool InCall;
        byte[] receivedByes;

        public Network()
        {
            receivedByes = new byte[1048576];
            MainWindow.receiveThread = new System.Threading.Thread(this.Receive);
        }

        public void StartListening(int port)
        {
            listener = new TcpListener(IPAddress.Any, 3301);
            MainWindow.listenThread = new System.Threading.Thread(this.Listen);
            MainWindow.listenThread.Start();
        }

        private void Listen()
        {
            listener.Start();
            while (true)
            {
                if (listener.Pending())
                {
                    MessageBoxResult result = MessageBox.Show("Incoming call from     Accept", "Incoming call", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                    {
                        break;
                    }
                    else if (result == MessageBoxResult.Yes)
                    {
                        client = listener.AcceptTcpClient();
                        receiveStream = client.GetStream();
                        sendStream = client.GetStream();
                        MainWindow.receiveThread.Start();
                    }
                }
            }
        }

        public void Send(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                sendStream.WriteByte(buffer[i]);
            }
        }

        public void TryConnect(string IPAddress)
        {
            try
            {
                client = new TcpClient(IPAddress, 3301);
                receiveStream = client.GetStream();
                sendStream = client.GetStream();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred: " + ex.Message.ToString());
            }
        }
        
        public void Receive ()
        {
            PacketHeader header = new PacketHeader();
            while (InCall)
            {
                if (receiveStream.DataAvailable)
                {
                    header.byte1 = (byte)receiveStream.ReadByte();
                    header.byte2 = (byte)receiveStream.ReadByte();
                    header.byte3 = (byte)receiveStream.ReadByte();
                    header.byte4 = (byte)receiveStream.ReadByte();
                    for (int i = 4; i < header.length; i++)
                    {
                        receivedByes[i - 4] = (byte)receiveStream.ReadByte();
                    }
                }
            }
        }
    }
}
