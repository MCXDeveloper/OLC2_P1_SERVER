using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FuncionStartsWith : Expresion
{
    public Expresion Parametro { get; set; }

    public FuncionStartsWith(Expresion parametro)
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

        if (valor is string)
        {
            return new TipoDato(TipoDato.Tipo.STRING);
        }
        else
        {
            return new TipoDato(TipoDato.Tipo.DESCONOCIDO);
        }
    }
}