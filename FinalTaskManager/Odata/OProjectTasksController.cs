using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.ModelBinding;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using FinalTaskManager.Models;

namespace FinalTaskManager.Odata
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.OData.Builder;
    using System.Web.OData.Extensions;
    using FinalTaskManager.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<ProjectTask>("ProjectTasks");
    builder.EntitySet<ApplicationUser>("ApplicationUsers"); 
    builder.EntitySet<Project>("Projects"); 
    builder.EntitySet<TaskStatus>("TaskStatus"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class OProjectTasksController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/ProjectTasks
        [EnableQuery]
        public IQueryable<ProjectTask> Get()
        {
            return db.ProjectTasks;
        }

        // GET: odata/ProjectTasks(5)
        [EnableQuery]
        public SingleResult<ProjectTask> GetProjectTask([FromODataUri] int key)
        {
            return SingleResult.Create(db.ProjectTasks.Where(projectTask => projectTask.Id == key));
        }

        // PUT: odata/ProjectTasks(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<ProjectTask> patch)
        {
            Validate(patch.GetInstance());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProjectTask projectTask = db.ProjectTasks.Find(key);
            if (projectTask == null)
            {
                return NotFound();
            }

            patch.Put(projectTask);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectTaskExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(projectTask);
        }

        // POST: odata/ProjectTasks
        public IHttpActionResult Post(ProjectTask projectTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProjectTasks.Add(projectTask);
            db.SaveChanges();

            return Created(projectTask);
        }

        // PATCH: odata/ProjectTasks(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<ProjectTask> patch)
        {
            Validate(patch.GetInstance());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProjectTask projectTask = db.ProjectTasks.Find(key);
            if (projectTask == null)
            {
                return NotFound();
            }

            patch.Patch(projectTask);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectTaskExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(projectTask);
        }

        // DELETE: odata/ProjectTasks(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            ProjectTask projectTask = db.ProjectTasks.Find(key);
            if (projectTask == null)
            {
                return NotFound();
            }

            db.ProjectTasks.Remove(projectTask);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ProjectTasks(5)/ClosedByUser
        [EnableQuery]
        public SingleResult<ApplicationUser> GetClosedByUser([FromODataUri] int key)
        {
            return SingleResult.Create(db.ProjectTasks.Where(m => m.Id == key).Select(m => m.ClosedByUser));
        }

        // GET: odata/ProjectTasks(5)/Project
        [EnableQuery]
        public SingleResult<Project> GetProject([FromODataUri] int key)
        {
            return SingleResult.Create(db.ProjectTasks.Where(m => m.Id == key).Select(m => m.Project));
        }

        // GET: odata/ProjectTasks(5)/TaskStatus
        [EnableQuery]
        public SingleResult<TaskStatus> GetTaskStatus([FromODataUri] int key)
        {
            return SingleResult.Create(db.ProjectTasks.Where(m => m.Id == key).Select(m => m.TaskStatus));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectTaskExists(int key)
        {
            return db.ProjectTasks.Count(e => e.Id == key) > 0;
        }
    }
}
