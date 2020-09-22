using PBDS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PBDS.Controllers
{
    public class AdminController : Controller
    {
        BATDONGSANEntities1 db = new BATDONGSANEntities1();
        // GET: Home
        public ActionResult Index()
        {
            if (Session["tentaikhoan"] != null)
            {
                return View(db.Baidangs.ToList());
            }
            else
            {
                return RedirectToAction("Login");
            }            
        }

        public ActionResult Details(int? idbaidang)
        {
            
            Baidang baidang = db.Baidangs.Where(m => m.idbaidang == idbaidang).FirstOrDefault();
            if (baidang == null)
            {
                return HttpNotFound();
            }
            baidang.trangthai = 3;
            db.Entry(baidang).State = EntityState.Modified;
            db.SaveChanges();
            return View(baidang);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Admin admin)
        {
            
            if (ModelState.IsValid)
            {
                var check = db.Admins.FirstOrDefault(s => s.tentaikhoan == admin.tentaikhoan);
                if (check == null)
                {
                    admin.matkhau = GetMD5(admin.matkhau);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Admins.Add(admin);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }


            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string tentaikhoan, string matkhau)
        {
            if (ModelState.IsValid)
            {
                var f_password = matkhau;
                var data = db.Admins.Where(s => s.tentaikhoan.Equals(tentaikhoan) && s.matkhau.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["tentaikhoan"] = data.FirstOrDefault().tentaikhoan ;                   
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }



        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }        
    }
}