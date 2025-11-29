using Client.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Windows;

namespace Client.ViewModels.Auth
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        private string email = "";

        [ObservableProperty]
        private string nickname = "";

        [ObservableProperty]
        private string password = "";
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public RegisterViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        private async Task Register()
        {
            var result = await _authService.RegisterAsync(
                nickname, email, password
            );

            if (!result)
            {
                MessageBox.Show("Ошибка регистрации");
                return;
            }

            MessageBox.Show("Успешная регистрация!");
        }
    }
}


