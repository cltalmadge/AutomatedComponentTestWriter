using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.CodeDom;

namespace AutomatedComponentTestWriter.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Main()
        {
            return View();
        }

        [HttpPost]
        public JsonResult PostAttribute(Attribute outGoingAttribute)
        {

            return null;
        }

    }
}
