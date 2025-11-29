using Client.Services.Interfaces;
using Client.ViewModels.File;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Windows;

namespace Client.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        private string title = "Добро пожаловать!";

        public MainViewModel(IAuthService authService)
        {
            _authService = authService;

            // пример: если токен есть, можно загрузить данные
            if (!string.IsNullOrEmpty(_authService.AccessToken))
            {
                title = "Вы успешно вошли!";
            }
        }

        [RelayCommand]
        private void Logout()
        {
            _authService.ClearToken();
            MessageBox.Show("Вы вышли из аккаунта");

            // после выхода вернуть пользователя на LoginView
            var loginWindow = new Client.Views.Auth.LoginView(
                App.Services.GetRequiredService<Client.ViewModels.Auth.LoginViewModel>()
            );

            loginWindow.Show();

            // закрыть текущее окно
            System.Windows.Application.Current.Windows[0]?.Close();
        }

        public FileListViewModel FileListVm { get; }

        public MainViewModel(FileListViewModel fileListVm)
        {
            FileListVm = fileListVm;
            title = "Облачноe хранилище";
        }

        public async Task InitializeAsync()
        {
            await FileListVm.LoadFilesAsync();
        }
    }
}


