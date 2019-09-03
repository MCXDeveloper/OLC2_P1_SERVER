using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FuncionSubstring : Expresion
{
    public Expresion Posicion { get; set; }
    public Expresion Cantidad { get; set; }

    public FuncionSubstring(Expresion posicion, Expresion cantidad)
    {
        Posicion = posicion;
        Cantidad = cantidad;
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