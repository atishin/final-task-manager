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
    public class ProjectChatsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProjectChats
        public IQueryable<ProjectChat> GetProjectChats()
        {
            return db.ProjectChats;
        }

        // GET: api/ProjectChats/5
        [ResponseType(typeof(ProjectChat))]
        public IHttpActionResult GetProjectChat(int id)
        {
            ProjectChat projectChat = db.ProjectChats.Find(id);
            if (projectChat == null)
            {
                return NotFound();
            }

            return Ok(projectChat);
        }

        // PUT: api/ProjectChats/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProjectChat(int id, ProjectChat projectChat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != projectChat.Id)
            {
                return BadRequest();
            }

            db.Entry(projectChat).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectChatExists(id))
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

        // POST: api/ProjectChats
        [ResponseType(typeof(ProjectChat))]
        public IHttpActionResult PostProjectChat(ProjectChat projectChat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProjectChats.Add(projectChat);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ProjectChatExists(projectChat.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = projectChat.Id }, projectChat);
        }

        // DELETE: api/ProjectChats/5
        [ResponseType(typeof(ProjectChat))]
        public IHttpActionResult DeleteProjectChat(int id)
        {
            ProjectChat projectChat = db.ProjectChats.Find(id);
            if (projectChat == null)
            {
                return NotFound();
            }

            db.ProjectChats.Remove(projectChat);
            db.SaveChanges();

            return Ok(projectChat);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectChatExists(int id)
        {
            return db.ProjectChats.Count(e => e.Id == id) > 0;
        }
    }
}