using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class AccesoCollection : Expresion
{
    public Expresion Posicion { get; set; }
    public string NombreColumna { get; set; }

    public AccesoCollection(string nombre_columna, Expresion posicion)
    {
        Posicion = posicion;
        NombreColumna = nombre_columna;
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