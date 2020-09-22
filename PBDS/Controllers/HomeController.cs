using PBDS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PBDS.Controllers
{
    public class HomeController : Controller
    {
        BATDONGSANEntities1 db  = new BATDONGSANEntities1();
        // GET: Home
        public ActionResult Index()
        {
            List<Baidang> lbd = new List<Baidang>();
            lbd = db.Baidangs.Where(m => m.trangthai == 3).ToList();
            return View(db.Baidangs.ToList());
        }                     

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="idbaidang,tenbaidang,diachi,gia,dientich,loaibaidang,mota,")] Baidang baidang,HttpPostedFileBase image)
        {
            if(image != null && image.ContentLength > 0)
            {
                baidang.image = new byte[image.ContentLength];
                image.InputStream.Read(baidang.image, 0, image.ContentLength);
                string filename = System.IO.Path.GetFileName(image.FileName);
                string urlimage = Server.MapPath("~/Images" + filename);
                string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(image.FileName));
                image.SaveAs(path);
                image.SaveAs(urlimage);
                //string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(image.FileName));
                //image.SaveAs(path);
                
                baidang.urlimage = "Images/" + filename;

            }
            baidang.trangthai = 1;
            baidang.ngaydang = DateTime.Now;
            baidang.ngaycapnhat = null;
            if (ModelState.IsValid)
            {
                db.Entry(baidang).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(baidang);
        }
        public ActionResult Edit()
        {
            return View();
        }
    }
}