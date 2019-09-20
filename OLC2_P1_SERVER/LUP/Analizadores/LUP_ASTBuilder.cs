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
            if(EstoyAca(actual, "INICIO"))
            {
                return new LUP_AST((LUP_Instruccion)Recorrido(actual.ChildNodes[0]));
            }
            else if(EstoyAca(actual, "SENTENCIAS"))
            {
                if (EstoyAca(actual.ChildNodes[0], "[+LOGIN]"))
                {
                    return new LoginPackage(GetLexema(actual, 1).Replace("[+USER]", "").Replace("[-USER]", "").Replace("\n", "").Trim(), GetLexema(actual, 2).Replace("[+PASS]", "").Replace("[-PASS]", "").Replace("\n", "").Trim());
                }
                else if (EstoyAca(actual.ChildNodes[0], "[+LOGOUT]"))
                {
                    return new LogoutPackage(GetLexema(actual, 1).Replace("[+USER]", "").Replace("[-USER]", "").Replace("\n", "").Trim());
                }
                else
                {
                    return new QueryPackage(GetLexema(actual, 1).Replace("[+USER]", "").Replace("[-USER]", "").Replace("\n", "").Trim(), GetLexema(actual, 2).Replace("[+DATA]", "").Replace("[-DATA]", ""));
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