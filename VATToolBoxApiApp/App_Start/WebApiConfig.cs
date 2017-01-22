using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MongoDB.Bson.Serialization;
using VATToolBoxApiApp.Repository.Implementation;

namespace VATToolBoxApiApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            BsonClassMap.RegisterClassMap<RatesBson>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(x => x.Rates).SetElementName("rates");
                cm.MapMember(x => x.Id).SetElementName("_id");
                cm.MapMember(x => x.Version).SetElementName("version");
            });

        }
    }
}
