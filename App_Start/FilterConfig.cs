using System.Web;
using System.Web.Mvc;

namespace _24DH111577_LTW_BE_16_10
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
