using _24DH111577_LTW_BE_16_10.Models;
using _24DH111577_LTW_BE_16_10.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace _24DH111577_LTW_BE_16_10.Areas.Customer.Controllers
{
    public class OrderController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        // GET: Customer/Order
        public ActionResult Index()
        {
            return View();
        }

        private CartService GetCartService()
        {
            return new CartService(Session);
        }
        //Get
        [Authorize]
        public ActionResult Checkout()
        {
            //var cart = Session["Cart"] as List<CartItem>;
            //if (cart == null || !cart.Any())
            //{
            //    TempData["Message"] = "Giỏ hàng trống hoặc chưa được khởi tạo";

            //    return RedirectToAction("Index", "Cart");
            //}

            //var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            //if (user == null)
            //{
            //    TempData["Message"] = "Không tìm thấy thông tin người dùng";

            //    return RedirectToAction("Login", "Account");
            //}

            //var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);
            //if (customer == null)
            //{
            //    return RedirectToAction("Login", "Account");
            //}

            //var model = new CheckoutVM
            //{
            //    CartItems = cart,
            //    TotalPrice = cart.Sum(item => item.TotalPrice),
            //    OrderDate = DateTime.Now,
            //    ShippingAddress = customer.CustomerAddress,
            //    CustomerID = customer.CustomerID,
            //    Username = customer.Username
            //};
            //return View(model);


            //Moi 
            var cartService = GetCartService();
            var cart = cartService.GetCart();
            var cartItems = cart.Items.ToList();

            if (!cartItems.Any())
            {
                TempData["Message"] = "Giỏ hàng trống hoặc chưa được khởi tạo";
                return RedirectToAction("Index", "Cart");
            }

            var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            if (user == null)
            {
                TempData["Message"] = "Không tìm thấy thông tin người dùng";
                return RedirectToAction("Login", "Account");
            }

            var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);
            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new CheckoutVM
            {
                CartItems = cartItems,
                TotalPrice = cart.TotalValue(),
                OrderDate = DateTime.Now,
                ShippingAddress = customer.CustomerAddress,
                CustomerID = customer.CustomerID,
                Username = customer.Username
            };
            return View(model);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Checkout(CheckoutVM model)
        {
            if (ModelState.IsValid)
            {
                //var cart = Session["Cart"] as List<CartItem>;
                //if (cart == null || !cart.Any())
                //{
                //    return RedirectToAction("Index", "Cart");
                //}

                //var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
                //if (user == null)
                //{
                //    return RedirectToAction("Login", "Account");
                //}

                //var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);
                //if (customer == null)
                //{
                //    return RedirectToAction("Login", "Account");
                //}

                //var order = new Order
                //{
                //    CustomerID = model.CustomerID,
                //    OrderDate = model.OrderDate,
                //    TotalAmount = model.TotalPrice,
                //    PaymentStatus = model.PaymentStatus,
                //    PaymentMethod = model.PaymentMethod,
                //    DeliveryMethod = model.DeliveryMethod,
                //    ShippingAddress = model.ShippingAddress,
                //    OrderDetails = cart.Select(item => new OrderDetail
                //    {
                //        ProductID = item.ProductID,
                //        Quantity = item.Quantity,
                //        UnitPrice   = item.UnitPrice,
                //        TotalPrice = item.TotalPrice
                //    }).ToList()
                //};
                //db.Orders.Add(order);
                //db.SaveChanges();
                //Session["Cart"] = null;
                //return RedirectToAction("OrderSuccess", new {id = order.OrderID});

                //Moi 
                var cartService = GetCartService();
                var cart = cartService.GetCart();
                var cartItems = cart.Items.ToList();

                if (!cartItems.Any())
                {
                    return RedirectToAction("Index", "Cart");
                }

                var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);
                if (customer == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var order = new Order
                {
                    CustomerID = model.CustomerID,
                    OrderDate = model.OrderDate,
                    TotalAmount = model.TotalPrice,
                    PaymentStatus = model.PaymentStatus,
                    PaymentMethod = model.PaymentMethod,
                    DeliveryMethod = model.DeliveryMethod,
                    ShippingAddress = model.ShippingAddress,
                    OrderDetails = cartItems.Select(item => new OrderDetail
                    {
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice
                    }).ToList()
                };

                db.Orders.Add(order);
                db.SaveChanges();

                // Xóa giỏ hàng sau khi đặt hàng thành công
                cartService.ClearCart();

                return RedirectToAction("OrderSuccess", new { id = order.OrderID });
            }
            return View(model);
        }
        public ActionResult OrderSuccess(int id)
        {
            var order = db.Orders.Include("OrderDetails").SingleOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        public ActionResult MyOrder(string orderCode = null, string productName = null)
        {
            string username = User.Identity.Name;

            // Lấy CustomerID từ username
            var customer = db.Customers.FirstOrDefault(c => c.Username == username);
            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Tìm đơn hàng qua CustomerID
            var query = db.Orders.Where(o => o.CustomerID == customer.CustomerID);

            // Apply search filters
            if (!string.IsNullOrEmpty(orderCode))
            {
                query = query.Where(o => o.OrderID.ToString().Contains(orderCode));
            }

            if (!string.IsNullOrEmpty(productName))
            {
                query = query.Where(o => o.OrderDetails.Any(od =>
                    od.Product.ProductName.Contains(productName)));
            }

            // Include related data and order by date
            var orders = query
                .Include(o => o.OrderDetails.Select(od => od.Product))
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        public ActionResult OrderDetail(int id)
        {
            string username = User.Identity.Name;
            var customer = db.Customers.FirstOrDefault(c => c.Username == username);
            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = db.Orders
                .Include(o => o.OrderDetails.Select(od => od.Product))
                .FirstOrDefault(o => o.OrderID == id && o.CustomerID == customer.CustomerID);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        [HttpPost]
        public ActionResult Reorder(int id)
        {
            string username = User.Identity.Name;
            var customer = db.Customers.FirstOrDefault(c => c.Username == username);
            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = db.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.OrderID == id && o.CustomerID == customer.CustomerID);

            if (order == null)
            {
                return HttpNotFound();
            }

            // Create new cart items from the order
            var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();

            foreach (var item in order.OrderDetails)
            {
                var product = db.Products.Find(item.ProductID);
                if (product != null)
                {
                    var cartItem = new CartItem
                    {
                        ProductID = product.ProductID,
                        ProductName = product.ProductName,
                        ProductImage = product.ProductImage,
                        Quantity = item.Quantity,
                        UnitPrice = product.ProductPrice
                    };
                    cart.Add(cartItem);
                }
            }

            Session["Cart"] = cart;
            return RedirectToAction("Index", "Cart");
        }

    }
}