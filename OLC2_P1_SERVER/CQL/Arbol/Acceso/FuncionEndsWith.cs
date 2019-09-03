using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FuncionEndsWith : Expresion
{
    public Expresion Parametro { get; set; }

    public FuncionEndsWith(Expresion parametro)
    {
        Parametro = parametro;
    }

    public object Ejecutar(Entorno ent)
    {
        return Parametro.Ejecutar(ent);
    }

    public TipoDato GetTipo(Entorno ent)
    {
        object valor = Ejecutar(ent);

        if(valor is string)
        {
            return new TipoDato(TipoDato.Tipo.STRING);
        }
        else
        {
            return new TipoDato(TipoDato.Tipo.DESCONOCIDO);
        }
    }
}