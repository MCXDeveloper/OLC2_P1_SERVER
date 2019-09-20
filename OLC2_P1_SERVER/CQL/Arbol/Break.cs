
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Break : Instruccion
{
    public object Ejecutar(Entorno ent)
    {
        return new Nulo();
    }
}