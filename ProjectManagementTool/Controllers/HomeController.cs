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
            var roles = userManager.GetRoles(User.Identity.GetUserId());

            if (roles[0] == "ItAdmin" )
            {
                return RedirectToAction("Index", "Account");
            }
            else if (roles[0] == "ProjectManager")
            {
                return RedirectToAction("Index", "Projects");
            }
            else
            {
                return RedirectToAction("Index", "Tasks");
            }
        }
    }
}