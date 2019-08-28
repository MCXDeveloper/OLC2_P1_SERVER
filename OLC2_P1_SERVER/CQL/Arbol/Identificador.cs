using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Identificador : Instruccion
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
}