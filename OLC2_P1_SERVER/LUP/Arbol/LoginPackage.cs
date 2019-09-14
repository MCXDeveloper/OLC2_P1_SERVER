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
                System.Diagnostics.Debug.Write("Error. No existe un usuario con el user y pass proporcionado." + Environment.NewLine);
            }
        }
        else
        {
            System.Diagnostics.Debug.Write("Error. Actualmente existe un usuario utilizando la base de datos." + Environment.NewLine);
        }

        // TODO | LoginPackage | Cambiar el return de aqui abajo
        //return "[+LOGIN][FAIL][-LOGIN]";
        CQL.UsuarioLogueado = Usuario;
        return "[+LOGIN][SUCCESS][-LOGIN]";
    }

}