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
        public Signin()
        {
            InitializeComponent();
        }
        private void LoginBTN(object sender, RoutedEventArgs e)
        {
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
                //TODO: Sign In Using Communicator
            }
        }
        private void SignupBTN(object sender, RoutedEventArgs e)
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
