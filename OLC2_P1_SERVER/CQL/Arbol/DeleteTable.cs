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
    public string NombreObjeto { get; set; }
    public Expresion ExpresionWhere { get; set; }
    public Expresion ExpresionPosicionObjeto { get; set; }

    public DeleteTable(string nombre_tabla, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        NombreTabla = nombre_tabla;
        NombreObjeto = string.Empty;
        ExpresionWhere = new Nulo();
        ExpresionPosicionObjeto = new Nulo();
    }

    public DeleteTable(string nombre_tabla, Expresion expresion_where, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        NombreTabla = nombre_tabla;
        NombreObjeto = string.Empty;
        ExpresionWhere = expresion_where;
        ExpresionPosicionObjeto = new Nulo();
    }

    public DeleteTable(string nombre_objeto, Expresion expresion_posicion_obj, string nombre_tabla, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        NombreTabla = nombre_tabla;
        ExpresionWhere = new Nulo();
        NombreObjeto = nombre_objeto;
        ExpresionPosicionObjeto = expresion_posicion_obj;
    }

    public DeleteTable(string nombre_objeto, Expresion expresion_posicion_obj, string nombre_tabla, Expresion expresion_where, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        NombreTabla = nombre_tabla;
        NombreObjeto = nombre_objeto;
        ExpresionWhere = expresion_where;
        ExpresionPosicionObjeto = expresion_posicion_obj;
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
                        // 4. Verifico si lo que se desea eliminar es un elemento de una collection.
                        if (ExpresionPosicionObjeto is Nulo)
                        {
                            CQL.EliminarTodosLosRegistrosDeTabla(NombreTabla);
                        }
                        else
                        {
                            return EliminarElementoDeCollection(CQL.ObtenerTabla(NombreTabla).Tabla, ent);
                        }
                    }
                    else
                    {
                        CQL.TuplaEnUso = null;
                        CQL.WhereFlag = true;
                        object delResp = EjecutarDeleteConWhere(CQL.ObtenerTabla(NombreTabla).Tabla, ent);
                        CQL.TuplaEnUso = null;
                        CQL.WhereFlag = false;
                        return delResp;
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

    private object ValidarYEliminarElemento(object rowVal, Entorno ent)
    {
        // 1. Verifico que la columna sea de tipo Collection, si no se debe arrojar un error.
        Columna col = CQL.ObtenerColumnaDeTabla(NombreTabla, NombreObjeto);
        TipoDato.Tipo colType = col.TipoDatoColumna.GetRealTipo();
        TipoDato.Tipo valueType = ExpresionPosicionObjeto.GetTipo(ent).GetRealTipo();
        object ClaveAEliminar = ExpresionPosicionObjeto.Ejecutar(ent);

        if (colType.Equals(TipoDato.Tipo.MAP))
        {
            // 2. Verifico que el MAP si contenga la clave que se está proporcionando, si no, se arroja un error.
            Map mapita = (Map)rowVal;

            if (mapita.Contains(ClaveAEliminar))
            {
                // 3. Verifico que el tipo de dato del valor correspondiente a esa clave concuerde con el que se le quiere eliminar.
                if (mapita.TipoDatoValor.GetRealTipo().Equals(valueType))
                {
                    mapita.Remove(ClaveAEliminar);
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[DELETE_TABLE]", "Error. La clave proporcionada para eliminar su valor no existe en la collection.", fila, columna);
            }
        }
        else if (colType.Equals(TipoDato.Tipo.LIST) || colType.Equals(TipoDato.Tipo.SET))
        {
            // 2. Si el tipo de la columna no es Map, entonces signfica que es o List o Set.  Para ello se agrega la validación adicional
            // de que el valor brindado como posición sea de tipo entero.

            if (ClaveAEliminar is int)
            {
                if (colType.Equals(TipoDato.Tipo.LIST))
                {
                    XList lx = (XList)rowVal;
                    object delResp = lx.Remove((int)ClaveAEliminar);
                    if (delResp is Exception)
                    {
                        return delResp;
                    }
                }
                else
                {
                    XSet lx = (XSet)rowVal;
                    object delResp = lx.Remove((int)ClaveAEliminar);
                    if (delResp is Exception)
                    {
                        return delResp;
                    }
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[DELETE_TABLE]", "Error. La clave proporcionada para eliminar su valor debe ser de tipo entero para las colecciones List/Set.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[DELETE_TABLE]", "Error. No se puede eliminar un valor en base a posición a una columna que no es de tipo Collection.", fila, columna);
        }

        return new Nulo();
    }

    private object EliminarElementoDeCollection(DataTable tablaOriginal, Entorno ent)
    {
        // 1. Verifico que exista la columna a la que se hace referencia como de tipo Collection.
        if (CQL.ExisteColumnaEnTabla(NombreTabla, NombreObjeto))
        {
            foreach (DataRow row in tablaOriginal.Rows)
            {
                object valDelResp = ValidarYEliminarElemento(row[NombreObjeto], ent);
                if (valDelResp is Exception)
                {
                    return valDelResp;
                }
            }   
        }
        else
        {
            if (!CQL.BatchFlag)
            {
                string mensaje = "Error. No se puede eliminar elemento de Collection ya que la columna especificada no existe.";
                CQL.AddLUPError("Semántico", "[DELETE_TABLE]", mensaje, fila, columna);
                if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'ColumnException' no capturada.  " + mensaje); }
                return new ColumnException(mensaje);
            }
            else
            {
                CQL.BatchErrorCounter++;
            }
        }

        return new Nulo();
    }

    private object EjecutarDeleteConWhere(DataTable tablaOriginal, Entorno ent)
    {
        for (int i = 0; i < tablaOriginal.Rows.Count; i++)
        {
            CQL.TuplaEnUso = tablaOriginal.Rows[i];
            object whereResponse = ExpresionWhere.Ejecutar(ent);

            if (whereResponse is bool)
            {
                if ((bool)whereResponse)
                {
                    if (ExpresionPosicionObjeto is Nulo)
                    {
                        tablaOriginal.Rows.Remove(tablaOriginal.Rows[i]);
                    }
                    else
                    {
                        object valDelResp = ValidarYEliminarElemento(tablaOriginal.Rows[i][NombreObjeto], ent);
                        if (valDelResp is Exception)
                        {
                            return valDelResp;
                        }
                    }
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[DELETE_TABLE]", "Error.  La operación WHERE en la consulta a la tabla '" + NombreTabla + "' no retorna un valor booleano.", fila, columna);
                break;
            }
        }

        return new Nulo();
    }
}