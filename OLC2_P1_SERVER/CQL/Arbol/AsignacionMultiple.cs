using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class AsignacionMultiple : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public List<string> ListaVariables { get; set; }
    public LlamadaProcedimiento ValorProcedimiento { get; set; }

    public AsignacionMultiple(List<string> lista_variables, LlamadaProcedimiento proc, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        ValorProcedimiento = proc;
        ListaVariables = lista_variables;
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Valido que todas las variables existan en el entorno.
        if (ExistenVariablesEnEntorno(ent))
        {
            // 2. Valido que la llamada a procedimiento devuelva una lista o un elemento diferente de Nulo.
            object callValues = ObtenerValorDeProcedimiento(ent);

            if (callValues is List<object>)
            {
                List<object> xCallValues = (List<object>)callValues;

                // 3. Valido que la cantidad de variables listadas concuerde con la cantidad de retornos del procedimiento (xCallValues).
                if (ListaVariables.Count.Equals(xCallValues.Count))
                {
                    // 4. Procedo a iterar sobre cada una de las variables asignando su valor en el entorno.
                    for (int i = 0; i < ListaVariables.Count; i++)
                    {
                        Variable simbolo = (Variable)ent.ObtenerVariable(ListaVariables[i]);
                        simbolo.Valor = xCallValues[i];
                    }
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[ASIGNACION_MULTIPLE]", "Error. No se puede realizar la asignación ya que la cantidad de valores devueltos por el procedimiento no concuerda con la cantidad de variables.", fila, columna);
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[ASIGNACION_MULTIPLE]", "Error. No se puede realizar la asignación ya que el procedimiento no devuelve ningún valor.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ASIGNACION_MULTIPLE]", "Error. Alguna de las variables proporcionadas no existe en el entorno.", fila, columna);
        }

        return new Nulo();
    }

    private bool ExistenVariablesEnEntorno(Entorno ent)
    {
        foreach (string var in ListaVariables)
        {
            object simbolo = ent.ObtenerVariable(var);
            
            if (simbolo is Nulo)
            {
                return false;
            }
        }

        return true;
    }

    private object ObtenerValorDeProcedimiento(Entorno ent)
    {
        List<object> retorno = new List<object>();
        object resp = ValorProcedimiento.Ejecutar(ent);

        if (resp is List<object>)
        {
            retorno.AddRange((List<object>)resp);
        }
        else if (resp is Nulo)
        {
            return resp;
        }
        else if (resp is object)
        {
            retorno.Add(resp);
        }

        return retorno;
    }
}