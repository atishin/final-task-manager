using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.OData.Extensions;
using System.Web.OData.Builder;
using FinalTaskManager.Models;

namespace FinalTaskManager
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.Count().Expand().Select().Filter().MaxTop(null).OrderBy();
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Project>("OProjects");
            builder.EntitySet<ApplicationUser>("OApplicationUsers");
            builder.EntitySet<ProjectChat>("OProjectChats");
            builder.EntitySet<ProjectTask>("OProjectTasks");
            builder.EntitySet<TaskStatus>("OTaskStatus");
            builder.EntitySet<Message>("OMessages");


            // Web API routes
            
            config.MapHttpAttributeRoutes();
            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());


        }
    }
}
