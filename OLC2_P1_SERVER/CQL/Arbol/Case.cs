using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Case : Instruccion
{
    private readonly Expresion condicion;
    private readonly List<Instruccion> sentencias;

    public Case(Expresion condicion, List<Instruccion> sentencias)
    {
        this.condicion = condicion;
        this.sentencias = sentencias;
    }

    public Expresion GetCondicion()
    {
        return condicion;
    }

    public object Ejecutar(Entorno ent)
    {
        // Si un caso se esta ejecutando, es porque la condicion de acá y la expresión del selecciona hicieron match.
        // Por lo que se procede a ejecutar cada una de las sentencias este caso.
        // Si encuentra un detener, devuelve el detener.  Si encuentra un retorno, devuelve el Retorno.

        Entorno local = new Entorno(ent);

        foreach(Instruccion ins in sentencias)
        {
            if(ins is Return)
            {
                return (Return)ins.Ejecutar(local);
            }
            else if (ins is Break)
            {
                return (Break)ins;
            }
            else
            {
                ins.Ejecutar(local);
            }
        }

        return new Nulo();

    }
}