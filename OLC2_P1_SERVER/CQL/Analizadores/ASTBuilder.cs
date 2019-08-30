using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Irony.Parsing;
using OLC2_P1_SERVER.CQL.Arbol;
using static Asignacion;
using static Entorno;

public class ASTBuilder
{
    public AST Analizar(ParseTreeNode raiz)
    {
        return (AST)Recorrido(raiz);
    }

    public object Recorrido(ParseTreeNode actual)
    {
        if (EstoyAca(actual, "INICIO"))
        {
            return Recorrido(actual.ChildNodes[0]);
        }
        else if (EstoyAca(actual, "LISTA_INSTRUCCIONES"))
        {
            List<Instruccion> instrucciones = new List<Instruccion>();
            foreach (ParseTreeNode hijo in actual.ChildNodes)
            {
                instrucciones.Add((Instruccion)Recorrido(hijo));
            }
            return new AST(instrucciones);
        }
        else if (EstoyAca(actual, "INSTRUCCION"))
        {
            return Recorrido(actual.ChildNodes[0]);
        }
        else if (EstoyAca(actual, "DECLARACION"))
        {
            switch (actual.ChildNodes.Count)
            {
                case 3:
                    // TIPO + LISTA_VARIABLES + puco
                    return new Declaracion((Tipo)Recorrido(actual.ChildNodes[0]), (List<string>)Recorrido(actual.ChildNodes[1]), GetFila(actual, 2), GetColumna(actual, 2));
                default:
                    // TIPO + LISTA_VARIABLES + igual + EXPRESION + puco
                    return new Declaracion((Tipo)Recorrido(actual.ChildNodes[0]), (List<string>)Recorrido(actual.ChildNodes[1]), (Expresion)Recorrido(actual.ChildNodes[3]), GetFila(actual, 2), GetColumna(actual, 2));
            }
        }
        else if (EstoyAca(actual, "ASIGNACION"))
        {
            switch (actual.ChildNodes.Count)
            {
                case 3:
                    // variable + igual + EXPRESION + puco
                    return new Asignacion(TipoAsignacion.AS_NORMAL, ObtenerLexema(actual, 0), (Expresion)Recorrido(actual.ChildNodes[2]), GetFila(actual, 1), GetColumna(actual, 1));
                default:
                    // variable + punto + LISTA_ACCESO + igual + EXPRESION + puco
                    return new AsignacionObjeto(TipoAsignacion.AS_NORMAL, ObtenerLexema(actual, 0), (List<string>)Recorrido(actual.ChildNodes[2]), (Expresion)Recorrido(actual.ChildNodes[4]), GetFila(actual, 1), GetColumna(actual, 1));
            }
        }
        else if (EstoyAca(actual, "LISTA_VARIABLES"))
        {
            List<Variable> lista_variables = new List<Variable>();
            foreach (ParseTreeNode hijo in actual.ChildNodes)
            {
                lista_variables.Add((Variable)Recorrido(hijo));
            }
            return lista_variables;
        }
        else if (EstoyAca(actual, "LISTA_ACCESO"))
        {
            List<string> lista_acceso = new List<string>();
            foreach (ParseTreeNode hijo in actual.ChildNodes)
            {
                lista_acceso.Add((string)Recorrido(hijo));
            }
            return lista_acceso;
        }
        else if (EstoyAca(actual, "LOG"))
        {
            return new Log((Instruccion)Recorrido(actual.ChildNodes[2]), GetFila(actual, 1), GetColumna(actual, 1));
        }
        else if (EstoyAca(actual, "SENTENCIA_IF"))
        {
            if (actual.ChildNodes.Count == 1)
            {
                return new If((Instruccion)Recorrido(actual.ChildNodes[0]));
            }
            else if (actual.ChildNodes.Count == 3)
            {
                return new If((Instruccion)Recorrido(actual.ChildNodes[0]), (List<Instruccion>)Recorrido(actual.ChildNodes[1]), (Instruccion)Recorrido(actual.ChildNodes[2]));
            }
            else
            {
                if (EstoyAca(actual.ChildNodes[1], "LISTA_ELSE_IF"))
                {
                    return new If((Instruccion)Recorrido(actual.ChildNodes[0]), (List<Instruccion>)Recorrido(actual.ChildNodes[1]));
                }
                else
                {
                    return new If((Instruccion)Recorrido(actual.ChildNodes[0]), (Instruccion)Recorrido(actual.ChildNodes[1]));
                }
            }
        }
        else if (EstoyAca(actual, "LISTA_ELSE_IF"))
        {
            List<Instruccion> lista_else_if = new List<Instruccion>();
            foreach (ParseTreeNode hijo in actual.ChildNodes)
            {
                lista_else_if.Add((Instruccion)Recorrido(hijo));
            }
            return lista_else_if;
        }
        else if (EstoyAca(actual, "IF"))
        {
            return new Else((Expresion)Recorrido(actual.ChildNodes[2]), (List<Instruccion>)Recorrido(actual.ChildNodes[5]));
        }
        else if (EstoyAca(actual, "ELSE_IF"))
        {
            return new Else((Expresion)Recorrido(actual.ChildNodes[3]), (List<Instruccion>)Recorrido(actual.ChildNodes[6]));
        }
        else if (EstoyAca(actual, "ELSE"))
        {
            return new Else((List<Instruccion>)Recorrido(actual.ChildNodes[2]));
        }
        else if (EstoyAca(actual, "SENTENCIA_SWITCH"))
        {
            return new Switch((Expresion)Recorrido(actual.ChildNodes[2]), (List<Instruccion>)Recorrido(actual.ChildNodes[5]), (List<Instruccion>)Recorrido(actual.ChildNodes[9]), GetFila(actual, 0), GetColumna(actual, 0));
        }
        else if (EstoyAca(actual, "LISTA_CASES"))
        {
            List<Instruccion> lista_cases = new List<Instruccion>();
            foreach (ParseTreeNode hijo in actual.ChildNodes)
            {
                lista_cases.Add((Instruccion)Recorrido(hijo));
            }
            return lista_cases;
        }
        else if (EstoyAca(actual, "CASE"))
        {
            return new Case((Instruccion)Recorrido(actual.ChildNodes[1]), (List<Instruccion>)Recorrido(actual.ChildNodes[4]));
        }
        else if (EstoyAca(actual, "SENTENCIA_INC_DEC"))
        {
            switch (actual.ChildNodes.Count)
            {
                case 2:
                    return new Operacion(new Identificador(ObtenerLexema(actual, 0)), Operacion.GetTipoOperacion(ObtenerLexema(actual, 1)), GetFila(actual, 1), GetColumna(actual, 1));
                default:
                    return new Operacion(new AccesoObjeto(ObtenerLexema(actual, 0), (List<string>)Recorrido(actual.ChildNodes[2])), Operacion.GetTipoOperacion(ObtenerLexema(actual, 1)), GetFila(actual, 1), GetColumna(actual, 1));
            }
        }
        else if (EstoyAca(actual, "LISTA_EXPRESIONES"))
        {
            List<Expresion> lista_expresiones = new List<Expresion>();
            foreach (ParseTreeNode hijo in actual.ChildNodes)
            {
                lista_expresiones.Add((Expresion)Recorrido(hijo));
            }
            return lista_expresiones;
        }
        else if (EstoyAca(actual, "EXPRESION"))
        {
            switch (actual.ChildNodes.Count)
            {
                case 3:
                    return Recorrido(actual.ChildNodes[1]);
                case 2:
                    return new Estructura(Recorrido(actual.ChildNodes[1]));
                default:
                    return Recorrido(actual.ChildNodes[0]);
            }
        }
        else if (EstoyAca(actual, "LISTA_ATR_MAP"))
        {
            List<AtributosMap> lista_atr_map = new List<AtributosMap>();
            foreach (ParseTreeNode hijo in actual.ChildNodes)
            {
                lista_atr_map.Add((AtributosMap)Recorrido(hijo));
            }
            return lista_atr_map;
        }
        else if (EstoyAca(actual, "ATR_MAP"))
        {
            return new AtributosMap((Expresion)Recorrido(actual.ChildNodes[0]), (Expresion)Recorrido(actual.ChildNodes[2]));
        }
        else if (EstoyAca(actual, "EXPRESION_ARITMETICA"))
        {
            if (actual.ChildNodes.Count == 2)
            {
                return new Operacion((Expresion)Recorrido(actual.ChildNodes[1]), Operacion.GetTipoOperacion(ObtenerLexema(actual, 0)), GetFila(actual, 0), GetColumna(actual, 0));
            }
            else
            {
                return new Operacion((Expresion)Recorrido(actual.ChildNodes[0]), (Expresion)Recorrido(actual.ChildNodes[2]), Operacion.GetTipoOperacion(ObtenerLexema(actual, 1)), GetFila(actual, 1), GetColumna(actual, 1));
            }

        }
        else if (EstoyAca(actual, "EXPRESION_RELACIONAL"))
        {
            return new Operacion((Expresion)Recorrido(actual.ChildNodes[0]), (Expresion)Recorrido(actual.ChildNodes[2]), Operacion.GetTipoOperacion(ObtenerLexema(actual, 1)), GetFila(actual, 1), GetColumna(actual, 1));
        }
        else if (EstoyAca(actual, "EXPRESION_LOGICA"))
        {
            if (actual.ChildNodes.Count == 2)
            {
                return new Operacion((Expresion)Recorrido(actual.ChildNodes[1]), Operacion.GetTipoOperacion(ObtenerLexema(actual, 0)), GetFila(actual, 0), GetColumna(actual, 0));
            }
            else
            {
                return new Operacion((Expresion)Recorrido(actual.ChildNodes[0]), (Expresion)Recorrido(actual.ChildNodes[2]), Operacion.GetTipoOperacion(ObtenerLexema(actual, 1)), GetFila(actual, 1), GetColumna(actual, 1));
            }
        }
        else if (EstoyAca(actual, "TIPO"))
        {
            if (actual.ChildNodes.Count > 1)
            {
                if (EstoyAca(actual.ChildNodes[0], "set"))
                {
                    return new Set(Recorrido(actual.ChildNodes[2]));
                }
                else if (EstoyAca(actual.ChildNodes[0], "list"))
                {
                    return new List(Recorrido(actual.ChildNodes[2]));
                }
                else
                {
                    return new Map(Recorrido(actual.ChildNodes[2]), Recorrido(actual.ChildNodes[4]));
                }
            }
            else
            {
                if (EstoyAca(actual.ChildNodes[0], "int"))
                {
                    return Tipo.INT;
                }
                else if (EstoyAca(actual.ChildNodes[0], "double"))
                {
                    return Tipo.DOUBLE;
                }
                else if (EstoyAca(actual.ChildNodes[0], "boolean"))
                {
                    return Tipo.BOOLEAN;
                }
                else if (EstoyAca(actual.ChildNodes[0], "string"))
                {
                    return Tipo.STRING;
                }
                else if (EstoyAca(actual.ChildNodes[0], "date"))
                {
                    return Tipo.DATE;
                }
                else if (EstoyAca(actual.ChildNodes[0], "time"))
                {
                    return Tipo.TIME;
                }
                else if (EstoyAca(actual.ChildNodes[0], "map"))
                {
                    return Tipo.MAP;
                }
                else if (EstoyAca(actual.ChildNodes[0], "set"))
                {
                    return Tipo.SET;
                }
                else if (EstoyAca(actual.ChildNodes[0], "list"))
                {
                    return Tipo.LIST;
                }
                else
                {
                    return Tipo.OBJECT;
                }
            }
            
        }
        else if (EstoyAca(actual, "TIPO_ASIGNACION"))
        {
            if(EstoyAca(actual, "="))
            {
                return TipoAsignacion.AS_NORMAL;
            }
            else if(EstoyAca(actual, "+="))
            {
                return TipoAsignacion.AS_SUMA;
            }
            else if (EstoyAca(actual, "-="))
            {
                return TipoAsignacion.AS_RESTA;
            }
            else if (EstoyAca(actual, "*="))
            {
                return TipoAsignacion.AS_MULTIPLICACION;
            }
            else if (EstoyAca(actual, "/="))
            {
                return TipoAsignacion.AS_DIVISION;
            }
        }
        else if (EstoyAca(actual, "PRIMITIVO"))
        {
            if (EstoyAca(actual.ChildNodes[0], "numero"))
            {
                return new Primitivo(Convert.ToInt32(ObtenerLexema(actual, 0)));
            }
            else if (EstoyAca(actual.ChildNodes[0], "decima"))
            {
                return new Primitivo(Convert.ToDouble(ObtenerLexema(actual, 0)));
            }
            else if (EstoyAca(actual.ChildNodes[0], "cadena"))
            {
                string aux = ObtenerLexema(actual, 0).ToString();
                aux = aux.Replace("\\n", "\n");
                aux = aux.Replace("\\t", "\t");
                aux = aux.Replace("\\r", "\r");
                aux = aux.Substring(1, aux.Length - 2);
                return new Primitivo(aux);
            }
            else if (EstoyAca(actual.ChildNodes[0], "fecha"))
            {
                return new Primitivo(new Date(ObtenerLexema(actual, 0)));
            }
            else if (EstoyAca(actual.ChildNodes[0], "hora"))
            {
                return new Primitivo(new Time(ObtenerLexema(actual, 0)));
            }
            else if (EstoyAca(actual.ChildNodes[0], "true"))
            {
                return new Primitivo(true);
            }
            else if (EstoyAca(actual.ChildNodes[0], "false"))
            {
                return new Primitivo(false);
            }
            else if (EstoyAca(actual.ChildNodes[0], "null"))
            {
                return new Primitivo(new Nulo());
            }
            else if (EstoyAca(actual.ChildNodes[0], "variable"))
            {
                return new Identificador(ObtenerLexema(actual, 0));
            }
        }

        return new Nulo();
    }

    static bool EstoyAca(ParseTreeNode nodo, string nombre)
    {
        return nodo.Term.Name.Equals(nombre, System.StringComparison.InvariantCultureIgnoreCase);
    }

    static string ObtenerLexema(ParseTreeNode nodo, int num)
    {
        return nodo.ChildNodes[num].Token.Text;
    }

    static int GetFila(ParseTreeNode nodo, int num)
    {
        return nodo.ChildNodes[num].Token.Location.Line;
    }

    static int GetColumna(ParseTreeNode nodo, int num)
    {
        return nodo.ChildNodes[num].Token.Location.Column;
    }
}