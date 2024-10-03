using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity.Common.Configuration;
using Web.Admin.Infrastructure.Filters;

namespace Web.Admin.Controllers
{
    [CustomAuthorize(new[] { AccountRole.Admin, AccountRole.Employee })]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}