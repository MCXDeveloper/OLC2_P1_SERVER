using System.Web;
using System.Web.Mvc;

namespace OLC2_P1_SERVER
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
