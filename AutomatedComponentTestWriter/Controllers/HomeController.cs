using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.CodeDom;
using Newtonsoft.Json;
using AutomatedComponentTestWriter.Models;
using System.CodeDom.Compiler;

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
        public ActionResult ReadDTO(ComponentTestDTO dto)
        {
            // TODO: Scaffold the component test with the codeDOM here?
            string test = "";
            foreach(var Property in dto.Properties)
            {
                // TODO: Do some processing to each attribute. Scaffold a method
                // to be generated onto the code?
                foreach(var Parameter in Property.Parameters)
                {
                    //if(!(Parameter.ExpectedMessage.Equals("")) || Parameter.ExpectedMessage != null)
                    test += "" + Parameter.ExpectedMessage + "\n";
                }
            }
            // TODO: Return some kind of action based on processing done to DTO.
            //return Content($"1: {dto.Properties.First().PropertyName} 2: {dto.Properties[1].PropertyName} 3: {dto.Properties[2].PropertyName}");
            return Content($"{test}");
        }

        [HttpPost]
        public void currentPropertyIndex(int currentIndex)
        {
            ViewBag.CurrentIndex = currentIndex;
        }
    }

    class ComponentTestGenerator
    {

        private CodeDomProvider test;
        
        public ComponentTestGenerator()
        {

        }

        
    }

    class UnitTest
    {

    }
}
