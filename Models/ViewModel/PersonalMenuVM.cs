using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _24DH111577_LTW_BE_16_10.Models.ViewModel
{
    public class PersonalMenuVM
    {
        public bool IsLoggedIn { get; set; }
        public string Username { get; set; }
        public int CartCount { get; set; }
    }
}