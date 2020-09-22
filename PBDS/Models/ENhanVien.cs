using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBDS.Models
{
    public class ENhanVien
    {
        int idnhanvien;
        string hoten;
        string email;
        string sodienthoai;
        string diachi;
        int idphongban;

        public ENhanVien(int idnhanvien,string hoten,string email,string sodienthoai, string diachi, int idphongban)
        {
            this.idnhanvien = idnhanvien;
            this.hoten = hoten;
            this.email = email;
            this.sodienthoai = sodienthoai;
            this.diachi = diachi;
            this.idphongban = idphongban;
        }
    }
}