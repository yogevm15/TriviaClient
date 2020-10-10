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
    /// Interaction logic for BestScores.xaml
    /// </summary>
    public partial class BestScores : Page
    {
        private App app;
        private SnackbarMessageQueue MyMessageQueue;
        private Thread thr;
        public BestScores()
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
                GetTopStatisticsRequest getTopStatisticsRequest = new GetTopStatisticsRequest();

                ResponseInfo response = app.communicator.SocketSendReceive(JsonSerializer.serializeRequest(getTopStatisticsRequest, Constants.GET_TOP_STATISTICS_REQUEST)).Result;

                GetTopStatisticsResponse getTopStatisticsResponse = JsonDeserializer.deserializeResponse<GetTopStatisticsResponse>(response.buffer);

                switch (getTopStatisticsResponse.status)
                {

                    case Constants.GET_TOP_STATISTICS_SUCCESS:
                        this.Dispatcher.Invoke(() =>
                        {
                            UpdateLeaderboardUI(getTopStatisticsResponse.topPlayerStatistics);
                        });
                        break;

                }

                System.Threading.Thread.Sleep(100);

            }
        }
        private void UpdateLeaderboardUI(PlayerStatistics[] results)
        {
            
            bool found;
            for (int i = 0; i < LeaderboardList.Items.Count; i++)
            {
                found = false;
                for (int j = 0; j < results.Length; j++)
                {
                    if (results[j].username == ((PlayerStatistics)LeaderboardList.Items[i]).username)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    LeaderboardList.Items.RemoveAt(i);
                }

            }
            
            for (int i = 0; i < results.Length; i++)
            {
                found = false;
                for (int j = 0; j < LeaderboardList.Items.Count; j++)
                {
                    if (((PlayerStatistics)LeaderboardList.Items[j]).username == results[i].username)
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
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.GoBack();
        }
    }
}
