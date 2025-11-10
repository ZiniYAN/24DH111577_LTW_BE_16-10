using _24DH111577_LTW_BE_16_10.Models;
using _24DH111577_LTW_BE_16_10.Models.ViewModel;
using System;
using System.Collections.Generic;
//using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace _24DH111577_LTW_BE_16_10.Areas.Customer.Controllers
{
    public class AccountController : Controller
    {
        //// GET: Customer/Account
        //public ActionResult Index()
        //{
        //    return View();
        //}
        private MyStoreEntities db = new MyStoreEntities();
        
        //Get Account /Register
        public ActionResult Register()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                //kiểm tra xem tên đăng nhập đã tồn tại chưa
                var existingUser = db.Users.SingleOrDefault(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập này đã tồn tại!");
                    return View(model);
                }
                //nếu chưa tồn tại thì tạo bản ghi thông tin tài khoản trong bảng User
                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password, // Lưu ý: Nên mã hóa mật khẩu trước khi lưu
                    UserRole = "Customer"
                };
                db.Users.Add(user);
                // và tạo bản ghi thông tin khách hàng trong bảng Customer
                var customer = new _24DH111577_LTW_BE_16_10.Models.Customer
                {
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    CustomerPhone = model.CustomerPhone,
                    CustomerAddress = model.CustomerAddress,
                    Username = model.Username
                };
                db.Customers.Add(customer);
                //lưu thông tin tài khoản và thông tin khách hàng vào CSDL
                db.SaveChanges();
                //try
                //{
                //    db.SaveChanges();
                //}
                //catch (DbEntityValidationException ex)
                //{
                //    foreach (var eve in ex.EntityValidationErrors)
                //    {
                //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //        foreach (var ve in eve.ValidationErrors)
                //        {
                //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                //                ve.PropertyName, ve.ErrorMessage);
                //        }
                //    }
                //    throw;
                //}

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.Username == model.Username
                                                      && u.Password == model.Password
                                                      && u.UserRole == "Customer");
                if (user != null)
                {
                    // Lưu trạng thái đăng nhập vào session
                    Session["Username"] = user.Username;
                    Session["UserRole"] = user.UserRole;

                    // lưu thông tin xác thực người dùng vào cookie
                    FormsAuthentication.SetAuthCookie(user.Username, false);

                    //Debug 
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }

            return View(model);
        }

        public ActionResult ProfileInfo()
        {
            // Get the current logged-in user's username
            string username = User.Identity.Name;

            // Retrieve the customer information
            var customerInfo = db.Customers.FirstOrDefault(c => c.Username == username);

            if (customerInfo == null)
            {
                return HttpNotFound();
            }

            return View(customerInfo);
        }
        public ActionResult Index()
        {
            string username = User.Identity.Name;
            var customer = db.Customers.FirstOrDefault(c => c.Username == username);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateContact(_24DH111577_LTW_BE_16_10.Models.Customer model)
        {
            if (ModelState.IsValid)
            {
                string username = User.Identity.Name;
                var customer = db.Customers.FirstOrDefault(c => c.Username == username);

                if (customer != null)
                {
                    customer.CustomerPhone = model.CustomerPhone;
                    customer.CustomerEmail = model.CustomerEmail;
                    customer.CustomerAddress = model.CustomerAddress;

                    db.SaveChanges();
                    TempData["Message"] = "Contact information updated successfully.";
                }
            }
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            string username = Session["Username"] as string;
            var user = db.Users.SingleOrDefault(u => u.Username == username && u.UserRole == "Customer");

            if (user != null)
            {
                // Trim both passwords and show more detailed debug info
                string storedPass = user.Password?.Trim() ?? "";
                string enteredPass = currentPassword?.Trim() ?? "";

                // Add detailed debug information
                //TempData["Debug"] = $@"Debug Info:
                //Username: {username}
                //Stored Password Length: {storedPass.Length}
                //Entered Password Length: {enteredPass.Length}
                //Stored Password: '{storedPass}'
                //Entered Password: '{enteredPass}'
                //Passwords Equal: {storedPass == enteredPass}
                //Password Bytes Match: {Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(storedPass)) ==
                //                     Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(enteredPass))}";

                // Compare trimmed passwords
                if (storedPass == enteredPass)
                {
                    if (string.IsNullOrEmpty(newPassword) || newPassword != confirmPassword)
                    {
                        TempData["Message"] = "New passwords don't match.";
                        return RedirectToAction("Index");
                    }

                    user.Password = newPassword.Trim();
                    db.SaveChanges();
                    TempData["Message"] = "Password changed successfully.";
                }
                else
                {
                    TempData["Message"] = "Current password is incorrect.";
                }
            }

            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [Authorize]
        public ActionResult Logout()
        {
            try
            {
                // 1. Xóa Forms Authentication Cookie
                FormsAuthentication.SignOut();

                // 2. Xóa Session
                Session.Clear();
                Session.Abandon();

                // 3. Xóa Cart nếu có
                if (Session["Cart"] != null)
                {
                    Session["Cart"] = null;
                }

                // 4. Xóa tất cả cookies
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName)
                    {
                        Expires = DateTime.Now.AddDays(-1)
                    };
                    Response.Cookies.Add(cookie);
                }

                // 5. Thông báo thành công
                TempData["Message"] = "Đăng xuất thành công!";

                // 6. Chuyển hướng về trang chủ
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                TempData["Error"] = "Có lỗi xảy ra khi đăng xuất: " + ex.Message;
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
        }

    }
}