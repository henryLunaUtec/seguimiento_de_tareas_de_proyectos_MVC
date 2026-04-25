using System.Web;
using System.Web.Mvc;

namespace seguimiento_de_tareas_de_proyectos_MVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
