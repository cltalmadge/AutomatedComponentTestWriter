using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.CodeDom;
using System.IO.Compression;
using Newtonsoft.Json;
using AutomatedComponentTestWriter.Models;
using System.CodeDom.Compiler;
using System.IO;

namespace AutomatedComponentTestWriter.Controllers
{
    public class HomeController : Controller
    {
        private ComponentTestDTO dataTransferObject;
        public ActionResult Main()
        {
            ComponentTestDTO dto = new ComponentTestDTO();
            return View(dto);
        }

        [HttpPost]
        public ActionResult ReadDTO(ComponentTestDTO dto)
        {
            //TODO: Scaffold the component test with the codeDOM here?
            //ComponentTestGenerator componentTest = new ComponentTestGenerator();
            DTOGenerator dtoTemplate = new DTOGenerator(dto, dto.DTOName + ".cs");
            ComponentTestGenerator componentTestsTemplate = new ComponentTestGenerator(dto, dto.DTOName + "Tests.cs");

            string path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Generated");

            System.IO.DirectoryInfo dirInfo = new DirectoryInfo(path);
            foreach(FileInfo file in dirInfo.GetFiles())
            {
                if(file.Extension.Equals(".cs"))
                {
                    file.Delete();
                }
            }

            dtoTemplate.GenerateCSharpCode();
            componentTestsTemplate.GenerateCSharpCode();

            //Response.Clear();
            //Response.AddHeader("Content-Disposition", "attatchment;filename=\"" + dto.DTOName + ".cs\"");
            //Response.WriteFile(path + @"\" + dto.DTOName + ".cs");
            string pathForZips = System.Web.Hosting.HostingEnvironment.MapPath(@"~/GeneratedCode");
            if(!Directory.Exists(pathForZips))
            {
                Directory.CreateDirectory(pathForZips);
            }

            
            string path2 = pathForZips + @"/" + dto.DTOName + "Archive";

            System.IO.DirectoryInfo dirInfo2 = new DirectoryInfo(pathForZips);

            // Get rid of old archives
            foreach (FileInfo file in dirInfo2.GetFiles())
            {
                if (file.Extension.Equals(".zip"))
                {
                    file.Delete();
                }
            }
            ZipFile.CreateFromDirectory(path, path2 + ".zip");
            
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attatchment;filename=\"" + dto.DTOName + "Archive.zip\"");
            Response.WriteFile(path2 + ".zip");
            return Content("OK");
        }

        [HttpPost]
        public void CurrentPropertyIndex(int currentIndex)
        {
            ViewBag.CurrentIndex = currentIndex;
        }
        

    }
}
