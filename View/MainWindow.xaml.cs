using System.Windows;
using System.Windows.Controls;
using SimpleClientsCountsapp.ViewModel;

namespace SimpleClientsCountsapp.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ListView AllCountsView;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new DataManageVM();
            AllCountsView = ViewAllCounts;
        }


    }
}
