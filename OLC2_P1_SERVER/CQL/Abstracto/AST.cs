using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class AST
{
    private List<Instruccion> instrucciones;

    public AST(List<Instruccion> instrucciones)
    {
        this.instrucciones = instrucciones;
    }
}