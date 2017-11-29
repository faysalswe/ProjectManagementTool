using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProjectManagementTool.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProjectManagementTool.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());

            if (user.Designation == "ItAdmin" )
            {
                return RedirectToAction("Index", "Account");
            }
            else if (user.Designation == "ProjectManager")
            {
                return RedirectToAction("Index", "Projects");
            }
            else
            {
                return RedirectToAction("Index", "Tasks");
            }

        }

        //[HttpPost]
        //public ActionResult Index(Project project)
        //{
        //    //Directory.CreateDirectory(Server.MapPath("~/Content/ProjectFile" + "\\" + project.Name));

        //    //foreach (var file in project.Files)
        //    //{
        //    //    if (file.ContentLength > 0)
        //    //    {
        //    //        var fileName = Path.GetFileName(file.FileName);
        //    //        var path = Path.Combine(Server.MapPath("~/Content/ProjectFile/"+ project.Name), fileName);
        //    //        file.SaveAs(path);
        //    //    }

        //    //}
        //    return View();
        //}

        public ActionResult About()
        {
            ViewBag.Message = "About Page";
            //string path = Server.MapPath("~/Content/ProjectFile/VVC");
            //List<Content> Contents = new List<Content>();
            //foreach (var files in Directory.GetFiles(path))
            //{ 
            //    FileInfo info = new FileInfo(files);
            //    Content content = new Content();
            //    content.Path = "~/Content/ProjectFile/VVC/" + Path.GetFileName(info.FullName);
            //    content.Name = Path.GetFileName(info.FullName);
            //    Contents.Add(content);
            //}
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}