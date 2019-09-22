using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class LoginPackage : LUP_Instruccion
{
    public string Usuario { get; set; }
    public string Contrasena { get; set; }

    public LoginPackage(string user, string pass)
    {
        Usuario = user;
        Contrasena = pass;
    }

    public object Ejecutar()
    {
        // 1. Primero verifico si existe actualmente un usuario logueado, de lo contrario no se podrá hacer uso de la base de datos.
        if (!CQL.ExisteUsuarioLogueado())
        {
            // 2. Verifico que el usuario exista en la lista de usuarios.
            if (CQL.ValidarLogin(Usuario, Contrasena))
            {
                CQL.UsuarioLogueado = Usuario;
                return "[+LOGIN][SUCCESS][-LOGIN]";
            }
            else
            {
                CQL.AddLUPMessage("Error. No existe un usuario con el user y pass proporcionado.");
            }
        }
        else
        {
            CQL.AddLUPMessage("Error. Actualmente existe un usuario utilizando la base de datos.");
        }

        return "[+LOGIN][FAIL][-LOGIN]";
    }

}