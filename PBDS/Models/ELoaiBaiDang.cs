using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBDS.Models
{
    public class ELoaiBaiDang
    {
        int idloaibaidang;
        string tenloaibaidang;
        string mota;

        public ELoaiBaiDang(int idloaibaidang, string tenloaibaidang, string mota)
        {
            this.idloaibaidang = idloaibaidang;
            this.tenloaibaidang = tenloaibaidang;
            this.mota = mota;
        }
    }
}