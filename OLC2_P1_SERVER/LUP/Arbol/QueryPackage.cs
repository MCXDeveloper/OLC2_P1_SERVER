using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Irony.Parsing;

public class QueryPackage : LUP_Instruccion
{
    public string Usuario { get; set; }
    public string Consulta { get; set; }
    
    public QueryPackage(string user, string consulta)
    {
        Usuario = user;
        Consulta = consulta;
    }

    public object Ejecutar()
    {
        string response = CQL.BuildLUPMessage("Ocurrió un error al intentar interpretar el mensaje LUP.  Ver log del servidor.");

        // 1. Primero verifico el usuario actualmente logueado es igual al proporcionado en el constructor.
        if (!(CQL.UsuarioLogueado is null))
        {
            //if (CQL.UsuarioLogueado.Equals(Usuario))
            //{
                // 2. Recibo el contenido de la entrada y procedo a enviarla al parser.
                Grammar gramatica = new Grammar();
                LanguageData lenguaje = new LanguageData(gramatica);
                Parser parser = new Parser(lenguaje);
                ParseTree arbol = parser.Parse(Consulta);

                if (arbol.ParserMessages.Count.Equals(0))
                {
                    ASTBuilder builder = new ASTBuilder();
                    AST auxArbol = builder.Analizar(arbol.Root);

                    if (!(auxArbol is null))
                    {
                        object parseResponse = auxArbol.Ejecutar(new Entorno(null));
                        response = (!(parseResponse is null)) ? CQL.BuildLUPMessage(parseResponse.ToString()) : response;
                    }
                    else
                    {
                        response = CQL.BuildLUPMessage("Error. No se pudo construir el árbol de CQL.");
                    }
                }
                else
                {
                    response = CQL.BuildLUPMessage("Hay errores lexicos o sintacticos. El arbol de Irony no se construyó.  La cadena es inválida.");
                }
            //}
            //else
            //{
                //response = CQL.BuildLUPMessage("Error. El usuario actualmente logueado no concuerda con el proporcionado." + Environment.NewLine);
            //}
        }
        else
        {
            response = CQL.BuildLUPMessage("Error. El usuario actualmente logueado no concuerda con el proporcionado." + Environment.NewLine);
        }

        return response;
    }
}