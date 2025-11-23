using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string _title = "Hello, WPF + MVVM + DI!";

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public MainViewModel()
        {
            // Инициализация данных или команд
        }
    }
}
