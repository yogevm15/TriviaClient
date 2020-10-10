using MaterialDesignThemes.Wpf;
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

namespace TriviaApp
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        private SnackbarMessageQueue MyMessageQueue;
        private App app;
        public Menu()
        {
            InitializeComponent();
            app = ((App)Application.Current);
            app.communicator.forceRelease();
            MyMessageQueue = ((MainWindow)App.Current.MainWindow).MySnackbar.MessageQueue;
        }

        private void CreateRoomBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new Uri("CreateRoom.xaml", UriKind.Relative));
        }

        private void JoinRoomBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new Uri("JoinRoom.xaml", UriKind.Relative));
        }

        private void StatsBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new Uri("MyStats.xaml", UriKind.Relative));
        }

        private void BestScoresBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new Uri("BestScores.xaml", UriKind.Relative));
        }

        private void SignoutBTN_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndeterminate(btn, true);
            MaterialDesignThemes.Wpf.ButtonProgressAssist.SetValue(btn, -1);
            MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndicatorVisible(btn, true);
            disableAllButtons();
            app.communicator.SocketSendReceive(JsonSerializer.serializeRequest(new SignoutRequest(), Constants.SIGNOUT_REQUEST_CODE)).ContinueWith(task =>
            {
                ResponseInfo response = task.Result;
                LoginResponse loginResponse = JsonDeserializer.deserializeResponse<LoginResponse>(response.buffer);
                switch (loginResponse.status)
                {
                    case Constants.LOGOUT_SUCCESS:
                        MyMessageQueue.Enqueue("Sign out Successfully!");
                        this.Dispatcher.Invoke(() =>
                        {
                            NavigationService ns = NavigationService.GetNavigationService(this);
                            ns.Navigate(new Uri("Signin.xaml", UriKind.Relative));
                        });
                        break;
                }
            });
        }
        private void disableAllButtons()
        {
            CreateRoomBTN.IsEnabled = false;
            JoinRoomBTN.IsEnabled = false;
            StatsBTN.IsEnabled = false;
            BestScoresBTN.IsEnabled = false;
            SignoutBTN.IsEnabled = false;
        }
        private void enableAllButtons()
        {
            CreateRoomBTN.IsEnabled = true;
            JoinRoomBTN.IsEnabled = true;
            StatsBTN.IsEnabled = true;
            BestScoresBTN.IsEnabled = true;
            SignoutBTN.IsEnabled = true;

        }
    }
}
