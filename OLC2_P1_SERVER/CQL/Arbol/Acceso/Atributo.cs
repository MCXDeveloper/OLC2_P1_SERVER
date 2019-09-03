using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Atributo : Expresion
{
    public string NombreAtributo { get; set; }

    public Atributo(string nombre_atributo)
    {
        NombreAtributo = nombre_atributo;
    }

    public object Ejecutar(Entorno ent)
    {
        return NombreAtributo;
    }

    public TipoDato GetTipo(Entorno ent)
    {
        return new TipoDato(TipoDato.Tipo.STRING);
    }
}