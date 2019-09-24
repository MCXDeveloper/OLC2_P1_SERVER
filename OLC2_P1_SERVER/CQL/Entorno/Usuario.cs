using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Usuario : InstruccionBD
{
    public string NombreUsuario { get; set; }
    public string PasswordUsuario { get; set; }

    public Usuario(string user, string pass)
    {
        NombreUsuario = user;
        PasswordUsuario = pass;
    }

    public string CrearChison()
    {
        throw new NotImplementedException();
    }

    public string CrearPaqueteLUP(string user)
    {
        throw new NotImplementedException();
    }
}