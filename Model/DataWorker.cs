using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleClientsCountsapp.Model.Data;

namespace SimpleClientsCountsapp.Model
{
    /// <summary>
    /// Все операции над БД
    /// </summary>
    public class DataWorker
    {
        /// <summary>
        /// Метод возравщает всех клиентов
        /// </summary>
        /// <returns>Список клиентов</returns>
        public static List<Client> GetAllClients()
        {
            List<Client> result;
            using (ApplicationContext db = new ApplicationContext())
            {
                result = db.Clients.ToList();
            }
            return result;
        }
        /// <summary>
        /// Метод возравщает всех счетов
        /// </summary>
        /// <returns>Список счетов</returns>
        public static List<Count> GetAllCounts()
        {
            List<Count> result;
            using (ApplicationContext db = new ApplicationContext())
            {
                result = db.Counts.ToList();
            }
            return result;
        }
        /// <summary>
        /// Метод возравщает всех клиентов
        /// </summary>
        /// <returns>Список клиентов</returns>
        public static List<History> GetAllHistory()
        {
            List<History> result;
            using (ApplicationContext db = new ApplicationContext())
            {
                result = db.History.ToList();
            }
            return result;
        }
        /// <summary>
        /// Метод перевода денег с одного счета на другой
        /// </summary>
        /// <param name="fromCount">Счет, с которого переводят</param>
        /// <param name="toCount">Счет, на который переводят</param>
        /// <param name="sum">Сумма перевода</param>
        /// <returns>Сообщение об успехе или не успехе типа sting</returns>
        public static string CountsTransaction(Count fromCount, Count toCount, float sum)
        {
            string result = "Недостаточно денег на исходном счете";
            using (ApplicationContext db = new ApplicationContext())
            {
                if (fromCount.Amount >= sum)
                {
                    Count count = db.Counts.FirstOrDefault(c => c.Id == fromCount.Id);
                    count.Amount -= sum;
                    History history = new History
                    {
                        Amount = count.Amount,
                        Diff = -sum,
                        Countid = count.Id, 
                        Time = DateTime.Now
                    };
                    db.History.Add(history);
                    count = db.Counts.FirstOrDefault(c => c.Id == toCount.Id);
                    count.Amount += sum;
                    history = new History
                    {
                        Amount = count.Amount,
                        Diff = -sum,
                        Countid = count.Id,
                        Time = DateTime.Now
                    };
                    db.History.Add(history);
                    var numOfDBChanges = db.SaveChanges();
                    result = "Деньги в количестве " + sum.ToString() + " со счета "
                             + fromCount.Id.ToString() + " переведены на счет "
                             + toCount.Id.ToString() + " (количество изменений в БД "
                             + numOfDBChanges.ToString() + ")";
                }
            }
            return result;
        }
    }
}
