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

    public void AddColumn(Columna col)
    {
        col.ColumnName = col.NombreColumna;
        col.DataType = GetTheType(col.TipoDatoColumna.GetRealTipo());
        Tabla.Columns.Add(col);
    }
    
    public void DeleteColumn()
    {

    }

    public void SetPrimaryKeys(Columna[] pks)
    {
        Tabla.PrimaryKey = pks;
    }

    private Type GetTheType(TipoDato.Tipo tipo)
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

    public string CrearPaqueteLUP()
    {
        throw new NotImplementedException();
    }
}