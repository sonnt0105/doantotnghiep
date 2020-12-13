using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using OfficeOpenXml;
using PBDS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PBDS.Controllers
{
    public partial class AdminController : Controller
    {
        DB_BatDongSan db = new DB_BatDongSan();
        

        public ActionResult Index()
        {
            if (Session["admin"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }
        public ActionResult Danhsach_DuyetBaiDang(EBatDongSansearch batdongsansearch)
        {
            var model = from o in new DB_BatDongSan().BatDongSans.Where(x => x.TrangThai == 1)
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
            if (Session["admin"] != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }

        }

        // Get Duyệt bài đăng Popup
        public ActionResult Duyet_partial(int? ID)
        {
            var batdongsan = db.BatDongSans.Where(m => m.ID == ID).FirstOrDefault();
            if (batdongsan == null)
            {
                return HttpNotFound();
            }            
            if (Session["admin"] != null)
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

        public ActionResult TinhLuong()
        {
            var model = from o in new DB_BatDongSan().PhanCongs.Where(x=>x.BatDongSan.TrangThai == 4)
                        select new EPhanCongSales
                        {
                            ID = o.ID,
                            IDBatDongSan = o.IDBatDongSan,
                            TenBatDongSan = o.BatDongSan.TenBatDongSan,
                            IDSales = o.IDSales,
                            TenSale = o.Sale.TenSales,
                            PhanTramHoaHong_string = o.PhanTramHoaHong*100+" %",
                            NgayTao = o.NgayTao,                                                       
                            GiaBatDongSan = o.BatDongSan.Gia+" "+o.BatDongSan.DonVi.TenDonVi,
                            HoaHong = (double)o.BatDongSan.Gia*o.PhanTramHoaHong+" "+o.BatDongSan.DonVi.TenDonVi,
                            
                        };
            if (Session["admin"] != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }      
        
        public ActionResult DSNhanVien()
        {
            var model = from o in new DB_BatDongSan().NhanViens.Where(x=>x.TrangThai == 1)
                        select new ENhanVien
                        {
                            ID = o.ID,
                            HoTen = o.HoTen,
                            TaiKhoan = o.TaiKhoan,
                            MatKhau = o.MatKhau,
                            Email = o.Email,
                            SoDienThoai = o.SoDienThoai,
                            DiaChi = o.DiaChi
                        };
            if (Session["admin"] != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }  

        public ActionResult DSSales()
        {
            var model = from o in new DB_BatDongSan().Sales.Where(x=>x.TrangThai == 1)
                        select new ESale
                        {
                            ID = o.ID,
                            TenSales = o.TenSales,
                            Email = o.Email,
                            SoDienThoai = o.SoDienThoai,
                            DiaChi = o.DiaChi,
                            TaiKhoan = o.TaiKhoan,
                            MatKhau = o.MatKhau
                        };
            if (Session["admin"] != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        // Get Duyệt Popup
        public ActionResult Sua_ThongTin_NhanVien(int? ID)
        {
            NhanVien nhanvien = db.NhanViens.Where(m => m.ID == ID).FirstOrDefault();
            if (nhanvien == null)
            {
                return HttpNotFound();
            }            
            return PartialView("Sua_ThongTin_NhanVien", nhanvien);   
        }

        [HttpPost]
        public ActionResult Sua_ThongTin_NhanVien(NhanVien nv)
        {            
            if (nv == null)
            {
                return HttpNotFound();
            }
            nv.TrangThai = 1;
            db.Entry(nv).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect("DSNhanVien");
        }

        public ActionResult ThemNhanVien()
        {
            return PartialView("ThemNhanVien");
        }

        [HttpPost]
        public ActionResult ThemNhanVien(NhanVien nv)
        {
            if (nv == null)
            {
                return HttpNotFound();
            }
            nv.TrangThai = 1;
            db.Entry(nv).State = EntityState.Added;
            db.SaveChanges();
            return Redirect("DSNhanVien");
        }

        public ActionResult XoaNhanVien(int? ID)
        {
            NhanVien nhanvien = db.NhanViens.Where(m => m.ID == ID).FirstOrDefault();
            if (nhanvien == null)
            {
                return HttpNotFound();
            }
            return PartialView("XoaNhanVien", nhanvien);
        }

        [HttpPost]
        public ActionResult XoaNhanVien(NhanVien nv)
        {
            if (nv == null)
            {
                return HttpNotFound();
            }
            nv.TrangThai = 2;
            db.Entry(nv).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect("DSNhanVien");
        }

        public ActionResult ThemSale()
        {
            return PartialView("ThemSale");
        }

        [HttpPost]
        public ActionResult ThemSale(Sale sa)
        {
            if (sa == null)
            {
                return HttpNotFound();
            }
            sa.TrangThai = 1;
            db.Entry(sa).State = EntityState.Added;
            db.SaveChanges();
            return Redirect("DSSales");
        }

        public ActionResult SuaTTSale(int? ID)
        {
            Sale sale = db.Sales.Where(m => m.ID == ID).FirstOrDefault();
            if (sale == null)
            {
                return HttpNotFound();
            }
            return PartialView("SuaTTSale", sale);
        }

        [HttpPost]
        public ActionResult SuaTTSale(Sale sa)
        {
            if (sa == null)
            {
                return HttpNotFound();
            }
            sa.TrangThai = 1;
            db.Entry(sa).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect("DSSales");
        }

        public ActionResult XoaSale(int? ID)
        {
            Sale sale = db.Sales.Where(m => m.ID == ID).FirstOrDefault();
            if (sale == null)
            {
                return HttpNotFound();
            }
            return PartialView("XoaSale", sale);
        }

        [HttpPost]
        public ActionResult XoaSale(Sale sa)
        {
            if (sa == null)
            {
                return HttpNotFound();
            }
            sa.TrangThai = 2;
            db.Entry(sa).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect("DSSales");
        }

        public ActionResult Uploads()
        {            
            List<string> dotthi = new List<string>();
            foreach(var item in db.DIEMs.ToList())
            {
                if (dotthi.Contains(item.TenBaiThi))
                    continue;
                dotthi.Add(item.TenBaiThi);
            }
            ViewBag.dotthi = dotthi;
            if (Session["admin"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }
        [HttpPost]
        public ActionResult Uploads(FormCollection formCollection)
        {
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                var supportedTypes = new[] { "xls", "xlsx" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt))
                {
                    ViewBag.ErrorMessage = "File không hợp lệ - chỉ được chọn file EXCEL";
                    return View();
                }
                if ((file != null) &&  !string.IsNullOrEmpty(file.FileName))   
                {
                    // lưu vào server
                    string filename = System.IO.Path.GetFileName(file.FileName);
                    string urlimage = Server.MapPath("~/Cauhoi_Excel" + filename);
                    string path = Path.Combine(Server.MapPath("~/Cauhoi_Excel"), Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    file.SaveAs(urlimage);
                }
            }           
            return View();
        }      

        public ActionResult ChoKiemTRa(Admin admin)
        {                       
            int? dotthi = db.CauHois.Where(x => x.ID == 1).FirstOrDefault().DotThi + 1;
            db.TraLois.RemoveRange(db.TraLois.ToList());
            db.CauHois.RemoveRange(db.CauHois.ToList());
            db.SaveChanges();
            FileInfo filecauhoi = new FileInfo("D:/DATN/Project_BDS/PBDS/PBDS/Cauhoi_Excel/test.xlsx");
            var listcauhoi = new List<CauHoi>();
            var listtraloi = new List<TraLoi>();
            if ((filecauhoi != null) && !string.IsNullOrEmpty(filecauhoi.FullName))
            {
                using (var package = new ExcelPackage(filecauhoi))
                {
                    ExcelPackage.LicenseContext = LicenseContext.Commercial;
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;

                    for (int rowquestion = 2; rowquestion < noOfRow; rowquestion += 2)
                    {
                        var cauhoi = new CauHoi();
                        var traloia = new TraLoi();
                        var traloib = new TraLoi();
                        var traloic = new TraLoi();
                        var traloid = new TraLoi();

                        cauhoi.ID = listcauhoi.Count() + 1;
                        cauhoi.NoiDung = workSheet.Cells[rowquestion, 2].Value.ToString();
                        listcauhoi.Add(cauhoi);

                        traloia.ID = listtraloi.Count() + 1;
                        traloia.IDCauHoi = int.Parse(workSheet.Cells[rowquestion, 1].Value.ToString());
                        traloia.DungSai = int.Parse(workSheet.Cells[rowquestion + 1, 3].Value.ToString());
                        traloia.DapAn = workSheet.Cells[rowquestion, 3].Value.ToString();
                        listtraloi.Add(traloia);

                        traloib.ID = listtraloi.Count() + 1;
                        traloib.IDCauHoi = int.Parse(workSheet.Cells[rowquestion, 1].Value.ToString());
                        traloib.DungSai = int.Parse(workSheet.Cells[rowquestion + 1, 4].Value.ToString());
                        traloib.DapAn = workSheet.Cells[rowquestion, 4].Value.ToString();
                        listtraloi.Add(traloib);

                        traloic.ID = listtraloi.Count() + 1;
                        traloic.IDCauHoi = int.Parse(workSheet.Cells[rowquestion, 1].Value.ToString());
                        traloic.DungSai = int.Parse(workSheet.Cells[rowquestion + 1, 5].Value.ToString());
                        traloic.DapAn = workSheet.Cells[rowquestion, 5].Value.ToString();
                        listtraloi.Add(traloic);

                        traloid.ID = listtraloi.Count() + 1;
                        traloid.IDCauHoi = int.Parse(workSheet.Cells[rowquestion, 1].Value.ToString());
                        traloid.DungSai = int.Parse(workSheet.Cells[rowquestion + 1, 6].Value.ToString());
                        traloid.DapAn = workSheet.Cells[rowquestion, 6].Value.ToString();
                        listtraloi.Add(traloid);
                    }
                }
            }
            int idtraloi = 1, idcauhoi = 1;
            foreach (var cauhoi in listcauhoi.OrderBy(a => Guid.NewGuid()))
            {
                CauHoi ch = new CauHoi
                {
                    ID = idcauhoi++,
                    NoiDung = cauhoi.NoiDung,
                    DotThi = dotthi,
                };
                db.Entry(ch).State = EntityState.Added;
                foreach (var traloi in listtraloi)
                {
                    if (cauhoi.ID == traloi.IDCauHoi)
                    {
                        TraLoi tl = new TraLoi()
                        {
                            ID = idtraloi++,
                            IDCauHoi = ch.ID,
                            DapAn = traloi.DapAn,
                            DungSai = traloi.DungSai,
                        };
                        db.Entry(tl).State = EntityState.Added;                        
                    }
                }
                if (idcauhoi > 20)
                    break;
            }
            
            var listnhanvien = db.NhanViens.ToList();
            var listsales = db.Sales.ToList();
            foreach(var nv in listnhanvien)
            {
                nv.ChoKiemTra = 1;
                db.Entry(nv).State = EntityState.Modified;
                db.SaveChanges();
            }
            foreach (var sa in listsales)
            {
                sa.ChoKiemTra = 1;
                db.Entry(sa).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Redirect("Uploads");
        }

        public List<EBangDiem> GetBangdiem()
        {
            List<EBangDiem> bangdiem = new List<EBangDiem>();
            foreach (var item in db.DIEMs.ToList())
            {
                if(item.IDNhanVien != null)
                {
                    bangdiem.Add(new EBangDiem
                    {
                        TenNhanVien = db.NhanViens.Where(x => x.ID == item.IDNhanVien).FirstOrDefault().HoTen,
                        Dotthi = item.TenBaiThi,
                        Diem =  item.SoDiem,
                        NgayThi = item.NgayThi
                    });
                }
                else
                {
                    bangdiem.Add(new EBangDiem
                    {
                        TenNhanVien = db.Sales.Where(x => x.ID == item.IDSale).FirstOrDefault().TenSales,
                        Dotthi = item.TenBaiThi,
                        Diem = item.SoDiem,
                        NgayThi = item.NgayThi
                    });
                }
            }
            return bangdiem;
        }

        public ActionResult ExportToExcel()
        {
            var gv = new GridView();
            gv.DataSource = this.GetBangdiem();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=BangDiem.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("Index");
        }
    }  
}