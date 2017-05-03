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
    builder.EntitySet<Message>("Messages");
    builder.EntitySet<ProjectChat>("ProjectChats"); 
    builder.EntitySet<ApplicationUser>("ApplicationUsers"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class OMessagesController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/Messages
        [EnableQuery]
        public IQueryable<Message> Get()
        {
            return db.Messages;
        }

        // GET: odata/Messages(5)
        [EnableQuery]
        public SingleResult<Message> GetMessage([FromODataUri] int key)
        {
            return SingleResult.Create(db.Messages.Where(message => message.Id == key));
        }

        // PUT: odata/Messages(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Message> patch)
        {
            Validate(patch.GetInstance());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Message message = db.Messages.Find(key);
            if (message == null)
            {
                return NotFound();
            }

            patch.Put(message);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(message);
        }

        // POST: odata/Messages
        public IHttpActionResult Post(Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Messages.Add(message);
            db.SaveChanges();

            return Created(message);
        }

        // PATCH: odata/Messages(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Message> patch)
        {
            Validate(patch.GetInstance());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Message message = db.Messages.Find(key);
            if (message == null)
            {
                return NotFound();
            }

            patch.Patch(message);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(message);
        }

        // DELETE: odata/Messages(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Message message = db.Messages.Find(key);
            if (message == null)
            {
                return NotFound();
            }

            db.Messages.Remove(message);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Messages(5)/ProjectChat
        [EnableQuery]
        public SingleResult<ProjectChat> GetProjectChat([FromODataUri] int key)
        {
            return SingleResult.Create(db.Messages.Where(m => m.Id == key).Select(m => m.ProjectChat));
        }

        // GET: odata/Messages(5)/User
        [EnableQuery]
        public SingleResult<ApplicationUser> GetUser([FromODataUri] int key)
        {
            return SingleResult.Create(db.Messages.Where(m => m.Id == key).Select(m => m.User));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MessageExists(int key)
        {
            return db.Messages.Count(e => e.Id == key) > 0;
        }
    }
}
