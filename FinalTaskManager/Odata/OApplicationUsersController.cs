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
using Microsoft.AspNet.Identity.EntityFramework;

namespace FinalTaskManager.Odata
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.OData.Builder;
    using System.Web.OData.Extensions;
    using FinalTaskManager.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<ApplicationUser>("ApplicationUsers");
    builder.EntitySet<IdentityUserClaim>("IdentityUserClaims"); 
    builder.EntitySet<IdentityUserLogin>("IdentityUserLogins"); 
    builder.EntitySet<Message>("Messages"); 
    builder.EntitySet<Project>("Projects"); 
    builder.EntitySet<IdentityUserRole>("IdentityUserRoles"); 
    builder.EntitySet<ProjectTask>("ProjectTasks"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class OApplicationUsersController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/ApplicationUsers
        [EnableQuery]
        public IQueryable<ApplicationUser> Get()
        {
            return db.Users;
        }

        // GET: odata/ApplicationUsers(5)
        [EnableQuery]
        public SingleResult<ApplicationUser> GetApplicationUser([FromODataUri] string key)
        {
            return SingleResult.Create(db.Users.Where(applicationUser => applicationUser.Id == key));
        }

        // PUT: odata/ApplicationUsers(5)
        public IHttpActionResult Put([FromODataUri] string key, Delta<ApplicationUser> patch)
        {
            Validate(patch.GetInstance());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationUser applicationUser = db.Users.Find(key);
            if (applicationUser == null)
            {
                return NotFound();
            }

            patch.Put(applicationUser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(applicationUser);
        }

        // POST: odata/ApplicationUsers
        public IHttpActionResult Post(ApplicationUser applicationUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(applicationUser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ApplicationUserExists(applicationUser.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(applicationUser);
        }

        // PATCH: odata/ApplicationUsers(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] string key, Delta<ApplicationUser> patch)
        {
            Validate(patch.GetInstance());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationUser applicationUser = db.Users.Find(key);
            if (applicationUser == null)
            {
                return NotFound();
            }

            patch.Patch(applicationUser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(applicationUser);
        }

        // DELETE: odata/ApplicationUsers(5)
        public IHttpActionResult Delete([FromODataUri] string key)
        {
            ApplicationUser applicationUser = db.Users.Find(key);
            if (applicationUser == null)
            {
                return NotFound();
            }

            db.Users.Remove(applicationUser);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ApplicationUsers(5)/Claims
        [EnableQuery]
        public IQueryable<IdentityUserClaim> GetClaims([FromODataUri] string key)
        {
            return db.Users.Where(m => m.Id == key).SelectMany(m => m.Claims);
        }

        // GET: odata/ApplicationUsers(5)/Logins
        [EnableQuery]
        public IQueryable<IdentityUserLogin> GetLogins([FromODataUri] string key)
        {
            return db.Users.Where(m => m.Id == key).SelectMany(m => m.Logins);
        }

        // GET: odata/ApplicationUsers(5)/Messages
        [EnableQuery]
        public IQueryable<Message> GetMessages([FromODataUri] string key)
        {
            return db.Users.Where(m => m.Id == key).SelectMany(m => m.Messages);
        }

        // GET: odata/ApplicationUsers(5)/Projects
        [EnableQuery]
        public IQueryable<Project> GetProjects([FromODataUri] string key)
        {
            return db.Users.Where(m => m.Id == key).SelectMany(m => m.Projects);
        }

        // GET: odata/ApplicationUsers(5)/Roles
        [EnableQuery]
        public IQueryable<IdentityUserRole> GetRoles([FromODataUri] string key)
        {
            return db.Users.Where(m => m.Id == key).SelectMany(m => m.Roles);
        }

        // GET: odata/ApplicationUsers(5)/Tasks
        [EnableQuery]
        public IQueryable<ProjectTask> GetTasks([FromODataUri] string key)
        {
            return db.Users.Where(m => m.Id == key).SelectMany(m => m.Tasks);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ApplicationUserExists(string key)
        {
            return db.Users.Count(e => e.Id == key) > 0;
        }
    }
}
