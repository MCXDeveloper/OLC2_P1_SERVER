using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Map
{
    private readonly object tipoIzq;
    private readonly object tipoDer;

    public Map(object tipoIzq, object tipoDer)
    {
        this.tipoIzq = tipoIzq;
        this.tipoDer = tipoDer;
    }
}