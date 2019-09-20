
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

public class LlamadaFuncion : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public string NombreFuncion { get; set; }
    public List<Expresion> ListaValores { get; set; }

    public LlamadaFuncion(string nombre_funcion, List<Expresion> lista_valores, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        ListaValores = lista_valores;
        NombreFuncion = nombre_funcion;
    }

    public object Ejecutar(Entorno ent)
    {
        string id = GenerarIdentificadorFuncion(ent);
        object func = ent.ObtenerFuncion(id);

        if (!(func is Nulo))
        {
            Funcion f = (Funcion)func;
            Entorno local = new Entorno(AST.global);

            if (ListaValores.Count == f.ListaParametros.Count)
            {
                for (int i = 0; i < ListaValores.Count; i++)
                {
                    object valorVariable = ListaValores[i].Ejecutar(ent);
                    string nombreVariable = f.ListaParametros[i].NombreParametro;
                    TipoDato tipoVariable = f.ListaParametros[i].TipoDatoParametro;
                    local.Agregar(nombreVariable, new Variable(tipoVariable, nombreVariable, valorVariable));
                }

                foreach (Instruccion ins in f.ListaInstrucciones)
                {
                    object result = ins.Ejecutar(local);

                    if (result is Return)
                    {
                        return ((Return)result).Ejecutar(local);
                    }
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[LLAMADA_FUNCION]", "Error en la llamada a función '" + NombreFuncion + "'.  La cantidad de parámetros no coincide.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[LLAMADA_FUNCION]", "Error.  No existe la función con el nombre de '" + NombreFuncion + "' (Key: " + id + ") en el entorno.", fila, columna);
        }

        return new Nulo();
    }

    public TipoDato GetTipo(Entorno ent)
    {
        Funcion func = (Funcion)ent.ObtenerFuncion(GenerarIdentificadorFuncion(ent));
        return func.TipoDatoFuncion;
    }

    public string GenerarIdentificadorFuncion(Entorno ent)
    {
        string id = "_" + NombreFuncion + "(";

        foreach (Expresion exp in ListaValores)
        {
            if (exp is LlamadaFuncion)
            {
                id += "_" + ((LlamadaFuncion)exp).GetTipo(ent).GetRealTipo();
            }
            else
            {
                object resultado = exp.Ejecutar(ent);

                if (resultado is int)
                {
                    id += "_" + TipoDato.Tipo.INT;
                }
                else if (resultado is double)
                {
                    id += "_" + TipoDato.Tipo.DOUBLE;
                }
                else if (resultado is string)
                {
                    id += "_" + TipoDato.Tipo.STRING;
                }
                else if (resultado is bool)
                {
                    id += "_" + TipoDato.Tipo.BOOLEAN;
                }
                else if (resultado is Date)
                {
                    id += "_" + TipoDato.Tipo.DATE;
                }
                else if (resultado is Time)
                {
                    id += "_" + TipoDato.Tipo.TIME;
                }
                else if (resultado is Map)
                {
                    id += "_" + TipoDato.Tipo.MAP;
                }
                else if (resultado is XList)
                {
                    id += "_" + TipoDato.Tipo.LIST;
                }
                else if (resultado is XSet)
                {
                    id += "_" + TipoDato.Tipo.SET;
                }
                else if (resultado is Objeto)
                {
                    id += "_" + TipoDato.Tipo.OBJECT + "_" + (string)((Objeto)resultado).TipoDatoObjeto.GetElemento();
                }
            }
        }

        id += ")";

        return id;
    }
}