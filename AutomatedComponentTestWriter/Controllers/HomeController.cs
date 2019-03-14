using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.CodeDom;
using Newtonsoft.Json;

namespace AutomatedComponentTestWriter.Controllers
{
    public class HomeController : Controller
    {
        IList<Attribute> attributeList;

        public ActionResult Main()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Yeet(string APIAction, string APIendpointURL)
        {
            return Content($"Yeah it's a {APIAction} {APIendpointURL}");
        }
        //GET Json from text area.

    }
}
