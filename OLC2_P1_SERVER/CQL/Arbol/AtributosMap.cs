using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class AtributosMap
{
    private readonly Expresion key;
    private readonly Expresion value;

    public AtributosMap(Expresion key, Expresion value)
    {
        this.key = key;
        this.value = value;
    }
}