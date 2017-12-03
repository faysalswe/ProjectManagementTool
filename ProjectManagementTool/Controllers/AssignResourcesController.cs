using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectManagementTool.Models;
using Microsoft.AspNet.Identity;

namespace ProjectManagementTool.Controllers
{
    [Authorize(Roles ="ProjectManager")]
    public class AssignResourcesController : Controller
    {
        private ProjectManagementToolEntities db = new ProjectManagementToolEntities();
        ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Index()
        {
            var result = from user in context.Users.AsEnumerable()
                         join resource in db.AssignResources
                         on user.Id equals resource.UserId
                         join project in db.Projects
                         on resource.ProjectId equals project.Id
                         select new AssignResourceViewModel
                         {
                             Id = resource.Id,
                             ProjectName = project.Name,
                             ResourcePerson = user.Name,
                             Designation = user.Designation
                         };
            return View(result.ToList());
        }

     
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.UserId = new SelectList(context.Users.Where(x => x.Designation != "ItAdmin" && x.Designation != "ProjectManager"), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectId,UserId")] AssignResource assignResource)
        {
            if (ModelState.IsValid)
            {
                if (db.AssignResources.Any(x => x.ProjectId == assignResource.ProjectId && x.UserId == assignResource.UserId) == false)
                {
                  assignResource.AssignerId = User.Identity.GetUserId();
                    db.AssignResources.Add(assignResource);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.UserId = new SelectList(context.Users.Where(x => x.Designation != "ItAdmin" && x.Designation != "ProjectManager"), "Id", "Name");
            ViewBag.Msg = "";
            return View(assignResource);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignResource assignResource = db.AssignResources.Find(id);
            if (assignResource == null)
            {
                return HttpNotFound();
            }
            db.AssignResources.Remove(assignResource);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
