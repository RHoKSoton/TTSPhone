using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace TTSPhone
{
    public partial class MainWindow : Window
    {
        public static Thread listenThread;
        Network network;
        public MainWindow()
        {
            InitializeComponent();
            network = new Network();
            network.StartListening(3301);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            network.TryConnect("localhost");
            network.Send(Encoding.ASCII.GetBytes("HELLO"));
        }
    }
}
