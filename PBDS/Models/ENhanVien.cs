using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBDS.Models
{
    public class ENhanVien
    {
        public int ID { get; set; }
        public string HoTen { get; set; }
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
    }
}