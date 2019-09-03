using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FuncionContains : Expresion
{
    public Expresion Elemento { get; set; }

    public FuncionContains(Expresion elemento)
    {
        Elemento = elemento;
    }

    public object Ejecutar(Entorno ent)
    {
        return Elemento.Ejecutar(ent);
    }

    public TipoDato GetTipo(Entorno ent)
    {
        object valor = Ejecutar(ent);

        if(valor is int)
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
        else if (valor is Map)
        {
            return new TipoDato(TipoDato.Tipo.MAP);
        }
        else if (valor is XSet)
        {
            return new TipoDato(TipoDato.Tipo.SET);
        }
        else if (valor is XList)
        {
            return new TipoDato(TipoDato.Tipo.LIST);
        }
        else
        {
            return new TipoDato(TipoDato.Tipo.OBJECT);
        }
    }
}