using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class DropTable : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public bool ExistsFlag { get; set; }
    public string NombreTabla { get; set; }

    public DropTable(bool exists_flag, string nombre_tabla, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        ExistsFlag = exists_flag;
        NombreTabla = nombre_tabla;
    }

    public object Ejecutar(Entorno ent)
    {
        // +-------------------------------------------------------------------------------------------------+
        // |                                              Nota                                               |
        // +-------------------------------------------------------------------------------------------------+
        // | Para realizar de forma correcta la eliminación de tablas, se deben seguir los siguientes pasos: |
        // | 1. Se debe validar que exista una base de datos en uso actual para poder eliminar la tabla.     |
        // | 2. Se debe validar que la tabla exista en la base de datos.                                     |
        // +-------------------------------------------------------------------------------------------------+

        // 1. Procedo a verificar si existe alguna base de datos en uso, de lo contrario, se reporta el error.
        if (!CQL.BaseDatosEnUso.Equals(String.Empty))
        {
            // 2. Procedo a verificar que la tabla que se desea eliminar exista en la base de datos.
            if (CQL.RootBD.GetDatabase(CQL.BaseDatosEnUso).ExisteTabla(NombreTabla))
            {
                // 3. Procedo a eliminar la tabla.
                CQL.RootBD.GetDatabase(CQL.BaseDatosEnUso).EliminarTabla(NombreTabla);
            }
            else
            {
                CQL.AddLUPError("Semántico", "[DROP_TABLE]", "Error.  La tabla especificada '" + NombreTabla + "' no existe en la base de datos actual, por lo tanto, no se puede eliminar.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[DROP_TABLE]", "Error.  No se puede eliminar una tabla si no se ha especificado la base de datos a utilizar.", fila, columna);
        }

        return new Nulo();
    }
}