using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Admin.Controllers
{
    public class HttpErrorsController : Controller
    {
        // GET: HttpErrors
        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult ErrorCode()
        {
            return View();
        }
    }
}