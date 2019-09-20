
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Continue : Instruccion
{
    public object Ejecutar(Entorno ent)
    {
        return new Nulo();
    }
}