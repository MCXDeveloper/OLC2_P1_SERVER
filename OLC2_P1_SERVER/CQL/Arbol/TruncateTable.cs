
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class TruncateTable : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public string NombreTabla { get; set; }

    public TruncateTable(string nombre_tabla, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        NombreTabla = nombre_tabla;
    }

    public object Ejecutar(Entorno ent)
    {
        // +----------------------------------------------------------------------------------------------+
        // |                                             Nota                                             |
        // +----------------------------------------------------------------------------------------------+
        // | Para realizar de forma correcta la truncada de tablas, se deben seguir los siguientes pasos: |
        // | 1. Se debe validar que exista una base de datos en uso actual para poder eliminar la tabla.  |
        // | 2. Se debe validar que la tabla exista en la base de datos.                                  |
        // +----------------------------------------------------------------------------------------------+

        // 1. Procedo a verificar si existe alguna base de datos en uso, de lo contrario, se reporta el error.
        if (CQL.ExisteBaseDeDatosEnUso())
        {
            // 2. Procedo a verificar que la tabla que se desea truncar exista en la base de datos.
            if (CQL.ExisteTablaEnBD(NombreTabla))
            {
                // 3. Procedo a truncar la tabla.
                CQL.TruncarTabla(NombreTabla);
                CQL.AddLUPMessage(">> La tabla '"+ NombreTabla +"' ha sido truncada exitosamente.");
            }
            else
            {
                CQL.AddLUPError("Semántico", "[TRUNCATE_TABLE]", "Error.  La tabla especificada '" + NombreTabla + "' no existe en la base de datos actual, por lo tanto, no se puede truncar.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[TRUNCATE_TABLE]", "Error.  No se puede truncar una tabla si no se ha especificado la base de datos a utilizar.", fila, columna);
        }

        return new Nulo();
    }
}