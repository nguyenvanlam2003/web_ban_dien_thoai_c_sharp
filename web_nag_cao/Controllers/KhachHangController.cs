using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_nag_cao.Models;

namespace web_nag_cao.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        public ActionResult HienThiKH(string search = "")
        {
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            List<KhachHang> ketqua = db.KhachHangs.Where(row => row.TenKH.Contains(search)).ToList();
            ViewBag.timkiem = search;
            return View(ketqua);
        }
        public ActionResult ThemKH()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemKH(KhachHang KH)
        {
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            db.KhachHangs.Add(KH);
            db.SaveChanges();
            return RedirectToAction("HienThiKH");
        }
        public ActionResult Edit(int id)
        {
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            KhachHang KH = db.KhachHangs.Where(row => row.MaKH == id).FirstOrDefault();
            return View(KH);
        }
        [HttpPost]
        public ActionResult Edit(KhachHang model)
        {
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            KhachHang khachhang = db.KhachHangs.Where(row => row.MaKH == model.MaKH).FirstOrDefault();

            khachhang.TenKH = model.TenKH;
            khachhang.DiaChi = model.DiaChi;
            khachhang.Email = model.Email;
            khachhang.SoDT = model.SoDT;
            db.SaveChanges();
            return RedirectToAction("HienThiKH");

        }
        public ActionResult Delete(int id)
        {
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            KhachHang kh = db.KhachHangs.Where(row => row.MaKH == id).FirstOrDefault();
            // Kiểm tra và xóa ảnh cũ
           
            db.KhachHangs.Remove(kh);
            db.SaveChanges();
            return RedirectToAction("HienThiKH");
        }
    }
}