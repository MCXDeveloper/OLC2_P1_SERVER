﻿using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class For : Instruccion
{
    private readonly Instruccion condicion;
    private readonly Instruccion inicializador;
    private readonly Instruccion incrementador;
    private readonly List<Instruccion> lista_instrucciones;

    public For(Instruccion inicializador, Instruccion condicion, Instruccion incrementador, List<Instruccion> lista_instrucciones)
    {
        this.condicion = condicion;
        this.inicializador = inicializador;
        this.incrementador = incrementador;
        this.lista_instrucciones = lista_instrucciones;
    }

    public object Ejecutar(Entorno ent)
    {
        inicializador.Ejecutar(ent);

        while ((bool)condicion.Ejecutar(ent))
        {
            incrementador.Ejecutar(ent);

            Entorno local = new Entorno(ent);

            foreach (Instruccion ins in lista_instrucciones)
            {
                ins.Ejecutar(ent);
            }

        }

        return new Nulo();
    }
}