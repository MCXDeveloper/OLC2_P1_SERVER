using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class MapType
{
    private readonly TipoDato tipoIzq;
    private readonly TipoDato tipoDer;

    public MapType(TipoDato tipoIzq, TipoDato tipoDer)
    {
        this.tipoIzq = tipoIzq;
        this.tipoDer = tipoDer;
    }
}