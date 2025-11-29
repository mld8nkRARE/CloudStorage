using System.Windows;
using System.Windows.Controls;
using Client.ViewModels.Auth;

namespace Client.Views.Auth
{
    public partial class RegisterView : Window
    {
        public RegisterView(RegisterViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel vm)
                vm.Password = ((PasswordBox)sender).Password;
        }
    }
}

