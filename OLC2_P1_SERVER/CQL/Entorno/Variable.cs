using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Entorno;

public class Variable
{
    private readonly Tipo tipo;
    private readonly object valor;
    private readonly string nombre;
    private readonly string nombreObj;

    public Variable(Tipo tipo, string nombre)
    {
        this.tipo = tipo;
        this.nombre = nombre;
        this.valor = new Nulo();
    }

    public Variable(Tipo tipo, string nombre, object valor)
    {
        this.tipo = tipo;
        this.valor = valor;
        this.nombre = nombre;
    }

    public Variable(string nombreObj, string nombre)
    {
        this.nombre = nombre;
        this.valor = new Nulo();
        this.nombreObj = nombreObj;
    }

    public Variable(string nombreObj, string nombre, object valor)
    {
        this.valor = valor;
        this.nombre = nombre;
        this.nombreObj = nombreObj;
    }

    public object GetValor()
    {
        return valor;
    }

}