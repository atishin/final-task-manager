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
    builder.EntitySet<Project>("Projects");
    builder.EntitySet<ApplicationUser>("ApplicationUsers"); 
    builder.EntitySet<ProjectChat>("ProjectChats"); 
    builder.EntitySet<ProjectTask>("ProjectTasks"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class OProjectsController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/Projects
        [EnableQuery]
        public IQueryable<Project> Get()
        {
            return db.Projects;
        }

        // GET: odata/Projects(5)
        [EnableQuery]
        public SingleResult<Project> GetProject([FromODataUri] int key)
        {
            return SingleResult.Create(db.Projects.Where(project => project.Id == key));
        }

        // PUT: odata/Projects(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Project> patch)
        {
            Validate(patch.GetInstance());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Project project = db.Projects.Find(key);
            if (project == null)
            {
                return NotFound();
            }

            patch.Put(project);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(project);
        }

        // POST: odata/Projects
        public IHttpActionResult Post(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Projects.Add(project);
            db.SaveChanges();

            return Created(project);
        }

        // PATCH: odata/Projects(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Project> patch)
        {
            Validate(patch.GetInstance());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Project project = db.Projects.Find(key);
            if (project == null)
            {
                return NotFound();
            }

            patch.Patch(project);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(project);
        }

        // DELETE: odata/Projects(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Project project = db.Projects.Find(key);
            if (project == null)
            {
                return NotFound();
            }

            db.Projects.Remove(project);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Projects(5)/Manager
        [EnableQuery]
        public SingleResult<ApplicationUser> GetManager([FromODataUri] int key)
        {
            return SingleResult.Create(db.Projects.Where(m => m.Id == key).Select(m => m.Manager));
        }

        // GET: odata/Projects(5)/ProjectChat
        [EnableQuery]
        public SingleResult<ProjectChat> GetProjectChat([FromODataUri] int key)
        {
            return SingleResult.Create(db.Projects.Where(m => m.Id == key).Select(m => m.ProjectChat));
        }

        // GET: odata/Projects(5)/Tasks
        [EnableQuery]
        public IQueryable<ProjectTask> GetTasks([FromODataUri] int key)
        {
            return db.Projects.Where(m => m.Id == key).SelectMany(m => m.Tasks);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectExists(int key)
        {
            return db.Projects.Count(e => e.Id == key) > 0;
        }
    }
}
