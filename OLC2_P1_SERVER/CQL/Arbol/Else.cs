using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Else : Instruccion
{
    private bool ValorCondicion;
    private readonly bool IsElse;
    private readonly Instruccion Condicion;
    private readonly List<Instruccion> Sentencias;

    public Else(List<Instruccion> sentencias)
    {
        this.IsElse = false;
        this.Condicion = new Nulo();
        this.Sentencias = sentencias;
    }

    public Else(Instruccion condicion, List<Instruccion> sentencias)
    {
        this.IsElse = false;
        this.Condicion = condicion;
        this.Sentencias = sentencias;
    }

    public object Ejecutar(Entorno ent)
    {
        ValorCondicion = (Condicion is Nulo) ? false : (bool)Condicion.Ejecutar(ent);

        if(ValorCondicion || IsElse)
        {
            Entorno local = new Entorno(ent);

            foreach(Instruccion ins in Sentencias)
            {
                if (ins is Return)
                {
                    return ins.Ejecutar(local);
                }
                else
                {
                    ins.Ejecutar(local);
                }
            }

        }

        return ValorCondicion;

    }
}