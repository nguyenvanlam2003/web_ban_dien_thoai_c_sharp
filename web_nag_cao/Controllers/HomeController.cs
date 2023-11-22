using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_nag_cao.Models;

namespace web_nag_cao.App_Start
{
    public class HomeController : Controller
    {
        // GET: Home
        
        public ActionResult Index(string search = "")
        {
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            List<SanPham> ketqua = db.SanPhams.Where(row => row.TenSP.Contains(search)).ToList();
            ViewBag.timkiem = search;
            ViewBag.PriceRange = "Giá";
            ViewBag.RamRange = "Ram";
            ViewBag.sapXep = "Sắp xếp";
            return View(ketqua);
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
        public ActionResult chiTietSP(int id)
        {
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            SanPham ketqua = db.SanPhams.Where(row => row.MaSP == id).FirstOrDefault();
            return View(ketqua);
        }
        public ActionResult trungTamBH()
        {
            return View();
        }
        public ActionResult themGioHang(int id)
        {
            string username = Request.Cookies["UserSession"]?.Value;

            if (!string.IsNullOrEmpty(username))
            {
                int  userId =int.Parse(Request.Cookies["UserSession"]["UserId"]);
                Web_nag_caoEntities db = new Web_nag_caoEntities();
                GioHang gh=db.GioHangs.Where(row=>row.MaKH==userId).FirstOrDefault();
                if (gh == null)
                {
                    gh = new GioHang
                    {
                        MaKH = userId,
                    };

                    // Thêm giỏ hàng mới vào cơ sở dữ liệu
                    db.GioHangs.Add(gh);
                    db.SaveChanges();
                    ChiTietGioHang ctGH = new ChiTietGioHang
                    {
                        MaGH = gh.MaGH,
                        MaSP = id,
                        SoLuong=1,
                    };
                    db.ChiTietGioHangs.Add(ctGH);
                    db.SaveChanges();
                   
                    return RedirectToAction("chiTietSP", "Home", new { id = id });
                }
                else
                {
                    ChiTietGioHang ctGH = db.ChiTietGioHangs.Where(row => row.MaSP == id && row.MaGH==gh.MaGH).FirstOrDefault();
                    if (ctGH==null)
                    {
                        ctGH = new ChiTietGioHang
                        {
                            MaGH = gh.MaGH,
                            MaSP = id,
                            SoLuong = 1,
                        };
                        db.ChiTietGioHangs.Add(ctGH);
                        db.SaveChanges();
                   
                        return RedirectToAction("chiTietSP", "Home", new { id = id });
                    }
                    else
                    {
                        ctGH.SoLuong = ctGH.SoLuong + 1;
                        db.SaveChanges();
                    
                        return RedirectToAction("chiTietSP", "Home", new { id = id });
                    }
                    
                }
                
            }
            else
            {
                
                return RedirectToAction("login", "Home"); // Chuyển hướng đến trang đăng nhập
            }
            
        }
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(string username, string password )
        {
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            KhachHang nd = db.KhachHangs.SingleOrDefault(row => row.TaiKhoan == username && row.MatKhau == password);

            if (nd != null)
            {
                // Tạo một cookie để đánh dấu đăng nhập thành công
                HttpCookie userCookie = new HttpCookie("UserSession");
                userCookie["Username"] = username;
                userCookie["UserId"] = nd.MaKH.ToString();
                userCookie.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(userCookie);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
        public ActionResult gioHang()
        {
            string username = Request.Cookies["UserSession"]?.Value;

            if (!string.IsNullOrEmpty(username))
            {
                int userId = int.Parse(Request.Cookies["UserSession"]["UserId"]);
                ViewBag.userId = userId;
                return View(userId);
            }
            else
            {
                // Cookie không tồn tại, có thể người dùng chưa đăng nhập
                // Hoặc cookie đã hết hạn hoặc bị thay đổi
                // Tiếp tục xử lý của bạn
                return RedirectToAction("login", "Home"); // Chuyển hướng đến trang đăng nhập
            }
           
        }
        public ActionResult muaTrongGioHang(int id, int maKH)
        {
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            ChiTietGioHang ctgh = db.ChiTietGioHangs.Where(row => row.MaSP == id).FirstOrDefault();
            HoaDon hd = new HoaDon
            {
                MaKh = maKH,
                NgayMua=DateTime.Now
            };
            db.HoaDons.Add(hd);
            db.SaveChanges();
            ChiTietHoaDon cthd = new ChiTietHoaDon
            {
                MaHD = hd.MaHD,
                MaSP=ctgh.MaSP,
                SoLuong=ctgh.SoLuong
            };
            db.ChiTietHoaDons.Add(cthd);
            db.SaveChanges();
            db.ChiTietGioHangs.Remove(ctgh);
            db.SaveChanges();
            return RedirectToAction("gioHang");
        }
        public ActionResult muaHang(int id)
        {
            string username = Request.Cookies["UserSession"]?.Value;

            if (!string.IsNullOrEmpty(username))
            {
                int userId = int.Parse(Request.Cookies["UserSession"]["UserId"]);
                Web_nag_caoEntities db = new Web_nag_caoEntities();
                HoaDon hd = new HoaDon
                {
                    MaKh = userId,
                    NgayMua = DateTime.Now
                };
                db.HoaDons.Add(hd);
                db.SaveChanges();
                ChiTietHoaDon cthd = new ChiTietHoaDon
                {
                    MaHD = hd.MaHD,
                    MaSP = id,
                    SoLuong = 1
                };
                db.ChiTietHoaDons.Add(cthd);
                db.SaveChanges();
                TempData["ThongBaoMuaHang"] = "Mua hàng thành công!";
                return RedirectToAction("chiTietSP", "Home", new { id = id });
            }
            else
            {
                // Cookie không tồn tại, có thể người dùng chưa đăng nhập
                // Hoặc cookie đã hết hạn hoặc bị thay đổi
                // Tiếp tục xử lý của bạn
                return RedirectToAction("login", "Home"); // Chuyển hướng đến trang đăng nhập
            }
        }
        public ActionResult xoaGioHang(int id)
        {
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            ChiTietGioHang ctgh = db.ChiTietGioHangs.Where(row => row.MaSP == id).FirstOrDefault();
            db.ChiTietGioHangs.Remove(ctgh);
            db.SaveChanges();
            return RedirectToAction("gioHang");
        }

        public ActionResult thongtin()
        {
            return View();
        }
        public ActionResult hienThiDH()
        {
            string username = Request.Cookies["UserSession"]?.Value;

            if (!string.IsNullOrEmpty(username))
            {
                int userId = int.Parse(Request.Cookies["UserSession"]["UserId"]);
                ViewBag.userId = userId;
                return View(userId);
            }
            else
            {
              
                return RedirectToAction("login", "Home"); // Chuyển hướng đến trang đăng nhập
            }
            
        }
        public ActionResult huyDH(int mahd, int makh, int mact)
        {
            Web_nag_caoEntities db = new Web_nag_caoEntities();
            HoaDon hd = db.HoaDons.Where(row => row.MaHD == mahd && row.MaKh==makh).FirstOrDefault();
            ChiTietHoaDon cthd = db.ChiTietHoaDons.Where(row => row.MaHD == mahd && row.ID==mact).FirstOrDefault();
            db.ChiTietHoaDons.Remove(cthd);
            db.HoaDons.Remove(hd);
            db.SaveChanges();
            return RedirectToAction("hienThiDH", "Home");
        }
        public ActionResult logout()
        {
            if (Request.Cookies["UserSession"] != null)
            {
                var userCookie = new HttpCookie("UserSession");
                userCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(userCookie);
            }
            return RedirectToAction("Index","Home");
        }
    }
}