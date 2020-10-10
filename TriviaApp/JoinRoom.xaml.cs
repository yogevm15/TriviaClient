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
    /// Interaction logic for JoinRoom.xaml
    /// </summary>
    public partial class JoinRoom : Page
    {
        private App app;
        private SnackbarMessageQueue MyMessageQueue;
        private Thread thr;
        public JoinRoom()
        {
            InitializeComponent();
            app = ((App)Application.Current);
            app.communicator.forceRelease();
            MyMessageQueue = ((MainWindow)App.Current.MainWindow).MySnackbar.MessageQueue;
            thr = new Thread(new ThreadStart(this.updateRooms));
            thr.IsBackground = true;
            thr.Start();


        }

        private void removeRooms(int[] ids)
        {
            for (int i = 0; i < RoomsList.Items.Count; i++)
            {
                GetRoomDetailsResponse temp = (GetRoomDetailsResponse)RoomsList.Items[i];
                if (!ids.Contains(temp.id))
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        RoomsList.Items.RemoveAt(i);
                    });
                }

            }
        }
        private void addOrUpdateRoom(GetRoomDetailsResponse getRoomDetailsResponse)
        {
            getRoomDetailsResponse.playersAmount = getRoomDetailsResponse.players.Length.ToString() + "/" + getRoomDetailsResponse.maxPlayers.ToString();
            for(int i = 0;i< RoomsList.Items.Count; i++)
            {
                GetRoomDetailsResponse temp =(GetRoomDetailsResponse) RoomsList.Items[i] ;
                if (temp.id== getRoomDetailsResponse.id)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        RoomsList.Items[i] = getRoomDetailsResponse;
                    });
                    return;
                }
                    
            }
            this.Dispatcher.Invoke(() =>
            {
                RoomsList.Items.Add(getRoomDetailsResponse);
            });
        }
        private void updateRooms()
        {
            while (true)
            {
                GetRoomsIdRequest getRoomsIdRequest = new GetRoomsIdRequest();
                ResponseInfo response = app.communicator.SocketSendReceive(JsonSerializer.serializeRequest(getRoomsIdRequest, Constants.GET_ROOMS_ID_REQUEST_CODE)).Result;
                GetRoomsIdResponse getRoomsIdResponse = JsonDeserializer.deserializeResponse<GetRoomsIdResponse>(response.buffer);
                removeRooms(getRoomsIdResponse.ids);
                for (int i = 0; i < getRoomsIdResponse.ids.Length; i++)
                {
                    GetRoomDetailsRequest getRoomDetailsRequest = new GetRoomDetailsRequest();
                    getRoomDetailsRequest.roomId = getRoomsIdResponse.ids[i];
                    ResponseInfo response1 = app.communicator.SocketSendReceive(JsonSerializer.serializeRequest(getRoomDetailsRequest, Constants.GET_ROOM_DETAILS_REQUEST)).Result;
                    GetRoomDetailsResponse getRoomDetailsResponse = JsonDeserializer.deserializeResponse<GetRoomDetailsResponse>(response1.buffer);
                    addOrUpdateRoom(getRoomDetailsResponse);
                }
                System.Threading.Thread.Sleep(500);
            }
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            thr.Abort();
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.GoBack();
        }

        private void joinRoom(int id)
        {
            JoinRoomRequest joinRoomRequest = new JoinRoomRequest();
            joinRoomRequest.roomId = id;
            app.communicator.SocketSendReceive(JsonSerializer.serializeRequest(joinRoomRequest, Constants.JOIN_ROOM_REQUEST)).ContinueWith(task =>
            {
                ResponseInfo response = task.Result;
                JoinRoomResponse joinRoomResponse = JsonDeserializer.deserializeResponse<JoinRoomResponse>(response.buffer);
                switch (joinRoomResponse.status)
                {
                    case Constants.JOIN_ROOM_SUCCESS:
                        MyMessageQueue.Enqueue("Joined Successfully");
                        this.Dispatcher.Invoke(() =>
                        {
                            thr.Abort();
                            app.admin = false;
                            NavigationService ns = NavigationService.GetNavigationService(this);
                            ns.Navigate(new Uri("RoomLobby.xaml", UriKind.Relative));
                        });
                        break;
                    case Constants.JOIN_ROOM_NOT_EXIST:
                        MyMessageQueue.Enqueue("Room not exist any more.");
                        break;
                    case Constants.JOIN_ROOM_MAXIMUM_USERS_IN_ROOM:
                        MyMessageQueue.Enqueue("Room is full.");
                        break;
                }

            });
        }

        protected void ItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            GetRoomDetailsResponse getRoomDetailsResponse = (GetRoomDetailsResponse)((ListViewItem)sender).Content;
            joinRoom(getRoomDetailsResponse.id);

        }

        private void JoinBTN_Click(object sender, RoutedEventArgs e)
        {
            GetRoomDetailsResponse getRoomDetailsResponse = (GetRoomDetailsResponse)((Button)sender).DataContext;
            joinRoom(getRoomDetailsResponse.id);
        }
    }
}
