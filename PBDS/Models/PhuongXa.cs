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
    
    public partial class PhuongXa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhuongXa()
        {
            this.BatDongSans = new HashSet<BatDongSan>();
            this.DuAns = new HashSet<DuAn>();
        }
    
        public int ID { get; set; }
        public string TenPhuongXa { get; set; }
        public Nullable<int> IDQuanHuyen { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BatDongSan> BatDongSans { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DuAn> DuAns { get; set; }
        public virtual QuanHuyen QuanHuyen { get; set; }
    }
}