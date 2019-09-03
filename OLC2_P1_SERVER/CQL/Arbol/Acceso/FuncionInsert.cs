using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FuncionInsert : Expresion
{
    public Expresion Clave { get; set; }
    public Expresion Valor { get; set; }

    public FuncionInsert(Expresion valor)
    {
        Valor = valor;
        Clave = new Nulo();
    }

    public FuncionInsert(Expresion clave, Expresion valor)
    {
        Clave = clave;
        Valor = valor;
    }

    public object Ejecutar(Entorno ent)
    {
        throw new NotImplementedException();
    }

    public TipoDato GetTipo(Entorno ent)
    {
        throw new NotImplementedException();
    }
}