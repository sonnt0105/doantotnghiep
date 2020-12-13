using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PBDS.Models
{
    public class EBangDiem
    {
        public string TenNhanVien { get; set; }
        public double Diem { get; set; }
        public string Dotthi { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayThi { get; set; }
        
    }
}