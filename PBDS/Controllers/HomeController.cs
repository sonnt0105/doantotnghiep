using PBDS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace PBDS.Controllers
{
    public class HomeController : Controller
    {
        DB_BatDongSan db  = new DB_BatDongSan();
        public int pageSize_bds = 6;
        public int pageSize_duan = 8;

        public ActionResult Trangchu()
        {
            ViewBag.listloainhadat = db.LoaiNhaDats.Where(x => x.IDLoaiBaiDang == 1).ToList();
            List<TinhThanhPho> ttp = new List<TinhThanhPho>();
            ttp.Add(new TinhThanhPho() { ID = 0, TenTinhThanhPho = "Tất cả tỉnh thành" });
            ttp.AddRange(db.TinhThanhPhoes.ToList());
            ViewBag.listtinhthanh = ttp.OrderBy(o => o.TenTinhThanhPho);
            ViewBag.listquanhuyen = db.QuanHuyens.Where(x => x.ID == 0).ToList();
            ViewBag.listphuongxa = db.PhuongXas.Where(x => x.ID == 0).ToList();
            ViewBag.listduan = db.DuAns.Where(x => x.ID == 0).ToList();
            
            var batdongsan1 = db.BatDongSans.Where(x=>x.TrangThai == 3).ToList().Take(8);
            ViewBag.batdongsan2 = db.BatDongSans.Where(x => x.TrangThai == 3).ToList().Skip(8).Take(8);
            ViewBag.tdtphcm = db.BatDongSans.Where(x => x.QuanHuyen.TinhThanhPho.ID == 140).Where(x => x.TrangThai == 3).Count();
            ViewBag.tdtphanoi = db.BatDongSans.Where(x => x.QuanHuyen.TinhThanhPho.ID == 1).Where(x => x.TrangThai == 3).Count();
            ViewBag.tdtpdanang = db.BatDongSans.Where(x => x.QuanHuyen.TinhThanhPho.ID == 4).Where(x => x.TrangThai == 3).Count();
            ViewBag.tdtinhdongnai = db.BatDongSans.Where(x => x.QuanHuyen.TinhThanhPho.ID == 96).Where(x => x.TrangThai == 3).Count();
            ViewBag.tdtpbinhduong = db.BatDongSans.Where(x => x.QuanHuyen.TinhThanhPho.ID == 77).Where(x => x.TrangThai == 3).Count();
            ViewBag.listduannoibat = db.DuAns.Where(x => x.ID != 1).Take(5).ToList();
            return View(batdongsan1);
        }                   
        
        public ActionResult Detail_batdongsan(int? ID)
        {
            var batdongsan = db.BatDongSans.Where(x => x.ID == ID).FirstOrDefault();
            if (batdongsan == null)
                return HttpNotFound();

            List<LoaiNhaDat> lnd = new List<LoaiNhaDat>();
            lnd.Add(new LoaiNhaDat() { ID = 0, TenLoaiNhaDat = "Tất cả loại nhà đất" });
            if(db.BatDongSans.Where(x=>x.ID == ID).FirstOrDefault().IDLoaiBaiDang == 1)
                lnd.AddRange(db.LoaiNhaDats.Where(x => x.IDLoaiBaiDang == 1).ToList());
            else
                lnd.AddRange(db.LoaiNhaDats.Where(x => x.IDLoaiBaiDang == 2).ToList());
            ViewBag.listloainhadat = lnd;

            List<TinhThanhPho> ttp = new List<TinhThanhPho>();
            ttp.Add(new TinhThanhPho() { ID = 0, TenTinhThanhPho = "Tất cả tỉnh thành" });
            ttp.AddRange(db.TinhThanhPhoes.ToList());
            ViewBag.listtinhthanh = ttp.OrderBy(o => o.TenTinhThanhPho);

            List<QuanHuyen> qh = new List<QuanHuyen>();
            qh.Add(new QuanHuyen() { ID = 0, TenQuanHuyen = "Tất cả quận huyện" });
            qh.AddRange(db.QuanHuyens.Where(x=>x.IDTinhThanhPho == batdongsan.IDTinhThanhPho).ToList());
            ViewBag.listquanhuyen = qh;

            ViewBag.listphuongxa = db.PhuongXas.Where(x => x.ID == 0).ToList();
            ViewBag.listduan = db.DuAns.Where(x => x.ID == 0).ToList();            
            ViewBag.nhanviensale = db.PhanCongs.Where(x => x.IDBatDongSan == ID).FirstOrDefault();
            return View(batdongsan);
        }       

        public ActionResult ThemBatDongSan()
        {
            ViewBag.listloaibaidang = db.LoaiBaiDangs.ToList();
            ViewBag.listquanhuyen = db.QuanHuyens.Where(x => x.ID == 0).ToList();
            List<TinhThanhPho> ttp = new List<TinhThanhPho>();
            ttp.Add(new TinhThanhPho() { ID = 0, TenTinhThanhPho = "Tỉnh thành" });
            ttp.AddRange(db.TinhThanhPhoes.ToList());
            ViewBag.listtinhthanh = ttp.OrderBy(o => o.TenTinhThanhPho);
            ViewBag.listdonvi = db.DonVis.Where(x => x.IDLoaiBaiDang == 0).ToList();
            ViewBag.listduan = db.DuAns.Where(x => x.ID == 0).ToList();
            ViewBag.listloainhadat = db.LoaiNhaDats.Where(x => x.ID == 0).ToList();
            ViewBag.listphuongxa = db.PhuongXas.Where(x => x.ID == 0).ToList();            
            return View();
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
            batdongsan.TrangThai = 1;
            batdongsan.NgayDang = DateTime.Now;
            batdongsan.NgayCapNhat = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(batdongsan).State = EntityState.Added;
                db.SaveChanges();
                return Redirect("/Home/ThemBatDongSan");
            }
            return View(batdongsan);
        }

        [HttpPost]
        public ActionResult GetDonViByIDLoaiBaiDang(int IDLoaiBaiDang)
        {
            List<DonVi> objdonvi = new List<DonVi>();
            objdonvi = db.DonVis.Where(x => x.IDLoaiBaiDang == IDLoaiBaiDang).ToList();
            SelectList obgdonvi = new SelectList(objdonvi, "Id", "TenDonVi", 0);
            return Json(obgdonvi);
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
            SelectList obgphuongxa = new SelectList(objphuongxa.OrderBy(o => o.TenPhuongXa), "ID", "TenPhuongXa");
            return Json(obgphuongxa);
        }        

        public ActionResult Login()
        {
            return View();
        }

        //Đăng nhập
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string tentaikhoan, string matkhau)
        {
            if (ModelState.IsValid)
            {
                var f_password = matkhau;
                var dataadmin = db.Admins.Where(s => s.TenTaiKhoan.Equals(tentaikhoan) && s.MatKhau.Equals(f_password)).ToList();
                var datanhanvien = db.NhanViens.Where(s => s.TaiKhoan.Equals(tentaikhoan) && s.MatKhau.Equals(f_password)).ToList();
                var datasale = db.Sales.Where(s => s.TaiKhoan.Equals(tentaikhoan) && s.MatKhau.Equals(f_password)).ToList();
                Session.Clear();
                if (datanhanvien.Count() > 0)
                {
                    //add session
                    Session["nhanvien"] = datanhanvien.FirstOrDefault().ID;
                    return RedirectToAction("Index", "NhanVien");
                }
                if (dataadmin.Count() > 0)
                {
                    //add session
                    Session["admin"] = dataadmin.FirstOrDefault().ID;
                    return RedirectToAction("Index", "Admin");
                }
                if (datasale.Count() > 0)
                {
                    //add session
                    Session["sale"] = datasale.FirstOrDefault().ID;
                    return RedirectToAction("Index", "Sale");
                }
                else
                {
                    ViewBag.Message = "Sai tài khoản hoặc mật khẩu";
                    ModelState.AddModelError("LogOnError", "Tài khoản hoặc mật khẩu không đúng");
                    return View("Login");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult GetQuanHuyenByIDTinhThanhPho(int IDTinhThanhPho)
        {
            List<QuanHuyen> objquanhuyen = new List<QuanHuyen>();
            objquanhuyen = db.QuanHuyens.Where(x => x.IDTinhThanhPho == IDTinhThanhPho).ToList();
            SelectList obgquanhuyen = new SelectList(objquanhuyen.OrderBy(o=>o.TenQuanHuyen), "Id", "TenQuanHuyen", 0);
            return Json(obgquanhuyen);
        }

        [HttpPost]
        public ActionResult GetDuAnbyIDQuanHuyen(int IDQuanHuyen)
        {
            List<DuAn> objduan = new List<DuAn>();
            objduan = db.DuAns.Where(x => x.IDQuanHuyen == IDQuanHuyen).Where(x=>x.ID != 1).ToList();
            SelectList obgduan = new SelectList(objduan, "ID", "TenDuAn");        

            return Json(obgduan);
        }

        public ActionResult NhaDatBan(int? page)
        {
            var model = from o in new DB_BatDongSan().BatDongSans.Where(x => x.TrangThai == 3).Where(x=>x.IDLoaiBaiDang == 1)
                        select new EBatDongSan
                        {
                            ID = o.ID,
                            TenBatDongSan = o.TenBatDongSan,
                            TenQuanHuyen = o.QuanHuyen.TenQuanHuyen,
                            TenTinhThanhPho = o.QuanHuyen.TinhThanhPho.TenTinhThanhPho,
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
                        };            
            ViewBag.listloainhadat = db.LoaiNhaDats.Where(x => x.IDLoaiBaiDang == 1).ToList();
            List<TinhThanhPho> ttp = new List<TinhThanhPho>();
            ttp.Add(new TinhThanhPho() { ID = 0, TenTinhThanhPho = "Tất cả tỉnh thành" });
            ttp.AddRange(db.TinhThanhPhoes.ToList());
            ViewBag.listtinhthanh = ttp.OrderBy(o => o.TenTinhThanhPho);
            ViewBag.listquanhuyen = db.QuanHuyens.Where(x => x.ID == 0).ToList();
            ViewBag.listphuongxa = db.PhuongXas.Where(x => x.ID == 0).ToList();
            ViewBag.listduan = db.DuAns.Where(x => x.ID == 0).ToList();

            if (page > 0)
            {
                page = page;
            }
            else
            {
                page = 1;
            }
            int start = (int)(page - 1) * pageSize_bds;
            ViewBag.pageCurrent = page;
            int totalPage = model.Count();
            float totalNumsize = (totalPage / (float)pageSize_bds);
            int numSize = (int)Math.Ceiling(totalNumsize);
            ViewBag.numSize = numSize;
            model = model.OrderByDescending(x => x.ID).Skip(start).Take(pageSize_bds);
            return View(model);
        }
        

        public ActionResult NhaDatChoThue(int? page)
        {
            var model = from o in new DB_BatDongSan().BatDongSans.Where(x => x.TrangThai == 3).Where(x => x.IDLoaiBaiDang == 2)
                        select new EBatDongSan
                        {
                            ID = o.ID,
                            TenBatDongSan = o.TenBatDongSan,
                            TenQuanHuyen = o.QuanHuyen.TenQuanHuyen,
                            TenTinhThanhPho = o.QuanHuyen.TinhThanhPho.TenTinhThanhPho,
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
                        };
            ViewBag.listloainhadat = db.LoaiNhaDats.Where(x => x.IDLoaiBaiDang == 2).ToList();
            List<TinhThanhPho> ttp = new List<TinhThanhPho>();
            ttp.Add(new TinhThanhPho() { ID = 0, TenTinhThanhPho = "Tất cả tỉnh thành" });
            ttp.AddRange(db.TinhThanhPhoes.ToList());
            ViewBag.listtinhthanh = ttp.OrderBy(o => o.TenTinhThanhPho);
            ViewBag.listquanhuyen = db.QuanHuyens.Where(x => x.ID == 0).ToList();
            ViewBag.listphuongxa = db.PhuongXas.Where(x => x.ID == 0).ToList();
            ViewBag.listduan = db.DuAns.Where(x => x.ID == 0).ToList();

            if (page > 0)
            {
                page = page;
            }
            else
            {
                page = 1;
            }            
            int start = (int)(page - 1) * pageSize_bds;
            ViewBag.pageCurrent = page;
            int totalPage = model.Count();
            float totalNumsize = (totalPage / (float)pageSize_bds);
            int numSize = (int)Math.Ceiling(totalNumsize);
            ViewBag.numSize = numSize;
            model = model.OrderByDescending(x => x.ID).Skip(start).Take(pageSize_bds);
            return View(model);
        }
        
        public ActionResult TimkiemBaiDang(EBatDongSansearch batdongsansearch, int? page)
        {
            var danhsachbatdongsan = GetBatDongSan(batdongsansearch);
            var model = from o in danhsachbatdongsan
                        select new EBatDongSan
                        {
                            ID = o.ID,
                            TenBatDongSan = o.TenBatDongSan,
                            TenQuanHuyen = o.QuanHuyen.TenQuanHuyen,
                            IDQuanHuyen = o.IDQuanHuyen,
                            TenTinhThanhPho = o.QuanHuyen.TinhThanhPho.TenTinhThanhPho,
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
                        };
            if (batdongsansearch.IDLoaiNhaDat.HasValue && batdongsansearch.IDLoaiNhaDat != 0)
                batdongsansearch.TenLoaiNhaDat = db.LoaiNhaDats.Where(x => x.ID == batdongsansearch.IDLoaiNhaDat).FirstOrDefault().TenLoaiNhaDat;
            if(batdongsansearch.IDTinhThanhPho.HasValue && batdongsansearch.IDTinhThanhPho != 0)
                batdongsansearch.TenTinhThanhPho = db.TinhThanhPhoes.Where(x => x.ID == batdongsansearch.IDTinhThanhPho).FirstOrDefault().TenTinhThanhPho;
            if(batdongsansearch.IDQuanHuyen.HasValue && batdongsansearch.IDQuanHuyen != 0)
                batdongsansearch.TenQuanHuyen = db.QuanHuyens.Where(x => x.ID == batdongsansearch.IDQuanHuyen).FirstOrDefault().TenQuanHuyen;
            batdongsansearch = Getebatdongsansearch(batdongsansearch);
            ViewBag.batdongsansearch = batdongsansearch;
            ViewBag.textsearch = batdongsansearch.searchstring;
            ViewBag.listloainhadat = db.LoaiNhaDats.Where(x => x.IDLoaiBaiDang == 1).ToList();
            List<TinhThanhPho> ttp = new List<TinhThanhPho>();
            ttp.Add(new TinhThanhPho() { ID = 0, TenTinhThanhPho = "Tất cả tỉnh thành" });
            ttp.AddRange(db.TinhThanhPhoes.ToList());
            ViewBag.listtinhthanh = ttp.OrderBy(o => o.TenTinhThanhPho);
            ViewBag.countbatdongsan = model.Count();            
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

            if (batdongsansearch.IDQuanHuyen.HasValue)
                ViewBag.listphuongxa = db.PhuongXas.Where(x => x.IDQuanHuyen == batdongsansearch.IDQuanHuyen).ToList();
            else
                ViewBag.listphuongxa = db.PhuongXas.Where(x => x.ID == 0).ToList();

            if (batdongsansearch.IDQuanHuyen.HasValue)
                ViewBag.listduan = db.DuAns.Where(x => x.IDQuanHuyen == batdongsansearch.IDQuanHuyen).ToList();
            else
                ViewBag.listduan = db.DuAns.Where(x => x.ID == 0).ToList();

            if (page > 0)
            {
                page = page;
            }
            else
            {
                page = 1;
            }            
            int start = (int)(page - 1) * pageSize_bds;
            ViewBag.pageCurrent = page;
            int totalPage = model.Count();
            float totalNumsize = (totalPage / (float)pageSize_bds);
            int numSize = (int)Math.Ceiling(totalNumsize);
            ViewBag.numSize = numSize;
            model = model.OrderByDescending(x => x.ID).Skip(start).Take(pageSize_bds);

            return View("TimKiemBaiDang",model);
        }
        public EBatDongSansearch Getebatdongsansearch(EBatDongSansearch batdongsansearch)
        {
            EBatDongSansearch ebds = new EBatDongSansearch();
            ebds = batdongsansearch;
            if (batdongsansearch.IDLoaiBaiDang.HasValue)
                ebds.LoaiBaiDang = db.BatDongSans.Where(x => x.IDLoaiBaiDang == batdongsansearch.IDLoaiBaiDang).FirstOrDefault().LoaiBaiDang;
            if (batdongsansearch.IDLoaiNhaDat.HasValue && batdongsansearch.IDLoaiNhaDat != 0 && db.BatDongSans.Where(x => x.IDLoaiNhaDat == batdongsansearch.IDLoaiNhaDat).FirstOrDefault() != null)
                ebds.LoaiNhaDat = db.BatDongSans.Where(x => x.IDLoaiNhaDat == batdongsansearch.IDLoaiNhaDat).FirstOrDefault().LoaiNhaDat;
            if (batdongsansearch.IDQuanHuyen.HasValue && batdongsansearch.IDQuanHuyen != 0 && db.BatDongSans.Where(x => x.IDQuanHuyen == batdongsansearch.IDQuanHuyen).FirstOrDefault() != null)
                ebds.QuanHuyen = db.BatDongSans.Where(x => x.IDQuanHuyen == batdongsansearch.IDQuanHuyen).FirstOrDefault().QuanHuyen;
            if (batdongsansearch.IDTinhThanhPho.HasValue && batdongsansearch.IDTinhThanhPho != 0 && db.BatDongSans.Where(x => x.IDTinhThanhPho == batdongsansearch.IDTinhThanhPho).FirstOrDefault() != null)
                ebds.TinhThanhPho = db.BatDongSans.Where(x => x.IDTinhThanhPho == batdongsansearch.IDTinhThanhPho).FirstOrDefault().TinhThanhPho;
            if (batdongsansearch.IDDuAn.HasValue && batdongsansearch.IDDuAn != 0 && db.BatDongSans.Where(x => x.IDDuAn == batdongsansearch.IDDuAn).FirstOrDefault() != null)
                ebds.DuAn = db.BatDongSans.Where(x => x.IDDuAn == batdongsansearch.IDDuAn).FirstOrDefault().DuAn;
            return ebds;
        }

        public IQueryable<BatDongSan> GetBatDongSan(EBatDongSansearch batdongsansearch)
        {
            var result = db.BatDongSans.Where(x => x.TrangThai == 3).ToList();
            if(batdongsansearch != null)
            {
                //search theo điều kiện loại bài đăng
                if (batdongsansearch.IDLoaiBaiDang.HasValue)
                    result = result.Where(x => x.IDLoaiBaiDang == batdongsansearch.IDLoaiBaiDang).ToList();
                //search theo điều kiện loại nhà đất
                if(batdongsansearch.IDLoaiNhaDat.HasValue && batdongsansearch.IDLoaiNhaDat != 0)
                    result = result.Where(x => x.IDLoaiNhaDat == batdongsansearch.IDLoaiNhaDat).ToList();
                //search theo điều kiện tỉnh thành phố
                if(batdongsansearch.IDTinhThanhPho.HasValue && batdongsansearch.IDTinhThanhPho != 0)
                    result = result.Where(x => x.QuanHuyen.IDTinhThanhPho == batdongsansearch.IDTinhThanhPho).ToList();
                //search theo điều kiện quận huyện
                if (batdongsansearch.IDQuanHuyen.HasValue && batdongsansearch.IDQuanHuyen != 0)
                    result = result.Where(x => x.IDQuanHuyen == batdongsansearch.IDQuanHuyen).ToList();

                //search theo điều kiện mức giá lớn hơn mức giá X
                if (batdongsansearch.GiaTu.HasValue)
                    result = TimKiemBaiDangTheoGiaTu(result, batdongsansearch.GiaTu);
                //search theo điều kiện mức giá nhỏ hơn mức giá X
                if (batdongsansearch.GiaDen.HasValue)
                    result = TimKiemBaiDangTheoGiaDen(result, batdongsansearch.GiaDen);

                //search theo điều kiện phường xã
                if (batdongsansearch.IDPhuongXa.HasValue && batdongsansearch.IDPhuongXa != 0)
                    result = result.Where(x => x.IDPhuongXa == batdongsansearch.IDPhuongXa).ToList();
                //search theo điều kiện diện dự án
                if (batdongsansearch.IDDuAn.HasValue && batdongsansearch.IDDuAn != 0)
                    result = result.Where(x => x.IDDuAn == batdongsansearch.IDDuAn).ToList();
                //search theo chuỗi
                if (!string.IsNullOrEmpty(batdongsansearch.searchstring))
                {
                    List<BatDongSan> listbds = new List<BatDongSan>();
                    foreach (var item_result in result)
                        foreach (var item_search in TimKiemBaiDangTheoChuoi(batdongsansearch.searchstring))
                            if (item_result.ID == item_search.ID)
                                listbds.Add(item_result);
                        return listbds.AsQueryable();
                }                    
            }
            return result.AsQueryable();
        }

        public List<BatDongSan> TimKiemBaiDangTheoGiaTu(List<BatDongSan> listbatdongsan, double? giatu)
        {
            var result = listbatdongsan;
            foreach (var item in listbatdongsan)
            {
                if (item.IDDonVi == 2)
                    item.Gia = item.Gia * 1000;
                if (item.IDDonVi == 3 || item.IDDonVi == 5|| item.IDDonVi == 8)
                    item.Gia = item.Gia/10;
                if (item.IDDonVi == 7)
                    item.Gia = item.Gia / 1000;
            }
            result = result.Where(x => x.Gia >= giatu).ToList();
            foreach (var item in result)
            {
                if (item.IDDonVi == 2)
                    item.Gia = item.Gia / 1000;
                if (item.IDDonVi == 3 || item.IDDonVi == 5 || item.IDDonVi == 8)
                    item.Gia = item.Gia * 10;
                if (item.IDDonVi == 7)
                    item.Gia = item.Gia * 1000;
            }
            return result;
        }

        public List<BatDongSan> TimKiemBaiDangTheoGiaDen(List<BatDongSan> listbatdongsan, double? giaden)
        {
            var result = listbatdongsan;
            foreach (var item in listbatdongsan)
            {
                if (item.IDDonVi == 2)
                    item.Gia = item.Gia * 1000;
                if (item.IDDonVi == 3 || item.IDDonVi == 5 || item.IDDonVi == 8)
                    item.Gia = item.Gia / 10;
                if (item.IDDonVi == 7)
                    item.Gia = item.Gia / 1000;
            }
            result = result.Where(x => x.Gia <= giaden).ToList();
            foreach (var item in result)
            {
                if (item.IDDonVi == 2)
                    item.Gia = item.Gia / 1000;
                if (item.IDDonVi == 3 || item.IDDonVi == 5 || item.IDDonVi == 8)
                    item.Gia = item.Gia * 10;
                if (item.IDDonVi == 7)
                    item.Gia = item.Gia * 1000;
            }
            return result;
        }

        public List<BatDongSan> TimKiemBaiDangTheoChuoi(string searchstring)
        {
            var tatcabds_hoatdong = db.BatDongSans.Where(x => x.TrangThai == 3).ToList();
            var result = tatcabds_hoatdong.Where(x => x.ID == 0).ToList();
            searchstring = convertToLatin(searchstring);// hàm loại bỏ dấu của tiếng việt
            string[] chuoi_latinsearch = searchstring.Split(' ');// hàm cắt chuỗi thành từng từ
            foreach(var item in db.BatDongSans.ToList())
            {
                int tenbds_trung = 0;
                // kiếm tra theo tên bất động sản
                for(int i = 0; i < chuoi_latinsearch.Length; i++)
                {
                    string tenbds = convertToLatin(item.TenBatDongSan);// loại bỏ dấu trong tiếng việt
                    string[] chuoi_tenbds = tenbds.Split(' ');// cắt chuỗi thành từng từ
                    for(int j = 0; j < chuoi_tenbds.Length; j++)
                    {
                        if (chuoi_latinsearch[i] == chuoi_tenbds[j])
                        {
                            tenbds_trung++;
                            // trùng 1 từ trong tên là hiện
                            if(tenbds_trung >= 1)
                            {
                                result.Add(item);
                                goto End;
                            }
                        }
                    }
                }
                // kiểm tra theo loại nhà đất
                int loainhadat_trung = 0;
                for (int i = 0; i < chuoi_latinsearch.Length; i++)
                {
                    string tenloainhadat = convertToLatin(item.LoaiNhaDat.TenLoaiNhaDat);// loại bỏ dấu trong tiếng việt
                    string[] chuoi_tenloainhadat = tenloainhadat.Split(' ');// cắt chuỗi thành từng từ
                    for (int j = 0; j < chuoi_tenloainhadat.Length; j++)
                    {
                        if (chuoi_latinsearch[i] == chuoi_tenloainhadat[j])
                        {
                            loainhadat_trung++;
                            // trùng 2 từ trong tên là hiện
                            if (loainhadat_trung >= 2)
                            {
                                result.Add(item);
                                goto End;
                            }
                        }
                    }
                }
                // kiểm tra theo dự án
                int duan_trung = 0;
                for (int i = 0; i < chuoi_latinsearch.Length; i++)
                {
                    string tenduan = convertToLatin(item.DuAn.TenDuAn);// loại bỏ dấu trong tiếng việt
                    string[] chuoi_tenduan = tenduan.Split(' ');// cắt chuỗi thành từng từ
                    for (int j = 0; j < chuoi_tenduan.Length; j++)
                    {
                        if (chuoi_latinsearch[i] == chuoi_tenduan[j])
                        {
                            duan_trung++;
                            if (duan_trung >= 2)
                            {
                                result.Add(item);
                                goto End;
                            }
                        }
                    }
                }
                // kiểm tra theo tỉnh thành
                int tinhthanh_trung = 0;
                for (int i = 0; i < chuoi_latinsearch.Length; i++)
                {
                    string tentinhthanh = convertToLatin(item.QuanHuyen.TinhThanhPho.TenTinhThanhPho);// loại bỏ dấu trong tiếng việt
                    string[] chuoi_tentinhthanh = tentinhthanh.Split(' ');// cắt chuỗi thành từng từ
                    for (int j = 0; j < chuoi_tentinhthanh.Length; j++)
                    {
                        if (chuoi_latinsearch[i] == chuoi_tentinhthanh[j])
                        {
                            tinhthanh_trung++;
                            if (tinhthanh_trung >= 2)
                            {
                                result.Add(item);
                                goto End;
                            }
                        }
                    }
                }
                // kiểm tra theo quận huyện
                int quanhuyen_trung = 0;
                for (int i = 0; i < chuoi_latinsearch.Length; i++)
                {
                    string tenquanhuyen = convertToLatin(item.QuanHuyen.TenQuanHuyen);// loại bỏ dấu trong tiếng việt
                    string[] chuoi_tenquanhuyen = tenquanhuyen.Split(' ');// cắt chuỗi thành từng từ
                    for (int j = 0; j < chuoi_tenquanhuyen.Length; j++)
                    {
                        if (chuoi_latinsearch[i] == chuoi_tenquanhuyen[j])
                        {
                            quanhuyen_trung++;
                            if (quanhuyen_trung >= 2)
                            {
                                result.Add(item);
                                goto End;
                            }
                        }
                    }
                }
                // kiểm tra theo phường xã
                int phuongxa_trung = 0;
                if(item.IDPhuongXa != null)
                {
                    for (int i = 0; i < chuoi_latinsearch.Length; i++)
                    {
                        string tenphuongxa = convertToLatin(item.PhuongXa.TenPhuongXa);// loại bỏ dấu trong tiếng việt
                        string[] chuoi_tenphuongxa = tenphuongxa.Split(' ');// cắt chuỗi thành từng từ
                        for (int j = 0; j < chuoi_tenphuongxa.Length; j++)
                        {
                            if (chuoi_latinsearch[i] == chuoi_tenphuongxa[j])
                            {
                                phuongxa_trung++;
                                if (phuongxa_trung >= 2)
                                {
                                    result.Add(item);
                                    goto End;
                                }
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

        public ActionResult Duan()
        {
            var model = from o in new DB_BatDongSan().DuAns.Where(x => x.ID != 1).Take(16)
                        select new EDuAn
                        {
                            ID = o.ID,
                            TenDuAn = o.TenDuAn,
                            TenQuanHuyen = o.QuanHuyen.TenQuanHuyen,
                            TenTinhThanh = o.QuanHuyen.TinhThanhPho.TenTinhThanhPho,
                            TenPhuongXa = o.PhuongXa.TenPhuongXa,
                            GiaBan = o.GiaBan,
                            DonViPhanPhoi = o.DonViPhanPhoi,
                            TongDienTich = o.TongDienTich,
                            IDLoaiDuAn = o.IDLoaiDuAn,
                            TenLoaiDuAn = o.LoaiDuAn.TenLoaiDuAn,
                            MoTa = o.MoTa,
                            Urlimage = o.Urlimage,
                            TrangThai = o.TrangThai,
                            NgayCapNhat = o.NgayCapNhat,
                            NgayDang = o.NgayDang,                            
                        };
            ViewBag.listloaiduan = db.LoaiDuAns.ToList();
            List<TinhThanhPho> ttp = new List<TinhThanhPho>();
            ttp.Add(new TinhThanhPho() { ID = 0, TenTinhThanhPho = "Tất cả tỉnh thành" });
            ttp.AddRange(db.TinhThanhPhoes.ToList());
            ViewBag.listtinhthanh = ttp.OrderBy(o=>o.TenTinhThanhPho);
            ViewBag.listquanhuyen = db.QuanHuyens.Where(x=>x.ID==0).ToList();
            ViewBag.listduankhunghiduongsinhthai = db.DuAns.Where(x => x.IDLoaiDuAn == 6).ToList().Take(6);
            ViewBag.listduankhucongnghiep = db.DuAns.Where(x => x.IDLoaiDuAn == 7).ToList().Take(6);
            ViewBag.listduancanhochungcu = db.DuAns.Where(x => x.IDLoaiDuAn == 1).ToList().Take(6);
            return View(model);
        }        

        public ActionResult Detail_duan(int? ID)
        {
            var duan = db.DuAns.Where(x => x.ID == ID).FirstOrDefault();
            if (duan == null)
                return HttpNotFound();

            List<LoaiDuAn> lda = new List<LoaiDuAn>();
            lda.Add(new LoaiDuAn() { ID = 0, TenLoaiDuAn = "Tất cả loại dự án" });
            lda.AddRange(db.LoaiDuAns.ToList());
            ViewBag.listloaiduan = lda;

            List<TinhThanhPho> ttp = new List<TinhThanhPho>();
            ttp.Add(new TinhThanhPho() { ID = 0, TenTinhThanhPho = "Tất cả tỉnh thành" });
            ttp.AddRange(db.TinhThanhPhoes.ToList());
            ViewBag.listtinhthanh = ttp.OrderBy(o => o.TenTinhThanhPho);

            List<QuanHuyen> qh = new List<QuanHuyen>();
            qh.Add(new QuanHuyen() { ID = 0, TenQuanHuyen = "Tất cả quận huyện" });
            qh.AddRange(db.QuanHuyens.Where(x => x.IDTinhThanhPho == duan.IDTinhThanhPho).ToList());
            ViewBag.listquanhuyen = qh;

            var listbatdognsanthuocduan = db.BatDongSans.Where(x => x.IDDuAn == duan.ID).ToList();
            var listloainhandat = new List<LoaiNhaDat>();
            foreach (var item in listbatdognsanthuocduan)
            {
                var loainhadat = db.LoaiNhaDats.Where(x => x.ID == item.IDLoaiNhaDat).FirstOrDefault();
                if (listloainhandat.Contains(loainhadat))
                    continue;
                listloainhandat.Add(loainhadat);
            }
            ViewBag.tinraothuocduan = listloainhandat;
            return View(duan);
        }

        public ActionResult TimKiemDuAn(eDuAnSearch duansearch, int? page)
        {
            var danhsachduan = GetDuAn(duansearch);
            var model = from o in danhsachduan
                        select new DuAn
                        {
                            ID = o.ID,
                            TenDuAn = o.TenDuAn,
                            QuanHuyen = o.QuanHuyen,
                            PhuongXa = o.PhuongXa,
                            TinhThanhPho = o.TinhThanhPho,
                            GiaBan = o.GiaBan,
                            DiaChi = o.DiaChi,
                            DonViPhanPhoi = o.DonViPhanPhoi,
                            TongDienTich = o.TongDienTich,
                            IDLoaiDuAn = o.IDLoaiDuAn,
                            LoaiDuAn = o.LoaiDuAn,
                            MoTa = o.MoTa,
                            Urlimage = o.Urlimage,
                            TrangThai = o.TrangThai,
                            NgayCapNhat = o.NgayCapNhat,
                            NgayDang = o.NgayDang,
                        };
            ViewBag.duansearch = duansearch;
            ViewBag.textsearch = duansearch.searchstring;
            List<TinhThanhPho> ttp = new List<TinhThanhPho>();
            ttp.Add(new TinhThanhPho() { ID = 0, TenTinhThanhPho = "Tất cả tỉnh thành" });
            ttp.AddRange(db.TinhThanhPhoes.ToList());
            ViewBag.listtinhthanh = ttp.OrderBy(o => o.TenTinhThanhPho);
            ViewBag.listloaiduan = db.LoaiDuAns.ToList();
            if (duansearch.IDTinhThanhPho.HasValue)
            {
                List<QuanHuyen> qh = new List<QuanHuyen>();
                qh.Add(new QuanHuyen() { ID = 0, TenQuanHuyen = "Tất cả quận huyện" });
                qh.AddRange(db.QuanHuyens.Where(x => x.IDTinhThanhPho == duansearch.IDTinhThanhPho).ToList());
                ViewBag.listquanhuyen = qh.OrderBy(o => o.TenQuanHuyen);
            }
            else
            {
                List<QuanHuyen> qh = new List<QuanHuyen>();
                qh.Add(new QuanHuyen() { ID = 0, TenQuanHuyen = "Tất cả quận huyện" });
                ViewBag.listquanhuyen = qh;
            }           

            if (page > 0)
            {
                page = page;
            }
            else
            {
                page = 1;
            }
            int start = (int)(page - 1) * pageSize_duan;
            ViewBag.pageCurrent = page;
            int totalPage = model.Count();
            float totalNumsize = (totalPage / (float)pageSize_duan);
            int numSize = (int)Math.Ceiling(totalNumsize);
            ViewBag.numSize = numSize;
            model = model.OrderByDescending(x => x.ID).Skip(start).Take(pageSize_duan);

            return View(model);
        }

        public IQueryable<DuAn> GetDuAn(eDuAnSearch duansearch)
        {
            var result = db.DuAns.Where(x => x.ID != 1).ToList();
            if (duansearch != null)
            {
                //search theo loại dự án
                if(duansearch.IDLoaiDuAn.HasValue && duansearch.IDLoaiDuAn !=0)
                    result = result.Where(x => x.IDLoaiDuAn == duansearch.IDLoaiDuAn).ToList();
                //search theo điều kiện tỉnh thành phố
                if (duansearch.IDTinhThanhPho.HasValue && duansearch.IDTinhThanhPho != 0)
                    result = result.Where(x => x.QuanHuyen.IDTinhThanhPho == duansearch.IDTinhThanhPho) .ToList();
                //search theo điều kiện quận huyện
                if (duansearch.IDQuanHuyen.HasValue && duansearch.IDQuanHuyen != 0)
                    result = result.Where(x => x.IDQuanHuyen == duansearch.IDQuanHuyen).ToList();               
                //search theo chuỗi
                if (!string.IsNullOrEmpty(duansearch.searchstring))
                {
                    List<DuAn> listduan = new List<DuAn>();
                    foreach (var item_result in result)
                        foreach (var item_search in TimKiemDuAnTheoChuoi(duansearch.searchstring))
                            if (item_result.ID == item_search.ID)
                                listduan.Add(item_result);
                    return listduan.AsQueryable();
                }
            }
            return result.AsQueryable();
        }

        public List<DuAn> TimKiemDuAnTheoChuoi(string searchstring)
        {
            var tatcabds_hoatdong = db.DuAns.Where(x => x.ID != 1).ToList();
            var result = tatcabds_hoatdong.Where(x => x.ID == 0).ToList();
            searchstring = convertToLatin(searchstring);// hàm loại bỏ dấu của tiếng việt
            string[] chuoi_latinsearch = searchstring.Split(' ');// hàm cắt chuỗi thành từng từ
            foreach (var item in db.DuAns.ToList())
            {
                int tenduan_trung = 0;
                // kiếm tra theo tên dự án
                for (int i = 0; i < chuoi_latinsearch.Length; i++)
                {
                    string tenduan = convertToLatin(item.TenDuAn);// loại bỏ dấu trong tiếng việt
                    string[] chuoi_tenduan = tenduan.Split(' ');// cắt chuỗi thành từng từ
                    for (int j = 0; j < chuoi_tenduan.Length; j++)
                    {
                        if (chuoi_latinsearch[i] == chuoi_tenduan[j])
                        {
                            tenduan_trung++;
                            // trùng 1 từ trong tên là hiện
                            if (tenduan_trung >= 1)
                            {
                                result.Add(item);
                                goto End;
                            }
                        }
                    }
                }                
                // kiểm tra theo tỉnh thành
                int tinhthanh_trung = 0;
                for (int i = 0; i < chuoi_latinsearch.Length; i++)
                {
                    string tentinhthanh = convertToLatin(item.QuanHuyen.TinhThanhPho.TenTinhThanhPho);// loại bỏ dấu trong tiếng việt
                    string[] chuoi_tentinhthanh = tentinhthanh.Split(' ');// cắt chuỗi thành từng từ
                    for (int j = 0; j < chuoi_tentinhthanh.Length; j++)
                    {
                        if (chuoi_latinsearch[i] == chuoi_tentinhthanh[j])
                        {
                            tinhthanh_trung++;
                            if (tinhthanh_trung >= 2)
                            {
                                result.Add(item);
                                goto End;
                            }
                        }
                    }
                }
                // kiểm tra theo quận huyện
                int quanhuyen_trung = 0;
                for (int i = 0; i < chuoi_latinsearch.Length; i++)
                {
                    string tenquanhuyen = convertToLatin(item.QuanHuyen.TenQuanHuyen);// loại bỏ dấu trong tiếng việt
                    string[] chuoi_tenquanhuyen = tenquanhuyen.Split(' ');// cắt chuỗi thành từng từ
                    for (int j = 0; j < chuoi_tenquanhuyen.Length; j++)
                    {
                        if (chuoi_latinsearch[i] == chuoi_tenquanhuyen[j])
                        {
                            quanhuyen_trung++;
                            if (quanhuyen_trung >= 2)
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
    }
}