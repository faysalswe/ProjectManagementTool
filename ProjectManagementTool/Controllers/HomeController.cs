using ProjectManagementTool.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagementTool.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Project project)
        {
            Directory.CreateDirectory(Server.MapPath("~/Content/ProjectFile" + "\\" + project.Name));

            foreach (var file in project.Files)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/ProjectFile/"+ project.Name), fileName);
                    file.SaveAs(path);
                }

            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "About Page";
            string path = Server.MapPath("~/Content/ProjectFile/VVC");
            List<Content> Contents = new List<Content>();
            foreach (var files in Directory.GetFiles(path))
            { 
                FileInfo info = new FileInfo(files);
                Content content = new Content();
                content.Path = "~/Content/ProjectFile/VVC/" + Path.GetFileName(info.FullName);
                content.Name = Path.GetFileName(info.FullName);
                Contents.Add(content);
            }
            return View(Contents);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}