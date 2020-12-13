using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBDS.Models
{
    public class Eluong
    {
        public int ID { get; set; }
        public Nullable<int> IDNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public Nullable<int> IDHoaHong { get; set; } 
        public Nullable<decimal> LuongCoBan { get; set; }
        public Nullable<double> HeSoLuong { get; set; }
        public Nullable<decimal> HoaHong { get; set; }
        public Nullable<decimal> PhuCap { get; set; }
        public Nullable<decimal> ThucLinh { get; set; }

        public virtual NhanVien NhanVien { get; set; }
        DB_BatDongSan db = new DB_BatDongSan();
        public decimal TinhLuong_HoaHong(DateTime ngay)
        {

            return 1;
        }
    }
}