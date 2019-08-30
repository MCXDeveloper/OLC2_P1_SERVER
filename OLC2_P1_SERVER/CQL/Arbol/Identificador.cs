using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Entorno;

public class Identificador : Expresion
{
    private readonly string id;

    public Identificador(string id)
    {
        this.id = id;
    }

    public object Ejecutar(Entorno ent)
    {
        Variable sim = (Variable)ent.ObtenerVariable(id);
        return (sim.GetValor() is Nulo) ? sim : sim.GetValor();
    }

    public Tipo GetTipo(Entorno ent)
    {
        object valor = Ejecutar(ent);

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
        // TODO corregir esta linea para manipular los objetos
        else if (valor is object)
        {
            return Tipo.OBJECT;
        }
        else
        {
            return Tipo.NULO;
        }

    }
}