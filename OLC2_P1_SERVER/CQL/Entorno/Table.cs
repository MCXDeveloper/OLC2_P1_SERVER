using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class Table : InstruccionBD
{
    public DataTable Tabla { get; set; }
    public string NombreTabla { get; set; }

    public Table(string nombre_tabla)
    {
        NombreTabla = nombre_tabla;
        Tabla = new DataTable(nombre_tabla);
    }

    public bool ExistsColumn(string colName)
    {
        return Tabla.Columns.Contains(colName);
    }

    public Columna GetColumn(string colName)
    {
        return (Columna)Tabla.Columns[colName];
    }

    public void AddColumn(Columna col)
    {
        col.ColumnName = col.NombreColumna;
        col.DataType = GetTheType(col.TipoDatoColumna.GetRealTipo());
        col.AllowDBNull = true;

        if (col.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.COUNTER))
        {
            col.AutoIncrement = true;
            col.AutoIncrementSeed = 1;
            col.AutoIncrementStep = 1;
        }

        Tabla.Columns.Add(col);
    }
    
    public void DeleteColumn(string colName)
    {
        Tabla.Columns.Remove(colName);
    }
    
    public bool IsPrimaryKeyColumn(string colName)
    {
        return Array.IndexOf(Tabla.PrimaryKey, Tabla.Columns[colName]) >= 0;
    }

    public void SetPrimaryKeys(Columna[] pks)
    {
        Tabla.PrimaryKey = pks;
    }

    public int GetColumnCountWithoutCounterColumns()
    {
        int x = 0;

        foreach (Columna col in Tabla.Columns)
        {
            x += (col.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.COUNTER) ? 0 : 1);
        }

        return x;
    }

    public void AddRow(List<object> ListValues)
    {
        DataRow row = Tabla.NewRow();

        for (int i = 0; i < Tabla.Columns.Count; i++)
        {
            row[((Columna)Tabla.Columns[i]).NombreColumna] = ((Columna)Tabla.Columns[i]).TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.COUNTER) ? null : ListValues[i];
        }

        Tabla.Rows.Add(row);
    }

    public void AddRow(List<string> ListFields, List<object> ListValues)
    {
        DataRow row = Tabla.NewRow();

        for (int i = 0; i < ListFields.Count; i++)
        {
            row[ListFields[i]] = (ListValues[i] is Nulo) ? DBNull.Value : ListValues[i];
        }

        Tabla.Rows.Add(row);
    }

    public static Type GetTheType(TipoDato.Tipo tipo)
    {
        switch (tipo)
        {
            case TipoDato.Tipo.INT:
                return Type.GetType("System.Int32");
            case TipoDato.Tipo.DOUBLE:
                return Type.GetType("System.Double");
            case TipoDato.Tipo.STRING:
                return Type.GetType("System.String");
            case TipoDato.Tipo.BOOLEAN:
                return Type.GetType("System.Boolean");
            case TipoDato.Tipo.DATE:
                return Type.GetType("System.DateTime");
            case TipoDato.Tipo.TIME:
                return Type.GetType("System.DateTime");
            case TipoDato.Tipo.COUNTER:
                return Type.GetType("System.Int32");
            default:
                return Type.GetType("System.Object");
        }
    }
    
    public string CrearChison()
    {
        throw new NotImplementedException();
    }

    public string CrearPaqueteLUP(string user)
    {
        string response = "[+TABLE]";

        // Nombre de la tabla.
        response += "[+NAME]"+ NombreTabla +"[-NAME]";

        // Columnas de la tabla.
        foreach (DataColumn col in Tabla.Columns)
        {
            response += "[+COLUMNS]"+ col.ColumnName +"[-COLUMNS]";
        }
        
        response += "[-TABLE]";

        return response;
    }
}