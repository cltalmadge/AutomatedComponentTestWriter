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
            //ComponentTestGenerator componentTest = new ComponentTestGenerator();
            DTOGenerator dtoTemplate = new DTOGenerator(dto, dto.DTOName + ".cs");
            ComponentTestGenerator componentTestsTemplate = new ComponentTestGenerator(dto, dto.DTOName + "Tests.cs");
            dtoTemplate.GenerateCSharpCode();
            componentTestsTemplate.GenerateCSharpCode();
            //dtoTemplate.GenerateCSharpCode();
            // TODO: Return some kind of action based on processing done to DTO.
            //return Content($"1: {dto.Properties.First().PropertyName} 2: {dto.Properties[1].PropertyName} 3: {dto.Properties[2].PropertyName}");
            return Content("Yes");
        }

        [HttpPost]
        public void CurrentPropertyIndex(int currentIndex)
        {
            ViewBag.CurrentIndex = currentIndex;
        }
    }
}
