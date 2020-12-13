﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PBDS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class BatDongSan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BatDongSan()
        {
            this.PhanCongs = new HashSet<PhanCong>();
        }

        public int ID { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập tên bất động sản")]
        public string TenBatDongSan { get; set; }
        [Required(ErrorMessage = "Bạn cần chọn quận huyện")]
        public int IDQuanHuyen { get; set; }
        public Nullable<int> IDPhuongXa { get; set; }
        [Required(ErrorMessage = "Bạn cần chọn tỉnh thành")]
        public int IDTinhThanhPho { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập địa chỉ")]
        public string DiaChi { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập giá")]
        public double Gia { get; set; }
        [Required(ErrorMessage = "Bạn cần chọn đơn vị")]
        public int IDDonVi { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập diện tích")]
        public double DienTich { get; set; }
        [Required(ErrorMessage = "Bạn cần chọn loại bài đăng")]
        public int IDLoaiBaiDang { get; set; }
        [Required(ErrorMessage = "Bạn cần chọn loại nhà đất")]
        public int IDLoaiNhaDat { get; set; }
        public Nullable<int> IDDuAn { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập mô tả")]
        public string Mota { get; set; }
        public Nullable<int> SoPhongNgu { get; set; }
        public string SoTang { get; set; }
        public string SoToilet { get; set; }
        public string NoiThat { get; set; }
        public string MatTien { get; set; }
        public string HuongNha { get; set; }
        public string Urlimage { get; set; }
        public Nullable<int> TrangThai { get; set; }
        public System.DateTime NgayDang { get; set; }
        public System.DateTime NgayCapNhat { get; set; }
        public string TenLienHe { get; set; }
        public string DiaChiLienHe { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập số điện thoại người liên hệ")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string SoDienThoaiLienHe { get; set; }
        public string EmailLienHe { get; set; }


        public virtual DonVi DonVi { get; set; }
        public virtual DuAn DuAn { get; set; }
        public virtual LoaiBaiDang LoaiBaiDang { get; set; }
        public virtual LoaiNhaDat LoaiNhaDat { get; set; }
        public virtual PhuongXa PhuongXa { get; set; }
        public virtual QuanHuyen QuanHuyen { get; set; }
        public virtual TinhThanhPho TinhThanhPho { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhanCong> PhanCongs { get; set; }
    }
}