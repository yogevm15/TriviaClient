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

namespace TriviaApp
{
    /// <summary>
    /// Interaction logic for Signup.xaml
    /// </summary>
    public partial class Signup : Page
    {
        public Signup()
        {
            InitializeComponent();
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
                //TODO: Sign In Using Communicator
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
    }
}
