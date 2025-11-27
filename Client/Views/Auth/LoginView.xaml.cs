using System.Windows;
using System.Windows.Controls;
using Client.ViewModels.Auth;

namespace Client.Views.Auth
{
    public partial class LoginView : Window
    {
        public LoginView(LoginViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
                vm.Password = ((PasswordBox)sender).Password;
        }
    }
}

