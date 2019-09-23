using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Identificador : Expresion
{
    public string Id { get; set; }

    public Identificador(string id)
    {
        Id = id;
    }

    public object Ejecutar(Entorno ent)
    {
        object objSim = ent.ObtenerVariable(Id);

        if (!(objSim is Nulo))
        {
            Variable sim = (Variable)objSim;
            return (sim.GetValor() is Nulo) ? sim : sim.GetValor();
        }

        return new Nulo();
    }

    public TipoDato GetTipo(Entorno ent)
    {
        object valor = Ejecutar(ent);

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
        else if (valor is Map)
        {
            return new TipoDato(TipoDato.Tipo.MAP);
        }
        else if (valor is XList)
        {
            return new TipoDato(TipoDato.Tipo.LIST);
        }
        else if (valor is XSet)
        {
            return new TipoDato(TipoDato.Tipo.SET);
        }
        else if (valor is Objeto)
        {
            return new TipoDato(TipoDato.Tipo.OBJECT);
        }
        else
        {
            return new TipoDato(TipoDato.Tipo.NULO);
        }

    }
}