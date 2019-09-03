using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FuncionSet : Expresion
{
    public Expresion Clave { get; set; }
    public Expresion Valor { get; set; }

    public FuncionSet(Expresion clave, Expresion valor)
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