
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class For : Instruccion
{
    private readonly Expresion condicion;
    private readonly Instruccion inicializador;
    private readonly Instruccion incrementador;
    private readonly List<Instruccion> lista_instrucciones;

    public For(Instruccion inicializador, Expresion condicion, Instruccion incrementador, List<Instruccion> lista_instrucciones)
    {
        this.condicion = condicion;
        this.inicializador = inicializador;
        this.incrementador = incrementador;
        this.lista_instrucciones = lista_instrucciones;
    }

    public object Ejecutar(Entorno ent)
    {
        Entorno xx = new Entorno(ent);

        inicializador.Ejecutar(xx);

        while ((bool)condicion.Ejecutar(xx))
        {
            Entorno local = new Entorno(xx);

            foreach (Instruccion ins in lista_instrucciones)
            {
                if (ins is Return || ins is Break || ins is Continue)
                {
                    return ins;
                }
                else
                {
                    ins.Ejecutar(local);
                }
            }

            incrementador.Ejecutar(xx);
        }

        return new Nulo();
    }
}