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
    /// Interaction logic for Signin.xaml
    /// </summary>
    public partial class Signin : Page
    {
        private App app;
        private SnackbarMessageQueue MyMessageQueue;
        public Signin()
        {
            InitializeComponent();
            app = ((App)Application.Current);
            app.communicator.forceRelease();
            MyMessageQueue = ((MainWindow)App.Current.MainWindow).MySnackbar.MessageQueue;
        }
        private void LoginBTN_Clicked(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndeterminate(btn, true);
            MaterialDesignThemes.Wpf.ButtonProgressAssist.SetValue(btn, -1);
            MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndicatorVisible(btn, true);
            btn.IsEnabled = false;
            SignupBTN.IsEnabled = false;
            bool EverythingFine = true;

            if (String.IsNullOrEmpty(UsernameBox.Text))
            {
                UsernameBox.GetBindingExpression(TextBox.TextProperty);
                ValidationError validationError = new ValidationError(new NotEmptyValidationRule(), UsernameBox.GetBindingExpression(TextBox.TextProperty));
                validationError.ErrorContent = "Field is required.";

                Validation.MarkInvalid(
                    UsernameBox.GetBindingExpression(TextBox.TextProperty),
                    validationError);
                EverythingFine = false;
            }
            else
            {
                Validation.ClearInvalid(UsernameBox.GetBindingExpression(TextBox.TextProperty));
            }
            if (String.IsNullOrEmpty(PasswordBox.Password))
            {
                PasswordBox.GetBindingExpression(TextBox.TextProperty);
                ValidationError validationError = new ValidationError(new NotEmptyValidationRule(), PasswordBox.GetBindingExpression(PasswordBox.DataContextProperty));
                validationError.ErrorContent = "Field is required.";

                Validation.MarkInvalid(
                    PasswordBox.GetBindingExpression(PasswordBox.DataContextProperty),
                    validationError);
                EverythingFine = false;
            }
            else if (PasswordBox.Password.Length < 1)
            {
                PasswordBox.GetBindingExpression(TextBox.TextProperty);
                ValidationError validationError = new ValidationError(new NotEmptyValidationRule(), PasswordBox.GetBindingExpression(PasswordBox.DataContextProperty));
                validationError.ErrorContent = "At least 8 characters.";

                Validation.MarkInvalid(
                    PasswordBox.GetBindingExpression(PasswordBox.DataContextProperty),
                    validationError);
                EverythingFine = false;
            }
            else
            {
                Validation.ClearInvalid(PasswordBox.GetBindingExpression(PasswordBox.DataContextProperty));
            }
            if (EverythingFine)
            {
                LoginRequest loginRequest = new LoginRequest();
                loginRequest.username = UsernameBox.Text;
                loginRequest.password = PasswordBox.Password;

                app.communicator.SocketSendReceive(JsonSerializer.serializeRequest(loginRequest, Constants.LOGIN_REQUEST_CODE)).ContinueWith(task =>
                {
                    ResponseInfo response = task.Result;
                    LoginResponse loginResponse = JsonDeserializer.deserializeResponse<LoginResponse>(response.buffer);
                    switch (loginResponse.status)
                    {
                        case Constants.LOGIN_SUCCESS:
                            MyMessageQueue.Enqueue("Sign in Successfully!");
                            this.Dispatcher.Invoke(() =>
                            {
                                app.username = UsernameBox.Text;
                                NavigationService ns = NavigationService.GetNavigationService(this);
                                ns.Navigate(new Uri("Menu.xaml", UriKind.Relative));
                            });
                            break;
                        case Constants.LOGIN_INCORRECT_PASSWORD:
                            MyMessageQueue.Enqueue("Incorrect password.");
                            break;
                        case Constants.LOGIN_USERNAME_NOT_EXIST:
                            MyMessageQueue.Enqueue("Username not exist.");
                            break;
                        case Constants.LOGIN_UNEXPECTED_ERR:
                            MyMessageQueue.Enqueue("There was an unexpected error.");
                            break;
                        case Constants.LOGIN_ALREADY_ONLINE:
                            MyMessageQueue.Enqueue("This Username is already online.");
                            break;
                    }
                    this.Dispatcher.Invoke(() =>
                    {
                        ButtonProgressAssist.SetIsIndeterminate(btn, false);
                        ButtonProgressAssist.SetIsIndicatorVisible(btn, false);
                        btn.IsEnabled = true;
                        SignupBTN.IsEnabled = true;
                    });
                });
                
                
            }
            else
            {
                MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndeterminate(btn, false);
                MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndicatorVisible(btn, false);
                btn.IsEnabled = true;
                SignupBTN.IsEnabled = true;
            }
            
        }
        private void SignupBTN_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new Uri("Signup.xaml", UriKind.Relative));
        }

        private void UsernameBox_TextChanged(object sender, TextChangedEventArgs e)

        {
            TextBox textBox = sender as TextBox;
            if (String.IsNullOrEmpty(textBox.Text))
            {
                textBox.GetBindingExpression(TextBox.TextProperty);
                ValidationError validationError = new ValidationError(new NotEmptyValidationRule(), textBox.GetBindingExpression(TextBox.TextProperty));
                validationError.ErrorContent = "Field is required.";

                Validation.MarkInvalid(
                    textBox.GetBindingExpression(TextBox.TextProperty),
                    validationError);
            }
            else
            {
                Validation.ClearInvalid(textBox.GetBindingExpression(TextBox.TextProperty));
            }


        }


        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox textBox = sender as PasswordBox;
            if (String.IsNullOrEmpty(textBox.Password))
            {
                textBox.GetBindingExpression(TextBox.TextProperty);
                ValidationError validationError = new ValidationError(new NotEmptyValidationRule(), textBox.GetBindingExpression(PasswordBox.DataContextProperty));
                validationError.ErrorContent = "Field is required.";

                Validation.MarkInvalid(
                    textBox.GetBindingExpression(PasswordBox.DataContextProperty),
                    validationError);
            }
            else if (textBox.Password.Length < 8)
            {
                textBox.GetBindingExpression(TextBox.TextProperty);
                ValidationError validationError = new ValidationError(new NotEmptyValidationRule(), textBox.GetBindingExpression(PasswordBox.DataContextProperty));
                validationError.ErrorContent = "At least 8 characters.";

                Validation.MarkInvalid(
                    textBox.GetBindingExpression(PasswordBox.DataContextProperty),
                    validationError);
            }
            else
            {
                Validation.ClearInvalid(textBox.GetBindingExpression(PasswordBox.DataContextProperty));
            }
        }
    }
}
