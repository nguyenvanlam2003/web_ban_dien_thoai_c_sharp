using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_nag_cao.Models;
using System.IO;

namespace web_nag_cao.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        
        public ActionResult Index(string search = "")
        {
            if (Session["UserId"] != null && !string.IsNullOrEmpty(Session["Username"] as string))
            {
                Web_nag_caoEntities db = new Web_nag_caoEntities();
                List<SanPham> ketqua = db.SanPhams.Where(row => row.TenSP.Contains(search)).ToList();
                ViewBag.timkiem = search;
                ViewBag.PriceRange = "Giá";
                ViewBag.RamRange = "Ram";
                ViewBag.sapXep = "Sắp xếp";
                return View(ketqua);
            }
            else
            {
                // Người dùng chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("login");
            }
           
        }
        public ActionResult hienthitheohang(string hangsx)
        {
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            List<SanPham> ketqua = db.SanPhams.Where(row => row.HangSX.Contains(hangsx)).ToList();
            ViewBag.PriceRange = "Giá";
            ViewBag.RamRange = "Ram";
            ViewBag.sapXep = "Sắp xếp";
            return View("Index", ketqua);
        }
        public ActionResult locgia(string priceRange = "")
        {
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            List<SanPham> ketqua = db.SanPhams.ToList();
            ViewBag.PriceRange = priceRange;
            ViewBag.RamRange = "RAM";
            ViewBag.sapXep = "Sắp xếp";
            switch (priceRange)
            {
                case "Giá":
                    break;
                case "Dưới 10 triệu":
                    ketqua = ketqua.Where(row => row.GiaBan < 10000000).ToList();
                    break;
                case "Từ 10 đến 20 triệu":
                    ketqua = ketqua.Where(row => row.GiaBan >= 10000000 && row.GiaBan <= 20000000).ToList();
                    break;
                case "Trên 20 triệu":
                    ketqua = ketqua.Where(row => row.GiaBan > 20000000).ToList();
                    break;
                default:
                    // Nếu không có lựa chọn hoặc lựa chọn không hợp lệ, giữ nguyên danh sách.
                    break;
            }

            return View("Index", ketqua);
        }

        public ActionResult locRam(string ramRange = "")
        {
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            List<SanPham> ketqua = db.SanPhams.ToList();
            ViewBag.RamRange = ramRange;
            ViewBag.PriceRange = "Giá";
            ViewBag.sapXep = "Sắp xếp";
            switch (ramRange)
            {
                case "RAM":
                    break;
                case "4GB":
                    ketqua = ketqua.Where(row => row.Ram == "4GB").ToList();
                    break;
                case "6GB":
                    ketqua = ketqua.Where(row => row.Ram == "6GB").ToList();
                    break;
                case "8GB":
                    ketqua = ketqua.Where(row => row.Ram == "8GB").ToList();
                    break;
                default:
                    // Nếu không có lựa chọn hoặc lựa chọn không hợp lệ, giữ nguyên danh sách.
                    break;
            }

            return View("Index", ketqua);
        }
        public ActionResult SapXepGia(string sortPrice = "")
        {
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            List<SanPham> ketqua = db.SanPhams.ToList();
            ViewBag.RamRange = "Ram";
            ViewBag.PriceRange = "Giá";
            ViewBag.sapXep = sortPrice;
            switch (sortPrice)
            {
                case "Sắp xếp":
                    break;
                case "Giá thấp đến cao":
                    ketqua = ketqua.OrderBy(row => row.GiaBan).ToList();
                    break;
                case "Giá cao đến thấp":
                    ketqua = ketqua.OrderByDescending(row => row.GiaBan).ToList();
                    break;
                default:
                    // Nếu không có lựa chọn hoặc lựa chọn không hợp lệ, giữ nguyên danh sách.
                    break;
            }

            return View("Index", ketqua);
        }
        public ActionResult ThemSP()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemSP(SanPham sp, HttpPostedFileBase HinhAnh)
        {
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            if (ModelState.IsValid)
            {
                if (HinhAnh != null && HinhAnh.ContentLength > 0)
                {
                    var filePath = Path.Combine(Server.MapPath("~/Assets/Img"), Path.GetFileName(HinhAnh.FileName));
                    HinhAnh.SaveAs(filePath);
                    sp.HinhAnh = "~/Assets/Img/" + HinhAnh.FileName;
                }
                db.SanPhams.Add(sp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sp);
        }
        public ActionResult Edit(int id)
        {
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            SanPham sp = db.SanPhams.Where(row => row.MaSP == id).FirstOrDefault();
            return View(sp);
        }
        [HttpPost]
        public ActionResult Edit(SanPham model, HttpPostedFileBase HinhAnh)
        {
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            SanPham sp = db.SanPhams.Where(row => row.MaSP == model.MaSP).FirstOrDefault();
            if (ModelState.IsValid)
            {

                // Kiểm tra và xóa ảnh cũ
                if (!string.IsNullOrEmpty(sp.HinhAnh))
                {
                    // Lấy tên tệp tin từ đường dẫn
                    string tenanh = Path.GetFileName(sp.HinhAnh);

                    // Đường dẫn đầy đủ của tệp tin ảnh cũ
                    string oldFilePath = Path.Combine(Server.MapPath("~/Assets/Img/"), tenanh);

                    // Kiểm tra xem tệp tin tồn tại trước khi xóa
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        // Xóa tệp tin từ thư mục
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Lưu ảnh mới
                string fileName = Path.GetFileName(HinhAnh.FileName);
                string filePath = Path.Combine(Server.MapPath("~/Assets/Img"), fileName);
                HinhAnh.SaveAs(filePath);

                // Cập nhật đường dẫn ảnh mới trong model
                model.HinhAnh = "~/Assets/Img/" + fileName;
                sp.TenSP = model.TenSP;
                sp.GiaBan = model.GiaBan;
                sp.HangSX = model.HangSX;
                sp.ManHinh = model.ManHinh;
                sp.Mau = model.Mau;
                sp.Ram = model.Ram;
                sp.ViXuLy = model.ViXuLy;
                sp.HinhAnh = model.HinhAnh;
                sp.DoPhanGiai = model.DoPhanGiai;
                sp.DungLuongPin = model.DungLuongPin;
                sp.BoNho = model.BoNho;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            SanPham sp = db.SanPhams.Where(row => row.MaSP == id).FirstOrDefault();
            // Kiểm tra và xóa ảnh cũ
            if (!string.IsNullOrEmpty(sp.HinhAnh))
            {
                // Lấy tên tệp tin từ đường dẫn
                string tenanh = Path.GetFileName(sp.HinhAnh);

                // Đường dẫn đầy đủ của tệp tin ảnh cũ
                string oldFilePath = Path.Combine(Server.MapPath("~/Assets/Img/"), tenanh);

                // Kiểm tra xem tệp tin tồn tại trước khi xóa
                if (System.IO.File.Exists(oldFilePath))
                {
                    // Xóa tệp tin từ thư mục
                    System.IO.File.Delete(oldFilePath);
                }
            }
            db.SanPhams.Remove(sp);
            db.SaveChanges();
            return RedirectToAction("index");
        }
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(string username, string password)
        {
            Web_nag_caoEntities1 db = new Web_nag_caoEntities1();
           TaiKhoanADmin ad=db.TaiKhoanADmins.Where(row => row.TaiKhoan == username && row.MatKhau == password).FirstOrDefault();

            if (ad != null)
            {
                // Lưu thông tin đăng nhập vào Session
                Session["UserId"] = ad.ID; // Thay UserId bằng thuộc tính tương ứng trong đối tượng TaiKhoanADmin
                Session["Username"] = ad.TaiKhoan;

                // Chuyển hướng đến trang Index sau khi đăng nhập thành công
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        public ActionResult logout()
        {
            // Xóa thông tin đăng nhập từ Session
            Session["UserId"] = null;
            Session["Username"] = null;

            // Chuyển hướng về trang đăng nhập
            return RedirectToAction("login");
        }
    }

}