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
    /// Interaction logic for MyStats.xaml
    /// </summary>
    public partial class MyStats : Page
    {
        private App app;
        private SnackbarMessageQueue MyMessageQueue;
        public MyStats()
        {
            InitializeComponent();
            app = ((App)Application.Current);
            app.communicator.forceRelease();
            MyMessageQueue = ((MainWindow)App.Current.MainWindow).MySnackbar.MessageQueue;
            getStats();
        }

        public void getStats()
        {
            GetMyStatisticsRequest getMyStatisticsRequest = new GetMyStatisticsRequest();


            app.communicator.SocketSendReceive(JsonSerializer.serializeRequest(getMyStatisticsRequest, Constants.GET_MY_STATISTICS_REQUEST)).ContinueWith(task =>
            {
                ResponseInfo response = task.Result;
                GetMyStatisticsResponse getMyStatisticsResponse = JsonDeserializer.deserializeResponse<GetMyStatisticsResponse>(response.buffer);
                switch (getMyStatisticsResponse.status)
                {
                    case Constants.GET_MY_STATISTICS_SUCCESS:
                        MyMessageQueue.Enqueue("Load Stats Successfully!");
                        this.Dispatcher.Invoke(() =>
                        {
                            loadStatsToUI(getMyStatisticsResponse);
                        });
                        break;

                }
            });
        }

        private void loadStatsToUI(GetMyStatisticsResponse getMyStatisticsResponse)
        {
            TotalGamesTXT.Text = "Total games you played: " + getMyStatisticsResponse.gamesPlayed;
            AverageTimeTXT.Text = "Average time for answer: " + getMyStatisticsResponse.averageAnswerTime;
            CorrectAnsTXT.Text = "Correct Answers: " + getMyStatisticsResponse.correctAnswers;
            WrongAnsTXT.Text = "Wrong Answers: " + getMyStatisticsResponse.wrongAnswers;
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.GoBack();
        }
    }
}
