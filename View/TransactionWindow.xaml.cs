using System.Windows;
using SimpleClientsCountsapp.ViewModel;

namespace SimpleClientsCountsapp.View
{
    /// <summary>
    /// Логика взаимодействия для TransactionWindow.xaml
    /// </summary>
    public partial class TransactionWindow : Window
    {
        public TransactionWindow()
        {
            InitializeComponent();
        }

        private void Accept_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
