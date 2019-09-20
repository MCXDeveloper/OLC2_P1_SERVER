using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class FuncionAgregacion : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public Select Consulta { get; set; }
    public TipoFuncionAgregacion TipoFuncionAGG { get; set; }

    public FuncionAgregacion(TipoFuncionAgregacion tipo_funcion_agg, Select consulta, int fila, int columna)
    {
        this.fila = fila;
        Consulta = consulta;
        this.columna = columna;
        TipoFuncionAGG = tipo_funcion_agg;
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Mando a ejecutar la consulta y verifico que la cantidad de columnas retornadas sea solo una.
        Consulta.IsFuncionAgregacion = true;
        object exec = Consulta.Ejecutar(ent);

        if (!(exec is Nulo))
        {
            Table tabSelect = (Table)exec;
            DataTable tab = tabSelect.Tabla;

            // 2. Verifico que la tabla que devolvio el select cuente con una sola columna de datos.
            if (tab.Columns.Count == 1)
            {
                // 3. Verifico que si la función de agregación a realizar es COUNT, para retornar unicamente el número de filas.
                if (TipoFuncionAGG.Equals(TipoFuncionAgregacion.COUNT))
                {
                    return tab.Rows.Count;
                }

                // 3. Verifico que si la función de agregación a realizar es MIN o MAX ya que esta se puede hacer con todos los primitivos.
                else if (TipoFuncionAGG.Equals(TipoFuncionAgregacion.MIN) || TipoFuncionAGG.Equals(TipoFuncionAgregacion.MAX))
                {
                    return GetMinMaxValue(tab);
                }

                // 3. Verifico que si la función de agregación a realizar es SUM o AVG ya que esta se puede hacer únicamente con valores numéricos.
                else
                {
                    return GetSumAvgValue(tab);
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[FUNCION_AGREGACION]", "Error.  La consulta no devolvió una sola columna para realizar la función de agregación.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[FUNCION_AGREGACION]", "Error.  La consulta no devolvió un valor válido para ejecutar la función de agregación.", fila, columna);
        }

        return new Nulo();
    }

    public TipoDato GetTipo(Entorno ent)
    {
        object valor = Ejecutar(ent);

        if (valor is int)
        {
            return new TipoDato(TipoDato.Tipo.INT);
        }
        else if (valor is double)
        {
            return new TipoDato(TipoDato.Tipo.DOUBLE);
        }
        else
        {
            return new TipoDato(TipoDato.Tipo.NULO);
        }
    }

    public List<int> GetListaDeEnteros(DataTable tab)
    {
        List<int> resp = new List<int>();

        foreach (DataRow row in tab.Rows)
        {
            resp.Add((int)row[0]);
        }

        return resp;
    }

    public List<double> GetListaDoubles(DataTable tab)
    {
        List<double> resp = new List<double>();

        foreach (DataRow row in tab.Rows)
        {
            resp.Add((double)row[0]);
        }

        return resp;
    }

    public List<string> GetListaStrings(DataTable tab)
    {
        List<string> resp = new List<string>();

        foreach (DataRow row in tab.Rows)
        {
            resp.Add((string)row[0]);
        }

        return resp;
    }

    public List<bool> GetListaBools(DataTable tab)
    {
        List<bool> resp = new List<bool>();

        foreach (DataRow row in tab.Rows)
        {
            resp.Add((bool)row[0]);
        }

        return resp;
    }

    public List<DateTime> GetListaDateTime(DataTable tab)
    {
        List<DateTime> resp = new List<DateTime>();

        foreach (DataRow row in tab.Rows)
        {
            resp.Add((DateTime
)row[0]);
        }

        return resp;
    }

    public object GetMinMaxValue(DataTable tab)
    {
        if (tab.Columns[0].DataType.Equals(typeof(int)))
        {
            List<int> lista = GetListaDeEnteros(tab);
            return TipoFuncionAGG.Equals(TipoFuncionAgregacion.MIN) ? lista.Min() : lista.Max();
        }
        else if (tab.Columns[0].DataType.Equals(typeof(double)))
        {
            List<double> lista = GetListaDoubles(tab);
            return TipoFuncionAGG.Equals(TipoFuncionAgregacion.MIN) ? lista.Min() : lista.Max();
        }
        else if (tab.Columns[0].DataType.Equals(typeof(string)))
        {
            List<string> lista = GetListaStrings(tab);
            return TipoFuncionAGG.Equals(TipoFuncionAgregacion.MIN) ? lista.Min() : lista.Max();
        }
        else if (tab.Columns[0].DataType.Equals(typeof(bool)))
        {
            List<bool> lista = GetListaBools(tab);
            return TipoFuncionAGG.Equals(TipoFuncionAgregacion.MIN) ? lista.Min() : lista.Max();
        }
        else if (tab.Columns[0].DataType.Equals(typeof(DateTime)))
        {
            List<DateTime> lista = GetListaDateTime(tab);
            return TipoFuncionAGG.Equals(TipoFuncionAgregacion.MIN) ? lista.Min() : lista.Max();
        }
        else
        {
            CQL.AddLUPError("Semántico", "[FUNCION_AGREGACION]", "Error.  Las funciones de agregación MIN & MAX necesitan que el valor de la columna sea de tipo primitivo.", fila, columna);
        }

        return new Nulo();
    }

    public object GetSumAvgValue(DataTable tab)
    {
        if (tab.Columns[0].DataType.Equals(typeof(int)))
        {
            List<int> lista = GetListaDeEnteros(tab);
            return TipoFuncionAGG.Equals(TipoFuncionAgregacion.SUM) ? lista.Sum() : lista.Average();
        }
        else if (tab.Columns[0].DataType.Equals(typeof(double)))
        {
            List<double> lista = GetListaDoubles(tab);
            return TipoFuncionAGG.Equals(TipoFuncionAgregacion.SUM) ? lista.Sum() : lista.Average();
        }
        else
        {
            CQL.AddLUPError("Semántico", "[FUNCION_AGREGACION]", "Error.  Las funciones de agregación SUM & AVG necesitan que el valor de la columna sea de tipo numérico.", fila, columna);
        }

        return new Nulo();
    }
}