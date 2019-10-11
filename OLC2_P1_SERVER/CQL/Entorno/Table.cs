using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
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

    public void AddRow(List<object> ListValues, int fila, int columna)
    {
        DataRow row = Tabla.NewRow();

        for (int i = 0; i < Tabla.Columns.Count; i++)
        {
            row[((Columna)Tabla.Columns[i]).NombreColumna] = ((Columna)Tabla.Columns[i]).TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.COUNTER) ? null : ListValues[i];
        }

        try
        {
            Tabla.Rows.Add(row);
        }
        catch (Exception ex)
        {
            CQL.AddLUPError("Semántico", "[INSERT_TABLE]", ex.Message, fila, columna);
        }
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
    
    public string CrearChison(int numTabs)
    {
        List<string> ChisonColumnas = new List<string>();
        List<string> ChisonValoresTabla = new List<string>();

        string chison = new string('\t', numTabs + 1) + "<" + Environment.NewLine;
        chison += new string('\t', numTabs + 2) + "\"CQL-TYPE\" = \"TABLE\", " + Environment.NewLine;
        chison += new string('\t', numTabs + 2) + "\"NAME\" = \"" + NombreTabla +"\", " + Environment.NewLine;
        chison += new string('\t', numTabs + 2) + "\"COLUMNS\" = [" + Environment.NewLine;

        /* COLUMNAS DE LA TABLA */
        foreach (Columna col in Tabla.Columns)
        {
            ChisonColumnas.Add(col.CrearChison(numTabs + 2));
        }

        chison += string.Join(", ", ChisonColumnas);
        chison += new string('\t', numTabs + 2) + "], " + Environment.NewLine;

        /* DATOS DE LA TABLA */
        chison += new string('\t', numTabs + 2) + "\"DATA\" = [" + Environment.NewLine;

        foreach (DataRow row in Tabla.Rows)
        {
            string subchison = new string('\t', numTabs + 3) + "<" + Environment.NewLine;

            foreach (Columna col in Tabla.Columns)
            {
                string tabColumn = "\"" + col.ColumnName + "\"";
                bool isLastColumn = Tabla.Columns[Tabla.Columns.Count - 1].Equals(col);
                string chisonval = GetChisonRowValue(col, row[col.ColumnName]);
                subchison += new string('\t', numTabs + 4) + tabColumn + " = " + chisonval + (isLastColumn ? Environment.NewLine : ", " + Environment.NewLine);
            }

            subchison += new string('\t', numTabs + 3) + ">" + Environment.NewLine;
            ChisonValoresTabla.Add(subchison);
        }

        chison += string.Join(", ", ChisonValoresTabla);
        chison += new string('\t', numTabs + 2) + "]" + Environment.NewLine;
        chison += new string('\t', numTabs + 1) + ">" + Environment.NewLine;

        return chison;
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

    private string GetChisonRowValue(Columna col, object val)
    {
        if (val is null || val is DBNull)
        {
            return "NULL";
        }
        else if (val is string)
        {
            return "\"" + val.ToString() + "\"";
        }
        else if (val is DateTime)
        {
            if (col.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.DATE))
            {
                return "'" + ((DateTime)val).Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "'";
            }
            else
            {
                return "'" + ((DateTime)val).ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture) + "'";
            }
        }
        else
        {
            if (col.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.MAP))
            {
                Map xx = (Map)val;
                return xx.GetChisonRepresentation();
            }
            else if (col.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.LIST))
            {
                XList xx = (XList)val;
                return xx.GetChisonRepresentation();
            }
            else if (col.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.SET))
            {
                XSet xx = (XSet)val;
                return xx.GetChisonRepresentation();
            }
            else if (col.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.OBJECT))
            {
                Objeto xx = (Objeto)val;
                return xx.GetChisonRepresentation();
            }
            else
            {
                return val.ToString();
            }    
        }
    }
}