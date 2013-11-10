using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using TestTrack.Models;
using TestTrack.ViewModels;


//TODO: Review way there's a project list VM instead of using IList<ProjectVM> ?
//TODO: implement automapper for mapping models to viewModels

namespace TestTrack.Controllers
{
    [Authorize]
    public class ProjectsController : BaseController
    {
        private TestTrackDBContext db = new TestTrackDBContext();

        // GET: /Projects/
        public ActionResult Index()
        {
            ProjectsListVM vm = new ProjectsListVM(); 
            UserSettings userSettings = SessionWrapper.UserSettings;
            var projects = (from p in db.Projects
                           orderby p.Title
                           select p).ToList();

           //TESTING Mapping example
           //var projectsVM = Mapper.Map<IList<Project>, IList<ProjectVM>>(projects);

            vm.SelectedProject = userSettings.workingProject;
            vm.Values = projects;
            return View(vm);
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

             var projectVM = new ProjectVM
             {
                 Description = project.Description,
                 ProjectID = project.ProjectID,
                 Title = project.Title
             };

             return View(projectVM);
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

        [HttpGet]
        public ActionResult Delete(int id = 0)
        {
            var project = db.Projects.Find(id);
            if (project == null) return HttpNotFound();

            return PartialView(project);
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
            UserSettings userSettings = SessionWrapper.UserSettings;

            var vm = new ProjectsDropdownVM();
            var projects = from p in db.Projects
                           orderby p.Title
                           select p;
            vm.Values = new SelectList(projects.ToList(), "ProjectID", "Title");
 
            if (userSettings.workingProject > 0)
            {
                // Select the current project in user settings.
                vm.SelectedValue = userSettings.workingProject;
            }
            else
            {
                if (vm.Values.Any())
                {
                    // Set the current project using the first value. This will be changed in the future.
                    userSettings.workingProject = Int32.Parse(vm.Values.First().Value);
                }
                else
                {
                    // TODO: handle this case.
                    throw new Exception("No projects.");
                }
            }

            return PartialView("_ProjectsDropdown", vm);
        }

        [HttpPost]
        public ActionResult SetCurrent(ProjectsDropdownVM vm)
        {
            setCurrentProjectID(vm.SelectedValue);
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult SetProject(int id)
        {
            setCurrentProjectID(id);
            return Redirect(Request.UrlReferrer.ToString());
        }

        private void setCurrentProjectID(int id)
        {
            // Save the project in session.
            UserSettings userSettings = SessionWrapper.UserSettings;
            userSettings.workingProject = id;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}