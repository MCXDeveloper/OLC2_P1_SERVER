using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Now : Expresion
{
    public object Ejecutar(Entorno ent)
    {
        return new Time(DateTime.Now.ToString("hh:mm:ss"));
    }

    public TipoDato GetTipo(Entorno ent)
    {
        return new TipoDato(TipoDato.Tipo.TIME);
    }
}