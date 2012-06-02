using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Net.Sockets;
using Microsoft.Phone.Net;
using System.Text;
using System.Windows.Threading;

namespace TTSPhone
{
    public class Network
    {
        Socket socket;
        public Network()
        {
        }

        public void Connect(string IPAddress, int port)
        {
            try
            {
                var hostEntry = new DnsEndPoint(IPAddress, port);
                var message = "This is a simple test";
                var buffer = Encoding.UTF8.GetBytes(message);
                var args = new SocketAsyncEventArgs();
                args.RemoteEndPoint = hostEntry;
                args.Completed += SocketAsyncEventArgs_Completed;
                args.SetBuffer(buffer, 0, buffer.Length);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                bool completesAsynchronously = socket.ConnectAsync(args);
                if (!completesAsynchronously)
                {
                    SocketAsyncEventArgs_Completed(args.ConnectSocket, args);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void SocketAsyncEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                CleanUp(e);
                return;
            }

            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    HandleConnect(e);
                    break;
                case SocketAsyncOperation.Send:
                    HandleSend(e);
                    break;
            }
        }

        private void HandleConnect(SocketAsyncEventArgs e)
        {
            if (e.ConnectSocket != null)
            {
                bool completesAsynchronously = e.ConnectSocket.SendAsync(e);
                if (!completesAsynchronously)
                {
                    SocketAsyncEventArgs_Completed(e.ConnectSocket, e);
                }
            }
        }

        private void HandleSend(SocketAsyncEventArgs e)
        {
            CleanUp(e);
        }

        private void CleanUp(SocketAsyncEventArgs e)
        {
            if (e.ConnectSocket != null)
            {
                e.ConnectSocket.Shutdown(SocketShutdown.Both);
                e.ConnectSocket.Close();
            }
        }
    }
}
