
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CreateDatabase : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public bool ExistsFlag { get; set; }
    public string NombreBD { get; set; }

    public CreateDatabase(bool exists_flag, string nombre_bd, int fila, int columna)
    {
        this.fila = fila;
        NombreBD = nombre_bd;
        this.columna = columna;
        ExistsFlag = exists_flag;
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Primero valido que la base de datos que se desea crear no exista en el sistema.
        if (!CQL.ExisteBaseDeDatos(NombreBD))
        {
            // 2. Registro la base de datos en el sistema.
            CQL.RegistrarBaseDeDatos(NombreBD);
        }
        else
        {
            if(!ExistsFlag)
            {
                CQL.AddLUPError("Semántico", "[CREATE_DATABASE]", "Error.  No se pudo crear la base de datos ya que existe una actualmente con el mismo nombre.", fila, columna);
            }
        }

        return new Nulo();
    }
}