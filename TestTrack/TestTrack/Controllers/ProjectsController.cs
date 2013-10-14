﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        // GET: /Projects/

        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        [HttpGet] // GET: /Projects/Create
        public ActionResult Create()
        {
            return View("Create", new ProjectVM());
        }

        [HttpPost] // GET: /Projects/Create
        public ActionResult Create(ProjectVM projectVM)
        {
            var project = new Project
            {
                ProjectID = 0,
                Description = projectVM.Description,
                Title = projectVM.Title
            };

            db.Projects.Add(project);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


         [HttpGet] // GET: /Projects/Edit/5
         public ActionResult Edit(int id = 0)
         {
             var project = db.Projects.Find(id);

             if (project == null)return HttpNotFound();

             var team = new ProjectVM
             {
                 Description = project.Description,
                 ProjectID = project.ProjectID,
                 Title = project.Title
             };

             return View(team);
         }


        [HttpPost]
         public ActionResult Edit(ProjectVM projectVM)
        {
            var project = db.Projects.Find(projectVM.ProjectID);

            if (project == null) return HttpNotFound();

            project.Description = projectVM.Description;
            project.Title = projectVM.Title;

            db.Entry(project).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: /Projects/Save/

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Save(Project project)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (project.ProjectID == 0)
        //        {
        //            db.Projects.Add(project);
        //        }
        //        else
        //        {
        //            db.Entry(project).State = EntityState.Modified;
        //        }

        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(project);
        //}

        [HttpGet]
        public ActionResult Delete(int id = 0)
        {
            var project = db.Projects.Find(id);
            if (project == null) return HttpNotFound();
            
            return View(project);
        }
         
        [HttpPost,ValidateAntiForgeryToken, ActionName("Delete")] 
        public ActionResult DeleteConfirm(int id)
        { 
            db.Projects.Remove( db.Projects.Find(id));

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult ProjectsDropdown()
        {
            return PartialView("_ProjectsDropdown", db.Projects.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}