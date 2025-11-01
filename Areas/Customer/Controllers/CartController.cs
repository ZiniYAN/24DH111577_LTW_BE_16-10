using _24DH111577_LTW_BE_16_10.Models;
using _24DH111577_LTW_BE_16_10.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Collections.Specialized.BitVector32;
namespace _24DH111577_LTW_BE_16_10.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
        //// GET: Customer/Cart
        //public ActionResult Index()
        //{
        //    return View();
        //}


        private MyStoreEntities db = new MyStoreEntities();
        private CartService GetCartService()
        {
            return new CartService(Session);
        }

        public ActionResult Index()
        {
            var cart = GetCartService().GetCart();
            return View(cart);

        }

        public ActionResult AddToCart(int id, int quantity = 1)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                var cartService = GetCartService();
                cartService.GetCart().AddItem(product.ProductID, product.ProductImage, product.ProductName,product.ProductPrice, quantity, product.Category.CategoryName);
            }
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(int id)
        {
            var cartService = GetCartService();
            cartService.GetCart().RemoveItem(id);
            return RedirectToAction("Index");
        }

        public ActionResult ClearCart()
        {
            GetCartService().ClearCart();
            return RedirectToAction("Index");
        }
        public ActionResult UpdateQuantity(int id, int quantity)
        {
            var cartService = GetCartService();
            cartService.GetCart().UpdateQuantity(id,quantity);
            return RedirectToAction("Index");
        }
    }
}