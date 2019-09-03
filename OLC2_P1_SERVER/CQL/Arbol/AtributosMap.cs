using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class AtributosMap
{
    public Expresion Key { get; set; }
    public Expresion Value { get; set; }
    
    public AtributosMap(Expresion key, Expresion value)
    {
        Key = key;
        Value = value;
    }
}