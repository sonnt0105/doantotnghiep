using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PBDS.Models
{
    public class EBaiDang
    {

        public int ID { get; set; }
        [Display(Name = "Tên bài đăng")]
        public string TenBaiDang { get; set; }
        public Nullable<int> IDQuanHuyen { get; set; }
        [Display(Name = "Quận Huyện")]
        public string TenQuanHuyen { get; set; }
        public Nullable<int> IDPhuongXa { get; set; }
        [Display(Name = "Phường Xã")]
        public string TenPhuongXa { get; set; }
        [Display(Name = "Giá")]
        public Nullable<double> Gia { get; set; }
        [Display(Name = "Diện tích")]
        public Nullable<int> DienTich { get; set; }
        [Display(Name = "Tên Loại Bài Đăng")]
        public string TenLoaiBaiDang { get; set; }
        public Nullable<int> IDLoaiBaiDang { get; set; }
        [Display(Name = "Mô Tả")]
        public string Mota { get; set; }
        [Display(Name = "Hình Ảnh")]
        public string Urlimage { get; set; }
        [Display(Name = "Trạng Thái")]
        public Nullable<int> TrangThai { get; set; }        
        [Display(Name = "Ngày Đăng")]
        public Nullable<System.DateTime> NgayDang { get; set; }
        [Display(Name = "Ngày Cập Nhật")]
        public Nullable<System.DateTime> NgayCapNhat { get; set; }
        [Display(Name = "Tên Liên Hệ")]
        public string TenLienHe { get; set; }
        [Display(Name = "Địa Chỉ Liên Hệ")]
        public string DiaChiLienHe { get; set; }
        [Display(Name = "Số Điện Thoại Liên Hệ")]
        public string SoDienThoaiLienHe { get; set; }
        [Display(Name = "Email Liên Hệ")]
        public string EmailLienHe { get; set; }

        public virtual LoaiBaiDang LoaiBaiDang { get; set; }
        public virtual QuanHuyen QuanHuyen { get; set; }
        
    }
}