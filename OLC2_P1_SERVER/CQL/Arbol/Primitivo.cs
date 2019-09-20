
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

    public TipoDato GetTipo(Entorno ent)
    {
        if (valor is int)
        {
            return new TipoDato(TipoDato.Tipo.INT);
        }
        else if (valor is double)
        {
            return new TipoDato(TipoDato.Tipo.DOUBLE);
        }
        else if (valor is string)
        {
            return new TipoDato(TipoDato.Tipo.STRING);
        }
        else if (valor is bool)
        {
            return new TipoDato(TipoDato.Tipo.BOOLEAN);
        }
        else if (valor is Date)
        {
            return new TipoDato(TipoDato.Tipo.DATE);
        }
        else if (valor is Time)
        {
            return new TipoDato(TipoDato.Tipo.TIME);
        }
        else if (valor is Nulo)
        {
            return new TipoDato(TipoDato.Tipo.NULO);
        }
        else
        {
            return new TipoDato(TipoDato.Tipo.DESCONOCIDO);
        }
    }
}