using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class LogoutPackage : LUP_Instruccion
{

    private readonly string user;

    public LogoutPackage(string user)
    {
        this.user = user;
    }

    public object Ejecutar()
    {
        throw new NotImplementedException();
    }

}