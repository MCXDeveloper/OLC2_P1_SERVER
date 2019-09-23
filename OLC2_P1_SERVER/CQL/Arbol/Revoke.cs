using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Revoke : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public string NombreBD { get; set; }
    public string NombreUsuario { get; set; }

    public Revoke(string nombre_usuario, string nombre_bd, int fila, int columna)
    {
        this.fila = fila;
        NombreBD = nombre_bd;
        this.columna = columna;
        NombreUsuario = nombre_usuario;
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Verifico que exista la base de datos a la que se le desea dar permiso a un usuario.
        if (CQL.ExisteBaseDeDatos(NombreBD))
        {
            // 2. Verifico que el usuario exista en el sistema.
            if (CQL.ExisteUsuarioEnSistema(NombreUsuario))
            {
                // 3. Verifico que el usuario se encuentre actualmente en la lista de usuarios con permisos de la base de datos.
                if (CQL.ExisteUsuarioEnPermisosBD(NombreBD, NombreUsuario))
                {
                    CQL.EliminarUsuarioEnPermisos(NombreBD, NombreUsuario);
                    CQL.AddLUPMessage("Se removieron correctamente los permisos al usuario '"+ NombreUsuario +"', sobre la base de datos '"+ NombreBD +"'.");
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[REVOKE]", "Error. El usuario '" + NombreUsuario + "' o no existe entre los usuarios con permisos o no se pueden eliminar los permisos del usuario creador de la BD.", fila, columna);
                }
            }
            else
            {
                string mensaje = "Error. No se puede quitar permisos al usuario '" + NombreUsuario + "' ya que no existe en el sistema.";
                CQL.AddLUPError("Semántico", "[REVOKE]", mensaje, fila, columna);
                if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'UserDontExists' no capturada.  " + mensaje); }
                return new UserDontExists(mensaje);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[REVOKE]", "Error. No se puede quitar permisos a un usuario sobre una base de datos inexistente.", fila, columna);
        }

        return new Nulo();
    }
}