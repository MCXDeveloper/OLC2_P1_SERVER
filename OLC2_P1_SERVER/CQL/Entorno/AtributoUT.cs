using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static TipoDato;

public class AtributoUT
{
    public TipoDato Tipo { get; set; }
    public string Identificador { get; set; }

    public AtributoUT(TipoDato tipo, string identificador)
    {
        Tipo = tipo;
        Identificador = identificador;
    }

    public override string ToString()
    {
        return "{ Tipo_Dato : "+ Tipo.GetRealTipo().ToString() +" | Elemento : "+ Identificador +"}";
    }
}