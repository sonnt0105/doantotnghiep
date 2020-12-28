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

        public ActionResult DSBatDongSan(EBatDongSansearch batdongsansearch)
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
            var dsbatdongsan = GetBatDongSan_DuocDang(batdongsansearch);
            var model = from o in dsbatdongsan
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
            
            if (Session["nhanvien"] != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        public IQueryable<BatDongSan> GetBatDongSan_DuocDang(EBatDongSansearch batdongsansearch)
        {
            var result = db.BatDongSans.Where(x => x.TrangThai == 3).ToList();
            if (batdongsansearch != null)
            {
                //search theo điều kiện loại bài đăng
                if (batdongsansearch.IDLoaiBaiDang.HasValue)
                    result = result.Where(x => x.IDLoaiBaiDang == batdongsansearch.IDLoaiBaiDang).ToList();
                //search theo điều kiện loại nhà đất
                if (batdongsansearch.IDLoaiNhaDat.HasValue && batdongsansearch.IDLoaiNhaDat != 0)
                    result = result.Where(x => x.IDLoaiNhaDat == batdongsansearch.IDLoaiNhaDat).ToList();
                //search theo điều kiện tỉnh thành phố
                if (batdongsansearch.IDTinhThanhPho.HasValue && batdongsansearch.IDTinhThanhPho != 0)
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

        public List<BatDongSan> TimKiemBaiDangTheoGiaTu(List<BatDongSan> listbatdongsan, double? giatu)
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

        public ActionResult DSDuAn(eDuAnSearch duansearch)
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
            var danhsachduan = GetDuAn(duansearch);
            var model = from o in danhsachduan
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

            ViewBag.listloaiduan = db.LoaiDuAns.ToList();
            List<TinhThanhPho> ttp = new List<TinhThanhPho>();
            ttp.Add(new TinhThanhPho() { ID = 0, TenTinhThanhPho = "Tất cả tỉnh thành" });
            ttp.AddRange(db.TinhThanhPhoes.ToList());
            ViewBag.listtinhthanh = ttp.OrderBy(o => o.TenTinhThanhPho);            
            if (duansearch.IDTinhThanhPho.HasValue)
            {
                List<QuanHuyen> qh = new List<QuanHuyen>();
                qh.Add(new QuanHuyen() { ID = 0, TenQuanHuyen = "Tất cả quận huyện" });
                qh.AddRange(db.QuanHuyens.Where(x => x.IDTinhThanhPho == duansearch.IDTinhThanhPho).ToList());
                ViewBag.listquanhuyen = qh;
            }
            else
            {
                List<QuanHuyen> qh = new List<QuanHuyen>();
                qh.Add(new QuanHuyen() { ID = 0, TenQuanHuyen = "Tất cả quận huyện" });
                ViewBag.listquanhuyen = qh;
            }
            if (Session["nhanvien"] != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        public IQueryable<DuAn> GetDuAn(eDuAnSearch duansearch)
        {
            var result = db.DuAns.Where(x => x.ID != 1).ToList();
            if (duansearch != null)
            {
                //search theo loại dự án
                if (duansearch.IDLoaiDuAn.HasValue && duansearch.IDLoaiDuAn != 0)
                    result = result.Where(x => x.IDLoaiDuAn == duansearch.IDLoaiDuAn).ToList();
                //search theo điều kiện tỉnh thành phố
                if (duansearch.IDTinhThanhPho.HasValue && duansearch.IDTinhThanhPho != 0)
                    result = result.Where(x => x.QuanHuyen.IDTinhThanhPho == duansearch.IDTinhThanhPho).ToList();
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
                            if (tenduan_trung >= 2)
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
        public ActionResult Danhsach_DuyetBaiDang(EBatDongSansearch batdongsansearch)
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
            var dsduyetbaidang = GetBatDongSan_CanDuyet(batdongsansearch);
            var model = from o in dsduyetbaidang
                        select new EBatDongSan
                        {
                            ID = o.ID,
                            TenBatDongSan = o.TenBatDongSan,
                            TenQuanHuyen = o.QuanHuyen.TenQuanHuyen,
                            Gia_DonVi = o.Gia + " " + o.DonVi.TenDonVi,
                            DiaChi = o.DiaChi,
                            TenLoaiBaiDang = o.LoaiBaiDang.TenLoaiBaiDang,
                            TenLoaiNhaDat = o.LoaiNhaDat.TenLoaiNhaDat,
                            DienTich = o.DienTich,
                            IDLoaiBaiDang = o.IDLoaiBaiDang,
                            Mota = o.Mota,
                            Urlimage = o.Urlimage,
                            TrangThai = o.TrangThai,
                            NgayCapNhat = o.NgayCapNhat,
                            NgayDang = o.NgayDang,

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

            if (Session["nhanvien"] != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }
        public IQueryable<BatDongSan> GetBatDongSan_CanDuyet(EBatDongSansearch batdongsansearch)
        {
            var result = db.BatDongSans.Where(x => x.TrangThai == 1).ToList();
            if (batdongsansearch != null)
            {
                //search theo điều kiện loại bài đăng
                if (batdongsansearch.IDLoaiBaiDang.HasValue)
                    result = result.Where(x => x.IDLoaiBaiDang == batdongsansearch.IDLoaiBaiDang).ToList();
                //search theo điều kiện loại nhà đất
                if (batdongsansearch.IDLoaiNhaDat.HasValue && batdongsansearch.IDLoaiNhaDat != 0)
                    result = result.Where(x => x.IDLoaiNhaDat == batdongsansearch.IDLoaiNhaDat).ToList();
                //search theo điều kiện tỉnh thành phố
                if (batdongsansearch.IDTinhThanhPho.HasValue && batdongsansearch.IDTinhThanhPho != 0)
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
        public ActionResult Duyet_partial(int? ID)
        {
            var batdongsan = db.BatDongSans.Where(m => m.ID == ID).FirstOrDefault();
            if (batdongsan == null)
            {
                return HttpNotFound();
            }
            if (Session["nhanvien"] != null)
            {
                return PartialView("Duyet_partial", batdongsan);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        // Hàm Post khi click button duyệt
        [HttpPost]
        public ActionResult Duyet(BatDongSan bds)
        {
            var batdongsan = db.BatDongSans.Where(s => s.ID == bds.ID).FirstOrDefault();
            if (batdongsan == null)
            {
                return HttpNotFound();
            }
            batdongsan.TrangThai = 2;
            db.Entry(batdongsan).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect("Danhsach_DuyetBaiDang");
        }
        [HttpPost]
        public ActionResult LoaiBo(BatDongSan bds)
        {
            var batdongsan = db.BatDongSans.Where(s => s.ID == bds.ID).FirstOrDefault();
            if (batdongsan == null)
            {
                return HttpNotFound();
            }
            db.Entry(batdongsan).State = EntityState.Deleted;
            db.SaveChanges();
            return Redirect("Danhsach_DuyetBaiDang");
        }

        public ActionResult TinhLuong(EBatDongSansearch batdongsansearch, ESale sale, string Submit)
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
            if (Submit == "Export")
            {
                var gv = new GridView();
                gv.DataSource = this.GetExportPhanCong(batdongsansearch, sale);
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=ThongKeDoanhThu.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();
            }
            var dsdoanhthu = GetPhancongtheothoigian(batdongsansearch, sale);
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
            if (Session["nhanvien"] != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        public IQueryable<PhanCong> GetPhancongtheothoigian(EBatDongSansearch batdongsansearch, ESale sale)
        {
            //DateTime dt = DateTime.MinValue;
            var result = db.PhanCongs.Where(x => x.BatDongSan.TrangThai == 4).ToList();
            //if (batdongsansearch.NgayCapNhatTu != dt)
                result = result.Where(x => x.BatDongSan.NgayCapNhat >= batdongsansearch.NgayCapNhatTu).ToList();

            //if (batdongsansearch.NgayCapNhatDen != dt)
                result = result.Where(x => x.BatDongSan.NgayCapNhat <= batdongsansearch.NgayCapNhatDen).ToList();
            if (sale.ID != 0)
            {
                result = result.Where(x => x.IDSales == sale.ID).ToList();
            }
            return result.AsQueryable();
        }

        public List<ExportPhanCong> GetExportPhanCong(EBatDongSansearch batdongsansearch, ESale sale)
        {
            var dsphancong = GetPhancongtheothoigian(batdongsansearch, sale);
            List<ExportPhanCong> exportphancong = new List<ExportPhanCong>();
            foreach (var item in dsphancong)
            {
                exportphancong.Add(new ExportPhanCong
                {
                    TenBatDongSan = item.BatDongSan.TenBatDongSan,
                    TenSale = item.Sale.TenSales,
                    Gia = item.BatDongSan.Gia + " " + item.BatDongSan.DonVi.TenDonVi,
                    PhanTramHoaHong = item.PhanTramHoaHong * 100 + " %",
                    HoaHong = item.BatDongSan.Gia * item.PhanTramHoaHong + " " + item.BatDongSan.DonVi.TenDonVi,
                });
            }
            return exportphancong;
        }
    }
}