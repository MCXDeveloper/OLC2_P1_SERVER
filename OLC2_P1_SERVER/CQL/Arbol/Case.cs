using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Case : Instruccion
{
    private readonly Instruccion condicion;
    private readonly LinkedList<Instruccion> sentencias;

    public Case(Instruccion condicion, LinkedList<Instruccion> sentencias)
    {
        this.condicion = condicion;
        this.sentencias = sentencias;
    }

    public Instruccion GetCondicion()
    {
        return condicion;
    }

    public object Ejecutar(Entorno ent, AST arbol)
    {
        // Si un caso se esta ejecutando, es porque la condicion de acá y la expresión del selecciona hicieron match.
        // Por lo que se procede a ejecutar cada una de las sentencias este caso.
        // Si encuentra un detener, devuelve el detener.  Si encuentra un retorno, devuelve el Retorno.

        Entorno local = new Entorno(ent);

        foreach(Instruccion ins in sentencias)
        {
            if(ins is Return)
            {
                return ((Return)ins).Ejecutar(local, arbol);
            }
            else if (ins is Break)
            {
                return (Break)ins;
            }
            else
            {
                ins.Ejecutar(local, arbol);
            }
        }

        return new Nulo();

    }
}