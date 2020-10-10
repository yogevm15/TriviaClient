using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MaterialDesignThemes.Wpf;

namespace TriviaApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            OTPCryptoAlgorithm cryptoAlgorithm = new OTPCryptoAlgorithm();
            JsonSerializer.cryptoAlgorithm = cryptoAlgorithm;
            JsonDeserializer.cryptoAlgorithm = cryptoAlgorithm;
            MySnackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1000));
            PageFrame.Source = new Uri("Signin.xaml", UriKind.Relative);
        }


        
        
        
    }
}
