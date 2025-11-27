using Client.Services;
using Client.Services.Interfaces;
using Client.ViewModels;
using Client.ViewModels.Auth;
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

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            Services = serviceCollection.BuildServiceProvider();

            // Получаем MainWindow из DI
            var mainWindow = Services.GetRequiredService<MainWindow>();
            //mainWindow.Show();

            base.OnStartup(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<LoginViewModel>();

            // Views
            services.AddSingleton<MainWindow>();
            services.AddSingleton<LoginView>();

            // Сервисы (если будут)
            // services.AddSingleton<IDataService, DataService>();
            services.AddHttpClient<IAuthService, AuthService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7260");
            });

        }
    }
}
