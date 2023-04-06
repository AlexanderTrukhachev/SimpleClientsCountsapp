using SimpleClientsCountsapp.Model;
using SimpleClientsCountsapp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Microsoft.EntityFrameworkCore.Design;
using System.Xml.Serialization;


namespace SimpleClientsCountsapp.ViewModel
{
    public class DataManageVM : INotifyPropertyChanged
    {
        #region Fields and Properties
        private string transactionSum = "100";

        public string TransactionSum
        {
            get => transactionSum;
            set
            {
                transactionSum = float.TryParse(value, out _) ? value : "0";
                NotifyPropertyChanged("TransactionSum");
            }
        }

        private Count countFrom;

        public Count CountFrom
        {
            get => countFrom;
            set
            {
                countFrom = value;
                NotifyPropertyChanged("CountFrom");
            }
        }

        private Count countTo;

        public Count CountTo
        {
            get => countTo;
            set
            {
                countTo = value;
                NotifyPropertyChanged("CountTo");
            }
        }


        private Client selectedClient;

        public Client SelectedClient
        {
            get => selectedClient;
            set
            {
                selectedClient = value;
                countsView.View.Refresh();
                CountFrom = AllCounts.FirstOrDefault(c => c.Clientsid == selectedClient.Id);
                CountTo = AllCounts.FirstOrDefault(c => (c.Clientsid == selectedClient.Id) && (c.Id != CountFrom.Id));
                NotifyPropertyChanged("SelectedClient");
            }
        }

        private List<Client> allClients = DataWorker.GetAllClients();

        public List<Client> AllClients
        {
            get => allClients;
            set
            {
                allClients = value; 
                NotifyPropertyChanged("AllClients");
            }
        }

        private List<History> allHistory = DataWorker.GetAllHistory();

        public List<History> AllHistory
        {
            get => allHistory;
            set
            {
                allHistory = value; 
                NotifyPropertyChanged("AllHistory");
            }
        }

        private ObservableCollection<Count> allCounts = new ObservableCollection<Count>(DataWorker.GetAllCounts());

        public ObservableCollection<Count> AllCounts
        {
            get => allCounts;
            set
            {
                allCounts = value;
                NotifyPropertyChanged("AllCounts");
            }
        }

        private CollectionViewSource countsView;

        public ICollectionView FilteredCounts
        {
            get => countsView.View;
        }
        #endregion

        public DataManageVM()
        {
            selectedClient = AllClients.First();
            CountFrom = AllCounts.FirstOrDefault(c => c.Clientsid == selectedClient.Id);
            CountTo = AllCounts.FirstOrDefault(c => (c.Clientsid == selectedClient.Id) && (c.Id != CountFrom.Id));
            countsView = new CollectionViewSource();
            countsView.Source = AllCounts;
            countsView.Filter += countsView_Filter;
        }

        void countsView_Filter(object sender, FilterEventArgs e)
        {
            if (SelectedClient is null)
            {
                e.Accepted = true;
                return;
            }

            e.Accepted = (((Count)e.Item).Clientsid == SelectedClient.Id);
        }

        #region Commands

        private RelayCommand saveToXML;

        public RelayCommand SaveToXML
        {
            get
            {
                return saveToXML ?? new RelayCommand(obj =>
                {
                    SaveToXMLMethod();
                });
            }
        }

        private RelayCommand openTransactionWindow;

        public RelayCommand OpenTransactionWindow
        {
            get
            {
                return openTransactionWindow ?? new RelayCommand(obj =>
                {
                    OpenTransactionWindowMethod();
                });
            }
        }

        private RelayCommand openHistoryWindow;

        public RelayCommand OpenHistoryWindow
        {
            get
            {
                return openHistoryWindow ?? new RelayCommand(obj =>
                {
                    OpenHistoryWindowMethod();
                });
            }
        }

        private RelayCommand transaction;

        public RelayCommand Transaction
        {
            get
            {
                return transaction ?? new RelayCommand(obj =>
                {
                    TransactionMethod();
                    AllCounts = new ObservableCollection<Count>(DataWorker.GetAllCounts());
                    countsView.Source = AllCounts;
                    MainWindow.AllCountsView.ItemsSource = null;
                    MainWindow.AllCountsView.Items.Clear();
                    MainWindow.AllCountsView.ItemsSource = FilteredCounts;
                    MainWindow.AllCountsView.Items.Refresh();
                    SelectedClient = SelectedClient;
                });
            }
        }
        #endregion

        #region Methods
        private void TransactionMethod()
        {
            var amountFrom = CountFrom.Amount;
            float sum = 0;
            if (!float.TryParse(TransactionSum, out sum))
            {
                MessageBox.Show("Что-то пошло не так в методе TransactionMethod");
                return;
            }

            if (sum > amountFrom)
            {
                MessageBox.Show("Недостаточно средств на счете");
                return;
            }

            var resultstr = "Перевод не совершен"; 
            resultstr = DataWorker.CountsTransaction(CountFrom, CountTo, sum);

        }

        private void OpenTransactionWindowMethod()
        {
            TransactionWindow newTransactionWindow = new TransactionWindow();
            newTransactionWindow.Owner = Application.Current.MainWindow;
            newTransactionWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (Application.Current.MainWindow != null)
                newTransactionWindow.DataContext = Application.Current.MainWindow.DataContext;
            newTransactionWindow.ShowDialog();
        }

        private void OpenHistoryWindowMethod()
        {
            HistoryWindow newHistoryWindow = new HistoryWindow();
            newHistoryWindow.Owner = Application.Current.MainWindow;
            newHistoryWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (Application.Current.MainWindow != null)
                newHistoryWindow.DataContext = Application.Current.MainWindow.DataContext;
            newHistoryWindow.ShowDialog();
        }

        private void SaveToXMLMethod()
        {
            XmlSerializer formater = new XmlSerializer(typeof(List<Client>));
            using (var stream = File.OpenWrite("Clients.xml"))
            {
                formater.Serialize(stream, AllClients);
            }
        }
        #endregion


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
