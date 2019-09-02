using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class TipoDato
{
    public enum Tipo
    {
        INT,
        MAP,
        SET,
        DATE,
        TIME,
        LIST,
        NULO,
        DOUBLE,
        STRING,
        OBJECT,
        BOOLEAN,
        DESCONOCIDO
    }

    private readonly Tipo tipo;
    private readonly object elemento;

    public TipoDato(Tipo tipo)
    {
        this.tipo = tipo;
    }

    public TipoDato(Tipo tipo, object elemento)
    {
        this.tipo = tipo;
        this.elemento = elemento;
    }

    public Tipo GetRealTipo()
    {
        return tipo;
    }

    public object GetElemento()
    {
        return elemento;
    }
}