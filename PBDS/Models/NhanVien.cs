//------------------------------------------------------------------------------
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
    
    public partial class NhanVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhanVien()
        {
            this.PhanCongSales = new HashSet<PhanCongSale>();
            this.PhiHoaHongs = new HashSet<PhiHoaHong>();
        }
    
        public int idnhanvien { get; set; }
        public string hoten { get; set; }
        public string taikhoan { get; set; }
        public string matkhau { get; set; }
        public string email { get; set; }
        public Nullable<int> sodienthoai { get; set; }
        public string diachi { get; set; }
        public Nullable<int> phongban { get; set; }
    
        public virtual PhongBan PhongBan1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhanCongSale> PhanCongSales { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhiHoaHong> PhiHoaHongs { get; set; }
    }
}