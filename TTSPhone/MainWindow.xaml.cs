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
        public static Thread receiveThread;
        public static Thread sendThread;
        Network network;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            network.TryConnect("localhost");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            network = new Network();
            network.StartListening(3301);
        }
    }
}
