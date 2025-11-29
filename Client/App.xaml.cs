using Client.Services;
using Client.Services.Interfaces;
using Client.ViewModels;
using Client.ViewModels.Auth;
using Client.ViewModels.File;
using Client.Views.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            Services = serviceCollection.BuildServiceProvider();

            // стартуем LoginView вместо MainWindow
            var login = Services.GetRequiredService<LoginView>();
            login.Show();

            var mainWindow = Services.GetRequiredService<MainWindow>();
            var mainVm = Services.GetRequiredService<MainViewModel>();

            await mainVm.InitializeAsync();

            mainWindow.Show();

            base.OnStartup(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<RegisterViewModel>();
            services.AddSingleton<FileListViewModel>();
            services.AddSingleton<FileItemViewModel>();

            // Views
            services.AddSingleton<LoginView>();
            services.AddSingleton<RegisterView>();
            services.AddSingleton<MainWindow>();

            // Services
            services.AddSingleton<IAuthService, AuthService>();

            // HttpClient
            services.AddHttpClient();

            services.AddHttpClient<IFileService, FileService>(c =>
            {
                c.BaseAddress = new Uri("https://localhost:7260"); // ваш сервер
            });
        }

    }
}
