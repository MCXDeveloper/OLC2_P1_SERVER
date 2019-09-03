using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Today : Expresion
{
    public object Ejecutar(Entorno ent)
    {
        return new Date(DateTime.Now.Date.ToString("yyyy-MM-dd"));
    }

    public TipoDato GetTipo(Entorno ent)
    {
        return new TipoDato(TipoDato.Tipo.DATE);
    }
}