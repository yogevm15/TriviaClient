using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TriviaApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public string username = "";
        public bool admin = false;
        public Communicator communicator = new Communicator("127.0.0.1", 12345);
        
    }
}
