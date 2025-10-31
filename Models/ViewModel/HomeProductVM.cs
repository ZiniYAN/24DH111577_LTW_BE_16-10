using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _24DH111577_LTW_BE_16_10.Models.ViewModel
{
    public class HomeProductVM
    {
        public string SearchTerm {  get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
        public List<Product> FeatureProducts { get; set; }
        public PagedList.IPagedList<Product> NewProducts { get; set; }

    }
}