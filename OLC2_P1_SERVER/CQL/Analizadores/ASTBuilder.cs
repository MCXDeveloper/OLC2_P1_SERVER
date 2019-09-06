using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Irony.Parsing;
using OLC2_P1_SERVER.CQL.Arbol;
using static Asignacion;

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
                    return new Declaracion((TipoDato)Recorrido(actual.ChildNodes[0]), (List<string>)Recorrido(actual.ChildNodes[1]), GetFila(actual, 2), GetColumna(actual, 2));
                default:
                    // TIPO + LISTA_VARIABLES + igual + EXPRESION + puco
                    return new Declaracion((TipoDato)Recorrido(actual.ChildNodes[0]), (List<string>)Recorrido(actual.ChildNodes[1]), (Expresion)Recorrido(actual.ChildNodes[3]), GetFila(actual, 2), GetColumna(actual, 2));
            }
        }
        else if (EstoyAca(actual, "ASIGNACION"))
        {
            switch (actual.ChildNodes.Count)
            {
                case 3:
                    // variable + igual + EXPRESION + puco
                    return new Asignacion(ObtenerLexema(actual, 0), (TipoAsignacion)Recorrido(actual.ChildNodes[1]), (Expresion)Recorrido(actual.ChildNodes[2]), GetFila(actual, 1), GetColumna(actual, 1));
                default:
                    // variable + punto + LISTA_ACCESO + igual + EXPRESION + puco
                    return new Asignacion(new AccesoObjeto(true, ObtenerLexema(actual, 0), (List<Expresion>)Recorrido(actual.ChildNodes[2]), GetFila(actual, 1), GetColumna(actual, 1)), TipoAsignacion.AS_NORMAL, (Expresion)Recorrido(actual.ChildNodes[4]), GetFila(actual, 1), GetColumna(actual, 1));
            }
        }
        else if (EstoyAca(actual, "CREATE_TYPE"))
        {
            switch (actual.ChildNodes.Count)
            {
                case 7:
                    return new CreateUserType(false, ObtenerLexema(actual, 2), (List<AtributoUT>)Recorrido(actual.ChildNodes[4]), GetFila(actual, 0), GetColumna(actual, 0));
                default:
                    return new CreateUserType(true, ObtenerLexema(actual, 2), (List<AtributoUT>)Recorrido(actual.ChildNodes[4]), GetFila(actual, 0), GetColumna(actual, 0));
            }
        }
        else if (EstoyAca(actual, "LISTA_ATR_TYPE"))
        {
            List<AtributoUT> lista_atr_type = new List<AtributoUT>();
            foreach (ParseTreeNode hijo in actual.ChildNodes)
            {
                lista_atr_type.Add((AtributoUT)Recorrido(hijo));
            }
            return lista_atr_type;
        }
        else if (EstoyAca(actual, "ATR_TYPE"))
        {
            return new AtributoUT((TipoDato)Recorrido(actual.ChildNodes[1]), ObtenerLexema(actual, 0));
        }
        else if (EstoyAca(actual, "LISTA_VARIABLES"))
        {
            List<string> lista_variables = new List<string>();
            foreach (ParseTreeNode hijo in actual.ChildNodes)
            {
                lista_variables.Add(hijo.Token.Text);
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
        else if (EstoyAca(actual, "SENTENCIA_DO_WHILE"))
        {
            return new DoWhile((Expresion)Recorrido(actual.ChildNodes[6]), (List<Instruccion>)Recorrido(actual.ChildNodes[2]));
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
                    return new Operacion(new AccesoObjeto(false, ObtenerLexema(actual, 0), (List<Expresion>)Recorrido(actual.ChildNodes[2]), GetFila(actual, 1), GetColumna(actual, 1)), Operacion.GetTipoOperacion(ObtenerLexema(actual, 1)), GetFila(actual, 1), GetColumna(actual, 1));
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
                case 1:
                    return Recorrido(actual.ChildNodes[0]);
                case 3:
                    if (EstoyAca(actual.ChildNodes[1], "LISTA_ATR_MAP"))
                    {
                        return new CollectionValue((List<AtributosMap>)Recorrido(actual.ChildNodes[1]));
                    }
                    else if (EstoyAca(actual.ChildNodes[1], "LISTA_EXPRESIONES"))
                    {
                        return new CollectionValue((List<Expresion>)Recorrido(actual.ChildNodes[1]));
                    }
                    else if (EstoyAca(actual.ChildNodes[1], "EXPRESION"))
                    {
                        return Recorrido(actual.ChildNodes[1]);
                    }
                    else
                    {
                        if(EstoyAca(actual.ChildNodes[0], "today"))
                        {
                            return new Today();
                        }else if (EstoyAca(actual.ChildNodes[0], "now"))
                        {
                            return new Now();
                        }
                        else
                        {
                            return new AccesoObjeto(true, ObtenerLexema(actual, 0), (List<Expresion>)Recorrido(actual.ChildNodes[2]), GetFila(actual, 1), GetColumna(actual, 1));
                        }
                    }
                case 2:
                    return new Estructura((TipoDato)Recorrido(actual.ChildNodes[1]));
                case 4:
                    return new CasteoExplicito((TipoDato)Recorrido(actual.ChildNodes[1]), (Expresion)Recorrido(actual.ChildNodes[3]), GetFila(actual, 0), GetColumna(actual, 0));
                default:
                    return new OperadorTernario((Expresion)Recorrido(actual.ChildNodes[0]), (Expresion)Recorrido(actual.ChildNodes[2]), (Expresion)Recorrido(actual.ChildNodes[4]), GetFila(actual, 1), GetColumna(actual, 1));
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
                    return new TipoDato(TipoDato.Tipo.SET, new SetType((TipoDato)Recorrido(actual.ChildNodes[2])));
                }
                else if (EstoyAca(actual.ChildNodes[0], "list"))
                {
                    return new TipoDato(TipoDato.Tipo.LIST, new ListType((TipoDato)Recorrido(actual.ChildNodes[2])));
                }
                else
                {
                    return new TipoDato(TipoDato.Tipo.MAP, new MapType((TipoDato)Recorrido(actual.ChildNodes[2]), (TipoDato)Recorrido(actual.ChildNodes[4])));
                }
            }
            else
            {
                if (EstoyAca(actual.ChildNodes[0], "int"))
                {
                    return new TipoDato(TipoDato.Tipo.INT);
                }
                else if (EstoyAca(actual.ChildNodes[0], "double"))
                {
                    return new TipoDato(TipoDato.Tipo.DOUBLE);
                }
                else if (EstoyAca(actual.ChildNodes[0], "boolean"))
                {
                    return new TipoDato(TipoDato.Tipo.BOOLEAN);
                }
                else if (EstoyAca(actual.ChildNodes[0], "string"))
                {
                    return new TipoDato(TipoDato.Tipo.STRING);
                }
                else if (EstoyAca(actual.ChildNodes[0], "date"))
                {
                    return new TipoDato(TipoDato.Tipo.DATE);
                }
                else if (EstoyAca(actual.ChildNodes[0], "time"))
                {
                    return new TipoDato(TipoDato.Tipo.TIME);
                }
                else if (EstoyAca(actual.ChildNodes[0], "map"))
                {
                    return new TipoDato(TipoDato.Tipo.MAP);
                }
                else if (EstoyAca(actual.ChildNodes[0], "set"))
                {
                    return new TipoDato(TipoDato.Tipo.SET);
                }
                else if (EstoyAca(actual.ChildNodes[0], "list"))
                {
                    return new TipoDato(TipoDato.Tipo.LIST);
                }
                else if (EstoyAca(actual.ChildNodes[0], "counter"))
                {
                    return new TipoDato(TipoDato.Tipo.COUNTER);
                }
                else
                {
                    return new TipoDato(TipoDato.Tipo.OBJECT, ObtenerLexema(actual, 0));
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
        else if (EstoyAca(actual, "LISTA_ACCESO"))
        {
            List<Expresion> lista_acceso = new List<Expresion>();
            foreach (ParseTreeNode hijo in actual.ChildNodes)
            {
                lista_acceso.Add((Expresion)Recorrido(hijo));
            }
            return lista_acceso;
        }
        else if (EstoyAca(actual, "ACCESO"))
        {
            if(EstoyAca(actual.ChildNodes[0], "insert"))
            {
                if(actual.ChildNodes.Count == 4)
                {
                    return new FuncionInsert((Expresion)Recorrido(actual.ChildNodes[2]));
                }
                else
                {
                    return new FuncionInsert((Expresion)Recorrido(actual.ChildNodes[2]), (Expresion)Recorrido(actual.ChildNodes[4]));
                }
            }
            else if (EstoyAca(actual.ChildNodes[0], "set"))
            {
                return new FuncionSet((Expresion)Recorrido(actual.ChildNodes[2]), (Expresion)Recorrido(actual.ChildNodes[4]));
            }
            else if (EstoyAca(actual.ChildNodes[0], "substring"))
            {
                return new FuncionSubstring((Expresion)Recorrido(actual.ChildNodes[2]), (Expresion)Recorrido(actual.ChildNodes[4]));
            }
            else if (EstoyAca(actual.ChildNodes[0], "remove"))
            {
                return new FuncionRemove((Expresion)Recorrido(actual.ChildNodes[2]));
            }
            else if (EstoyAca(actual.ChildNodes[0], "contains"))
            {
                return new FuncionContains((Expresion)Recorrido(actual.ChildNodes[2]));
            }
            else if (EstoyAca(actual.ChildNodes[0], "startswith"))
            {
                return new FuncionStartsWith((Expresion)Recorrido(actual.ChildNodes[2]));
            }
            else if (EstoyAca(actual.ChildNodes[0], "endswith"))
            {
                return new FuncionEndsWith((Expresion)Recorrido(actual.ChildNodes[2]));
            }
            else if (EstoyAca(actual.ChildNodes[0], "get"))
            {
                return new FuncionGet((Expresion)Recorrido(actual.ChildNodes[2]));
            }
            else if (EstoyAca(actual.ChildNodes[0], "size"))
            {
                return new FuncionSize();
            }
            else if (EstoyAca(actual.ChildNodes[0], "clear"))
            {
                return new FuncionClear();
            }
            else if (EstoyAca(actual.ChildNodes[0], "length"))
            {
                return new FuncionLength();
            }
            else if (EstoyAca(actual.ChildNodes[0], "touppercase"))
            {
                return new FuncionToUpperCase();
            }
            else if (EstoyAca(actual.ChildNodes[0], "tolowercase"))
            {
                return new FuncionToLowerCase();
            }
            else if (EstoyAca(actual.ChildNodes[0], "getyear"))
            {
                return new FuncionGetYear();
            }
            else if (EstoyAca(actual.ChildNodes[0], "getmonth"))
            {
                return new FuncionGetMonth();
            }
            else if (EstoyAca(actual.ChildNodes[0], "getday"))
            {
                return new FuncionGetDay();
            }
            else if (EstoyAca(actual.ChildNodes[0], "gethour"))
            {
                return new FuncionGetHour();
            }
            else if (EstoyAca(actual.ChildNodes[0], "getminutes"))
            {
                return new FuncionGetMinutes();
            }
            else if (EstoyAca(actual.ChildNodes[0], "getseconds"))
            {
                return new FuncionGetSeconds();
            }
            else
            {
                return new Atributo(ObtenerLexema(actual, 0));
            }
        }
        else if (EstoyAca(actual, "DECLARACION_FUNCION"))
        {
            return new DeclaracionFuncion((TipoDato)Recorrido(actual.ChildNodes[0]), ObtenerLexema(actual, 1), (List<Parametro>)Recorrido(actual.ChildNodes[3]), (List<Instruccion>)Recorrido(actual.ChildNodes[6]), GetFila(actual, 2), GetColumna(actual, 2));
        }
        else if (EstoyAca(actual, "LISTA_PARAMETROS"))
        {
            List<Parametro> lista_params = new List<Parametro>();
            foreach (ParseTreeNode hijo in actual.ChildNodes)
            {
                lista_params.Add((Parametro)Recorrido(hijo));
            }
            return lista_params;
        }
        else if (EstoyAca(actual, "PARAMETRO"))
        {
            return new Parametro((TipoDato)Recorrido(actual.ChildNodes[0]), ObtenerLexema(actual, 1));
        }
        else if (EstoyAca(actual, "LLAMADA_FUNCION"))
        {
            return new LlamadaFuncion(ObtenerLexema(actual, 0), (List<Expresion>)Recorrido(actual.ChildNodes[2]), GetFila(actual, 1), GetColumna(actual, 1));
        }
        else if (EstoyAca(actual, "DECLARACION_PROCEDIMIENTO"))
        {
            return new DeclaracionProcedimiento(ObtenerLexema(actual, 1), (List<Parametro>)Recorrido(actual.ChildNodes[3]), (List<Parametro>)Recorrido(actual.ChildNodes[7]), (List<Instruccion>)Recorrido(actual.ChildNodes[10]));
        }
        else if (EstoyAca(actual, "LLAMADA_PROCEDIMIENTO"))
        {
            return new LlamadaProcedimiento(ObtenerLexema(actual, 1), (List<Expresion>)Recorrido(actual.ChildNodes[3]), GetFila(actual, 0), GetColumna(actual, 0));
        }
        else if (EstoyAca(actual, "SENTENCIA_DB_CREATE"))
        {
            return actual.ChildNodes.Count == 4 ? new CreateDatabase(false, ObtenerLexema(actual, 2), GetFila(actual, 0), GetColumna(actual, 0)) : new CreateDatabase(true, ObtenerLexema(actual, 5), GetFila(actual, 0), GetColumna(actual, 0));
        }
        else if (EstoyAca(actual, "SENTENCIA_DB_USE"))
        {
            return new UseDatabase(ObtenerLexema(actual, 1), GetFila(actual, 0), GetColumna(actual, 0));
        }
        else if (EstoyAca(actual, "SENTENCIA_DB_DROP"))
        {
            return new DropDatabase(ObtenerLexema(actual, 2), GetFila(actual, 0), GetColumna(actual, 0));
        }
        else if (EstoyAca(actual, "SENTENCIA_TB_CREATE"))
        {
            switch (actual.ChildNodes.Count)
            {
                case 7:
                    return new CreateTable(false, ObtenerLexema(actual, 2), (List<Columna>)Recorrido(actual.ChildNodes[4]), GetFila(actual, 0), GetColumna(actual, 0));
                case 10:
                    return new CreateTable(true, ObtenerLexema(actual, 2), (List<Columna>)Recorrido(actual.ChildNodes[4]), GetFila(actual, 0), GetColumna(actual, 0));
                case 12:
                    return new CreateTable(false, ObtenerLexema(actual, 2), (List<Columna>)Recorrido(actual.ChildNodes[4]), (List<string>)Recorrido(actual.ChildNodes[10]), GetFila(actual, 0), GetColumna(actual, 0));
                default:
                    return new CreateTable(true, ObtenerLexema(actual, 2), (List<Columna>)Recorrido(actual.ChildNodes[4]), (List<string>)Recorrido(actual.ChildNodes[10]), GetFila(actual, 0), GetColumna(actual, 0));
            }
        }
        else if (EstoyAca(actual, "LISTA_COLUMNAS"))
        {
            List<Columna> lista_columnas = new List<Columna>();
            foreach (ParseTreeNode hijo in actual.ChildNodes)
            {
                lista_columnas.Add((Columna)Recorrido(hijo));
            }
            return lista_columnas;
        }
        else if (EstoyAca(actual, "COLUMNA"))
        {
            switch (actual.ChildNodes.Count)
            {
                case 2:
                    return new Columna(false, ObtenerLexema(actual, 0), (TipoDato)Recorrido(actual.ChildNodes[1]));
                default:
                    return new Columna(true, ObtenerLexema(actual, 0), (TipoDato)Recorrido(actual.ChildNodes[1]));
            }
        }
        else if (EstoyAca(actual, "LISTA_IDENTIFICADORES"))
        {
            List<string> lista_ids = new List<string>();
            foreach (ParseTreeNode hijo in actual.ChildNodes)
            {
                lista_ids.Add((string)Recorrido(hijo));
            }
            return lista_ids;
        }
        else if (EstoyAca(actual, "SENTENCIA_TB_ALTER"))
        {
            if(EstoyAca(actual.ChildNodes[3], "add"))
            {
                return new AlterTable(ObtenerLexema(actual, 2), (List<Columna>)Recorrido(actual.ChildNodes[4]), GetFila(actual, 0), GetColumna(actual, 0));
            }
            else
            {
                return new AlterTable(ObtenerLexema(actual, 2), (List<string>)Recorrido(actual.ChildNodes[4]), GetFila(actual, 0), GetColumna(actual, 0));
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
        return nodo.ChildNodes[num].Token.Text.ToLower();
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