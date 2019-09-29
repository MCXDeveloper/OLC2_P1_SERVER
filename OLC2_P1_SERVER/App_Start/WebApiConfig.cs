using System.Diagnostics;
using System.Web.Http;

public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        // Ejecuto las acciones iniciales necesarias para comenzar con el parseo.
        CQL.AccionesIniciales();

        // Agrego el usuario admin.
        CQL.ListaUsuariosDisponibles.Add(new Usuario("admin", "admin"));

        //CQL.UsuarioLogueado = "admin";

        // Cargo toda la información de los archivos de CHISON a memoria.
        Rollback rb = new Rollback(true, -1, -1);
        rb.Ejecutar(null);
        Debug.WriteLine("Chison Loader has finished.");

        // Web API configuration and services
        config.EnableCors();
        // Web API routes
        config.MapHttpAttributeRoutes();

        config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
        );
    }
}
