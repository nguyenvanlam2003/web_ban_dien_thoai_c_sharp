using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_nag_cao.Models;

namespace web_nag_cao.Controllers
{
    public class HoaDonController : Controller
    {
        // GET: HoaDon
        public ActionResult hienThiHD(string search = "")
        {
            ViewBag.timkiem = search;
            return View();
        }
        public ActionResult suaHD(int id )
        {
            ViewBag.MaHD = id;
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            List<KhachHang> danhSachKhachHang = db.KhachHangs.ToList();
            ViewBag.DanhSachTenKhachHang = new SelectList(danhSachKhachHang, "MaKH", "TenKH");
            List<SanPham> danhSachSP = db.SanPhams.ToList();
            ViewBag.DanSachSP = new SelectList(danhSachSP, "MaSP","TenSP");
         
            return View();
        }
        [HttpPost]
        public ActionResult suaHD(int id , int MaKh, int MaSP, int SoLuong )
        {
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            HoaDon hd = db.HoaDons.Where(row => row.MaHD == id).FirstOrDefault();
            ChiTietHoaDon cthd = db.ChiTietHoaDons.Where(row => row.MaHD == id ).FirstOrDefault();
            cthd.MaSP = MaSP;
            cthd.SoLuong = SoLuong;
            hd.MaKh = MaKh;
            db.SaveChanges();
            return RedirectToAction("hienThiHD");
        }
        public ActionResult xoaHD(int id)
        {
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            HoaDon hd = db.HoaDons.Where(row => row.MaHD == id).FirstOrDefault();
            ChiTietHoaDon cthd = db.ChiTietHoaDons.Where(row => row.MaHD == id).FirstOrDefault();
            db.ChiTietHoaDons.Remove(cthd);
            db.HoaDons.Remove(hd);
            db.SaveChanges();
            return RedirectToAction("hienThiHD");
        }
    }
}