using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectManagementTool.Models;

namespace ProjectManagementTool.Controllers
{
    public class AssignResourcesController : Controller
    {
        private ProjectManagementToolEntities db = new ProjectManagementToolEntities();
        ApplicationDbContext context = new ApplicationDbContext();

        // GET: AssignResources
        public ActionResult Index()
        {
            var assignResources = db.AssignResources.Include(a => a.Project);
            return View(assignResources.ToList());
        }

        // GET: AssignResources/Details/5
        public ActionResult Details(int? id)
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
            return View(assignResource);
        }

        // GET: AssignResources/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.UserId = new SelectList(context.Users.Where(x => x.Designation != "ItAdmin" && x.Designation != "ProjectManager"), "Id", "Name");
            return View();
        }

        // POST: AssignResources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectId,UserId")] AssignResource assignResource)
        {
            if (ModelState.IsValid)
            {
                db.AssignResources.Add(assignResource);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", assignResource.ProjectId);
            return View(assignResource);
        }

        // GET: AssignResources/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", assignResource.ProjectId);
            return View(assignResource);
        }

        // POST: AssignResources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,UserId")] AssignResource assignResource)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assignResource).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", assignResource.ProjectId);
            return View(assignResource);
        }

        // GET: AssignResources/Delete/5
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
            return View(assignResource);
        }

        // POST: AssignResources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AssignResource assignResource = db.AssignResources.Find(id);
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
