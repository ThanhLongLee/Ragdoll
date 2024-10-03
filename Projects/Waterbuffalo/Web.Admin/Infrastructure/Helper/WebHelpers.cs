using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity.Common.Configuration;
using Unity.Common.Extensions;

namespace Web.Admin.Infrastructure.Helper
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString IsActive(this HtmlHelper htmlHelper, string actions, string controller, string activeClass, string inActiveClass = "")
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            var routeAction = routeValues["action"].ToString();
            var routeController = routeValues["controller"].ToString();
            var listActions = actions.Split(',').Select(s => s.TrimEmpty()).ToList();
            var returnActive = (controller == routeController && listActions.Contains(routeAction));

            return new MvcHtmlString(returnActive ? activeClass : inActiveClass);
        }


        public static string IsCheckedByListGuid(Guid currently, List<Guid> guids)
        {
            if(guids != null && guids.Any())
            {
                if (guids.Contains(currently))
                {
                    return "checked";
                }
            }
            return string.Empty;
        }
    }

    public static class RenderHelper
    {
        public static string PartialView(Controller controller, string viewName, object model)
        {
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, new ViewDataDictionary(model), new TempDataDictionary(), sw);

                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);

                return sw.ToString();
            }
        }
    }

    public static class WebHelper
    {
      
    }
}