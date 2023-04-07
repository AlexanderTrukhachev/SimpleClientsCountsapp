using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using SimpleClientsCountsapp.Model;

namespace SimpleClientsCountsapp.ViewModel.Converters
{
    /// <summary>
    /// Конвертер из класса Counts в string
    /// </summary>
    public class CountsToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var count = (Count) value;
            if (count == null) 
                return "";
            return count.Id.ToString() + " (" + count.Amount.ToString() + ")";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long id;
            if (long.TryParse(((string)value).Split('(',')')[0], out id)) 
                return DataWorker.GetAllCounts().Select(o => o.Id == id);
            else
                return null;
        }
    }
}
