using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Break : Instruccion
{
    public object Ejecutar(Entorno ent, AST arbol)
    {
        return new Nulo();
    }
}