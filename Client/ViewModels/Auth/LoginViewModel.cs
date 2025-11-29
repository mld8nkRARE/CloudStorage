using Client.Services.Interfaces;
using Client.Views.Auth;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Windows;

namespace Client.ViewModels.Auth
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        private string email = "";

        [ObservableProperty]
        private string password = "";
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        private async Task Login()
        {
            var result = await _authService.LoginAsync(new Models.Auth.LoginRequest
            {
                Email = email,
                Password = Password
            });

            if (result == null)
            {
                MessageBox.Show("Неверный email или пароль");
                return;
            }

            MessageBox.Show("Успешный вход!");

            // после успешного входа — открываем MainWindow
            var main = App.Services.GetRequiredService<MainWindow>();
            main.Show();

            // закрываем окно авторизации
            Application.Current.Windows[0]?.Close();
        }

        [RelayCommand]
        private void OpenRegister()
        {
            var reg = App.Services.GetRequiredService<RegisterView>();
            reg.Show();

            Application.Current.Windows[0]?.Close();
        }

    }
}
