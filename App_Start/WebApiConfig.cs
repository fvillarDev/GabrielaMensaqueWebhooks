using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GabrielaMensaqueWebhooks
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de API web
            

            // Rutas de API web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "WebhooksApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Loads PayPal
            config.InitializeReceivePaypalWebHooks();
        }
    }
}
