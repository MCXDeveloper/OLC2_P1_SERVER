using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Nulo : Instruccion, Expresion
{
    public object Ejecutar(Entorno ent)
    {
        return new Nulo();
    }

    public TipoDato GetTipo(Entorno ent)
    {
        return new TipoDato(TipoDato.Tipo.NULO);
    }

    public override string ToString()
    {
        return "NULL";
    }
}