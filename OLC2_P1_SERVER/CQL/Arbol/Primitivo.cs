using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Entorno;

public class Primitivo : Expresion
{
    private readonly object valor;

    public Primitivo(object valor)
    {
        this.valor = valor;
    }

    public object Ejecutar(Entorno ent)
    {
        return valor;
    }

    public Tipo GetTipo(Entorno ent)
    {
        if (valor is int)
        {
            return Tipo.INT;
        }
        else if (valor is double)
        {
            return Tipo.DOUBLE;
        }
        else if (valor is string)
        {
            return Tipo.STRING;
        }
        else if (valor is bool)
        {
            return Tipo.BOOLEAN;
        }
        else
        {
            return Tipo.OBJECT;
        }
    }
}