using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBDS.Models
{
    public class EBaiDang
    {
        int idbaidang;
        string tenbaidang;
        int diachi;
        decimal gia;
        int dientich;
        int loaibaidang;
        string mota;
        int trangthai;
        DateTime ngaydang, ngaycapnhat;

        public EBaiDang(int idbaidang, string tenbaidang,int diachi, decimal gia, int dientich, int loaibaidang,
            string mota, int trangthai, DateTime ngaydang, DateTime ngaycapnhat)
        {
            this.idbaidang = idbaidang;
            this.tenbaidang = tenbaidang;
            this.diachi = diachi;
            this.gia = gia;
            this.dientich = dientich;
            this.loaibaidang = loaibaidang;
            this.mota = mota;
            this.trangthai = trangthai;
            this.ngaydang = ngaydang;
            this.ngaycapnhat = ngaycapnhat;
        }
    }
}