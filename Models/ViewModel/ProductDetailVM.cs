using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList.Mvc;
namespace _24DH111577_LTW_BE_16_10.Models.ViewModel
{
    public class ProductDetailVM
    {
        public Product product {  get; set; }
        public int quantity { get; set; } = 1;
        public decimal estimatedValue { get; set; }
        public int PageNumber {  get; set; }
        public int PageSize { get; set; } = 3;
        public List<Product> RelatedProduct { get; set; }
        public PagedList.IPagedList<Product> TopProducts { get;set; }
    }
}