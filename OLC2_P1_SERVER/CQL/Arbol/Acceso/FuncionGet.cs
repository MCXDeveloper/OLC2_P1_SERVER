using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FuncionGet : Expresion
{
    public Expresion Posicion { get; set; }

    public FuncionGet(Expresion posicion)
    {
        Posicion = posicion;
    }

    public object Ejecutar(Entorno ent)
    {
        return Posicion.Ejecutar(ent);
    }

    public TipoDato GetTipo(Entorno ent)
    {
        object valor = Ejecutar(ent);

        if (valor is int)
        {
            return new TipoDato(TipoDato.Tipo.INT);
        }
        else
        {
            return new TipoDato(TipoDato.Tipo.DESCONOCIDO);
        }
    }
}