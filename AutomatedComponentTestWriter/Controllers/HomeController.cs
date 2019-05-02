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
        public ActionResult Main()
        {
            ComponentTestDTO dto = new ComponentTestDTO();
            return View(dto);
        }

        [HttpPost]
        public ActionResult ReadDTO(ComponentTestDTO dto)
        {
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

            // Generate the code for the client.
            dtoTemplate.GenerateCSharpCode();
            componentTestsTemplate.GenerateCSharpCode();

            // If the directory for storing generated code doesn't exist yet, create it.
            string pathForZips = System.Web.Hosting.HostingEnvironment.MapPath(@"~/GeneratedCode");
            if(!Directory.Exists(pathForZips))
            {
                Directory.CreateDirectory(pathForZips);
            }
            
            string path2 = pathForZips + @"/" + dto.DTOName + "Archive";

            System.IO.DirectoryInfo dirInfo2 = new DirectoryInfo(pathForZips);

            // Get rid of old archives since they are no longer needed.
            foreach (FileInfo file in dirInfo2.GetFiles())
            {
                if (file.Extension.Equals(".zip"))
                {
                    file.Delete();
                }
            }

            // Creates the archive to send to the client.
            ZipFile.CreateFromDirectory(path, path2 + ".zip");
            
            // Clear the response header and then send the zip to the client.
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attatchment;filename=\"" + dto.DTOName + "Archive.zip\"");
            Response.WriteFile(path2 + ".zip");

            
            return Content("Success");
        }

        [HttpPost]
        public void CurrentPropertyIndex(int currentIndex)
        {
            ViewBag.CurrentIndex = currentIndex;
        }
        

    }
}
