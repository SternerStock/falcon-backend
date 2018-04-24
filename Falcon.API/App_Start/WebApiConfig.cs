namespace Falcon.API
{
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using System.Web.Http.Routing;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            var cors = new EnableCorsAttribute("http://localhost:8000", "*", "*");
            config.EnableCors(cors);

            config.Routes.MapHttpRoute(
                "DefaultApiWithId",
                "Api/{controller}/{id}",
                new { id = RouteParameter.Optional },
                new { id = @"\d+" });

            config.Routes.MapHttpRoute(
                "DefaultApiWithAction",
                "Api/{controller}/{action}/{key}",
                new { key = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                "DefaultApiGet",
                "Api/{controller}",
                new { action = "Get" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });

            config.Routes.MapHttpRoute(
                "DefaultApiPost",
                "Api/{controller}",
                new { action = "Post" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) });
        }
    }
}