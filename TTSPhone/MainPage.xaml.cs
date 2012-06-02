using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Net;
using System.Net.Sockets;

namespace TTSPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        Network network;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            network = new Network();
            network.NetworkListener();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            network = new Network();
            network.Connect("localhost", 13001);
        }
    }
}