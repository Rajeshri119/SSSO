using System.Web;
using System.Web.Mvc;
//using SSSSO_MHG.CustomFilter;

namespace SSSSO_MHG
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            
            filters.Add(new HandleErrorAttribute());
        }
    }
}
