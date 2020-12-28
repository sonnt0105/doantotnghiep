using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace PBDS.Models
{
    public class EBatDongSansearch
    {
        public int ID { get; set; }
        public string TenBatDongSan { get; set; }
        public Nullable<int> IDPhuongXa { get; set; }
        public Nullable<int> IDQuanHuyen { get; set; }
        public Nullable<int> IDTinhThanhPho { get; set; }
        public string TenQuanHuyen { get; set; }
        public string TenTinhThanhPho { get; set; }
        public Nullable<double> GiaTu { get; set; }
        public Nullable<double> GiaDen { get; set; }
        public Nullable<int> DienTichTu { get; set; }
        public Nullable<int> DienTichDen { get; set; }
        public Nullable<int> IDLoaiBaiDang { get; set; }
        public string TenLoaiNhaDat { get; set; }
        public Nullable<int> IDLoaiNhaDat { get; set; }
        public string TenLoaiBaiDang { get; set; }
        public Nullable<int> IDDuAn { get; set; }
        public string TenDuAn { get; set; }
        public string searchstring { get; set; }
        public DateTime NgayDang { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayCapNhatTu { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayCapNhatDen { get; set; }
        public string MucGia { get; set; }

        public virtual DuAn DuAn { get; set; }
        public virtual LoaiBaiDang LoaiBaiDang { get; set; }
        public virtual LoaiNhaDat LoaiNhaDat { get; set; }
        public virtual PhuongXa PhuongXa { get; set; }
        public virtual QuanHuyen QuanHuyen { get; set; }
        public virtual TinhThanhPho TinhThanhPho { get; set; }
    }    
}