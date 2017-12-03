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
    [Authorize(Roles = "Employee")]
    public class TasksController : Controller
    {
        private ProjectManagementToolEntities db = new ProjectManagementToolEntities();
        ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Index()
        {
            var MemberCountByProject = from AssignResource in db.AssignResources
                                       group AssignResource.UserId by AssignResource.ProjectId into g
                                       select new { ProjectId = g.Key, NumberOfMember = g.Count() };

            var TaskCountByProject = from Task in db.Tasks
                                     group Task.Id by Task.ProjectId into g
                                     select new { ProjectId = g.Key, NumberOfTask = g.Count() };
            string userId = User.Identity.GetUserId();
            var result = from Project in db.Projects
                         join a in MemberCountByProject on
                         Project.Id equals a.ProjectId
                         join b in TaskCountByProject on
                         Project.Id equals b.ProjectId
                         join c in db.AssignResources.Where(x => x.UserId == userId) on
                         Project.Id equals c.ProjectId
                         select new ProjectInfo
                         {
                             Id = Project.Id,
                             Name = Project.Name,
                             CodeName = Project.CodeName,
                             Status = Project.Status,
                             NumberOfMember = a.NumberOfMember,
                             NumberOfTask = b.NumberOfTask
                         };

            TaskIndex taskIndex = new TaskIndex();
            taskIndex.ProjectInfos = result;
            taskIndex.Tasks = db.Tasks.Where(x => x.UserId == userId).Include(t => t.Project);
            return View(taskIndex);
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }


        public ActionResult Create(int? id)
        {
            if(id == null)
            {
                ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
                ViewBag.UserId = new SelectList(context.Users.Where(x => x.Designation != "ItAdmin" && x.Designation != "ProjectManager"), "Id", "Name");
            }
            else
            {
                ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", id);
                var UserByProjectId = from project in db.AssignResources.Where(x => x.ProjectId == id).AsEnumerable()
                                 join user in context.Users.AsEnumerable()
                                 on project.UserId equals user.Id
                                 select new 
                                 {
                                     Id = user.Id,
                                     Name = user.Name
                                 };
                ViewBag.UserId = new SelectList(UserByProjectId.ToList(), "Id", "Name");
                    //new SelectList(context.Users.Where(x => x.Designation != "ItAdmin" && x.Designation != "ProjectManager" ), "Id", "Name");
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectId,UserId,Description,Duedate,Priority,Name")] Task task)
        {
            if (ModelState.IsValid)
            {
                task.CreatorOwnerId = User.Identity.GetUserId();

                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", task.ProjectId);
            
            return View(task);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", task.ProjectId);
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,UserId,Description,Duedate,Priority")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", task.ProjectId);
            return View(task);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
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
