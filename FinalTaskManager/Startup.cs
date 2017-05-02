using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(FinalTaskManager.Startup))]

namespace FinalTaskManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.Map("/signalr", (map) =>
            {
                map.UseCors(CorsOptions.AllowAll);
                map.RunSignalR();
            });
            ConfigureAuth(app);
            
        }
    }
}
