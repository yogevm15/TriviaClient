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
    /// Interaction logic for QuestionScreen.xaml
    /// </summary>
    public partial class QuestionScreen : Page
    {
        private int questionTime;
        private App app;
        private SnackbarMessageQueue MyMessageQueue;
        private Thread thr;
        public int score = 0;
        public QuestionScreen()
        {
            InitializeComponent();
            app = ((App)Application.Current);
            app.communicator.forceRelease();
            MyMessageQueue = ((MainWindow)App.Current.MainWindow).MySnackbar.MessageQueue;
            ScoreTXT.Text = score.ToString();
            disableAllButtons();
            loadQuestion();
        }
        private void loadQuestion()
        {
            GetQuestionRequest getQuestionRequest = new GetQuestionRequest();

            app.communicator.SocketSendReceive(JsonSerializer.serializeRequest(getQuestionRequest, Constants.GET_QUESTION_REQUEST)).ContinueWith(task =>
            {
                ResponseInfo response = task.Result;
                GetQuestionResponse getQuestionResponse = JsonDeserializer.deserializeResponse<GetQuestionResponse>(response.buffer);

                switch (getQuestionResponse.status)
                {

                    case Constants.GET_QUESTION_SUCCESS:

                        this.Dispatcher.Invoke(() =>
                        {
                            
                            LoadQuestionToUI(getQuestionResponse);
                        });
                        break;
                    case Constants.GET_QUESTION_FINISHED:
                        thr.Abort();
                        MyMessageQueue.Enqueue("You finished!");
                        this.Dispatcher.Invoke(() =>
                        {
                            NavigationService ns = NavigationService.GetNavigationService(this);
                            ns.Navigate(new Uri("LeaderboardScreen.xaml", UriKind.Relative));
                        });
                        break;

                }
            });
                
        }
        private void changeStyleToRed()
        {
            SolidColorBrush red = Brushes.Red;
            app.Resources["PrimaryHueDarkBrush"] = red;
        }
        private void changeStyleToGreen()
        {
            SolidColorBrush green = Brushes.Green;
            app.Resources["PrimaryHueDarkBrush"] = green;
        }
        private void changeStyleToOriginal()
        {
            SolidColorBrush dark = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#320b86"));
            app.Resources["PrimaryHueDarkBrush"] = dark;
            SolidColorBrush light = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9a67ea"));
            app.Resources["PrimaryHueLightBrush"] = light;
            Answer1BTN.Style = (Style)app.Resources["MaterialDesignRaisedLightButton"];
            Answer2BTN.Style = (Style)app.Resources["MaterialDesignRaisedLightButton"];
            Answer3BTN.Style = (Style)app.Resources["MaterialDesignRaisedLightButton"];
            Answer4BTN.Style = (Style)app.Resources["MaterialDesignRaisedLightButton"];
        }
        private void showAnswers(int correct,int answer)
        {
            if (answer == correct)
            {
                changeStyleToGreen();
            }
            else
            {
                changeStyleToRed();
            }
            
            
            switch (answer)
            {
                case 1:
                    Answer1BTN.Style = (Style)app.Resources["MaterialDesignRaisedDarkButton"];
                    break;
                case 2:
                    Answer2BTN.Style = (Style)app.Resources["MaterialDesignRaisedDarkButton"];
                    break;
                case 3:
                    Answer3BTN.Style = (Style)app.Resources["MaterialDesignRaisedDarkButton"];
                    break;
                case 4:
                    Answer4BTN.Style = (Style)app.Resources["MaterialDesignRaisedDarkButton"];
                    break;
            }
            
            wait(3).ContinueWith(task =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    changeStyleToOriginal();
                    loadQuestion();
                });
            });
            
        }
        private void LoadQuestionToUI(GetQuestionResponse response)
        {
            questionTime = response.questionTime;
            QuestionsCountTXT.Text = response.currentQuestion + "/" + response.totalQuestions;
            Answer1BTN.Content = response.answers[0];
            Answer2BTN.Content = response.answers[1];
            Answer3BTN.Content = response.answers[2];
            Answer4BTN.Content = response.answers[3];
            questionTXT.Text = response.question;
            thr = new Thread(new ThreadStart(this.timerThread));
            thr.IsBackground = true;
            thr.Start();
            enableAllButtons();
        }

        public void timerThread()
        {
            for(int i = questionTime; i >= 0; i--)
            {
                this.Dispatcher.Invoke(() =>
                {
                    TimeLeftTXT.Text = i.ToString();
                });
                System.Threading.Thread.Sleep(1000);
            }
            this.Dispatcher.Invoke(() =>
            {
                submitAnswer(999);
            });
        }

        private void submitAnswer(int answer)
        {
            thr.Abort();
            disableAllButtons();
            SubmitQuestionRequest submitQuestionRequest = new SubmitQuestionRequest();
            submitQuestionRequest.answerId = answer;
            app.communicator.SocketSendReceive(JsonSerializer.serializeRequest(submitQuestionRequest, Constants.SUBMIT_ANSWER_REQUEST)).ContinueWith(task =>
            {
                ResponseInfo response = task.Result;
                SubmitQuestionResponse submitQuestionResponse = JsonDeserializer.deserializeResponse<SubmitQuestionResponse>(response.buffer);

                switch (submitQuestionResponse.status)
                {

                    case Constants.SUBMIT_ANSWER_SUCCESS:
                        if (answer == submitQuestionResponse.correctAnswerId)
                        {
                            score++;
                        }
                        this.Dispatcher.Invoke(() =>
                        {
                            ScoreTXT.Text = score.ToString();
                            showAnswers(submitQuestionResponse.correctAnswerId,answer);
                            
                        });
                        break;
                   

                }
            });
        }
        public async Task<int> wait(int secondToWait)
        {

            Task<int> t = Task.Run(() =>
            {
                // Send request to the server.
                Task.Delay(secondToWait*1000).Wait();
                return 0;

            });
            return await t;



        }
        private void disableAllButtons()
        {
            Answer1BTN.IsHitTestVisible = false;
            Answer2BTN.IsHitTestVisible = false;
            Answer3BTN.IsHitTestVisible = false;
            Answer4BTN.IsHitTestVisible = false;
        }
        private void enableAllButtons()
        {
            Answer1BTN.IsHitTestVisible = true;
            Answer2BTN.IsHitTestVisible = true;
            Answer3BTN.IsHitTestVisible = true;
            Answer4BTN.IsHitTestVisible = true;
        }
        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            LeaveGameRequest leaveGameRequest = new LeaveGameRequest();

            app.communicator.SocketSendReceive(JsonSerializer.serializeRequest(leaveGameRequest, Constants.LEAVE_GAME_REQUEST)).ContinueWith(task =>
            {
                ResponseInfo response = task.Result;
                LeaveGameResponse leaveGameResponse = JsonDeserializer.deserializeResponse<LeaveGameResponse>(response.buffer);
                switch (leaveGameResponse.status)
                {
                    case Constants.LEAVE_GAME_SUCCESS:
                        thr.Abort();
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

        private void Answer1BTN_Click(object sender, RoutedEventArgs e)
        {
            submitAnswer(1);
        }

        private void Answer2BTN_Click(object sender, RoutedEventArgs e)
        {
            submitAnswer(2);
        }

        private void Answer3BTN_Click(object sender, RoutedEventArgs e)
        {
            submitAnswer(3);
        }

        private void Answer4BTN_Click(object sender, RoutedEventArgs e)
        {
            submitAnswer(4);
        }


    }
}
