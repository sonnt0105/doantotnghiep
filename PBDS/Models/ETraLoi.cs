using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBDS.Models
{
    public class ETraLoi
    {
        public int ID { get; set; }
        public Nullable<int> IDCauHoi { get; set; }
        public string DapAn { get; set; }
        public int DungSai { get; set; }

        public virtual CauHoi CauHoi { get; set; }
    }
}