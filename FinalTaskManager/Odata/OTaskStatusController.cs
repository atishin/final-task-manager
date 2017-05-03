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
    builder.EntitySet<TaskStatus>("TaskStatus");
    builder.EntitySet<ProjectTask>("ProjectTasks"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class OTaskStatusController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/TaskStatus
        [EnableQuery]
        public IQueryable<TaskStatus> Get()
        {
            return db.TaskStatus;
        }

        // GET: odata/TaskStatus(5)
        [EnableQuery]
        public SingleResult<TaskStatus> Get([FromODataUri] int key)
        {
            return SingleResult.Create(db.TaskStatus.Where(taskStatus => taskStatus.Id == key));
        }

        // PUT: odata/TaskStatus(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<TaskStatus> patch)
        {
            Validate(patch.GetInstance());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TaskStatus taskStatus = db.TaskStatus.Find(key);
            if (taskStatus == null)
            {
                return NotFound();
            }

            patch.Put(taskStatus);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskStatusExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(taskStatus);
        }

        // POST: odata/TaskStatus
        public IHttpActionResult Post(TaskStatus taskStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TaskStatus.Add(taskStatus);
            db.SaveChanges();

            return Created(taskStatus);
        }

        // PATCH: odata/TaskStatus(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<TaskStatus> patch)
        {
            Validate(patch.GetInstance());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TaskStatus taskStatus = db.TaskStatus.Find(key);
            if (taskStatus == null)
            {
                return NotFound();
            }

            patch.Patch(taskStatus);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskStatusExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(taskStatus);
        }

        // DELETE: odata/TaskStatus(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            TaskStatus taskStatus = db.TaskStatus.Find(key);
            if (taskStatus == null)
            {
                return NotFound();
            }

            db.TaskStatus.Remove(taskStatus);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/TaskStatus(5)/Tasks
        [EnableQuery]
        public IQueryable<ProjectTask> GetTasks([FromODataUri] int key)
        {
            return db.TaskStatus.Where(m => m.Id == key).SelectMany(m => m.Tasks);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskStatusExists(int key)
        {
            return db.TaskStatus.Count(e => e.Id == key) > 0;
        }
    }
}
