using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class SetType
{
    public TipoDato TipoDatoSet { get; set; }

    public SetType(TipoDato tipo_dato_set)
    {
        TipoDatoSet = tipo_dato_set;
    }
}