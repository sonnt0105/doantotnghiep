using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBDS.Models
{
    public class EPhanCongSales
    {
        public int ID { get; set; }
        public Nullable<int> IDBatDongSan { get; set; }
        public string TenBatDongSan { get; set; }
        public Nullable<int> IDSales { get; set; }
        public string TenSale { get; set; }
        public Nullable<double> PhanTramHoaHong { get; set; }
        public string PhanTramHoaHong_string { get; set; }
        public string HoaHong { get; set; }
        public string GiaBatDongSan { get; set; } 
        public Nullable<System.DateTime> NgayTao { get; set; }        
        public virtual Sale Sale { get; set; }
        public string Gia_DonVi { get; set; }
        public string DiaChi { get; set; }

        public string TenDonVi { get; set; }
        public string TenTinhThanh { get; set; }
        public string TenQuanHuyen { get; set; }
        public double DienTich { get; set; }
        public string TenLoaiBaiDang { get; set; }
        public string TenLoaiNhaDat { get;set; }

        public string SoDienThoaiLienHe { get; set; }
    

    }
}