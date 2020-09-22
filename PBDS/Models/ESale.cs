using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBDS.Models
{
    public class ESale
    {
        int idsales;
        string hoten;
        string email;
        int sodienthoai;
        string diachi;
        int baidang;

        public ESale(int idsales,string hoten,string email, int sodienthoai, string diachi, int baidang)
        {
            this.idsales = idsales;
            this.hoten = hoten;
            this.email = email;
            this.sodienthoai = sodienthoai;
            this.diachi = diachi;
            this.baidang = baidang;
        }
    }
}