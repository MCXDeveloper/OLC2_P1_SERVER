
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class DropDatabase : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public string NombreBD { get; set; }

    public DropDatabase(string nombre_bd, int fila, int columna)
    {
        this.fila = fila;
        NombreBD = nombre_bd;
        this.columna = columna;
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Primero valido que la base de datos que se desea eliminar exista en el sistema.
        if (CQL.ExisteBaseDeDatos(NombreBD))
        {
            // 2. Verifico si el usuario que está logueado tiene permisos sobre esa base de datos como para eliminarla.
            if(CQL.TienePermisosSobreBaseDeDatos(CQL.UsuarioLogueado, NombreBD))
            {
                // 3. Elimino del sistema la base de datos indicada.
                CQL.EliminarBaseDeDatos(NombreBD);
            }
            else
            {
                CQL.AddLUPError("Semántico", "[DROP_DATABASE]", "Error.  El usuario ("+ CQL.UsuarioLogueado +") no tiene permisos para eliminar la base de datos (" + NombreBD + ").", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[DROP_DATABASE]", "Error.  La base de datos que se desea eliminar (" + NombreBD + ") no existe en el sistema.", fila, columna);
        }

        return new Nulo();
    }
}