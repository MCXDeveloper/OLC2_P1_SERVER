using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class LogoutPackage : LUP_Instruccion
{
    public string Usuario { get; set; }

    public LogoutPackage(string user)
    {
        Usuario = user;
    }

    public object Ejecutar()
    {
        // 1. Primero verifico el usuario actualmente logueado es igual al proporcionado en el constructor.
        if (!(CQL.UsuarioLogueado is null))
        {
            if(CQL.UsuarioLogueado.Equals(Usuario))
            {
                CQL.UsuarioLogueado = String.Empty;
                return "[+LOGOUT][SUCCESS][-LOGOUT]";
            }
        }
        else
        {
            CQL.AddLUPMessage("Error. El usuario actualmente logueado no concuerda con el que desea cerrar sesión.");
        }

        return "[+LOGOUT][FAIL][-LOGOUT]";
    }

}