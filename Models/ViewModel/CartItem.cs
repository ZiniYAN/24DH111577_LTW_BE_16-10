using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _24DH111577_LTW_BE_16_10.Models.ViewModel
{
    [Serializable]
    public class CartItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductImage { get; set; }

        public decimal TotalPrice { get { return Quantity * UnitPrice; } }
    }
}