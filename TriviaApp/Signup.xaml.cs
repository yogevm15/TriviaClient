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
using System.Text.RegularExpressions;
using MaterialDesignThemes.Wpf;

namespace TriviaApp
{
    /// <summary>
    /// Interaction logic for Signup.xaml
    /// </summary>
    public partial class Signup : Page
    {
        private App app;
        private SnackbarMessageQueue MyMessageQueue;
        public Signup()
        {
            InitializeComponent();
            app = ((App)Application.Current);
            app.communicator.forceRelease();
            MyMessageQueue = ((MainWindow)App.Current.MainWindow).MySnackbar.MessageQueue;
        }

        private void EmailBox_TextChanged(object sender, TextChangedEventArgs e)
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
            else if (!IsValidEmail(EmailBox.Text))
            {
                EmailBox.GetBindingExpression(TextBox.TextProperty);
                ValidationError validationError = new ValidationError(new NotEmptyValidationRule(), EmailBox.GetBindingExpression(TextBox.TextProperty));
                validationError.ErrorContent = "Must be a valid email.";

                Validation.MarkInvalid(
                    EmailBox.GetBindingExpression(TextBox.TextProperty),
                    validationError);
            }
            else
            {
                Validation.ClearInvalid(textBox.GetBindingExpression(TextBox.TextProperty));
            }
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
        private void SignupBTN(object sender, RoutedEventArgs e)
        {
            bool EverythingFine = true;
            if (String.IsNullOrEmpty(EmailBox.Text))
            {
                EmailBox.GetBindingExpression(TextBox.TextProperty);
                ValidationError validationError = new ValidationError(new NotEmptyValidationRule(), EmailBox.GetBindingExpression(TextBox.TextProperty));
                validationError.ErrorContent = "Field is required.";

                Validation.MarkInvalid(
                    EmailBox.GetBindingExpression(TextBox.TextProperty),
                    validationError);
                EverythingFine = false;
            }
            else if (!IsValidEmail(EmailBox.Text))
            {
                EmailBox.GetBindingExpression(TextBox.TextProperty);
                ValidationError validationError = new ValidationError(new NotEmptyValidationRule(), EmailBox.GetBindingExpression(TextBox.TextProperty));
                validationError.ErrorContent = "Must be a valid email.";

                Validation.MarkInvalid(
                    EmailBox.GetBindingExpression(TextBox.TextProperty),
                    validationError);
                EverythingFine = false;
            }
            else
            {
                Validation.ClearInvalid(UsernameBox.GetBindingExpression(TextBox.TextProperty));
            }
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
            else if (PasswordBox.Password.Length < 8)
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
                SignupRequest signupRequest = new SignupRequest();
                signupRequest.username = UsernameBox.Text;
                signupRequest.password = PasswordBox.Password;
                signupRequest.email = EmailBox.Text;

                app.communicator.SocketSendReceive(JsonSerializer.serializeRequest(signupRequest, Constants.SIGNUP_REQUEST_CODE)).ContinueWith((task) =>
                {
                    ResponseInfo response = task.Result;
                    SignupResponse signupResponse = JsonDeserializer.deserializeResponse<SignupResponse>(response.buffer);
                    switch (signupResponse.status)
                    {
                        case Constants.SIGNUP_SUCCESS:
                            MyMessageQueue.Enqueue("Sign up Successfully!");
                            this.Dispatcher.Invoke(() =>
                            {
                                app.username = UsernameBox.Text;
                                NavigationService ns = NavigationService.GetNavigationService(this);
                                ns.Navigate(new Uri("Menu.xaml", UriKind.Relative));
                            });
                            break;
                        case Constants.SIGNUP_EMAIL_OR_USERNAME_EXIST:
                            MyMessageQueue.Enqueue("Username or email already exist.");
                            break;
                        case Constants.SIGNUP_UNEXPECTED_ERR:
                            MyMessageQueue.Enqueue("There was an unexpected error.");
                            break;
                    }
                });
                
            }
        }
        bool IsValidEmail(string email)
        {
            try
            {
                Regex rx = new Regex(
            @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$");
                return rx.IsMatch(email);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.GoBack();
        }
    }
}
