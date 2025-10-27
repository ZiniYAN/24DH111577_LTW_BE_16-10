using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList.Mvc;

namespace _24DH111577_LTW_BE_16_10.Models.ViewModel
{
    public class ProductSearchVM
    {
        public string SearchTerm { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string SortOrder {  get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
        public PagedList.IPagedList<Product> Products { get; set; } 
        //public List<Product> Products { get; set; }
    }
}