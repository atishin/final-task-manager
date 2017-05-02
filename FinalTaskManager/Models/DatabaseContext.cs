using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalTaskManager.Models
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

    }
}