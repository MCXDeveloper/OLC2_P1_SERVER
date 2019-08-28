using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Return : Instruccion
{
    private object exp_val;
    private Instruccion valor;

    public Return()
    {
        this.valor = new Nulo();
        this.exp_val = new Nulo();
    }

    public Return(Instruccion valor)
    {
        this.valor = valor;
        this.exp_val = new Nulo();
    }

    public object GetExpVal()
    {
        return exp_val;
    }

    public object Ejecutar(Entorno ent, AST arbol)
    {
        if(!(valor is Nulo))
        {
            this.exp_val = valor.Ejecutar(ent, arbol);
            return this;
        }
        else
        {
            return new Nulo();
        }
    }
}