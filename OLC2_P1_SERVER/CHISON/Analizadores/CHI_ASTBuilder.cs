using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Irony.Parsing;
using OLC2_P1_SERVER.CHISON.Arbol;
using OLC2_P1_SERVER.CHISON.Abstracto;
using OLC2_P1_SERVER.CHISON.Constantes;
using OLC2_P1_SERVER.CHISON.Manejadores;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace OLC2_P1_SERVER.Analizadores
{
    public class CHI_ASTBuilder
    {
        public CHI_AST Analizar(ParseTreeNode raiz)
        {
            return (CHI_AST)Recorrido(raiz);
        }

        public object Recorrido(ParseTreeNode actual)
        {
            if (EstoyAca(actual, "INICIO"))
            {
                return new CHI_AST(Recorrido(actual.ChildNodes[4]), Recorrido(actual.ChildNodes[10]));
            }

            else if (EstoyAca(actual, "IMPORTAR"))
            {
                return new CHI_Importar(ObtenerLexema(actual, 1), actual.ChildNodes[0].Token.Location.Position, actual.ChildNodes[4].Token.Location.Position + 2, GetFila(actual, 0), GetColumna(actual, 0));
            }

            else if (EstoyAca(actual, "ELEMENTO_TABLA"))
            {
                return new CHI_Tabla(ObtenerLexemaSinComillas(actual, 7), Recorrido(actual.ChildNodes[12]), Recorrido(actual.ChildNodes[18]));
            }

            else if (EstoyAca(actual, "ELEMENTO_USERTYPE"))
            {
                return new CHI_UserType(ObtenerLexemaSinComillas(actual, 7), Recorrido(actual.ChildNodes[12]));
            }

            else if (EstoyAca(actual, "ELEMENTO_PROCEDURE"))
            {
                return new CHI_Procedure(ObtenerLexemaSinComillas(actual, 7), Recorrido(actual.ChildNodes[12]), CleanProcedureContent(ObtenerLexema(actual, 17)));
            }

            else if (EstoyAca(actual, "VAL"))
            {
                return new CHI_Val(ObtenerLexemaSinComillas(actual, 0), Recorrido(actual.ChildNodes[2]));
            }

            else if (EstoyAca(actual, "VALUE"))
            {
                return new CHI_Value((List<CHI_Val>)Recorrido(actual.ChildNodes[1]));
            }

            else if (EstoyAca(actual, "PERMISO"))
            {
                return new CHI_Permiso(ObtenerLexemaSinComillas(actual, 3));
            }

            else if (EstoyAca(actual, "ATRIBUTO"))
            {
                return new CHI_Atributo(ObtenerLexemaSinComillas(actual, 3), (CHIDataType)Recorrido(actual.ChildNodes[7]));
            }

            else if (EstoyAca(actual, "DATABASE"))
            {
                return new CHI_Database(ObtenerLexemaSinComillas(actual, 3), Recorrido(actual.ChildNodes[8]));
            }

            else if (EstoyAca(actual, "PARAMETRO"))
            {
                return new CHI_Parametro(ObtenerLexemaSinComillas(actual, 3), (CHIDataType)Recorrido(actual.ChildNodes[7]), (string)Recorrido(actual.ChildNodes[11]));
            }

            else if (EstoyAca(actual, "COLUMNA"))
            {
                return new CHI_Columna(ObtenerLexemaSinComillas(actual, 3), (CHIDataType)Recorrido(actual.ChildNodes[7]), (bool)Recorrido(actual.ChildNodes[11]));
            }

            else if (EstoyAca(actual, "USUARIO"))
            {
                return new CHI_Usuario(ObtenerLexemaSinComillas(actual, 3), ObtenerLexema(actual, 7), Recorrido(actual.ChildNodes[12]));
            }

            else if (EstoyAca(actual, "LISTA_VAL"))
            {
                List<CHI_Val> lista_values = new List<CHI_Val>();
                foreach (ParseTreeNode hijo in actual.ChildNodes)
                {
                    lista_values.Add((CHI_Val)Recorrido(hijo));
                }
                return lista_values;
            }

            else if (EstoyAca(actual, "LISTA_VALUES"))
            {
                List<CHI_Value> lista_values = new List<CHI_Value>();
                foreach (ParseTreeNode hijo in actual.ChildNodes)
                {
                    lista_values.Add((CHI_Value)Recorrido(hijo));
                }
                return lista_values;
            }

            else if (EstoyAca(actual, "LISTA_USUARIOS"))
            {
                List<CHI_Usuario> lista_usuarios = new List<CHI_Usuario>();
                foreach (ParseTreeNode hijo in actual.ChildNodes)
                {
                    lista_usuarios.Add((CHI_Usuario)Recorrido(hijo));
                }
                return lista_usuarios;
            }

            else if (EstoyAca(actual, "LISTA_PERMISOS"))
            {
                List<CHI_Permiso> lista_permisos = new List<CHI_Permiso>();
                foreach (ParseTreeNode hijo in actual.ChildNodes)
                {
                    lista_permisos.Add((CHI_Permiso)Recorrido(hijo));
                }
                return lista_permisos;
            }

            else if (EstoyAca(actual, "LISTA_COLUMNAS"))
            {
                List<CHI_Columna> lista_columnas = new List<CHI_Columna>();
                foreach (ParseTreeNode hijo in actual.ChildNodes)
                {
                    lista_columnas.Add((CHI_Columna)Recorrido(hijo));
                }
                return lista_columnas;
            }

            else if (EstoyAca(actual, "LISTA_DATABASE"))
            {
                List<CHI_Database> lista_database = new List<CHI_Database>();
                foreach (ParseTreeNode hijo in actual.ChildNodes)
                {
                    lista_database.Add((CHI_Database)Recorrido(hijo));
                }
                return lista_database;
            }

            else if (EstoyAca(actual, "LISTA_ELEMENTOS"))
            {
                List<CHI_Instruccion> lista_elementos = new List<CHI_Instruccion>();
                foreach (ParseTreeNode hijo in actual.ChildNodes)
                {
                    lista_elementos.Add((CHI_Instruccion)Recorrido(hijo));
                }
                return lista_elementos;
            }

            else if (EstoyAca(actual, "LISTA_ATRIBUTOS"))
            {
                List<CHI_Atributo> lista_atributos = new List<CHI_Atributo>();
                foreach (ParseTreeNode hijo in actual.ChildNodes)
                {
                    lista_atributos.Add((CHI_Atributo)Recorrido(hijo));
                }
                return lista_atributos;
            }

            else if (EstoyAca(actual, "LISTA_PARAMETROS"))
            {
                List<CHI_Parametro> lista_parametros = new List<CHI_Parametro>();
                foreach (ParseTreeNode hijo in actual.ChildNodes)
                {
                    lista_parametros.Add((CHI_Parametro)Recorrido(hijo));
                }
                return lista_parametros;
            }

            else if (EstoyAca(actual, "LISTA_PRIMITIVOS"))
            {
                List<object> lista_primitivos = new List<object>();
                foreach (ParseTreeNode hijo in actual.ChildNodes)
                {
                    lista_primitivos.Add(Recorrido(hijo));
                }
                return lista_primitivos;
            }

            else if (EstoyAca(actual, "BLOQUE_DATABASE"))
            {
                return Recorrido(actual.ChildNodes[0]);
            }

            else if (EstoyAca(actual, "BLOQUE_USUARIOS"))
            {
                return Recorrido(actual.ChildNodes[0]);
            }

            else if (EstoyAca(actual, "BLOQUE_PERMISOS"))
            {
                return Recorrido(actual.ChildNodes[0]);
            }

            else if (EstoyAca(actual, "BLOQUE_DATA"))
            {
                return Recorrido(actual.ChildNodes[0]);
            }

            else if (EstoyAca(actual, "BLOQUE_PARAMETROS"))
            {
                return Recorrido(actual.ChildNodes[0]);
            }

            else if (EstoyAca(actual, "BLOQUE_ATRIBUTOS"))
            {
                return Recorrido(actual.ChildNodes[0]);
            }

            else if (EstoyAca(actual, "BLOQUE_COLUMNAS"))
            {
                return Recorrido(actual.ChildNodes[0]);
            }

            else if (EstoyAca(actual, "BLOQUE_VALUES"))
            {
                return Recorrido(actual.ChildNodes[0]);
            }

            else if (EstoyAca(actual, "ELEMENTO"))
            {
                return Recorrido(actual.ChildNodes[0]);
            }

            else if (EstoyAca(actual, "EXPRESION"))
            {
                switch (actual.ChildNodes.Count)
                {
                    case 3:
                        return Recorrido(actual.ChildNodes[1]);
                    default:
                        return Recorrido(actual.ChildNodes[0]);
                }
            }

            else if (EstoyAca(actual, "TIPO_PARAM"))
            {
                return ObtenerLexema(actual, 0);
            }

            else if (EstoyAca(actual, "VALOR_PRIMITIVO"))
            {
                if (EstoyAca(actual.ChildNodes[0], "numero"))
                {
                    double result = Convert.ToDouble(ObtenerLexema(actual, 0));
                    try
                    {
                        int result2 = Convert.ToInt32(ObtenerLexema(actual, 0));
                        return result2;
                    }
                    catch (Exception)
                    {
                        return result;
                    }
                }
                else if (EstoyAca(actual.ChildNodes[0], "cadena"))
                {
                    string aux = ObtenerLexema(actual, 0).ToString();
                    aux = aux.Replace("\\n", "\n");
                    aux = aux.Replace("\\t", "\t");
                    aux = aux.Replace("\\r", "\r");
                    return aux;
                }
                else if (EstoyAca(actual.ChildNodes[0], "fecha"))
                {
                    return ObtenerLexema(actual, 0).Replace("'", "");
                }
                else if (EstoyAca(actual.ChildNodes[0], "hora"))
                {
                    return ObtenerLexema(actual, 0).Replace("'", "");
                }
                else if (EstoyAca(actual.ChildNodes[0], "true"))
                {
                    return true;
                }
                else if (EstoyAca(actual.ChildNodes[0], "false"))
                {
                    return false;
                }
                /*else if (EstoyAca(actual.ChildNodes[0], "null"))
                {
                    return null;
                }*/
            }

            else if (EstoyAca(actual, "TIPO_DATO"))
            {
                if (EstoyAca(actual.ChildNodes[0], "int"))
                {
                    return new CHIDataType(CHITipoDato.INT, "");
                }
                else if (EstoyAca(actual.ChildNodes[0], "double"))
                {
                    return new CHIDataType(CHITipoDato.DOUBLE, "");
                }
                else if (EstoyAca(actual.ChildNodes[0], "string"))
                {
                    return new CHIDataType(CHITipoDato.STRING, "");
                }
                else if (EstoyAca(actual.ChildNodes[0], "boolean"))
                {
                    return new CHIDataType(CHITipoDato.BOOLEAN, "");
                }
                else if (EstoyAca(actual.ChildNodes[0], "date"))
                {
                    return new CHIDataType(CHITipoDato.DATE, "");
                }
                else if (EstoyAca(actual.ChildNodes[0], "time"))
                {
                    return new CHIDataType(CHITipoDato.TIME, "");
                }
                else if (EstoyAca(actual.ChildNodes[0], "counter"))
                {
                    return new CHIDataType(CHITipoDato.COUNTER, "");
                }
                else if (EstoyAca(actual.ChildNodes[0], "cursor"))
                {
                    return new CHIDataType(CHITipoDato.CURSOR, "");
                }
                else
                {
                    return new CHIDataType(CHITipoDato.OTRO, ObtenerLexemaSinComillas(actual, 0));
                }
            }

            else if (EstoyAca(actual, "TIPO_BOOLEANO"))
            {
                if (EstoyAca(actual.ChildNodes[0], "true"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return null;
        }

        static bool EstoyAca(ParseTreeNode nodo, string nombre)
        {
            return nodo.Term.Name.Equals(nombre, StringComparison.InvariantCultureIgnoreCase);
        }

        static string ObtenerLexema(ParseTreeNode nodo, int num)
        {
            return nodo.ChildNodes[num].Token.Text;
        }

        static string ObtenerLexemaSinComillas(ParseTreeNode actual, int num)
        {
            string aux = ObtenerLexema(actual, num).ToString();
            aux = aux.Replace("\\n", "\n");
            aux = aux.Replace("\\t", "\t");
            aux = aux.Replace("\\r", "\r");
            aux = aux.Replace("\"", "\"");
            aux = aux.Substring(1, aux.Length - 2);
            return aux;
        }

        static int GetFila(ParseTreeNode nodo, int num)
        {
            return nodo.ChildNodes[num].Token.Location.Line;
        }

        static int GetColumna(ParseTreeNode nodo, int num)
        {
            return nodo.ChildNodes[num].Token.Location.Column;
        }

        static string CleanProcedureContent(string content)
        {
            string aux = content.Trim();
            aux = aux.Replace("\\n", "\n");
            aux = aux.Replace("\\r", "\r");
            aux = aux.Replace("\\t", "\t");
            aux = aux.Replace("$", "");
            return aux;
        }
    }
}