
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
        if(CQL.ExisteBaseDeDatos(NombreBD))
        {
            // 2. Verifico que el usuario actualmente logueado tenga permisos para usar la base de datos.
            if (CQL.TienePermisosSobreBaseDeDatos(CQL.UsuarioLogueado, NombreBD))
            {
                // 3. Una vez verifico que la base de datos exista, procedo a actualizar mi variable estática que me indica la base de datos en uso.
                CQL.BaseDatosEnUso = NombreBD;
            }
            else
            {
                CQL.BaseDatosEnUso = string.Empty;
                CQL.AddLUPError("Semántico", "[USE_DATABASE]", "Error.  El usuario '"+ CQL.UsuarioLogueado +"' no cuenta con los permisos suficientes para utilizar la base de datos '"+ NombreBD +"'.", fila, columna);
            }
        }
        else
        {
            CQL.BaseDatosEnUso = string.Empty;
            string mensaje = "Error.  La base de datos que se desea utilizar (" + NombreBD + ") no existe en el sistema.";
            CQL.AddLUPError("Semántico", "[USE_DATABASE]", mensaje, fila, columna);
            if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'BDDontExists' no capturada.  " + mensaje); }
            return new BDDontExists(mensaje);
        }
        
        return new Nulo();
    }
}