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
    
    public partial class DIEM
    {
        public int ID { get; set; }
        public string TenBaiThi { get; set; }
        public double SoDiem { get; set; }
        public System.DateTime NgayThi { get; set; }
        public Nullable<int> IDNhanVien { get; set; }
        public Nullable<int> IDSale { get; set; }
    
        public virtual NhanVien NhanVien { get; set; }
        public virtual Sale Sale { get; set; }
    }
}
