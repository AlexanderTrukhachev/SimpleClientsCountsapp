using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClientsCountsapp.Model
{
    public class Count
    {

        public long Id { get; set; }
        public float Amount { get; set; }

        public long? Clientsid { get; set; }
        public Client? Client { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public ICollection<History> History { get; set; }
    }
}
