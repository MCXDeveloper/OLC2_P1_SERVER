using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Primitivo : Instruccion
{
    private readonly object valor;

    public Primitivo(object valor)
    {
        this.valor = valor;
    }

    public object Ejecutar(Entorno ent, AST arbol)
    {
        return valor;
    }
}