using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class UseDatabase : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public string NombreBD { get; set; }

    public UseDatabase(string nombre_bd, int fila, int columna)
    {
        this.fila = fila;
        NombreBD = nombre_bd;
        this.columna = columna;
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Primero verifico que la base de datos que se quiere utilizar exista en el sistema.
        if(CQL.RootBD.ExistsDatabase(NombreBD))
        {
            // 2. Una vez verifico que la base de datos exista, procedo a actualizar mi variable estática que me indica la base de datos en uso.
            CQL.BaseDatosEnUso = NombreBD;
        }
        else
        {
            CQL.AddLUPError("Semántico", "[USE_DATABASE]", "Error.  La base de datos que se desea utilizar ("+ NombreBD +") no existe en el sistema.", fila, columna);
        }

        return new Nulo();
    }
}