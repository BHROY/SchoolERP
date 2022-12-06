using System.Web;
using System.Web.Mvc;
using SchoolManagement.Helpers;

namespace SchoolManagement
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           filters.Add(new ErrorLoggerAttribute());
        }
    }
}
