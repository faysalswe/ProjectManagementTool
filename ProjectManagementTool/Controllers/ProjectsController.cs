using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectManagementTool.Models;
using System.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ProjectManagementTool.Controllers
{
    
    public class ProjectsController : Controller
    {
        private ProjectManagementToolEntities db = new ProjectManagementToolEntities();
        private ApplicationDbContext adb = new ApplicationDbContext();
        [Authorize(Roles = "ProjectManager")]
        public ActionResult Index(int? id)
        {
            //var result = db.AssignResources.Where(x => x.ProjectId == id).Distinct().Count();
            //var query = @"select count(*) as count,dept.DNAME 
            //            from emp 
            //            inner join dept on emp.DEPTNO = dept.DEPTNO 
            //            group by dept.DNAME";

            var MemberCountByProject = from AssignResource in db.AssignResources
                      group AssignResource.UserId by AssignResource.ProjectId into g
                      select new { ProjectId = g.Key, NumberOfMember = g.Count() };
   

            var TaskCountByProject = from Task in db.Tasks
                     group Task.Id by Task.ProjectId into g
                     select new { ProjectId = g.Key, NumberOfTask = g.Count() };

            var result = from Project in db.Projects
                         join a in MemberCountByProject on
                         Project.Id equals a.ProjectId
                         join b in TaskCountByProject on
                         Project.Id equals b.ProjectId
                         select new ProjectInfo
                         {
                            Id = Project.Id,
                            Name = Project.Name,
                            CodeName = Project.CodeName,
                            Status = Project.Status,
                            NumberOfMember = a.NumberOfMember,
                            NumberOfTask = b.NumberOfTask
                         };
            
            return View(result.ToList());
        }
        [Authorize(Roles = "ProjectManager, Employee")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            string path = Server.MapPath(project.FilesPath);
            List<Content> Files = new List<Content>();
            foreach (var files in Directory.GetFiles(path))
            {
                FileInfo info = new FileInfo(files);
                Content content = new Content();
                content.Path = project.FilesPath +"/"+ Path.GetFileName(info.FullName);
                content.Name = Path.GetFileName(info.FullName);
                Files.Add(content);
            }
            ViewBag.FileList = Files;
            var res = from s in db.AssignResources.Where(x => x.ProjectId == project.Id).AsEnumerable()
                      join t in adb.Users.AsEnumerable()
                      on s.UserId equals t.Id
                      select new ProjectAssign { Name = t.Name, Designation = t.Designation, Id=t.Id };
            ViewBag.Member = res.ToList();

            return View(project);
        }
        [Authorize(Roles = "ProjectManager")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CodeName,Description,PossibleStartDate,PossibleEndDate,Duration,FilesPath,Status,Files")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.CreatorOwnerId = User.Identity.GetUserId();
                Directory.CreateDirectory(Server.MapPath("~/Content/ProjectFile" + "/" + project.Name));

                foreach (var file in project.Files)
                {
                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/ProjectFile/" + project.Name), fileName);
                        file.SaveAs(path);
                    }
                }

                project.FilesPath = "~/Content/ProjectFile" + "/" + project.Name;
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }
        [Authorize(Roles = "ProjectManager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }
        [Authorize(Roles = "ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CodeName,Description,PossibleStartDate,PossibleEndDate,Duration,FilesPath,Status")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }
        [Authorize(Roles = "ProjectManager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }
        [Authorize(Roles = "ProjectManager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
