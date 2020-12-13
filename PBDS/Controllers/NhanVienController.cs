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
    public class NhanVienController : Controller
    {
        DB_BatDongSan db = new DB_BatDongSan();
        // GET: NhanVien
        public ActionResult Index()
        {
            int idnhanvien = Convert.ToInt32(Session["nhanvien"]);
            if (idnhanvien != 0)
            {
                ViewBag.tennhanvien = db.NhanViens.Where(x => x.ID == idnhanvien).FirstOrDefault().HoTen;
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
            if (Session["nhanvien"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        public ActionResult ThemBatDongSan()
        {
            int idnhanvien = Convert.ToInt32(Session["nhanvien"]);
            if (idnhanvien != 0)
            {
                ViewBag.tennhanvien = db.NhanViens.Where(x => x.ID == idnhanvien).FirstOrDefault().HoTen;
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
            ViewBag.listloaibaidang = db.LoaiBaiDangs.ToList();
            ViewBag.listquanhuyen = db.QuanHuyens.Where(x=>x.ID == 0).ToList();
            List<TinhThanhPho> ttp = new List<TinhThanhPho>();
            ttp.Add(new TinhThanhPho() { ID = 0, TenTinhThanhPho = "Tỉnh thành" });
            ttp.AddRange(db.TinhThanhPhoes.ToList());
            ViewBag.listtinhthanh = ttp.OrderBy(o=>o.TenTinhThanhPho);
            ViewBag.listdonvi = db.DonVis.Where(x => x.IDLoaiBaiDang == 0).ToList();
            ViewBag.listduan = db.DuAns.Where(x => x.ID == 0).ToList();
            ViewBag.listloainhadat = db.LoaiNhaDats.Where(x => x.ID == 0).ToList();
            ViewBag.listphuongxa = db.PhuongXas.Where(x => x.ID == 0).ToList();
            if (Session["nhanvien"] != null)
            {                
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        [HttpPost]
        public ActionResult GetDonViByIDLoaiBaiDang(int IDLoaiBaiDang)
        {
            List<DonVi> objdonvi = new List<DonVi>();
            objdonvi = db.DonVis.Where(x => x.IDLoaiBaiDang == IDLoaiBaiDang).ToList();
            SelectList obgdonvi = new SelectList(objdonvi, "ID", "TenDonVi");
            ViewBag.listdonvi = obgdonvi;
            return Json(obgdonvi);
        }

        [HttpPost]
        public ActionResult GetQuanHuyenByIDTinhThanhPho(int IDTinhThanhPho)
        {
            List<QuanHuyen> objquanhuyen = new List<QuanHuyen>();
            objquanhuyen = db.QuanHuyens.Where(x => x.IDTinhThanhPho == IDTinhThanhPho).ToList();
            SelectList obgquanhuyen = new SelectList(objquanhuyen.OrderBy(o => o.TenQuanHuyen), "Id", "TenQuanHuyen", 0);
            return Json(obgquanhuyen);
        }


        [HttpPost]
        public ActionResult GetDuAnbyIDQuanHuyen(int IDQuanHuyen)
        {
            List<DuAn> objduan = new List<DuAn>();
            objduan = db.DuAns.Where(x => x.IDQuanHuyen == IDQuanHuyen).ToList();
            SelectList obgduan = new SelectList(objduan, "ID", "TenDuAn");
            return Json(obgduan);
        }

        [HttpPost]
        public ActionResult GetLoaiNhaDatByIDLoaiBaiDang(int IDLoaiBaiDang)
        {
            List<LoaiNhaDat> objloainhadat = new List<LoaiNhaDat>();
            objloainhadat = db.LoaiNhaDats.Where(x => x.IDLoaiBaiDang == IDLoaiBaiDang).ToList();
            SelectList obgloainhadat = new SelectList(objloainhadat, "Id", "TenLoaiNhaDat", 0);
            return Json(obgloainhadat);
        }

        [HttpPost]
        public ActionResult GetPhuongXaByIDQuanHuyen(int IDQuanHuyen)
        {
            List<PhuongXa> objphuongxa = new List<PhuongXa>();
            objphuongxa = db.PhuongXas.Where(x => x.IDQuanHuyen == IDQuanHuyen).ToList();
            SelectList obgphuongxa = new SelectList(objphuongxa.OrderBy(o=>o.TenPhuongXa), "ID", "TenPhuongXa");
            return Json(obgphuongxa);
        }        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemBatDongSan([Bind(Include = "TenBatDongSan,IDTinhThanhPho,IDQuanHuyen,IDPhuongXa,DiaChi,gia,IDDonVi,dientich,IDLoaiBaiDang,IDLoaiNhaDat,IDDuAn,mota,SoPhongNgu,SoTang,SoToilet,NoiThat,MatTien,HuongNha,TenLienHe,DiaChiLienHe,SoDienThoaiLienHe,EmailLienHe")] BatDongSan batdongsan, HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                string filename = System.IO.Path.GetFileName(image.FileName);
                string urlimage = Server.MapPath("~/Images" + filename);
                string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(image.FileName));
                image.SaveAs(path);
                image.SaveAs(urlimage);
                batdongsan.Urlimage = "Images/" + filename;

            }            
            if (batdongsan.IDDuAn == 0 || batdongsan.IDDuAn == null)
            {
                batdongsan.IDDuAn = 1;
            }
            batdongsan.Mota = batdongsan.Mota.Replace("\r\n", "<br />");
            batdongsan.TrangThai = 2;
            batdongsan.NgayDang = DateTime.Now;
            batdongsan.NgayCapNhat = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(batdongsan).State = EntityState.Added;
                db.SaveChanges();
                return Redirect("/NhanVien/DSBatDongSan");
            }
            return View(batdongsan);
        }

        public ActionResult DSBatDongSan()
        {
            int idnhanvien = Convert.ToInt32(Session["nhanvien"]);
            if(idnhanvien != 0)
            {
                ViewBag.tennhanvien = db.NhanViens.Where(x => x.ID == idnhanvien).FirstOrDefault().HoTen;
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
            var model = from o in new DB_BatDongSan().BatDongSans.Where(x => x.TrangThai == 3)
                        select new EBatDongSan
                        {
                            ID = o.ID,
                            TenBatDongSan = o.TenBatDongSan,
                            TenQuanHuyen = o.QuanHuyen.TenQuanHuyen,
                            TenDuAn = o.DuAn.TenDuAn,
                            Gia = o.Gia,
                            Gia_DonVi = o.Gia +" "+o.DonVi.TenDonVi,
                            TenDonVi = o.DonVi.TenDonVi,
                            DienTich = o.DienTich,
                            IDLoaiBaiDang = o.IDLoaiBaiDang,
                            TenLoaiBaiDang = o.LoaiBaiDang.TenLoaiBaiDang,
                            Mota = o.Mota,
                            Urlimage = o.Urlimage,
                            TrangThai = o.TrangThai,
                            NgayCapNhat = o.NgayCapNhat,
                            NgayDang = o.NgayDang,
                            DiaChi = o.QuanHuyen.TenQuanHuyen+", "+ o.QuanHuyen.TinhThanhPho.TenTinhThanhPho,
                        };
            if (Session["nhanvien"] != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }        

        public ActionResult XemBatDongSan(int? ID)
        {
            BatDongSan batdongsan = db.BatDongSans.Where(x => x.ID == ID).FirstOrDefault();
            if (batdongsan == null)
                return HttpNotFound();            
            if (Session["nhanvien"] != null)
            {
                return PartialView("XemBatDongSan", batdongsan);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        public ActionResult ThemDuAn()
        {
            int idnhanvien = Convert.ToInt32(Session["nhanvien"]);
            if (idnhanvien != 0)
            {
                ViewBag.tennhanvien = db.NhanViens.Where(x => x.ID == idnhanvien).FirstOrDefault().HoTen;
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
            ViewBag.listloaiduan = db.LoaiDuAns.ToList();
            List<TinhThanhPho> ttp = new List<TinhThanhPho>();
            ttp.Add(new TinhThanhPho() { ID = 0, TenTinhThanhPho = "Tỉnh thành" });
            ttp.AddRange(db.TinhThanhPhoes.ToList());
            ViewBag.listtinhthanh = ttp.OrderBy(o=>o.TenTinhThanhPho);
            ViewBag.listquanhuyen = db.QuanHuyens.Where(x=>x.ID==0).ToList();
            ViewBag.listphuongxa = db.PhuongXas.Where(x => x.ID == 0).ToList();
            if (Session["nhanvien"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemDuAn([Bind(Include = "TenDuan,IDTinhThanhPho,IDQuanHuyen,IDPhuongXa,DiaChi,GiaBan,DonViPhanPhoi,TongDienTich,IDLoaiDuAn,Mota,ChuDauTu,QuyMoDuAn,MatDoXayDung")] DuAn duan, HttpPostedFileBase image)
        {       
            if (image != null && image.ContentLength > 0)
            {
                string filename = System.IO.Path.GetFileName(image.FileName);
                string urlimage = Server.MapPath("~/Images" + filename);
                string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(image.FileName));
                image.SaveAs(path);
                image.SaveAs(urlimage);
                duan.Urlimage = "Images/" + filename;
            }            
            duan.MoTa = duan.MoTa.Replace("\r\n", "<br />");
            duan.TrangThai = 1;
            duan.NgayDang = DateTime.Now;
            duan.NgayCapNhat = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(duan).State = EntityState.Added;
                db.SaveChanges();
                return Redirect("/NhanVien/DSDuAn");
            }
            return View(duan);
        }

        public ActionResult DSDuAn()
        {
            int idnhanvien = Convert.ToInt32(Session["nhanvien"]);
            if (idnhanvien != 0)
            {
                ViewBag.tennhanvien = db.NhanViens.Where(x => x.ID == idnhanvien).FirstOrDefault().HoTen;
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
            var model = from o in new DB_BatDongSan().DuAns.Where(x=>x.ID != 1)
                        select new EDuAn
                        {
                            ID = o.ID,
                            TenDuAn = o.TenDuAn,
                            IDQuanHuyen = o.IDQuanHuyen,
                            TenQuanHuyen = o.QuanHuyen.TenQuanHuyen,
                            DonViPhanPhoi = o.DonViPhanPhoi,
                            TongDienTich = o.TongDienTich,
                            IDLoaiDuAn = o.IDLoaiDuAn,
                            TenLoaiDuAn = o.LoaiDuAn.TenLoaiDuAn,
                            MoTa = o.MoTa,
                            Urlimage = o.Urlimage,
                            TrangThai = o.TrangThai,
                            NgayDang = o.NgayDang,
                            NgayCapNhat = o.NgayCapNhat,
                            DiaChi = o.DiaChi,
                            DienTich_DonVi =o.TongDienTich+" m2",
                            GiaBan_DonVi = o.GiaBan+" Triệu/m2"
                        };
            if (Session["nhanvien"] != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        public ActionResult XemDuAn(int? ID)
        {
            DuAn duan = db.DuAns.Where(x => x.ID == ID).FirstOrDefault();
            if (duan == null)
                return HttpNotFound();
            if (Session["nhanvien"] != null)
            {
                return PartialView("XemDuAn", duan);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        public ActionResult Danhsach_PhanCongSales()
        {
            int idnhanvien = Convert.ToInt32(Session["nhanvien"]);
            if (idnhanvien != 0)
            {
                ViewBag.tennhanvien = db.NhanViens.Where(x => x.ID == idnhanvien).FirstOrDefault().HoTen;
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
            var model = from o in new DB_BatDongSan().BatDongSans.Where(x =>x.TrangThai == 2)
                        select new EBatDongSan
                        {
                            ID = o.ID,
                            TenBatDongSan = o.TenBatDongSan,
                            TenQuanHuyen = o.QuanHuyen.TenQuanHuyen,
                            TenDuAn = o.DuAn.TenDuAn,
                            Gia = o.Gia,
                            TenDonVi = o.DonVi.TenDonVi,
                            DienTich = o.DienTich,
                            IDLoaiBaiDang = o.IDLoaiBaiDang,
                            TenLoaiBaiDang = o.LoaiBaiDang.TenLoaiBaiDang,
                            Mota = o.Mota,
                            Urlimage = o.Urlimage,
                            TrangThai = o.TrangThai,
                            NgayCapNhat = o.NgayCapNhat,
                            NgayDang = o.NgayDang,
                            Gia_DonVi = o.Gia+" "+o.DonVi.TenDonVi,
                            DiaChi = o.QuanHuyen.TenQuanHuyen+", "+o.QuanHuyen.TinhThanhPho.TenTinhThanhPho,
                        };
            if (Session["nhanvien"] != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        public ActionResult PhanCongSales(int? ID)
        {
            BatDongSan bds = db.BatDongSans.Where(m => m.ID == ID).FirstOrDefault();
            if (bds == null)
            {
                return HttpNotFound();
            }
            ViewBag.listsales = db.Sales.ToList();            
            if (Session["nhanvien"] != null)
            {
                return PartialView("PhanCongSales", bds);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        // Hàm Phân Công bất động sản khi click button Phân Công
        [HttpPost]
        public ActionResult PhanCong(int? idbatdongsan,int? idsale)
        {
            BatDongSan batdongsan = db.BatDongSans.Where(s => s.ID == idbatdongsan).FirstOrDefault();
            if (batdongsan == null)
            {
                return HttpNotFound();
            }
            batdongsan.TrangThai = 3;
            db.Entry(batdongsan).State = EntityState.Modified;
            db.SaveChanges();
            PhanCong pc = new PhanCong()
            {
                IDBatDongSan = idbatdongsan,
                IDSales = idsale,
                PhanTramHoaHong = 0.05,
                NgayTao = DateTime.Now
            };
            db.Entry(pc).State = EntityState.Added;
            db.SaveChanges();
            return Redirect("Danhsach_PhanCongSales");
        }       

        public ActionResult noKiemTra()
        {
            int idnhanvien = Convert.ToInt32(Session["nhanvien"]);
            if (idnhanvien != 0)
            {
                ViewBag.tennhanvien = db.NhanViens.Where(x => x.ID == idnhanvien).FirstOrDefault().HoTen;
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
            if (Session["nhanvien"] != null)
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
            int idnhanvien = Convert.ToInt32(Session["nhanvien"]);
            if (idnhanvien != 0)
            {
                ViewBag.tennhanvien = db.NhanViens.Where(x => x.ID == idnhanvien).FirstOrDefault().HoTen;
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
            var nhanvien = db.NhanViens.Where(x => x.ID == idnhanvien).FirstOrDefault();
            if (nhanvien.ChoKiemTra == 0)
                return View("noKiemTra");
            ViewBag.listcauhoi = db.CauHois.ToList();
            ViewBag.listtraloi = db.TraLois.ToList();
            ViewBag.dotkiemtra = db.CauHois.Where(x => x.ID == 1).FirstOrDefault().DotThi;
            if (Session["nhanvien"] != null)
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
                TenBaiThi = "Đợt "+dotthi,
                SoDiem = diem * 5,
                NgayThi = DateTime.Now,
                IDNhanVien = Convert.ToInt32(Session["nhanvien"]),
            };
            db.Entry(d).State = EntityState.Added;
            int idnhanvien = Convert.ToInt32(Session["nhanvien"]);
            var nv = db.NhanViens.Where(x => x.ID == idnhanvien).FirstOrDefault();
            nv.ChoKiemTra = 0;
            db.Entry(nv).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect("Index");
        }

        public ActionResult XemDiem()
        {
            int idnhanvien = Convert.ToInt32(Session["nhanvien"]);
            if (idnhanvien != 0)
            {
                ViewBag.tennhanvien = db.NhanViens.Where(x => x.ID == idnhanvien).FirstOrDefault().HoTen;
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
            var listdiem = db.DIEMs.Where(x => x.IDNhanVien == idnhanvien).ToList();            
            if (Session["nhanvien"] != null)
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