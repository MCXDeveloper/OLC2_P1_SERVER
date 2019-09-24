using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class DeleteTable : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public string NombreTabla { get; set; }
    public Expresion ExpresionWhere { get; set; }

    public DeleteTable(string nombre_tabla, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        NombreTabla = nombre_tabla;
        ExpresionWhere = new Nulo();
    }

    public DeleteTable(string nombre_tabla, Expresion expresion_where, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        NombreTabla = nombre_tabla;
        ExpresionWhere = expresion_where;
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Valido que exista una base de datos en uso.
        if (CQL.ExisteBaseDeDatosEnUso())
        {
            // 2. Valido que exista la tabla de la cual se quieren eliminar los registros.
            if (CQL.ExisteTablaEnBD(NombreTabla))
            {
                // Valido si la instrucción se esta validando desde un BATCH
                if (!CQL.BatchFlag)
                {
                    // 3. Verifico si la ExpresionWhere es Nulo ya que si lo es, se eliminan todos los registros, de lo contrario, se eliminan solo los filtrados por el Where.
                    if (ExpresionWhere is Nulo)
                    {
                        CQL.EliminarTodosLosRegistrosDeTabla(NombreTabla);
                    }
                    else
                    {
                        CQL.TuplaEnUso = null;
                        CQL.WhereFlag = true;
                        EjecutarDeleteConWhere(CQL.ObtenerTabla(NombreTabla).Tabla, ent);
                        CQL.TuplaEnUso = null;
                        CQL.WhereFlag = false;
                    }
                }   
            }
            else
            {
                // Valido si la instrucción se esta validando desde un BATCH
                if (!CQL.BatchFlag)
                {
                    string mensaje = "Error.  La tabla de la que se desean eliminar los registros (" + NombreTabla + ") no existe en la base de datos.";
                    CQL.AddLUPError("Semántico", "[DELETE_TABLE]", mensaje, fila, columna);
                    if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'TableDontExists' no capturada.  " + mensaje); }
                    return new TableDontExists(mensaje);
                }
                else
                {
                    CQL.BatchErrorCounter++;
                }
            }
        }
        else
        {
            // Valido si la instrucción se esta validando desde un BATCH
            if (!CQL.BatchFlag)
            {
                string mensaje = "Error.  No se puede eliminar registros de una tabla si no se ha especificado la base de datos a utilizar.";
                CQL.AddLUPError("Semántico", "[DELETE_TABLE]", mensaje, fila, columna);
                if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'UseBDException' no capturada.  " + mensaje); }
                return new UseBDException(mensaje);
            }
            else
            {
                CQL.BatchErrorCounter++;
            }
        }

        return new Nulo();
    }

    private void EjecutarDeleteConWhere(DataTable tablaOriginal, Entorno ent)
    {
        for (int i = 0; i < tablaOriginal.Rows.Count; i++)
        {
            CQL.TuplaEnUso = tablaOriginal.Rows[i];
            object whereResponse = ExpresionWhere.Ejecutar(ent);

            if (whereResponse is bool)
            {
                if ((bool)whereResponse)
                {
                    tablaOriginal.Rows.Remove(tablaOriginal.Rows[i]);
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[DELETE_TABLE]", "Error.  La operación WHERE en la consulta a la tabla '" + NombreTabla + "' no retorna un valor booleano.", fila, columna);
                break;
            }
        }
    }
}