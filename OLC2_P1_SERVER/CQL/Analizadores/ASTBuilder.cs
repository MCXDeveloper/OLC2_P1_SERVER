﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Irony.Parsing;
using static Asignacion;

public class ASTBuilder
{
    public bool AccesoObjetoFlag { get; set; }

    public ASTBuilder()
    {
        AccesoObjetoFlag = false;
    }

    public AST Analizar(ParseTreeNode raiz)
    {
        return (AST)Recorrido(raiz);
    }

    public object Recorrido(ParseTreeNode actual)
    {
        if (EstoyAca(actual, "INICIO"))
        {
            return new AST((List<Instruccion>)Recorrido(actual.ChildNodes[0]));
        }
        else if (EstoyAca(actual, "LISTA_INSTRUCCIONES"))
        {
            List<Instruccion> instrucciones = new List<Instruccion>();
            foreach (ParseTreeNode hijo in actual.ChildNodes)
            {
                instrucciones.Add((Instruccion)Recorrido(hijo));
            }
            return instrucciones;
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
                case 4:
                    // variable + igual + EXPRESION + puco
                    return new Asignacion(ObtenerLexema(actual, 0), (TipoAsignacion)Recorrido(actual.ChildNodes[1]), (Expresion)Recorrido(actual.ChildNodes[2]), GetFila(actual, 0), GetColumna(actual, 0));
                default:
                    // variable + punto + LISTA_ACCESO + igual + EXPRESION + puco
                    return new Asignacion(new AccesoObjeto(false, ObtenerLexema(actual, 0), (List<Expresion>)Recorrido(actual.ChildNodes[2]), GetFila(actual, 1), GetColumna(actual, 1)), TipoAsignacion.AS_NORMAL, (Expresion)Recorrido(actual.ChildNodes[4]), GetFila(actual, 1), GetColumna(actual, 1));
            }
        }
        else if (EstoyAca(actual, "CREATE_TYPE"))
        {
            switch (actual.ChildNodes.Count)
            {
                case 6:
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
            List<Expresion> lista_acceso = new List<Expresion>();
            foreach (ParseTreeNode hijo in actual.ChildNodes)
            {
                lista_acceso.Add((Expresion)Recorrido(hijo));
            }
            return lista_acceso;
        }
        else if (EstoyAca(actual, "LOG"))
        {
            return new Log((Expresion)Recorrido(actual.ChildNodes[2]), GetFila(actual, 1), GetColumna(actual, 1));
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
        else if (EstoyAca(actual, "SENTENCIA_WHILE"))
        {
            return new While((Expresion)Recorrido(actual.ChildNodes[2]), (List<Instruccion>)Recorrido(actual.ChildNodes[5]));
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
            return new Case((Expresion)Recorrido(actual.ChildNodes[1]), (List<Instruccion>)Recorrido(actual.ChildNodes[4]));
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
        else if (EstoyAca(actual, "SENTENCIA_ACCESO"))
        {
            AccesoObjeto ao = new AccesoObjeto(AccesoObjetoFlag, ObtenerLexema(actual, 0), (List<Expresion>)Recorrido(actual.ChildNodes[2]), GetFila(actual, 1), GetColumna(actual, 1));
            AccesoObjetoFlag = false;
            return ao;
        }
        else if (EstoyAca(actual, "EXPRESION"))
        {
            switch (actual.ChildNodes.Count)
            {
                case 1:

                    if (EstoyAca(actual.ChildNodes[0], "identificador"))
                    {
                        return new ColumnaTabla(ObtenerLexema(actual, 0), GetFila(actual, 0), GetColumna(actual, 0));
                    }
                    else if (EstoyAca(actual.ChildNodes[0], "null"))
                    {
                        return new Nulo();
                    }
                    else
                    {
                        if (EstoyAca(actual.ChildNodes[0], "SENTENCIA_ACCESO"))
                        {
                            AccesoObjetoFlag = true;
                        }

                        return Recorrido(actual.ChildNodes[0]);
                    }

                case 3:

                    if (EstoyAca(actual.ChildNodes[1], "LISTA_ATR_MAP"))
                    {
                        return new CollectionValue((List<AtributosMap>)Recorrido(actual.ChildNodes[1]), GetFila(actual, 0), GetColumna(actual, 0));
                    }
                    else if (EstoyAca(actual.ChildNodes[1], "LISTA_EXPRESIONES"))
                    {
                        return new CollectionValue((List<Expresion>)Recorrido(actual.ChildNodes[1]), GetFila(actual, 0), GetColumna(actual, 0));
                    }
                    else if (EstoyAca(actual.ChildNodes[1], "EXPRESION"))
                    {
                        return Recorrido(actual.ChildNodes[1]);
                    }
                    else
                    {
                        if (EstoyAca(actual.ChildNodes[0], "today"))
                        {
                            return new Today();
                        }
                        else if (EstoyAca(actual.ChildNodes[0], "now"))
                        {
                            return new Now();
                        }
                        else if (EstoyAca(actual.ChildNodes[0], "identificador"))
                        {
                            if (EstoyAca(actual.ChildNodes[1], "."))
                            {
                                return new AccesoColumna(ObtenerLexema(actual, 0), (List<Expresion>)Recorrido(actual.ChildNodes[2]), GetFila(actual, 0), GetColumna(actual, 0));
                            }
                            else
                            {
                                return new AccesoCollection(ObtenerLexema(actual, 0), (Expresion)Recorrido(actual.ChildNodes[2]), GetFila(actual, 0), GetColumna(actual, 0));
                            }
                        }
                    }

                    break;

                case 2:
                    return new Estructura((TipoDato)Recorrido(actual.ChildNodes[1]));
                case 4:

                    if (EstoyAca(actual.ChildNodes[0], "identificador"))
                    {
                        return new AccesoCollection(ObtenerLexema(actual, 0), (Expresion)Recorrido(actual.ChildNodes[2]), GetFila(actual, 0), GetColumna(actual, 0));
                    }
                    else
                    {
                        return new CasteoExplicito((TipoDato)Recorrido(actual.ChildNodes[1]), (Expresion)Recorrido(actual.ChildNodes[3]), GetFila(actual, 0), GetColumna(actual, 0));
                    }

                default:

                    if (EstoyAca(actual.ChildNodes[1], "?"))
                    {
                        return new OperadorTernario((Expresion)Recorrido(actual.ChildNodes[0]), (Expresion)Recorrido(actual.ChildNodes[2]), (Expresion)Recorrido(actual.ChildNodes[4]), GetFila(actual, 1), GetColumna(actual, 1));
                    }
                    else
                    {
                        return new ObjectValue((List<Expresion>)Recorrido(actual.ChildNodes[1]), ObtenerLexema(actual, 4), GetFila(actual, 0), GetColumna(actual, 0));
                    }
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
                return new Operacion((Expresion)Recorrido(actual.ChildNodes[1]), Operacion.TipoOperacion.NEGATIVO, GetFila(actual, 0), GetColumna(actual, 0));
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
            if (EstoyAca(actual.ChildNodes[0], "="))
            {
                return TipoAsignacion.AS_NORMAL;
            }
            else if (EstoyAca(actual.ChildNodes[0], "+="))
            {
                return TipoAsignacion.AS_SUMA;
            }
            else if (EstoyAca(actual.ChildNodes[0], "-="))
            {
                return TipoAsignacion.AS_RESTA;
            }
            else if (EstoyAca(actual.ChildNodes[0], "*="))
            {
                return TipoAsignacion.AS_MULTIPLICACION;
            }
            else if (EstoyAca(actual.ChildNodes[0], "/="))
            {
                return TipoAsignacion.AS_DIVISION;
            }
        }
        else if (EstoyAca(actual, "PRIMITIVO"))
        {
            if (EstoyAca(actual.ChildNodes[0], "numero"))
            {
                double result = Convert.ToDouble(ObtenerLexema(actual, 0));
                try
                {
                    int result2 = Convert.ToInt32(ObtenerLexema(actual, 0));
                    return new Primitivo(result2);
                }
                catch (Exception)
                {
                    return new Primitivo(result);
                }
            }
            else if (EstoyAca(actual.ChildNodes[0], "cadena"))
            {
                string aux = ObtenerLexema(actual, 0).ToString();
                aux = aux.Replace("\\n", "\n");
                aux = aux.Replace("\\t", "\t");
                aux = aux.Replace("\\r", "\r");
                aux = aux.Replace("\"", "\"");
                aux = aux.Substring(1, aux.Length - 2);
                return new Primitivo(aux);
            }
            else if (EstoyAca(actual.ChildNodes[0], "fecha"))
            {
                return new Primitivo(new Date(ObtenerLexema(actual, 0).Replace("'", "")));
            }
            else if (EstoyAca(actual.ChildNodes[0], "hora"))
            {
                return new Primitivo(new Time(ObtenerLexema(actual, 0).Replace("'", "")));
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
            if (EstoyAca(actual.ChildNodes[0], "insert"))
            {
                if (actual.ChildNodes.Count == 4)
                {
                    return new FuncionInsert((Expresion)Recorrido(actual.ChildNodes[2]), GetFila(actual, 0), GetColumna(actual, 0));
                }
                else
                {
                    return new FuncionInsert((Expresion)Recorrido(actual.ChildNodes[2]), (Expresion)Recorrido(actual.ChildNodes[4]), GetFila(actual, 0), GetColumna(actual, 0));
                }
            }
            else if (EstoyAca(actual.ChildNodes[0], "set"))
            {
                return new FuncionSet((Expresion)Recorrido(actual.ChildNodes[2]), (Expresion)Recorrido(actual.ChildNodes[4]), GetFila(actual, 0), GetColumna(actual, 0));
            }
            else if (EstoyAca(actual.ChildNodes[0], "substring"))
            {
                return new FuncionSubstring((Expresion)Recorrido(actual.ChildNodes[2]), (Expresion)Recorrido(actual.ChildNodes[4]), GetFila(actual, 0), GetColumna(actual, 0));
            }
            else if (EstoyAca(actual.ChildNodes[0], "remove"))
            {
                return new FuncionRemove((Expresion)Recorrido(actual.ChildNodes[2]), GetFila(actual, 0), GetColumna(actual, 0));
            }
            else if (EstoyAca(actual.ChildNodes[0], "contains"))
            {
                return new FuncionContains((Expresion)Recorrido(actual.ChildNodes[2]), GetFila(actual, 0), GetColumna(actual, 0));
            }
            else if (EstoyAca(actual.ChildNodes[0], "startswith"))
            {
                return new FuncionStartsWith((Expresion)Recorrido(actual.ChildNodes[2]), GetFila(actual, 0), GetColumna(actual, 0));
            }
            else if (EstoyAca(actual.ChildNodes[0], "endswith"))
            {
                return new FuncionEndsWith((Expresion)Recorrido(actual.ChildNodes[2]), GetFila(actual, 0), GetColumna(actual, 0));
            }
            else if (EstoyAca(actual.ChildNodes[0], "get"))
            {
                return new FuncionGet((Expresion)Recorrido(actual.ChildNodes[2]), GetFila(actual, 0), GetColumna(actual, 0));
            }
            else if (EstoyAca(actual.ChildNodes[0], "size"))
            {
                return new FuncionSize(GetFila(actual, 0), GetColumna(actual, 0));
            }
            else if (EstoyAca(actual.ChildNodes[0], "clear"))
            {
                return new FuncionClear(GetFila(actual, 0), GetColumna(actual, 0));
            }
            else if (EstoyAca(actual.ChildNodes[0], "length"))
            {
                return new FuncionLength(GetFila(actual, 0), GetColumna(actual, 0));
            }
            else if (EstoyAca(actual.ChildNodes[0], "touppercase"))
            {
                return new FuncionToUpperCase(GetFila(actual, 0), GetColumna(actual, 0));
            }
            else if (EstoyAca(actual.ChildNodes[0], "tolowercase"))
            {
                return new FuncionToLowerCase(GetFila(actual, 0), GetColumna(actual, 0));
            }
            else if (EstoyAca(actual.ChildNodes[0], "getyear"))
            {
                return new FuncionGetYear(GetFila(actual, 0), GetColumna(actual, 0));
            }
            else if (EstoyAca(actual.ChildNodes[0], "getmonth"))
            {
                return new FuncionGetMonth(GetFila(actual, 0), GetColumna(actual, 0));
            }
            else if (EstoyAca(actual.ChildNodes[0], "getday"))
            {
                return new FuncionGetDay(GetFila(actual, 0), GetColumna(actual, 0));
            }
            else if (EstoyAca(actual.ChildNodes[0], "gethour"))
            {
                return new FuncionGetHour(GetFila(actual, 0), GetColumna(actual, 0));
            }
            else if (EstoyAca(actual.ChildNodes[0], "getminuts"))
            {
                return new FuncionGetMinutes(GetFila(actual, 0), GetColumna(actual, 0));
            }
            else if (EstoyAca(actual.ChildNodes[0], "getseconds"))
            {
                return new FuncionGetSeconds(GetFila(actual, 0), GetColumna(actual, 0));
            }
            else
            {
                return new Atributo(ObtenerLexema(actual, 0), GetFila(actual, 0), GetColumna(actual, 0));
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
                case 6:
                    return new CreateTable(false, ObtenerLexema(actual, 2), (List<Columna>)Recorrido(actual.ChildNodes[4]), GetFila(actual, 0), GetColumna(actual, 0));
                case 9:
                    return new CreateTable(true, ObtenerLexema(actual, 5), (List<Columna>)Recorrido(actual.ChildNodes[7]), GetFila(actual, 0), GetColumna(actual, 0));
                case 11:
                    return new CreateTable(false, ObtenerLexema(actual, 2), (List<Columna>)Recorrido(actual.ChildNodes[4]), (List<string>)Recorrido(actual.ChildNodes[9]), GetFila(actual, 0), GetColumna(actual, 0));
                default:
                    return new CreateTable(true, ObtenerLexema(actual, 5), (List<Columna>)Recorrido(actual.ChildNodes[7]), (List<string>)Recorrido(actual.ChildNodes[12]), GetFila(actual, 0), GetColumna(actual, 0));
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
                lista_ids.Add(hijo.Token.Text);
            }
            return lista_ids;
        }
        else if (EstoyAca(actual, "SENTENCIA_CREATE_USER"))
        {
            return new CreateUser(ObtenerLexema(actual, 2), ObtenerLexema(actual, 5).Replace("\"", ""), GetFila(actual, 0), GetColumna(actual, 0));
        }
        else if (EstoyAca(actual, "SENTENCIA_GRANT"))
        {
            return new Grant(ObtenerLexema(actual, 1), ObtenerLexema(actual, 3), GetFila(actual, 0), GetColumna(actual, 0));
        }
        else if (EstoyAca(actual, "SENTENCIA_REVOKE"))
        {
            return new Revoke(ObtenerLexema(actual, 1), ObtenerLexema(actual, 3), GetFila(actual, 0), GetColumna(actual, 0));
        }
        else if (EstoyAca(actual, "SENTENCIA_TB_ALTER"))
        {
            if (EstoyAca(actual.ChildNodes[3], "add"))
            {
                return new AlterTable(ObtenerLexema(actual, 2), (List<Columna>)Recorrido(actual.ChildNodes[4]), GetFila(actual, 0), GetColumna(actual, 0));
            }
            else
            {
                return new AlterTable(ObtenerLexema(actual, 2), (List<string>)Recorrido(actual.ChildNodes[4]), GetFila(actual, 0), GetColumna(actual, 0));
            }
        }
        else if (EstoyAca(actual, "SENTENCIA_TB_DROP"))
        {
            switch (actual.ChildNodes.Count)
            {
                case 3:
                    return new DropTable(false, ObtenerLexema(actual, 2), GetFila(actual, 0), GetColumna(actual, 0));
                default:
                    return new DropTable(true, ObtenerLexema(actual, 4), GetFila(actual, 0), GetColumna(actual, 0));
            }
        }
        else if (EstoyAca(actual, "SENTENCIA_TB_TRUNCATE"))
        {
            return new TruncateTable(ObtenerLexema(actual, 2), GetFila(actual, 0), GetColumna(actual, 0));
        }
        else if (EstoyAca(actual, "SENTENCIA_TB_INSERT"))
        {
            switch (actual.ChildNodes.Count)
            {
                case 7:
                    return new InsertTable(ObtenerLexema(actual, 2), (List<Expresion>)Recorrido(actual.ChildNodes[5]), GetFila(actual, 0), GetColumna(actual, 0));
                default:
                    return new InsertTable(ObtenerLexema(actual, 2), (List<string>)Recorrido(actual.ChildNodes[4]), (List<Expresion>)Recorrido(actual.ChildNodes[8]), GetFila(actual, 0), GetColumna(actual, 0));
            }
        }
        else if (EstoyAca(actual, "SENTENCIA_TB_SELECT"))
        {
            switch (actual.ChildNodes.Count)
            {
                //r_select + por + r_from + identificador
                //| r_select + LISTA_EXPRESIONES + r_from + identificador
                case 4:

                    if (EstoyAca(actual.ChildNodes[1], "LISTA_EXPRESIONES"))
                    {
                        return new Select((List<Expresion>)Recorrido(actual.ChildNodes[1]), ObtenerLexema(actual, 3), null, null, null, GetFila(actual, 0), GetColumna(actual, 0));
                    }
                    else
                    {
                        return new Select(null, ObtenerLexema(actual, 3), null, null, null, GetFila(actual, 0), GetColumna(actual, 0));
                    }

                //| r_select + por + r_from + identificador + r_where + EXPRESION
                //| r_select + por + r_from + identificador + r_limit + EXPRESION
                //| r_select + LISTA_EXPRESIONES + r_from + identificador + r_where + EXPRESION
                //| r_select + LISTA_EXPRESIONES + r_from + identificador + r_limit + EXPRESION
                case 6:

                    if (EstoyAca(actual.ChildNodes[4], "where"))
                    {
                        if (EstoyAca(actual.ChildNodes[1], "LISTA_EXPRESIONES"))
                        {
                            return new Select((List<Expresion>)Recorrido(actual.ChildNodes[1]), ObtenerLexema(actual, 3), (Expresion)Recorrido(actual.ChildNodes[5]), null, null, GetFila(actual, 0), GetColumna(actual, 0));
                        }
                        else
                        {
                            return new Select(null, ObtenerLexema(actual, 3), (Expresion)Recorrido(actual.ChildNodes[5]), null, null, GetFila(actual, 0), GetColumna(actual, 0));
                        }
                    }
                    else
                    {
                        if (EstoyAca(actual.ChildNodes[1], "LISTA_EXPRESIONES"))
                        {
                            return new Select((List<Expresion>)Recorrido(actual.ChildNodes[1]), ObtenerLexema(actual, 3), null, (Expresion)Recorrido(actual.ChildNodes[5]), null, GetFila(actual, 0), GetColumna(actual, 0));
                        }
                        else
                        {
                            return new Select(null, ObtenerLexema(actual, 3), null, (Expresion)Recorrido(actual.ChildNodes[5]), null, GetFila(actual, 0), GetColumna(actual, 0));
                        }
                    }

                //| r_select + por + r_from + identificador + r_order + r_by + LISTA_ORDER
                //| r_select + LISTA_EXPRESIONES + r_from + identificador + r_order + r_by + LISTA_ORDER
                case 7:

                    if (EstoyAca(actual.ChildNodes[1], "LISTA_EXPRESIONES"))
                    {
                        return new Select((List<Expresion>)Recorrido(actual.ChildNodes[1]), ObtenerLexema(actual, 3), null, null, (List<Order>)Recorrido(actual.ChildNodes[6]), GetFila(actual, 0), GetColumna(actual, 0));
                    }
                    else
                    {
                        return new Select(null, ObtenerLexema(actual, 3), null, null, (List<Order>)Recorrido(actual.ChildNodes[6]), GetFila(actual, 0), GetColumna(actual, 0));
                    }

                // | r_select + por + r_from + identificador + r_where + EXPRESION + r_limit + EXPRESION
                // | r_select + LISTA_EXPRESIONES + r_from + identificador + r_where + EXPRESION + r_limit + EXPRESION
                case 8:

                    if (EstoyAca(actual.ChildNodes[1], "LISTA_EXPRESIONES"))
                    {
                        return new Select((List<Expresion>)Recorrido(actual.ChildNodes[1]), ObtenerLexema(actual, 3), (Expresion)Recorrido(actual.ChildNodes[5]), (Expresion)Recorrido(actual.ChildNodes[7]), null, GetFila(actual, 0), GetColumna(actual, 0));
                    }
                    else
                    {
                        return new Select(null, ObtenerLexema(actual, 3), (Expresion)Recorrido(actual.ChildNodes[5]), (Expresion)Recorrido(actual.ChildNodes[7]), null, GetFila(actual, 0), GetColumna(actual, 0));
                    }

                // | r_select + por + r_from + identificador + r_where + EXPRESION + r_order + r_by + LISTA_ORDER
                // | r_select + por + r_from + identificador + r_order + r_by + LISTA_ORDER + r_limit + EXPRESION
                // | r_select + LISTA_EXPRESIONES + r_from + identificador + r_where + EXPRESION + r_order + r_by + LISTA_ORDER
                // | r_select + LISTA_EXPRESIONES + r_from + identificador + r_order + r_by + LISTA_ORDER + r_limit + EXPRESION
                case 9:

                    if (EstoyAca(actual.ChildNodes[4], "where"))
                    {
                        if (EstoyAca(actual.ChildNodes[1], "LISTA_EXPRESIONES"))
                        {
                            return new Select((List<Expresion>)Recorrido(actual.ChildNodes[1]), ObtenerLexema(actual, 3), (Expresion)Recorrido(actual.ChildNodes[5]), null, (List<Order>)Recorrido(actual.ChildNodes[8]), GetFila(actual, 0), GetColumna(actual, 0));
                        }
                        else
                        {
                            return new Select(null, ObtenerLexema(actual, 3), (Expresion)Recorrido(actual.ChildNodes[5]), null, (List<Order>)Recorrido(actual.ChildNodes[8]), GetFila(actual, 0), GetColumna(actual, 0));
                        }
                    }
                    else
                    {
                        if (EstoyAca(actual.ChildNodes[1], "LISTA_EXPRESIONES"))
                        {
                            return new Select((List<Expresion>)Recorrido(actual.ChildNodes[1]), ObtenerLexema(actual, 3), null, (Expresion)Recorrido(actual.ChildNodes[8]), (List<Order>)Recorrido(actual.ChildNodes[6]), GetFila(actual, 0), GetColumna(actual, 0));
                        }
                        else
                        {
                            return new Select(null, ObtenerLexema(actual, 3), null, (Expresion)Recorrido(actual.ChildNodes[8]), (List<Order>)Recorrido(actual.ChildNodes[6]), GetFila(actual, 0), GetColumna(actual, 0));
                        }
                    }

                // | r_select + por + r_from + identificador + r_where + EXPRESION + r_order + r_by + LISTA_ORDER + r_limit + EXPRESION
                // | r_select + LISTA_EXPRESIONES + r_from + identificador + r_where + EXPRESION + r_order + r_by + LISTA_ORDER + r_limit + EXPRESION
                default:
                    if (EstoyAca(actual.ChildNodes[1], "LISTA_EXPRESIONES"))
                    {
                        return new Select((List<Expresion>)Recorrido(actual.ChildNodes[1]), ObtenerLexema(actual, 3), (Expresion)Recorrido(actual.ChildNodes[5]), (Expresion)Recorrido(actual.ChildNodes[10]), (List<Order>)Recorrido(actual.ChildNodes[8]), GetFila(actual, 0), GetColumna(actual, 0));
                    }
                    else
                    {
                        return new Select(null, ObtenerLexema(actual, 3), (Expresion)Recorrido(actual.ChildNodes[5]), (Expresion)Recorrido(actual.ChildNodes[10]), (List<Order>)Recorrido(actual.ChildNodes[8]), GetFila(actual, 0), GetColumna(actual, 0));
                    }
            }
        }
        else if (EstoyAca(actual, "SENTENCIA_TB_DELETE"))
        {
            switch (actual.ChildNodes.Count)
            {
                case 3:
                    return new DeleteTable(ObtenerLexema(actual, 2), GetFila(actual, 0), GetColumna(actual, 0));
                default:
                    return new DeleteTable(ObtenerLexema(actual, 2), (Expresion)Recorrido(actual.ChildNodes[4]), GetFila(actual, 0), GetColumna(actual, 0));
            }
        }
        else if (EstoyAca(actual, "LISTA_ORDER"))
        {
            List<Order> lista_columnas = new List<Order>();
            foreach (ParseTreeNode hijo in actual.ChildNodes)
            {
                lista_columnas.Add((Order)Recorrido(hijo));
            }
            return lista_columnas;
        }
        else if (EstoyAca(actual, "ORDER"))
        {
            switch (actual.ChildNodes.Count)
            {
                case 2:
                    return new Order(ObtenerLexema(actual, 0), ObtenerLexema(actual, 1));
                default:
                    return new Order(ObtenerLexema(actual, 0), "");
            }
        }
        else if (EstoyAca(actual, "SENTENCIA_BREAK"))
        {
            return new Break();
        }
        else if (EstoyAca(actual, "SENTENCIA_RETURN"))
        {
            return new Return((Expresion)Recorrido(actual.ChildNodes[1]));
        }
        else if (EstoyAca(actual, "SENTENCIA_CONTINUE"))
        {
            return new Continue();
        }
        else if (EstoyAca(actual, "SENTENCIA_FOR"))
        {
            return new For((Instruccion)Recorrido(actual.ChildNodes[2]), (Expresion)Recorrido(actual.ChildNodes[4]), (Instruccion)Recorrido(actual.ChildNodes[6]), (List<Instruccion>)Recorrido(actual.ChildNodes[9]));
        }
        else if (EstoyAca(actual, "FUNCION_AGREGACION"))
        {
            return new FuncionAgregacion((TipoFuncionAgregacion)Recorrido(actual.ChildNodes[0]), (Select)Recorrido(actual.ChildNodes[4]), GetFila(actual, 1), GetColumna(actual, 1));
        }
        else if (EstoyAca(actual, "TIPO_FUN_AGG"))
        {
            if (EstoyAca(actual.ChildNodes[0], "count"))
            {
                return TipoFuncionAgregacion.COUNT;
            }
            else if (EstoyAca(actual.ChildNodes[0], "min"))
            {
                return TipoFuncionAgregacion.MIN;
            }
            else if (EstoyAca(actual.ChildNodes[0], "max"))
            {
                return TipoFuncionAgregacion.MAX;
            }
            else if (EstoyAca(actual.ChildNodes[0], "sum"))
            {
                return TipoFuncionAgregacion.SUM;
            }
            else if (EstoyAca(actual.ChildNodes[0], "avg"))
            {
                return TipoFuncionAgregacion.AVG;
            }
        }
        else if (EstoyAca(actual, "SENTENCIA_TRY_CATCH"))
        {
            return new TryCatch((List<Instruccion>)Recorrido(actual.ChildNodes[2]), (TipoExcepcion)Recorrido(actual.ChildNodes[6]), ObtenerLexema(actual, 7), (List<Instruccion>)Recorrido(actual.ChildNodes[10]), GetFila(actual, 0), GetColumna(actual, 0));
        }
        else if (EstoyAca(actual, "TIPO_EXCEPCION"))
        {
            if (EstoyAca(actual.ChildNodes[0], "bddontexists"))
            {
                return TipoExcepcion.BD_DONT_EXISTS;
            }
            else if (EstoyAca(actual.ChildNodes[0], "typedontexists"))
            {
                return TipoExcepcion.TYPE_DONT_EXISTS;
            }
            else if (EstoyAca(actual.ChildNodes[0], "usebdexception"))
            {
                return TipoExcepcion.USE_DB_EXCEPTION;
            }
            else if (EstoyAca(actual.ChildNodes[0], "userdontexists"))
            {
                return TipoExcepcion.USER_DONT_EXISTS;
            }
            else if (EstoyAca(actual.ChildNodes[0], "batchexception"))
            {
                return TipoExcepcion.BATCH_EXCEPTION;
            }
            else if (EstoyAca(actual.ChildNodes[0], "bdalreadyexists"))
            {
                return TipoExcepcion.BD_ALREADY_EXISTS;
            }
            else if (EstoyAca(actual.ChildNodes[0], "tabledontexists"))
            {
                return TipoExcepcion.TABLE_DONT_EXISTS;
            }
            else if (EstoyAca(actual.ChildNodes[0], "valuesexception"))
            {
                return TipoExcepcion.VALUES_EXCEPTION;
            }
            else if (EstoyAca(actual.ChildNodes[0], "columnexception"))
            {
                return TipoExcepcion.COLUMN_EXCEPTION;
            }
            else if (EstoyAca(actual.ChildNodes[0], "typealreadyexists"))
            {
                return TipoExcepcion.TYPE_ALREADY_EXISTS;
            }
            else if (EstoyAca(actual.ChildNodes[0], "useralreadyexists"))
            {
                return TipoExcepcion.USER_ALREADY_EXISTS;
            }
            else if (EstoyAca(actual.ChildNodes[0], "indexoutexception"))
            {
                return TipoExcepcion.INDEX_OUT_EXCEPTION;
            }
            else if (EstoyAca(actual.ChildNodes[0], "tablealreadyexists"))
            {
                return TipoExcepcion.TABLE_ALREADY_EXISTS;
            }
            else if (EstoyAca(actual.ChildNodes[0], "arithmeticexception"))
            {
                return TipoExcepcion.ARITHMETIC_EXCEPTION;
            }
            else if (EstoyAca(actual.ChildNodes[0], "objectalreadyexists"))
            {
                return TipoExcepcion.OBJECT_ALREADY_EXISTS;
            }
            else if (EstoyAca(actual.ChildNodes[0], "countertypeexception"))
            {
                return TipoExcepcion.COUNTER_TYPE_EXCEPTION;
            }
            else if (EstoyAca(actual.ChildNodes[0], "nullpointerexception"))
            {
                return TipoExcepcion.NULL_POINTER_EXCEPTION;
            }
            else if (EstoyAca(actual.ChildNodes[0], "functionalreadyexists"))
            {
                return TipoExcepcion.FUNCTION_ALREADY_EXISTS;
            }
            else if (EstoyAca(actual.ChildNodes[0], "numberreturnsexception"))
            {
                return TipoExcepcion.NUMBER_RETURNS_EXCEPTION;
            }
            else if (EstoyAca(actual.ChildNodes[0], "procedurealreadyexists"))
            {
                return TipoExcepcion.PROCEDURE_ALREADY_EXISTS;
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

    static int GetFila(ParseTreeNode nodo, int num)
    {
        return nodo.ChildNodes[num].Token.Location.Line;
    }

    static int GetColumna(ParseTreeNode nodo, int num)
    {
        return nodo.ChildNodes[num].Token.Location.Column;
    }
}