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
        public ActionResult ReadDTO(ComponentTestDTO dto)
        {
            // TODO: Scaffold the component test with the codeDOM here?

            foreach(var Property in dto.Properties)
            {
                // TODO: Do some processing to each attribute. Scaffold a method
                // to be generated onto the code?
            }
            // TODO: Return some kind of action based on processing done to DTO.
            return Content($"{dto.Properties.First().DataType},{dto.APIAction},{dto.Properties.First().Required},{dto.Properties.First().Parameters.First().ExpectedMessage},{dto.Properties.First().Parameters.First().HTTPResponse},{dto.Properties.First().Parameters.First().NullParam},{dto.Properties.First().Parameters.First().BlankParam},{dto.Properties.First().Parameters.First().RandomParam},{dto.Properties.First().Parameters.First().TestName}");
        }
        //GET Json from text area.

    }
}
