using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for CreateRoom.xaml
    /// </summary>
    public partial class CreateRoom : Page
    {
        private App app;
        private SnackbarMessageQueue MyMessageQueue;

        public CreateRoom()
        {
            InitializeComponent();
            app = ((App)Application.Current);
            app.communicator.forceRelease();
            MyMessageQueue = ((MainWindow)App.Current.MainWindow).MySnackbar.MessageQueue;
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            bool t = regex.IsMatch(e.Text);
            bool t1 = (sender as TextBox).Text.Length < 3;
            bool final = !t && t1;
            e.Handled = !final;
        }

        private void MaxPlayersBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MaxPlayersBox.Text = new String(MaxPlayersBox.Text.Where(Char.IsDigit).ToArray());
            if (MaxPlayersBox.Text.Length > 3)
            {
                MaxPlayersBox.Text = MaxPlayersBox.Text.Substring(0, 3);
            }
            if (String.IsNullOrEmpty(MaxPlayersBox.Text))
            {
                setError(MaxPlayersBox, "Field is required.");
            }
            else
            {
                int count = int.Parse(MaxPlayersBox.Text);
                if (count > 15)
                {
                    setError(MaxPlayersBox, "Can't be more than 15 players.");
                }
                else if (count < 2)
                {
                    setError(MaxPlayersBox, "Can't be less than 2 players.");
                }
                else
                {
                    Validation.ClearInvalid(MaxPlayersBox.GetBindingExpression(TextBox.TextProperty));
                }
            }
        }

        private void QuestionsBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            QuestionsBox.Text = new String(QuestionsBox.Text.Where(Char.IsDigit).ToArray());
            if (QuestionsBox.Text.Length > 3)
            {
                QuestionsBox.Text = QuestionsBox.Text.Substring(0, 3);
            }
            if (String.IsNullOrEmpty(QuestionsBox.Text))
            {
                setError(QuestionsBox, "Field is required.");
            }
            else
            {
                int count = int.Parse(QuestionsBox.Text);
                if (count > 20)
                {
                    setError(QuestionsBox, "Can't be more than 20 questions.");
                }
                else if(count < 2)
                {
                    setError(QuestionsBox, "Can't be less than 2 questions.");
                }
                else
                {
                    Validation.ClearInvalid(QuestionsBox.GetBindingExpression(TextBox.TextProperty));
                }
            }
         

        }

        private void SecondsBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SecondsBox.Text = new String(SecondsBox.Text.Where(Char.IsDigit).ToArray());
            if (SecondsBox.Text.Length > 3)
            {
                SecondsBox.Text = SecondsBox.Text.Substring(0, 3);
            }
            if (String.IsNullOrEmpty(SecondsBox.Text))
            {
                setError(SecondsBox, "Field is required.");
            }
            else
            {
                int count = int.Parse(SecondsBox.Text);
                if (count > 100)
                {
                    setError(SecondsBox, "Can't be more than 100 seconds.");
                }
                else if(count < 10)
                {
                    setError(SecondsBox, "Can't be less than 10 seconds.");
                }
                else
                {
                    Validation.ClearInvalid(SecondsBox.GetBindingExpression(TextBox.TextProperty));
                }
            }
        }

        private void setError(TextBox box, string err)
        {
            box.GetBindingExpression(TextBox.TextProperty);
            ValidationError validationError = new ValidationError(new NotEmptyValidationRule(), box.GetBindingExpression(TextBox.TextProperty));
            validationError.ErrorContent = err;

            Validation.MarkInvalid(
                box.GetBindingExpression(TextBox.TextProperty),
                validationError);
        }

        private void CreateBTN_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndeterminate(btn, true);
            MaterialDesignThemes.Wpf.ButtonProgressAssist.SetValue(btn, -1);
            MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndicatorVisible(btn, true);
            btn.IsEnabled = false;
            bool allFine = true;
            MaxPlayersBox.Text = new String(MaxPlayersBox.Text.Where(Char.IsDigit).ToArray());
            if (MaxPlayersBox.Text.Length > 3)
            {
                MaxPlayersBox.Text = MaxPlayersBox.Text.Substring(0, 3);
            }
            if (String.IsNullOrEmpty(MaxPlayersBox.Text))
            {
                setError(MaxPlayersBox, "Field is required.");
                allFine = false;
            }
            else
            {
                int count = int.Parse(MaxPlayersBox.Text);
                if (count > 15)
                {
                    setError(MaxPlayersBox, "Can't be more than 15 players.");
                    allFine = false;
                }
                else if (count < 2)
                {
                    setError(MaxPlayersBox, "Can't be less than 2 players.");
                    allFine = false;
                }
                else
                {
                    Validation.ClearInvalid(MaxPlayersBox.GetBindingExpression(TextBox.TextProperty));
                }
            }
            QuestionsBox.Text = new String(QuestionsBox.Text.Where(Char.IsDigit).ToArray());
            if (QuestionsBox.Text.Length > 3)
            {
                QuestionsBox.Text = QuestionsBox.Text.Substring(0, 3);
            }
            if (String.IsNullOrEmpty(QuestionsBox.Text))
            {
                setError(QuestionsBox, "Field is required.");
                allFine = false;
            }
            else
            {
                int count = int.Parse(QuestionsBox.Text);
                if (count > 20)
                {
                    setError(QuestionsBox, "Can't be more than 20 questions.");
                    allFine = false;
                }
                else if (count < 2)
                {
                    setError(QuestionsBox, "Can't be less than 2 questions.");
                    allFine = false;
                }
                else
                {
                    Validation.ClearInvalid(QuestionsBox.GetBindingExpression(TextBox.TextProperty));
                }
            }
            SecondsBox.Text = new String(SecondsBox.Text.Where(Char.IsDigit).ToArray());
            if (SecondsBox.Text.Length > 3)
            {
                SecondsBox.Text = SecondsBox.Text.Substring(0, 3);
            }
            if (String.IsNullOrEmpty(SecondsBox.Text))
            {
                setError(SecondsBox, "Field is required.");
                allFine = false;
            }
            else
            {
                int count = int.Parse(SecondsBox.Text);
                if (count > 100)
                {
                    setError(SecondsBox, "Can't be more than 100 seconds.");
                    allFine = false;
                }
                else if (count < 10)
                {
                    setError(SecondsBox, "Can't be less than 10 seconds.");
                    allFine = false;
                }
                else
                {
                    Validation.ClearInvalid(SecondsBox.GetBindingExpression(TextBox.TextProperty));
                }
            }
            if (String.IsNullOrEmpty(RoomNameBox.Text))
            {
                setError(RoomNameBox, "Field is required.");
                allFine = false;
            }
            else
            {
                Validation.ClearInvalid(RoomNameBox.GetBindingExpression(TextBox.TextProperty));
            }
            if (allFine)
            {
                CreateRoomRequest createRoomRequest = new CreateRoomRequest();
                createRoomRequest.answerTimeout = int.Parse(SecondsBox.Text);
                createRoomRequest.maxUsers = int.Parse(MaxPlayersBox.Text);
                createRoomRequest.questionCount = int.Parse(QuestionsBox.Text);
                createRoomRequest.roomName = RoomNameBox.Text;
                app.communicator.SocketSendReceive(JsonSerializer.serializeRequest(createRoomRequest, Constants.CREATE_ROOM_REQUEST_CODE)).ContinueWith(task =>
                {
                    ResponseInfo response = task.Result;
                    CreateRoomResponse createRoomResponse = JsonDeserializer.deserializeResponse<CreateRoomResponse>(response.buffer);
                    switch (createRoomResponse.status)
                    {
                        case Constants.CREATE_ROOM_SUCCESS:
                            MyMessageQueue.Enqueue("Room Created Successfully!");
                            this.Dispatcher.Invoke(() =>
                            {
                                app.admin = true;
                                NavigationService ns = NavigationService.GetNavigationService(this);
                                ns.Navigate(new Uri("RoomLobby.xaml", UriKind.Relative));
                            });
                            break;
                        case Constants.CREATE_ROOM_MAXIMUM_ROOMS_IN_SERVER:
                            MyMessageQueue.Enqueue("Max rooms reached.\nTry again later.");
                            break;
                    }
                    this.Dispatcher.Invoke(() =>
                    {
                        ButtonProgressAssist.SetIsIndeterminate(btn, false);
                        ButtonProgressAssist.SetIsIndicatorVisible(btn, false);
                        btn.IsEnabled = true;

                    });
                });
            }
            else
            {
                ButtonProgressAssist.SetIsIndeterminate(btn, false);
                ButtonProgressAssist.SetIsIndicatorVisible(btn, false);
                btn.IsEnabled = true;
            }
        }

        private void RoomNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
            if (String.IsNullOrEmpty(RoomNameBox.Text))
            {
                setError(RoomNameBox, "Field is required.");

            }
            else
            {
                Validation.ClearInvalid(RoomNameBox.GetBindingExpression(TextBox.TextProperty));
            }
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.GoBack();
        }
    }
}
