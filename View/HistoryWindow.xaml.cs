using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SimpleClientsCountsapp.ViewModel;

namespace SimpleClientsCountsapp.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class HistoryWindow : Window
    {


        public HistoryWindow()
        {
            InitializeComponent();

        }

        private void Accept_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

    }
}
