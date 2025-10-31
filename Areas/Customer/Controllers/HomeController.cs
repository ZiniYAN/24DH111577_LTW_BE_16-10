﻿using _24DH111577_LTW_BE_16_10.Models;
using _24DH111577_LTW_BE_16_10.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList; 
using System.Net;
namespace _24DH111577_LTW_BE_16_10.Areas.Customer.Controllers
{
    public class HomeController : Controller
    {
        // GET: Customer/Home
        private MyStoreEntities db = new MyStoreEntities();
        //Get
        public ActionResult Index(string searchTerm, int? page)
        {
            var model = new HomeProductVM();
            var products = db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p =>
                                        p.ProductName.Contains(searchTerm) ||
                                        p.ProductDescription.Contains(searchTerm) ||
                                        p.Category.CategoryName.Contains(searchTerm));
            }
            int pageNumber = page ?? 1;
            int pageSize = 6;
            model.FeatureProducts = products.OrderByDescending(p => p.OrderDetails.Count()).Take(10).ToList();

            model.NewProducts = products.OrderBy(p => p.OrderDetails.Count()).Take(20).ToPagedList(pageNumber, pageSize);
            return View(model);
        }
        public ActionResult ProductDetails(int? id, int? quantity, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product pro = db.Products.Find(id);
            if (pro == null) 
            {
                return HttpNotFound();
            }

            var products = db.Products.Where( p => p.CategoryID == pro.CategoryID && p.ProductID != pro.ProductID).AsQueryable();

            ProductDetailVM model = new ProductDetailVM();

            //Phan trang
            int pageNumber = page ?? 1;
            int pageSize = model.PageSize;
            model.product = pro;
            model.RelatedProduct = products.OrderBy(p => p.ProductID).Take(8).ToList(); // đỏi từ ToPageList(pageNumber, pageSize) -> 
            model.TopProducts = products.OrderByDescending(p => p.OrderDetails.Count()).Take(8).ToPagedList(pageNumber, pageSize);

            if (quantity.HasValue)
            {
                model.quantity = quantity.Value;
            }
            return View(model);
        }

    }
}