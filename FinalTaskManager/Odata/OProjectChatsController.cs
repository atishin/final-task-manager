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
    builder.EntitySet<ProjectChat>("ProjectChats");
    builder.EntitySet<Message>("Messages"); 
    builder.EntitySet<Project>("Projects"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class OProjectChatsController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/ProjectChats
        [EnableQuery]
        public IQueryable<ProjectChat> Get()
        {
            return db.ProjectChats;
        }

        // GET: odata/ProjectChats(5)
        [EnableQuery]
        public SingleResult<ProjectChat> GetProjectChat([FromODataUri] int key)
        {
            return SingleResult.Create(db.ProjectChats.Where(projectChat => projectChat.Id == key));
        }

        // PUT: odata/ProjectChats(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<ProjectChat> patch)
        {
            Validate(patch.GetInstance());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProjectChat projectChat = db.ProjectChats.Find(key);
            if (projectChat == null)
            {
                return NotFound();
            }

            patch.Put(projectChat);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectChatExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(projectChat);
        }

        // POST: odata/ProjectChats
        public IHttpActionResult Post(ProjectChat projectChat)
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

            return Created(projectChat);
        }

        // PATCH: odata/ProjectChats(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<ProjectChat> patch)
        {
            Validate(patch.GetInstance());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProjectChat projectChat = db.ProjectChats.Find(key);
            if (projectChat == null)
            {
                return NotFound();
            }

            patch.Patch(projectChat);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectChatExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(projectChat);
        }

        // DELETE: odata/ProjectChats(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            ProjectChat projectChat = db.ProjectChats.Find(key);
            if (projectChat == null)
            {
                return NotFound();
            }

            db.ProjectChats.Remove(projectChat);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ProjectChats(5)/Messages
        [EnableQuery]
        public IQueryable<Message> GetMessages([FromODataUri] int key)
        {
            return db.ProjectChats.Where(m => m.Id == key).SelectMany(m => m.Messages);
        }

        // GET: odata/ProjectChats(5)/Project
        [EnableQuery]
        public SingleResult<Project> GetProject([FromODataUri] int key)
        {
            return SingleResult.Create(db.ProjectChats.Where(m => m.Id == key).Select(m => m.Project));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectChatExists(int key)
        {
            return db.ProjectChats.Count(e => e.Id == key) > 0;
        }
    }
}
