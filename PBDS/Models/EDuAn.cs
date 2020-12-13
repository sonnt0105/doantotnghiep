using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PBDS.Models
{
    public class EDuAn
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Bắt buộc phải có tên dự án")]
        public string TenDuAn { get; set; }
        public int IDQuanHuyen { get; set; }
        public int IDPhuongXa { get; set; }
        public string TenQuanHuyen { get; set; }
        public string TenTinhThanh { get; set; }
        public string TenPhuongXa { get; set; }        
        public string DiaChi { get; set; }        
        public double? GiaBan { get; set; }
        public string GiaBan_DonVi { get; set; }
        public string DonViPhanPhoi { get; set; }        
        public double? TongDienTich { get; set; }
        public string DienTich_DonVi { get; set; }
        public int IDLoaiDuAn { get; set; }
        public string TenLoaiDuAn { get; set; }        
        public string MoTa { get; set; }
        public string Urlimage { get; set; }
        public Nullable<int> TrangThai { get; set; }
        public Nullable<System.DateTime> NgayDang { get; set; }
        public Nullable<System.DateTime> NgayCapNhat { get; set; }
    }
}