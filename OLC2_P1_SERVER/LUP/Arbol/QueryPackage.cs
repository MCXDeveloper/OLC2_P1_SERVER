using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class QueryPackage : LUP_Instruccion
{

    private readonly string user;
    private readonly string consulta;

    public QueryPackage(string user, string consulta)
    {
        this.user = user;
        this.consulta = consulta;
    }

    public object Ejecutar()
    {
        throw new NotImplementedException();
    }
}