using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class AtributoObjeto
{
    public object Valor { get; set; }
    public string Nombre { get; set; }
    public TipoDato Tipo { get; set; }

    public AtributoObjeto(TipoDato tipo, string nombre, object valor)
    {
        Tipo = tipo;
        Valor = valor;
        Nombre = nombre;
    }
}