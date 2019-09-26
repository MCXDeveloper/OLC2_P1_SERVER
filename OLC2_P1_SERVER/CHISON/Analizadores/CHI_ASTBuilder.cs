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
                object ob1 = Recorrido(actual.ChildNodes[4]);
                object ob2 = Recorrido(actual.ChildNodes[10]);

                if (EstoyAca(actual.ChildNodes[1], "\"DATABASES\""))
                {
                    return new CHI_AST(ob1, ob2);
                }
                else
                {
                    return new CHI_AST(ob2, ob1);
                }
            }

            else if (EstoyAca(actual, "IMPORTAR"))
            {
                return new CHI_Importar(ObtenerLexema(actual, 1), actual.ChildNodes[0].Token.Location.Position, actual.ChildNodes[4].Token.Location.Position + 2, GetFila(actual, 0), GetColumna(actual, 0));
            }

            else if (EstoyAca(actual, "VAL"))
            {
                return new CHI_Val(Recorrido(actual.ChildNodes[0]), Recorrido(actual.ChildNodes[2]));
            }

            else if (EstoyAca(actual, "PERMISO"))
            {
                return new CHI_Permiso(ObtenerLexemaSinComillas(actual, 3));
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

            else if (EstoyAca(actual, "LISTA_BLOCK"))
            {
                object[] objList = new object[4];
                foreach (ParseTreeNode hijo in actual.ChildNodes)
                {
                    object[] x = (object[])Recorrido(hijo);

                    if (x != null)
                    {
                        objList[(int)x[0]] = x[1];
                    }
                }
                return objList;
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

            else if (EstoyAca(actual, "LISTA_ELEMENTOS"))
            {
                List<object> lista_elementos = new List<object>();
                foreach (ParseTreeNode hijo in actual.ChildNodes)
                {
                    lista_elementos.Add(Recorrido(hijo));
                }
                return lista_elementos;
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

            else if (EstoyAca(actual, "BLOCK"))
            {
                if (EstoyAca(actual.ChildNodes[0], "\"CQL-TYPE\""))
                {
                    return new object[] { 0, ObtenerLexemaSinComillas(actual, 2) };
                }
                else if (EstoyAca(actual.ChildNodes[0], "\"NAME\""))
                {
                    return new object[] { 1, ObtenerLexemaSinComillas(actual, 2) };
                }
                else if (EstoyAca(actual.ChildNodes[0], "\"ATTRS\""))
                {
                    return new object[] { 2, Recorrido(actual.ChildNodes[3]) };
                }
                else if (EstoyAca(actual.ChildNodes[0], "\"DATA\""))
                {
                    return new object[] { 2, Recorrido(actual.ChildNodes[3]) };
                }
                else if (EstoyAca(actual.ChildNodes[0], "\"PARAMETERS\""))
                {
                    return new object[] { 2, Recorrido(actual.ChildNodes[3]) };
                }
                else if (EstoyAca(actual.ChildNodes[0], "\"TYPE\""))
                {
                    return new object[] { 2, (CHIDataType)Recorrido(actual.ChildNodes[2]) };
                }
                else if (EstoyAca(actual.ChildNodes[0], "\"PASSWORD\""))
                {
                    return new object[] { 2, ObtenerLexema(actual, 2) };
                }
                else if (EstoyAca(actual.ChildNodes[0], "\"COLUMNS\""))
                {
                    return new object[] { 3, Recorrido(actual.ChildNodes[3]) };
                }
                else if (EstoyAca(actual.ChildNodes[0], "\"INSTR\""))
                {
                    return new object[] { 3, CleanProcedureContent(ObtenerLexema(actual, 2)) };
                }
                else if (EstoyAca(actual.ChildNodes[0], "\"AS\""))
                {
                    return new object[] { 3, (string)Recorrido(actual.ChildNodes[2]) };
                }
                else if (EstoyAca(actual.ChildNodes[0], "\"PK\""))
                {
                    return new object[] { 3, (bool)Recorrido(actual.ChildNodes[2]) };
                }
                else if (EstoyAca(actual.ChildNodes[0], "\"PERMISSIONS\""))
                {
                    return new object[] { 3, Recorrido(actual.ChildNodes[3]) };
                }
            }

            else if (EstoyAca(actual, "BLOQUE_PERMISOS"))
            {
                return Recorrido(actual.ChildNodes[0]);
            }

            else if (EstoyAca(actual, "BLOQUE_DATA"))
            {
                return Recorrido(actual.ChildNodes[0]);
            }

            else if (EstoyAca(actual, "ELEMENTO"))
            {
                if (EstoyAca(actual.ChildNodes[1], "LISTA_BLOCK"))
                {
                    object[] listObj = (object[])Recorrido(actual.ChildNodes[1]);

                    if (listObj[0] != null)
                    {
                        if (((string)listObj[0]).Equals("TABLE"))
                        {
                            return new CHI_Tabla((string)listObj[1], listObj[3], listObj[2]);
                        }
                        else if (((string)listObj[0]).Equals("OBJECT"))
                        {
                            return new CHI_UserType((string)listObj[1], listObj[2]);
                        }
                        else
                        {
                            return new CHI_Procedure((string)listObj[1], listObj[2], (string)listObj[3]);
                        }
                    }
                    else if (listObj[3] != null)
                    {
                        if (listObj[3] is string)
                        {
                            return new CHI_Parametro((string)listObj[1], (CHIDataType)listObj[2], (string)listObj[3]);
                        }
                        else if (listObj[3] is bool)
                        {
                            return new CHI_Columna((string)listObj[1], (CHIDataType)listObj[2], (bool)listObj[3]);
                        }
                        else
                        {
                            return new CHI_Usuario((string)listObj[1], (string)listObj[2], listObj[3]);
                        }
                    }
                    else
                    {
                        if (listObj[2] is CHIDataType)
                        {
                            return new CHI_Atributo((string)listObj[1], (CHIDataType)listObj[2]);
                        }
                        else
                        {
                            return new CHI_Database((string)listObj[1], listObj[2]);
                        }
                    }
                }
                else
                {
                    return Recorrido(actual.ChildNodes[1]);
                }
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
                    return ObtenerLexema(actual, 0);
                }
                else if (EstoyAca(actual.ChildNodes[0], "hora"))
                {
                    return ObtenerLexema(actual, 0);
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