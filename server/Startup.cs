using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using server.Hubs;

namespace server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Registers Microsoft.Identity.Web authentication handler
            // This will use settings from configuration to automatically authenticate
            // incoming requests using JWT tokens. The similar AddMicrosoftWebAppAuthentication
            // metho would setup authentication based on cookies.
            services.AddMicrosoftWebApiAuthentication(Configuration, "AzureAdB2C");

            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseAuthentication and .UseAuthorization are required for ASP.NET Core auth to work
            // Auth will automatically apply to any controllers or SignalR hubs that have [Authorize] attributes
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<TestHub>("/TestHub");
            });
        }
    }
}
