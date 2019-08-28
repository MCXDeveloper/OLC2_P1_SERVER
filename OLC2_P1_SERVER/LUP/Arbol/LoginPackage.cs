using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class LoginPackage : LUP_Instruccion
{

    private readonly string user;
    private readonly string pass;

    public LoginPackage(string user, string pass)
    {
        this.user = user;
        this.pass = pass;
    }

    public object Ejecutar()
    {
        throw new NotImplementedException();
    }

}