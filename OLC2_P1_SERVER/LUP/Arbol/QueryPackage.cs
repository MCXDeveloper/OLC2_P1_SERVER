using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Irony;
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
        Grammar gramatica = new Grammar();
        LanguageData lenguaje = new LanguageData(gramatica);
        Parser parser = new Parser(lenguaje);
        ParseTree arbol = parser.Parse(Consulta);

        if (arbol.ParserMessages.Count.Equals(0))
        {
            ASTBuilder builder = new ASTBuilder(Consulta);
            AST auxArbol = builder.Analizar(arbol.Root);

            if (!(auxArbol is null))
            {
                object parseResponse = auxArbol.Ejecutar((AST.global is null) ? new Entorno(null) : AST.global);

                if (parseResponse is Nulo)
                {
                    CQL.AddLUPMessage("Análisis realizado exitosamente.");
                }
            }
            else
            {
                CQL.AddLUPMessage("Error. No se pudo construir el árbol de CQL.");
            }
        }
        else
        {
            CQL.AddLUPMessage("Hay errores lexicos o sintacticos.");
            CQL.AddLUPMessage("El arbol de Irony no se construyó.");
            CQL.AddLUPMessage("La cadena es inválida.");

            foreach (LogMessage err in arbol.ParserMessages)
            {
                CQL.AddLUPError("Sintáctico", "Parser", err.Message, err.Location.Line, err.Location.Column);
            }
        }

        return CQL.GetCompleteResponse();
    }
}