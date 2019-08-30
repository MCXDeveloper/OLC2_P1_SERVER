using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Irony.Parsing;

namespace OLC2_P1_SERVER.LUP.Analizadores
{
    public class LUP_ASTBuilder
    {
        public LUP_AST Analizar(ParseTreeNode raiz)
        {
            return (LUP_AST)Recorrido(raiz);
        }

        public object Recorrido(ParseTreeNode actual)
        {
            if(EstoyAca(actual, "INI"))
            {
                return Recorrido(actual.ChildNodes[0]);
            }
            else if(EstoyAca(actual, "SENTENCIAS"))
            {
                // Significa que viene una sentencia logout.
                if (actual.ChildNodes.Count == 5)
                {
                    return new LogoutPackage(GetLexema(actual, 2));
                }
                else
                {
                    // Significa que viene una sentencia login.
                    if (EstoyAca(actual.ChildNodes[0], "r_open_login"))
                    {
                        return new LoginPackage(GetLexema(actual, 2), GetLexema(actual, 5));
                    }
                    // Significa que viene una sentencia query.
                    else
                    {
                        return new QueryPackage(GetLexema(actual, 2), GetLexema(actual, 5));
                    }
                }
            }

            return null;
        }

        static bool EstoyAca(ParseTreeNode nodo, string nombre)
        {
            return nodo.Term.Name.Equals(nombre, System.StringComparison.InvariantCultureIgnoreCase);
        }

        static string GetLexema(ParseTreeNode nodo, int num)
        {
            return nodo.ChildNodes[num].Token.Text;
        }
    }
}