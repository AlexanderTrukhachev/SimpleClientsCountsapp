using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClientsCountsapp.Model
{
    public partial class History
    {
        public DateTime Time { get; set; }
        public float? Diff { get; set; }
        public long Countid { get; set; }
        public float? Amount { get; set; }

        public virtual Counts Count { get; set; }
    }
}
