using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _24DH111577_LTW_BE_16_10.Models
{
//    public class UserMetadata
//    {
//        [Required(ErrorMessage ="Username not null")]
//        [StringLength(30,MinimumLength =5)]
//        [RegularExpression("\r\n ^[a-zA-Z0-9](?!.*[._]{2})[a-zA-Z0-9._]{6,20}[a-zA-Z0-9]$ ")]
//        public string Username { get; set; }
//        [Required]
//        [DataType(DataType.Password)]
//        public string Password { get; set; }
//        [Required]
//        public string UserRole { get; set; }
//    }

    //public class CategoryMetadata
    //{
    //    [HiddenInput]
    //    public int CategoryID { get; set; }
    //    [Required]
    //    [StringLength(50,MinimumLength = 5)]
    //    public string CategoryName { get; set; }
    //}
    //public class CustomerMetadata { }
    //public class MyStoreModelMetadata { }
    //public class OrderMetadata { }
    //public class OrderDetailsMetadata { }
    public class ProductMetadata 
    {
        [Display(Name="Mã sản phẩm")]
        public int ProductID { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage ="Phải nhập tên sản phẩm")]
        [Display(Name ="Tên sản phẩm")]
        
        public string ProductName { get; set; }

        [Display(Name = "Loại sản phẩm")]
        
        public int CategoryID { get; set; }
        //[Display(Name = "Link ảnh sản phẩm")]
        //[DefaultValue(~/Content)]
        //public string ProductImage { get; set; }
        [Display(Name = "Mô tả sản phẩm")]
        public string ProductDescription { get; set; }
        //[DefaultValue(true)]
        //public System.DateTime CreatedData { get; set; }
    }
}