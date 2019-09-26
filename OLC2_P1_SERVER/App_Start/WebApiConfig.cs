using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Irony;
using Irony.Parsing;
using OLC2_P1_SERVER.Analizadores;
using OLC2_P1_SERVER.CHISON.Abstracto;
using OLC2_P1_SERVER.CHISON.Estaticas;
using OLC2_P1_SERVER.CQL;

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
        LoadChisonFiles();

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

    private static void LoadChisonFiles()
    {
        string path = System.Web.Hosting.HostingEnvironment.MapPath("~/ChisonFilesContainer/Principal.chison");
        string[] lines = File.ReadAllLines(path);
        string contenido = string.Join("\n", lines);
        BuildChisonParsing(contenido);
    }

    private static object BuildChisonParsing(string entrada)
    {
        CHI_Grammar gramatica = new CHI_Grammar();
        LanguageData lenguaje = new LanguageData(gramatica);
        Parser parser = new Parser(lenguaje);
        ParseTree arbol = parser.Parse(entrada);

        if (arbol.ParserMessages.Count.Equals(0))
        {
            CHI_ASTBuilder builder = new CHI_ASTBuilder();
            CHI_AST auxArbol = builder.Analizar(arbol.Root);

            if (!(auxArbol is null))
            {
                StaticChison.InitializeStaticEnvironment();
                StaticChison.EstablecerYSepararCadenaEntrada(entrada);

                object parseResponse = auxArbol.Ejecutar();

                if (parseResponse is null)
                {
                    Debug.WriteLine("Análisis realizado exitosamente.");
                }
                else if (parseResponse is string)
                {
                    string nuevaCadena = (string)parseResponse;
                    BuildChisonParsing(nuevaCadena);
                }
            }
            else
            {
                Debug.WriteLine("Error. No se pudo construir el árbol de CHISON.");
            }
        }
        else
        {
            Debug.WriteLine("*************************************************");
            Debug.WriteLine("*************************************************");
            Debug.WriteLine("Hay errores lexicos o sintacticos.");
            Debug.WriteLine("El arbol de Irony no se construyó.");
            Debug.WriteLine("La cadena es inválida.");
            Debug.WriteLine("*************************************************");
            Debug.WriteLine("*************************************************");

            foreach (LogMessage err in arbol.ParserMessages)
            {
                Debug.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
                Debug.WriteLine("Error sintáctico en el parser.");
                Debug.WriteLine("El mensaje es: " + err.Message);
                Debug.WriteLine("La linea es: " + err.Location.Line);
                Debug.WriteLine("La columna es: " + err.Location.Column);
                Debug.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
            }

            Debug.WriteLine("*************************************************");
            Debug.WriteLine("*************************************************");
        }

        return null;
    }

}
