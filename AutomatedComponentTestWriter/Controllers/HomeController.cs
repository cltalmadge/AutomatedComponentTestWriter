using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.CodeDom;
using Newtonsoft.Json;
using AutomatedComponentTestWriter.Models;

namespace AutomatedComponentTestWriter.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Main()
        {
            ComponentTestDTO dto = new ComponentTestDTO();
            return View(dto);
        }
        
        [HttpPost]
        public ActionResult Yeet(ComponentTestDTO dto)
        {
            // TODO: Scaffold the component test with the codeDOM here?

            foreach(var Property in dto.Properties)
            {
                // TODO: Do some processing to each attribute. Scaffold a method
                // to be generated onto the code?
            }
            // TODO: Return some kind of action based on processing done to DTO.
            return null;
        }
        //GET Json from text area.

    }
}
