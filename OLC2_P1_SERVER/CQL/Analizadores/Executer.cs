using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Irony.Ast;
using Irony.Parsing;

public class Executer
{
    public void Analizar(string cadena)
    {
        Grammar gramatica = new Grammar();
        LanguageData lenguaje = new LanguageData(gramatica);
        Parser parser = new Parser(lenguaje);
        ParseTree arbol = parser.Parse(cadena);

        if (arbol.ParserMessages.Count == 0)
        {
            ASTBuilder builder = new ASTBuilder();
            AST ast = builder.Analizar(arbol.Root);
            ast.Ejecutar(new Entorno(null));
        }
        else
        {
            // TODO Devolver mensaje de error LUP - No se construyo el árbol de Irony.
        }
    }
}