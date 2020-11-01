using Owin;
using System.Web.Http;

namespace RMS.Simulator
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Adding to the pipeline with our own middleware
            app.Use(async (context, next) =>
            {
                // Add Header
                //context.Response.Headers[Constant.Headers.Product] = Constant.Product.Information;

                // Call next middleware
                await next.Invoke();
            });

            // Custom Middleare
            //app.Use(typeof(CustomMiddleware));

            // Configure Web API for self-host. 
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "CommandApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Remove the XML formatter
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Add(config.Formatters.JsonFormatter);

            // Web Api
            app.UseWebApi(config);


            // Enable CORS
            //app.UseCors(CorsOptions.AllowAll);
        }
    }
}
