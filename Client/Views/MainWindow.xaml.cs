using System.Windows;
using Client.ViewModels;

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
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
