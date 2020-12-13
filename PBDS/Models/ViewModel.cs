using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBDS.Models
{
    public class ViewModel
    {
        DB_BatDongSan db = new DB_BatDongSan();      
        public IEnumerable<QuanHuyen> Quanhuyens { get; set; }
        public IEnumerable<TinhThanhPho> Tinhthanhphos { get; set; }
        public IEnumerable<LoaiBaiDang> Loaibaidangs { get; set; }
    }
}