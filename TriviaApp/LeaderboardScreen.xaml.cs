using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for LeaderboardScreen.xaml
    /// </summary>
    public partial class LeaderboardScreen : Page
    { 
        private App app;
        private SnackbarMessageQueue MyMessageQueue;
        private Thread thr;

        public LeaderboardScreen()
        {
            InitializeComponent();
            app = ((App)Application.Current);
            app.communicator.forceRelease();
            MyMessageQueue = ((MainWindow)App.Current.MainWindow).MySnackbar.MessageQueue;
            
            
            thr = new Thread(new ThreadStart(this.updateLeaderboard));
            thr.IsBackground = true;
            thr.Start();
        }

        public void updateLeaderboard()
        {
            while (true)
            {
                GetResultsRequest getResultsRequest = new GetResultsRequest();

                ResponseInfo response = app.communicator.SocketSendReceive(JsonSerializer.serializeRequest(getResultsRequest, Constants.GET_RESULTS_REQUEST)).Result;

                GetResultsResponse getResultsResponse = JsonDeserializer.deserializeResponse<GetResultsResponse>(response.buffer);

                switch (getResultsResponse.status)
                {

                    case Constants.GET_RESULTS_SUCCESS:
                        this.Dispatcher.Invoke(() =>
                        {
                            UpdateLeaderboardUI(getResultsResponse.results);
                        });
                        break;

                }
 
                System.Threading.Thread.Sleep(100);

            }
        }
        private void UpdateLeaderboardUI(PlayerResult[] results)
        {
            bool found;
            for (int i = 0; i < results.Length; i++)
            {
                found = false;
                for(int j = 0;j< LeaderboardList.Items.Count; j++)
                {
                    PlayerResult temp = (PlayerResult)LeaderboardList.Items[j];
                    if (temp.username == results[i].username)
                    {
                        LeaderboardList.Items[j] = results[i];
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    LeaderboardList.Items.Add(results[i]);
                }
                
            }
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            thr.Abort();
            LeaveGameRequest leaveGameRequest = new LeaveGameRequest();

            app.communicator.SocketSendReceive(JsonSerializer.serializeRequest(leaveGameRequest, Constants.LEAVE_GAME_REQUEST)).ContinueWith(task =>
            {
                ResponseInfo response = task.Result;
                LeaveGameResponse leaveGameResponse = JsonDeserializer.deserializeResponse<LeaveGameResponse>(response.buffer);
                switch (leaveGameResponse.status)
                {
                    case Constants.LEAVE_GAME_SUCCESS:
                        
                        MyMessageQueue.Enqueue("You left the game successfuly.");
                        this.Dispatcher.Invoke(() =>
                        {
                            NavigationService ns = NavigationService.GetNavigationService(this);
                            ns.Navigate(new Uri("Menu.xaml", UriKind.Relative));
                        });

                        break;

                }

            });
        }
    }
}
