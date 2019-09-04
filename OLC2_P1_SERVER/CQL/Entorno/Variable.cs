using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Entorno;
using static TipoDato;

public class Variable
{
    public object Valor { get; set; }
    public string Nombre { get; set; }
    public TipoDato Tipo { get; set; }
    
    public Variable(TipoDato tipo, string nombre)
    {
        Tipo = tipo;
        Nombre = nombre;
        Valor = new Nulo();
    }

    public Variable(TipoDato tipo, string nombre, object valor)
    {
        Tipo = tipo;
        Valor = valor;
        Nombre = nombre;
    }

    public object GetValor()
    {
        return Valor;
    }

    public Tipo GetTipo()
    {
        return Tipo.GetRealTipo();
    }

    public string GetNombre()
    {
        return Nombre;
    }

}