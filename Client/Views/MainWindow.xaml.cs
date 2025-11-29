using Client.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    //public partial class MainWindow : Window
    //{
    //    private MainViewModel vm;
    //    public MainWindow()
    //    {
    //        InitializeComponent();
    //        DataContext = vm = new MainViewModel();
    //    }
    //}
    //public partial class MainWindow : Window
    //{
    //    public MainWindow(MainViewModel vm)
    //    {
    //        InitializeComponent();
    //        DataContext = vm;
    //    }
    //}
    public class MainViewModel : ObservableObject
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public MainViewModel()
        {
        }
    }
}
