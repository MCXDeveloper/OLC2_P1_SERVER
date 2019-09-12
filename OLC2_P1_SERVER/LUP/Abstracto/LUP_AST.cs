using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class LUP_AST : LUP_Instruccion
{
    public LUP_Instruccion Instruccion { get; set; }

    public LUP_AST(LUP_Instruccion instruccion)
    {
        Instruccion = instruccion;
    }

    public object Ejecutar()
    {
        if (Instruccion is LoginPackage)
        {
            LoginPackage lp = (LoginPackage)Instruccion;
            return lp.Ejecutar();
        }
        else if (Instruccion is LogoutPackage)
        {
            LogoutPackage lp = (LogoutPackage)Instruccion;
            return lp.Ejecutar();
        }
        else if (Instruccion is QueryPackage)
        {
            QueryPackage qp = (QueryPackage)Instruccion;
            return qp.Ejecutar();
        }

        return null;
    }
}