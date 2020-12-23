using OfficeOpenXml;
using PBDS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PBDS.Controllers
{
    public class SaleController : Controller
    {
        DB_BatDongSan db;
        public SaleController()
        {
            db = new DB_BatDongSan();
        }
        // GET: Sale
        public ActionResult Index()
        {
            int idsale = Convert.ToInt32(Session["sale"]);
            if (idsale != 0)
            {
                ViewBag.tensale = db.Sales.Where(x => x.ID == idsale).FirstOrDefault().TenSales;
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
            if (Session["sale"] != null)
            {                
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        public ActionResult QuanLyBatDongSan(EBatDongSansearch batdongsansearch)
        {
            int idsale = Convert.ToInt32(Session["sale"]);
            if (idsale != 0)
            {
                ViewBag.tensale = db.Sales.Where(x => x.ID == idsale).FirstOrDefault().TenSales;
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
            var dsphancong = GetPhancong(batdongsansearch, idsale);
            var model = from o in dsphancong
                        select new EPhanCongSales
                        {
                            ID = o.ID,
                            TenBatDongSan = o.BatDongSan.TenBatDongSan,
                            TenTinhThanh = o.BatDongSan.QuanHuyen.TinhThanhPho.TenTinhThanhPho,
                            TenQuanHuyen = o.BatDongSan.QuanHuyen.TenQuanHuyen,
                            TenSale = o.Sale.TenSales,
                            PhanTramHoaHong = o.PhanTramHoaHong,
                            NgayTao = o.NgayTao,
                            TenDonVi = o.BatDongSan.DonVi.TenDonVi,
                            DienTich = o.BatDongSan.DienTich,
                            SoDienThoaiLienHe = o.BatDongSan.SoDienThoaiLienHe,
                            TenLoaiBaiDang = o.BatDongSan.LoaiBaiDang.TenLoaiBaiDang,
                            TenLoaiNhaDat = o.BatDongSan.LoaiNhaDat.TenLoaiNhaDat,
                            Gia_DonVi = o.BatDongSan.Gia + " " + o.BatDongSan.DonVi.TenDonVi,
                            DiaChi = o.BatDongSan.DiaChi,
                        };
            ViewBag.loaibaidang = db.LoaiBaiDangs.ToList();
            ViewBag.listloainhadat = db.LoaiNhaDats.Where(x => x.IDLoaiBaiDang == 1).ToList();
            List<TinhThanhPho> ttp = new List<TinhThanhPho>();
            ttp.Add(new TinhThanhPho() { ID = 0, TenTinhThanhPho = "Tất cả tỉnh thành" });
            ttp.AddRange(db.TinhThanhPhoes.ToList());
            ViewBag.listtinhthanh = ttp.OrderBy(o => o.TenTinhThanhPho);
            if (batdongsansearch.IDLoaiBaiDang.HasValue)
            {
                List<LoaiNhaDat> lnd = new List<LoaiNhaDat>();
                lnd.Add(new LoaiNhaDat() { ID = 0, TenLoaiNhaDat = "Tất cả Loại nhà đất" });
                lnd.AddRange(db.LoaiNhaDats.Where(x => x.IDLoaiBaiDang == batdongsansearch.IDLoaiBaiDang).ToList());
                ViewBag.listloainhadat = lnd;
            }
            if (batdongsansearch.IDTinhThanhPho.HasValue)
            {
                List<QuanHuyen> qh = new List<QuanHuyen>();
                qh.Add(new QuanHuyen() { ID = 0, TenQuanHuyen = "Tất cả quận huyện" });
                qh.AddRange(db.QuanHuyens.Where(x => x.IDTinhThanhPho == batdongsansearch.IDTinhThanhPho).ToList());
                ViewBag.listquanhuyen = qh;
            }
            else
            {
                List<QuanHuyen> qh = new List<QuanHuyen>();
                qh.Add(new QuanHuyen() { ID = 0, TenQuanHuyen = "Tất cả quận huyện" });
                ViewBag.listquanhuyen = qh;
            }
            if (Session["sale"] != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        public IQueryable<PhanCong> GetPhancong(EBatDongSansearch batdongsansearch,int idsale)
        {
            var result = db.PhanCongs.Where(x => x.IDSales == idsale).ToList();
            if (batdongsansearch != null)
            {
                //search theo điều kiện loại bài đăng
                if (batdongsansearch.IDLoaiBaiDang.HasValue)
                    result = result.Where(x => x.BatDongSan.IDLoaiBaiDang == batdongsansearch.IDLoaiBaiDang).ToList();
                //search theo điều kiện loại nhà đất
                if (batdongsansearch.IDLoaiNhaDat.HasValue && batdongsansearch.IDLoaiNhaDat != 0)
                    result = result.Where(x => x.BatDongSan.IDLoaiNhaDat == batdongsansearch.IDLoaiNhaDat).ToList();
                //search theo điều kiện tỉnh thành phố
                if (batdongsansearch.IDTinhThanhPho.HasValue && batdongsansearch.IDTinhThanhPho != 0)
                    result = result.Where(x => x.BatDongSan.QuanHuyen.IDTinhThanhPho == batdongsansearch.IDTinhThanhPho).ToList();
                //search theo điều kiện quận huyện
                if (batdongsansearch.IDQuanHuyen.HasValue && batdongsansearch.IDQuanHuyen != 0)
                    result = result.Where(x => x.BatDongSan.IDQuanHuyen == batdongsansearch.IDQuanHuyen).ToList();
                //search theo điều kiện mức giá lớn hơn mức giá X
                if (batdongsansearch.GiaTu.HasValue)
                    result = TimKiemBaiDangTheoGiaTu(result, batdongsansearch.GiaTu);
                //search theo điều kiện mức giá nhỏ hơn mức giá X
                if (batdongsansearch.GiaDen.HasValue)
                    result = TimKiemBaiDangTheoGiaDen(result, batdongsansearch.GiaDen);

                if (!string.IsNullOrEmpty(batdongsansearch.searchstring))
                {
                    List<PhanCong> listpc = new List<PhanCong>();
                    foreach (var item_result in result)
                        foreach (var item_search in TimKiemBaiDangTheoChuoi(batdongsansearch.searchstring))
                            if (item_result.ID == item_search.ID)
                                listpc.Add(item_result);
                    return listpc.AsQueryable();
                }
            }
            return result.AsQueryable();
        }

        public List<BatDongSan> TimKiemBaiDangTheoChuoi(string searchstring)
        {
            var tatcabds_hoatdong = db.BatDongSans.Where(x => x.TrangThai == 3).ToList();
            var result = tatcabds_hoatdong.Where(x => x.ID == 0).ToList();
            searchstring = convertToLatin(searchstring);// hàm loại bỏ dấu của tiếng việt
            string[] chuoi_latinsearch = searchstring.Split(' ');// hàm cắt chuỗi thành từng từ
            foreach (var item in db.BatDongSans.ToList())
            {
                int tenbds_trung = 0;
                // kiếm tra theo tên bất động sản
                for (int i = 0; i < chuoi_latinsearch.Length; i++)
                {
                    string tenbds = convertToLatin(item.TenBatDongSan);// loại bỏ dấu trong tiếng việt
                    string[] chuoi_tenbds = tenbds.Split(' ');// cắt chuỗi thành từng từ
                    for (int j = 0; j < chuoi_tenbds.Length; j++)
                    {
                        if (chuoi_latinsearch[i] == chuoi_tenbds[j])
                        {
                            tenbds_trung++;
                            // trùng 1 từ trong tên là hiện
                            if (tenbds_trung >= 2)
                            {
                                result.Add(item);
                                goto End;
                            }
                        }
                    }
                }
            End:;
            }
            return result;
        }

        // hàm chuyển bỏ hết ba cái dấu tiếng việt đi cho dễ search :)
        public string convertToLatin(string s)
        {
            // loại bỏ các ký tự đặc biệt trước khi xóa dấu tiếng việt
            //s = Regex.Replace(s, @"(~|!|@|#|$|%|^|&|*|(|)|_|+|-|=|`|[|{|}|]|\|,|<|.|>|/|?)", " ");
            s = s.ToLower();
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }

        public List<PhanCong> TimKiemBaiDangTheoGiaTu(List<PhanCong> listbatdongsan, double? giatu)
        {
            var result = listbatdongsan;
            foreach (var item in listbatdongsan)
            {
                if (item.BatDongSan.IDDonVi == 2)
                    item.BatDongSan.Gia = item.BatDongSan.Gia * 1000;
                if (item.BatDongSan.IDDonVi == 3 || item.BatDongSan.IDDonVi == 5 || item.BatDongSan.IDDonVi == 8)
                    item.BatDongSan.Gia = item.BatDongSan.Gia / 10;
                if (item.BatDongSan.IDDonVi == 7)
                    item.BatDongSan.Gia = item.BatDongSan.Gia / 1000;
            }
            result = result.Where(x => x.BatDongSan.Gia >= giatu).ToList();
            foreach (var item in result)
            {
                if (item.BatDongSan.IDDonVi == 2)
                    item.BatDongSan.Gia = item.BatDongSan.Gia / 1000;
                if (item.BatDongSan.IDDonVi == 3 || item.BatDongSan.IDDonVi == 5 || item.BatDongSan.IDDonVi == 8)
                    item.BatDongSan.Gia = item.BatDongSan.Gia * 10;
                if (item.BatDongSan.IDDonVi == 7)
                    item.BatDongSan.Gia = item.BatDongSan.Gia * 1000;
            }
            return result;
        }

        public List<PhanCong> TimKiemBaiDangTheoGiaDen(List<PhanCong> listbatdongsan, double? giaden)
        {
            var result = listbatdongsan;
            foreach (var item in listbatdongsan)
            {
                if (item.BatDongSan.IDDonVi == 2)
                    item.BatDongSan.Gia = item.BatDongSan.Gia * 1000;
                if (item.BatDongSan.IDDonVi == 3 || item.BatDongSan.IDDonVi == 5 || item.BatDongSan.IDDonVi == 8)
                    item.BatDongSan.Gia = item.BatDongSan.Gia / 10;
                if (item.BatDongSan.IDDonVi == 7)
                    item.BatDongSan.Gia = item.BatDongSan.Gia / 1000;
            }
            result = result.Where(x => x.BatDongSan.Gia <= giaden).ToList();
            foreach (var item in result)
            {
                if (item.BatDongSan.IDDonVi == 2)
                    item.BatDongSan.Gia = item.BatDongSan.Gia / 1000;
                if (item.BatDongSan.IDDonVi == 3 || item.BatDongSan.IDDonVi == 5 || item.BatDongSan.IDDonVi == 8)
                    item.BatDongSan.Gia = item.BatDongSan.Gia * 10;
                if (item.BatDongSan.IDDonVi == 7)
                    item.BatDongSan.Gia = item.BatDongSan.Gia * 1000;
            }
            return result;
        }

        public ActionResult XemPhanCong(int? ID)
        {
            PhanCong phancong = db.PhanCongs.Where(x => x.ID == ID).FirstOrDefault();
            ViewBag.batdongsan = db.BatDongSans.Where(x => x.ID == phancong.IDBatDongSan).FirstOrDefault();            
            if (phancong == null)
                return HttpNotFound();            
            return PartialView("XemPhanCong",phancong);
        }

        public ActionResult XacNhanBanThanhCong(int? ID)
        {
            PhanCong phancong = db.PhanCongs.Where(x => x.ID == ID).FirstOrDefault();            
            if (phancong == null)
                return HttpNotFound();
            phancong.PhanTramHoaHong = phancong.PhanTramHoaHong * 100;
            ViewBag.tienhoahong = phancong.BatDongSan.Gia * phancong.PhanTramHoaHong/100;
            return PartialView("XacNhanBanThanhCong", phancong);
        }

        [HttpPost]
        public ActionResult XacNhanBanThanhCong(PhanCong epcs)
        {
            if (epcs == null)
                return HttpNotFound();
            var phancong = db.PhanCongs.Where(x => x.ID == epcs.ID).FirstOrDefault();
            BatDongSan batdongsan = db.BatDongSans.Where(x => x.ID == epcs.IDBatDongSan).FirstOrDefault();
            batdongsan.TrangThai = 4;
            batdongsan.NgayCapNhat = DateTime.Now;
            db.Entry(batdongsan).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect("Index");
        }

        public ActionResult noKiemTra()
        {
            int idsale = Convert.ToInt32(Session["sale"]);
            if(idsale != 0)
            {
                ViewBag.tensale = db.Sales.Where(x => x.ID == idsale).FirstOrDefault().TenSales;
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
            if (Session["sale"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        public ActionResult KiemTra()
        {
            int idsale = Convert.ToInt32(Session["sale"]);
            if (idsale != 0)
            {
                ViewBag.tensale = db.Sales.Where(x => x.ID == idsale).FirstOrDefault().TenSales;
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
            var sale = db.Sales.Where(x => x.ID == idsale).FirstOrDefault();
            if (sale.ChoKiemTra == 0)
                return View("noKiemTra");                        
            ViewBag.listcauhoi = db.CauHois.ToList();
            ViewBag.listtraloi = db.TraLois.ToList();
            ViewBag.dotkiemtra = db.CauHois.Where(x => x.ID == 1).FirstOrDefault().DotThi;
            if (Session["sale"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        public ActionResult ChamDiem(double diem)
        {
            int? dotthi = db.CauHois.Where(x => x.ID == 1).FirstOrDefault().DotThi;
            DIEM d = new DIEM()
            {
                TenBaiThi = "Đợt " + dotthi,
                SoDiem = diem * 5,
                NgayThi = DateTime.Now,
                IDSale = Convert.ToInt32(Session["sale"]),
            };
            db.Entry(d).State = EntityState.Added;
            int idsale = Convert.ToInt32(Session["sale"]);
            var sa = db.Sales.Where(x => x.ID == idsale).FirstOrDefault();
            sa.ChoKiemTra = 0;
            db.Entry(sa).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect("Index");
        }

        public ActionResult XemDiem()
        {
            int idsale = Convert.ToInt32(Session["sale"]);
            if (idsale != 0)
            {
                ViewBag.tensale = db.Sales.Where(x => x.ID == idsale).FirstOrDefault().TenSales;
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
            var listdiem = db.DIEMs.Where(x => x.IDSale == idsale).ToList();
            if (Session["sale"] != null)
            {
                return View(listdiem);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        public ActionResult XemHoaHong(EBatDongSansearch batdongsansearch, ESale sale, string Submit)
        {
            int idsale = Convert.ToInt32(Session["sale"]);
            if (idsale != 0)
            {
                ViewBag.tensale = db.Sales.Where(x => x.ID == idsale).FirstOrDefault().TenSales;
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }            
            var dsdoanhthu = GetPhancongtheothoigian(batdongsansearch, idsale);
            var model = from o in dsdoanhthu
                        select new EPhanCongSales
                        {
                            ID = o.ID,
                            IDBatDongSan = o.IDBatDongSan,
                            TenBatDongSan = o.BatDongSan.TenBatDongSan,
                            IDSales = o.IDSales,
                            TenSale = o.Sale.TenSales,
                            PhanTramHoaHong_string = o.PhanTramHoaHong * 100 + " %",
                            NgayTao = o.NgayTao,
                            GiaBatDongSan = o.BatDongSan.Gia + " " + o.BatDongSan.DonVi.TenDonVi,
                            HoaHong = (double)o.BatDongSan.Gia * o.PhanTramHoaHong + " " + o.BatDongSan.DonVi.TenDonVi,

                        };
            ViewBag.ngaycapnhattu = batdongsansearch.NgayCapNhatTu.ToString(string.Format("yyyy-MM-dd"));
            ViewBag.ngaycapnhatden = batdongsansearch.NgayCapNhatDen.ToString(string.Format("yyyy-MM-dd"));
            ViewBag.listsales = db.Sales.ToList();
            if (Session["sale"] != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        public IQueryable<PhanCong> GetPhancongtheothoigian(EBatDongSansearch batdongsansearch, int idsale)
        {
            DateTime dt = DateTime.MinValue;
            var result = db.PhanCongs.Where(x => x.BatDongSan.TrangThai == 4).ToList();
            if (batdongsansearch.NgayCapNhatTu != dt)
                result = result.Where(x => x.BatDongSan.NgayCapNhat >= batdongsansearch.NgayCapNhatTu).ToList();

            if (batdongsansearch.NgayCapNhatDen != dt)
                result = result.Where(x => x.BatDongSan.NgayCapNhat <= batdongsansearch.NgayCapNhatDen).ToList();

            result = result.Where(x => x.IDSales == idsale).ToList();

            return result.AsQueryable();
        }
    }
}