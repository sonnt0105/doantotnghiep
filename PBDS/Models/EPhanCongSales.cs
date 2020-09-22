using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBDS.Models
{
    public class EPhanCongSales
    {
        int idphancong;
        int baidang;
        int nhanvien;
        int sales;
        float phantramhoahong;
        DateTime ngaytao;

        public EPhanCongSales(int idphancong, int baidang, int nhanvien, int sales, float phantramhoahong,DateTime ngaytao)
        {
            this.idphancong = idphancong;
            this.baidang = baidang;
            this.nhanvien = nhanvien;
            this.sales = sales;
            this.phantramhoahong = phantramhoahong;
            this.ngaytao = ngaytao;
        }
    }
}