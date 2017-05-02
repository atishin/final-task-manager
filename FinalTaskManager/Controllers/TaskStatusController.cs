using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using FinalTaskManager.Models;

namespace FinalTaskManager.Controllers
{
    public class TaskStatusController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/TaskStatus
        public IQueryable<TaskStatus> GetTaskStatus()
        {
            return db.TaskStatus;
        }

        // GET: api/TaskStatus/5
        [ResponseType(typeof(TaskStatus))]
        public IHttpActionResult GetTaskStatus(int id)
        {
            TaskStatus taskStatus = db.TaskStatus.Find(id);
            if (taskStatus == null)
            {
                return NotFound();
            }

            return Ok(taskStatus);
        }

        // PUT: api/TaskStatus/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTaskStatus(int id, TaskStatus taskStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != taskStatus.Id)
            {
                return BadRequest();
            }

            db.Entry(taskStatus).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskStatusExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/TaskStatus
        [ResponseType(typeof(TaskStatus))]
        public IHttpActionResult PostTaskStatus(TaskStatus taskStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TaskStatus.Add(taskStatus);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = taskStatus.Id }, taskStatus);
        }

        // DELETE: api/TaskStatus/5
        [ResponseType(typeof(TaskStatus))]
        public IHttpActionResult DeleteTaskStatus(int id)
        {
            TaskStatus taskStatus = db.TaskStatus.Find(id);
            if (taskStatus == null)
            {
                return NotFound();
            }

            db.TaskStatus.Remove(taskStatus);
            db.SaveChanges();

            return Ok(taskStatus);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskStatusExists(int id)
        {
            return db.TaskStatus.Count(e => e.Id == id) > 0;
        }
    }
}