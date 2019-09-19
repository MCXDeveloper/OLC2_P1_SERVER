using OLC2_P1_SERVER.CQL.Arbol;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CasteoExplicito : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public Expresion Valor { get; set; }
    public TipoDato TipoCasteo { get; set; }
    
    public CasteoExplicito(TipoDato tipo_casteo, Expresion valor, int fila, int columna)
    {
        Valor = valor;
        this.fila = fila;
        this.columna = columna;
        TipoCasteo = tipo_casteo;
    }

    public object Ejecutar(Entorno ent)
    {
        object result = new Nulo();
        TipoDato ValorType = Valor.GetTipo(ent);

        // Primero valido que el tipo de casteo sea int|double|time|date|string.  De lo contrario se muestra un error.
        if (ValidarTiposDeCasteo())
        {
            if (ValorType.GetRealTipo().Equals(TipoDato.Tipo.STRING))
            {
                result = CastString((string)Valor.Ejecutar(ent));
            }
            else if (ValorType.GetRealTipo().Equals(TipoDato.Tipo.INT))
            {
                result = CastInt((int)Valor.Ejecutar(ent));
            }
            else if (ValorType.GetRealTipo().Equals(TipoDato.Tipo.DOUBLE))
            {
                result = CastDouble((double)Valor.Ejecutar(ent));
            }
            else if (ValorType.GetRealTipo().Equals(TipoDato.Tipo.DATE))
            {
                result = CastDate((Date)Valor.Ejecutar(ent));
            }
            else if (ValorType.GetRealTipo().Equals(TipoDato.Tipo.TIME))
            {
                result = CastTime((Time)Valor.Ejecutar(ent));
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[CASTEO_EXPLICITO]", "Error de tipos.  El casteo solo puede hacerse entre tipos de datos primitivos.", fila, columna);
        }

        if(result is Nulo)
        {
            CQL.AddLUPError("Semántico", "[CASTEO_EXPLICITO]", "Error al intentar realizar casteo.  Se intentó castear un valor de tipo '"+ ValorType.GetRealTipo().ToString() +"' a un tipo '"+ TipoCasteo.GetRealTipo().ToString() + "', lo cual no está permitido.", fila, columna);
        }

        return result;
    }

    public TipoDato GetTipo(Entorno ent)
    {
        object valor = Ejecutar(ent);

        if(valor is int)
        {
            return new TipoDato(TipoDato.Tipo.INT);
        }
        else if (valor is double)
        {
            return new TipoDato(TipoDato.Tipo.DOUBLE);
        }
        else if (valor is string)
        {
            return new TipoDato(TipoDato.Tipo.STRING);
        }
        else if (valor is Date)
        {
            return new TipoDato(TipoDato.Tipo.DATE);
        }
        else if (valor is Time)
        {
            return new TipoDato(TipoDato.Tipo.TIME);
        }
        else
        {
            return new TipoDato(TipoDato.Tipo.DESCONOCIDO);
        }
    }

    private bool ValidarTiposDeCasteo()
    {
        return (
            (TipoCasteo.GetRealTipo().Equals(TipoDato.Tipo.INT))    ||
            (TipoCasteo.GetRealTipo().Equals(TipoDato.Tipo.DOUBLE)) ||
            (TipoCasteo.GetRealTipo().Equals(TipoDato.Tipo.TIME))   ||
            (TipoCasteo.GetRealTipo().Equals(TipoDato.Tipo.DATE))   ||
            (TipoCasteo.GetRealTipo().Equals(TipoDato.Tipo.STRING))
        );
    }

    private object CastString(string val)
    {
        if (TipoCasteo.GetRealTipo().Equals(TipoDato.Tipo.INT))
        {
            return Int32.Parse(val);
        }
        else if (TipoCasteo.GetRealTipo().Equals(TipoDato.Tipo.DOUBLE))
        {
            return double.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
        }
        else if (TipoCasteo.GetRealTipo().Equals(TipoDato.Tipo.DATE))
        {
            // Verifico que la fecha ingresada tenga el formato correcto
            DateTime dt;
            if (DateTime.TryParseExact(val, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return new Date(val);
            }
            else
            {
                CQL.AddLUPError("Semántico", "[CASTEO_EXPLICITO]", "Error de formato.  El valor proporcionado como fecha no cumple con el formato yyyy-MM-dd.", fila, columna);
            }
        }
        else if (TipoCasteo.GetRealTipo().Equals(TipoDato.Tipo.DATE))
        {
            // Verifico que la hora ingresada tenga el formato correcto
            DateTime outTime;
            if (DateTime.TryParseExact(val, "hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out outTime))
            {
                return new Time(val);
            }
            else
            {
                CQL.AddLUPError("Semántico", "[CASTEO_EXPLICITO]", "Error de formato.  El valor proporcionado como hora no cumple con el formato hh:mm:ss.", fila, columna);
            }
        }

        return new Nulo();
    }

    private object CastInt(int val)
    {
        if (TipoCasteo.GetRealTipo().Equals(TipoDato.Tipo.STRING))
        {
            return val.ToString();
        }

        return new Nulo();
    }

    private object CastDouble(double val)
    {
        if (TipoCasteo.GetRealTipo().Equals(TipoDato.Tipo.STRING))
        {
            return val.ToString();
        }

        return new Nulo();
    }

    private object CastDate(Date val)
    {
        if (TipoCasteo.GetRealTipo().Equals(TipoDato.Tipo.STRING))
        {
            return val.Fecha;
        }

        return new Nulo();
    }

    private object CastTime(Time val)
    {
        if (TipoCasteo.GetRealTipo().Equals(TipoDato.Tipo.STRING))
        {
            return val.Tiempo;
        }

        return new Nulo();
    }
}