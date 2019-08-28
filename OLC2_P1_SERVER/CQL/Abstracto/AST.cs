using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class AST
{
    private LinkedList<Instruccion> instrucciones;

    public AST(LinkedList<Instruccion> instrucciones)
    {
        this.instrucciones = instrucciones;
    }
}