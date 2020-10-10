using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for RoomLobby.xaml
    /// </summary>
    public partial class RoomLobby : Page
    {
        private App app;
        private SnackbarMessageQueue MyMessageQueue;
        private bool edited = false;
        private Thread thr;
        public RoomLobby()
        {
            InitializeComponent();

            app = ((App)Application.Current);
            app.communicator.forceRelease();
            MyMessageQueue = ((MainWindow)App.Current.MainWindow).MySnackbar.MessageQueue;
            if (!app.admin)
            {
                disableAllButtonsAndText();
            }
            thr = new Thread(new ThreadStart(this.updateRoom));
            thr.IsBackground = true;
            thr.Start();
           

        }
        private void updateRoomTexts(GetRoomDetailsResponse getRoomDetailsResponse)
        {
            this.Dispatcher.Invoke(() =>
            {
                RoomNameBox.Text = getRoomDetailsResponse.name;
                MaxPlayersBox.Text = getRoomDetailsResponse.maxPlayers.ToString();
                SecondsBox.Text = getRoomDetailsResponse.secondsForQuestions.ToString();
                QuestionsBox.Text = getRoomDetailsResponse.questionsCount.ToString();
            });
        }
        private void updateRoomUI(GetRoomDetailsResponse getRoomDetailsResponse)
        {
            this.Dispatcher.Invoke(() =>
            {
                for (int i = 0; i < PlayersList.Items.Count; i++)
                {
                    if (!getRoomDetailsResponse.players.Contains(PlayersList.Items[i]))
                    {
                        PlayersList.Items.RemoveAt(i);
                    }

                }
                for (int i = 0; i < getRoomDetailsResponse.players.Length; i++)
                {
                    if (!PlayersList.Items.Contains(getRoomDetailsResponse.players[i]))
                    {
                        string name = getRoomDetailsResponse.players[i];
                        PlayersList.Items.Add(name);
                    }
                }
                if (!edited)
                {
                    RoomNameBox.Text = getRoomDetailsResponse.name;
                    MaxPlayersBox.Text = getRoomDetailsResponse.maxPlayers.ToString();
                    SecondsBox.Text = getRoomDetailsResponse.secondsForQuestions.ToString();
                    QuestionsBox.Text = getRoomDetailsResponse.questionsCount.ToString();
                }
            });
        }

        private void updateRoom()
        {
            while (true)
            {
                GetRoomDetailsRequest getRoomDetailsRequest = new GetRoomDetailsRequest();
                getRoomDetailsRequest.roomId = 0;
                ResponseInfo response = app.communicator.SocketSendReceive(JsonSerializer.serializeRequest(getRoomDetailsRequest, Constants.GET_ROOM_DETAILS_REQUEST)).Result;
                GetRoomDetailsResponse getRoomDetailsResponse = JsonDeserializer.deserializeResponse<GetRoomDetailsResponse>(response.buffer);
                
                switch (getRoomDetailsResponse.status)
                {
                    
                    case Constants.GET_ROOM_DETAILS_SUCCESS:
                        if (getRoomDetailsResponse.isActive == true)
                        {
                            
                            MyMessageQueue.Enqueue("The admin started the game.");
                            this.Dispatcher.Invoke(() =>
                            {
                                NavigationService ns = NavigationService.GetNavigationService(this);
                                ns.Navigate(new Uri("QuestionScreen.xaml", UriKind.Relative));
                            });
                            thr.Abort();
                            break;
                        }
                        else if(!app.admin&&getRoomDetailsResponse.admin == app.username)
                        {
                            MyMessageQueue.Enqueue("You are admin now.");
                            app.admin = true;
                            NewAdminRequest newAdminRequest = new NewAdminRequest();

                            ResponseInfo response1 = app.communicator.SocketSendReceive(JsonSerializer.serializeRequest(newAdminRequest, Constants.NEW_ADMIN_REQUEST)).Result;
                            
                                
                            NewAdminResponse newAdminResponse = JsonDeserializer.deserializeResponse<NewAdminResponse>(response.buffer);
                            switch (newAdminResponse.status)
                            {
                                case Constants.NEW_ADMIN_SUCCESS:
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        NavigationService ns = NavigationService.GetNavigationService(this);
                                        ns.Refresh();
                                    });
                                    thr.Abort();
                                    break;

                            }

                        }
                        else
                        {
                            updateRoomUI(getRoomDetailsResponse);
                        }
                        
                        break;
                    case Constants.GET_ROOM_DETAILS_NOT_EXIST:
                        
                        MyMessageQueue.Enqueue("The admin closed the room.");
                        this.Dispatcher.Invoke(() =>
                        {
                            NavigationService ns = NavigationService.GetNavigationService(this);
                            ns.Navigate(new Uri("Menu.xaml", UriKind.Relative));
                        });
                        thr.Abort();
                        break;

                }
                System.Threading.Thread.Sleep(100);

            }
                
            
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            edited = true;
            Regex regex = new Regex("[^0-9]+");
            bool t = regex.IsMatch(e.Text);
            bool t1 = (sender as TextBox).Text.Length < 3;
            bool final = !t && t1;
            e.Handled = !final;
        }
        private void NoValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            edited = true;
            e.Handled = false;
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
                else if (count < 2)
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
                else if (count < 10)
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
        private void SaveBTN_Click(object sender, RoutedEventArgs e)
        {
            disableAllButtonsAndText();
            startLoadingAnim();
            bool allFine = true;
            if (String.IsNullOrEmpty(RoomNameBox.Text))
            {
                setError(RoomNameBox, "Field is required.");
                allFine = false;

            }
            else
            {
                Validation.ClearInvalid(RoomNameBox.GetBindingExpression(TextBox.TextProperty));
            }
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
            
            if (allFine)
            {
                EditRoomRequest editRoomRequest = new EditRoomRequest();
                editRoomRequest.answerTimeout = int.Parse(SecondsBox.Text);
                editRoomRequest.questionCount = int.Parse(QuestionsBox.Text);
                editRoomRequest.maxUsers = int.Parse(MaxPlayersBox.Text);
                editRoomRequest.roomName = RoomNameBox.Text;
                app.communicator.SocketSendReceive(JsonSerializer.serializeRequest(editRoomRequest, Constants.EDIT_ROOM_REQUEST)).ContinueWith(task =>
                {
                    ResponseInfo response = task.Result;
                    EditRoomResponse editRoomResponse = JsonDeserializer.deserializeResponse<EditRoomResponse>(response.buffer);
                    switch (editRoomResponse.status)
                    {
                        case Constants.EDIT_ROOM_SUCCESS:
                            GetRoomDetailsRequest getRoomDetailsRequest = new GetRoomDetailsRequest();
                            getRoomDetailsRequest.roomId = 0;
                            app.communicator.SocketSendReceive(JsonSerializer.serializeRequest(getRoomDetailsRequest, Constants.GET_ROOM_DETAILS_REQUEST)).ContinueWith(task1 =>
                            {
                                ResponseInfo response1 = task1.Result;
                                GetRoomDetailsResponse getRoomDetailsResponse = JsonDeserializer.deserializeResponse<GetRoomDetailsResponse>(response1.buffer);
                                updateRoomTexts(getRoomDetailsResponse);
                                MyMessageQueue.Enqueue("Saved successfully!");
                                enableAllButtonsAndText();
                                stopLoadingAnim();

                            });
                            break;
                        case Constants.EDIT_ROOM_NOT_EXIST:
                            thr.Abort();
                            MyMessageQueue.Enqueue("Room not exist anymore.");
                            this.Dispatcher.Invoke(() =>
                            {
                                NavigationService ns = NavigationService.GetNavigationService(this);
                                ns.Navigate(new Uri("Menu.xaml", UriKind.Relative));
                            });
                            break;
                    }

                });
            }
            else
            {
                enableAllButtonsAndText();
                stopLoadingAnim();
            }
            
        }
        private void enableAllButtonsAndText()
        {
            this.Dispatcher.Invoke(() =>
            {
                SecondsBox.IsEnabled = true;
                QuestionsBox.IsEnabled = true;
                MaxPlayersBox.IsEnabled = true;
                RoomNameBox.IsEnabled = true;
                SaveBTN.IsEnabled = true;
                StartBTN.IsEnabled = true;
            });
        }
        private void disableAllButtonsAndText()
        {
            this.Dispatcher.Invoke(() =>
            {
                SecondsBox.IsEnabled = false;
                QuestionsBox.IsEnabled = false;
                MaxPlayersBox.IsEnabled = false;
                RoomNameBox.IsEnabled = false;
                SaveBTN.IsEnabled = false;
                StartBTN.IsEnabled = false;
            });
        }
        private void startLoadingAnim()
        {
            this.Dispatcher.Invoke(() =>
            {
                MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndeterminate(SaveBTN, true);
                MaterialDesignThemes.Wpf.ButtonProgressAssist.SetValue(SaveBTN, -1);
                MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndicatorVisible(SaveBTN, true);
                MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndeterminate(StartBTN, true);
                MaterialDesignThemes.Wpf.ButtonProgressAssist.SetValue(StartBTN, -1);
                MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndicatorVisible(StartBTN, true);
            });
        }
        private void stopLoadingAnim()
        {
            this.Dispatcher.Invoke(() =>
            {
                MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndeterminate(SaveBTN, false);
                MaterialDesignThemes.Wpf.ButtonProgressAssist.SetValue(SaveBTN, -1);
                MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndicatorVisible(SaveBTN, false);
                MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndeterminate(StartBTN, false);
                MaterialDesignThemes.Wpf.ButtonProgressAssist.SetValue(StartBTN, -1);
                MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndicatorVisible(StartBTN, false);
            });
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            
            if (app.admin)
            {
                CloseRoomRequest closeRoomRequest = new CloseRoomRequest();

                app.communicator.SocketSendReceive(JsonSerializer.serializeRequest(closeRoomRequest, Constants.CLOSE_ROOM_REQUEST)).ContinueWith(task =>
                {
                    ResponseInfo response = task.Result;
                    CloseRoomResponse closeRoomResponse = JsonDeserializer.deserializeResponse<CloseRoomResponse>(response.buffer);
                    switch (closeRoomResponse.status)
                    {
                        case Constants.CLOSE_ROOM_SUCCESS:
                            thr.Abort();
                            MyMessageQueue.Enqueue("You leaved the room successfuly.");
                            this.Dispatcher.Invoke(() =>
                            {
                                NavigationService ns = NavigationService.GetNavigationService(this);
                                ns.Navigate(new Uri("Menu.xaml", UriKind.Relative));
                            });

                            break;
                        
                    }

                });
            }
            else
            {
                LeaveRoomRequest leaveRoomRequest = new LeaveRoomRequest();

                app.communicator.SocketSendReceive(JsonSerializer.serializeRequest(leaveRoomRequest, Constants.LEAVE_ROOM_REQUEST)).ContinueWith(task =>
                {
                    ResponseInfo response = task.Result;
                    LeaveRoomResponse leaveRoomResponse = JsonDeserializer.deserializeResponse<LeaveRoomResponse>(response.buffer);
                    switch (leaveRoomResponse.status)
                    {
                        case Constants.LEAVE_ROOM_SUCCESS:
                            thr.Abort();
                            MyMessageQueue.Enqueue("You left the room successfuly.");
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

        private void StartBTN_Click(object sender, RoutedEventArgs e)
        {
            

            app.communicator.SocketSendReceive(JsonSerializer.serializeRequest(new StartGameRequest(), Constants.START_GAME_REQUEST)).ContinueWith(task =>
            {
                ResponseInfo response = task.Result;
                StartGameResponse startGameResponse = JsonDeserializer.deserializeResponse<StartGameResponse>(response.buffer);
                switch (startGameResponse.status)
                {
                    case Constants.START_GAME_SUCCESS:
                        thr.Abort();
                        MyMessageQueue.Enqueue("Room started successfuly.");
                        this.Dispatcher.Invoke(() =>
                        {
                            NavigationService ns = NavigationService.GetNavigationService(this);
                            ns.Navigate(new Uri("QuestionScreen.xaml", UriKind.Relative));
                        });
                        break;
                    case Constants.START_GAME_NOT_EXIST:
                        thr.Abort();
                        MyMessageQueue.Enqueue("Room not exist anymore.");
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
