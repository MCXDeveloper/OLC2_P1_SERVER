using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ListType
{
    public TipoDato TipoDatoList { get; set; }

    public ListType(TipoDato tipo_dato_list)
    {
        TipoDatoList = tipo_dato_list;
    }
}