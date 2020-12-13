using OfficeOpenXml;
using PBDS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult QuanLyBatDongSan()
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
            var model = from o in new DB_BatDongSan().PhanCongs.Where(x => x.BatDongSan.TrangThai == 3).Where(x => x.IDSales == idsale)
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
            if (Session["sale"] != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
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
            BatDongSan batdongsan = db.BatDongSans.Where(x => x.ID == epcs.IDBatDongSan).FirstOrDefault();
            batdongsan.TrangThai = 4;
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
    }
}