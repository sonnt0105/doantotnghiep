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
    
    public partial class DonVi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DonVi()
        {
            this.BatDongSans = new HashSet<BatDongSan>();
        }
    
        public int ID { get; set; }
        public string TenDonVi { get; set; }
        public Nullable<int> IDLoaiBaiDang { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BatDongSan> BatDongSans { get; set; }
        public virtual LoaiBaiDang LoaiBaiDang { get; set; }
    }
}