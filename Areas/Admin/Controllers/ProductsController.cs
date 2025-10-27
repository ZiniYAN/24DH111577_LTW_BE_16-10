using _24DH111577_LTW_BE_16_10.Models;
using _24DH111577_LTW_BE_16_10.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using PagedList;
namespace _24DH111577_LTW_BE_16_10.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        //GET: Admin/Products
        //public ActionResult Index()
        //{
        //    var products = db.Products.Include(p => p.Category);
        //    return View(products.ToList());
        //}
        public ActionResult Index(string searchTerm, decimal? minPrice, decimal? maxPrice,string sortOrder,int? page)
        {
            var model = new ProductSearchVM();
            var products = db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p =>
                                        p.ProductName.Contains(searchTerm) ||
                                        p.ProductDescription.Contains(searchTerm) ||
                                        p.Category.CategoryName.Contains(searchTerm));
            }
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.ProductPrice >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.ProductPrice <= maxPrice.Value);
            }
            switch (sortOrder)
            {
                case "name_asc":
                    products = products.OrderBy(p => p.ProductName);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.ProductName);
                    break;
                case "price_asc":
                    products = products.OrderBy(p => p.ProductPrice);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.ProductPrice);
                    break;
                // Mặc định sắp xếp theo tên
                default:
                    products = products.OrderBy(p => p.ProductName);
                    break;
            }
            model.SortOrder = sortOrder;
            // Phân trang
            // Lấy trang hiện tại | nếu ko có thì = 1
            int pageNumber = page ?? 1;
            // Số sản phẩm mỗi trang
            int pageSize = 4;
            model.Products = products.ToPagedList(pageNumber, pageSize);
            return View(model);
        }
        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,CategoryID,ProductName,ProductDescription,ProductPrice,ProductImage,UploadImg")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.UploadImg != null)
                {
                    string filename = Path.GetFileName(product.UploadImg.FileName);
                    string savePath = "~/Content/images/";
                    product.ProductImage = savePath + filename;
                    product.UploadImg.SaveAs(Path.Combine(Server.MapPath(savePath), filename));
                }
           
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,CategoryID,ProductName,ProductPrice,ProductImage,ProductDescription")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
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
    }
}
