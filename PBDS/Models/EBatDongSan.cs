using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PBDS.Models
{
    public class EBatDongSan
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập thông tin")]
        public string TenBatDongSan { get; set; }
        public Nullable<int> IDQuanHuyen { get; set; }
        public string TenQuanHuyen { get; set; }
        public Nullable<int> IDPhuongXa { get; set; }
        public string TenPhuongXa { get; set; }
        public string TenTinhThanhPho { get; set; }
        public string DiaChi { get; set; }
        public Nullable<double> Gia { get; set; }
        public Nullable<int> IDDonVi { get; set; }
        public string Gia_DonVi { get; set; }
        public string TenDonVi { get; set; }
        public double DienTich { get; set; }
        public Nullable<int> IDLoaiBaiDang { get; set; }
        public string TenLoaiBaiDang { get; set; }
        public Nullable<int> IDLoaiNhaDat { get; set; }
        public string TenLoaiNhaDat { get; set; }
        public Nullable<int> IDDuAn { get; set; }
        public string TenDuAn { get; set; }
        public string Mota { get; set; }
        public string Urlimage { get; set; }
        public Nullable<int> TrangThai { get; set; }
        public string TenTrangThai { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayDang { get; set; }
        public Nullable<System.DateTime> NgayCapNhat { get; set; }
        public string TenLienHe { get; set; }
        public string DiaChiLienHe { get; set; }
        public string SoDienThoaiLienHe { get; set; }
        public string EmailLienHe { get; set; }

        public virtual DonVi DonVi { get; set; }
        public virtual DuAn DuAn { get; set; }
        public virtual LoaiBaiDang LoaiBaiDang { get; set; }
        public virtual QuanHuyen QuanHuyen { get; set; }
    }
}