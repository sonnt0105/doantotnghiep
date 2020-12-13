using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBDS.Models
{
    public class eDuAnSearch
    {
        public int ID { get; set; }
        public string searchstring { get; set; }
        public Nullable<int> IDTinhThanhPho { get; set; }
        public Nullable<int> IDQuanHuyen { get; set; }
        public Nullable<int> IDLoaiDuAn { get; set; }
    }
}