
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class OperadorTernario : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public Expresion Condicion { get; set; }
    public Expresion CondicionFalsa { get; set; }
    public Expresion CondicionVerdadera { get; set; }

    public OperadorTernario(Expresion condicion, Expresion condicion_verdadera, Expresion condicion_falsa, int fila, int columna)
    {
        this.fila = fila;
        Condicion = condicion;
        this.columna = columna;
        CondicionFalsa = condicion_falsa;
        CondicionVerdadera = condicion_verdadera;
    }

    public object Ejecutar(Entorno ent)
    {
        object response = new Nulo();
        object exec = Condicion.Ejecutar(ent);

        if(exec is bool)
        {
            response = ((bool)exec) ? CondicionVerdadera.Ejecutar(ent) : CondicionFalsa.Ejecutar(ent);
        }
        else
        {
            CQL.AddLUPError("Semántico", "[OPERADOR_TERNARIO]", "En la operacion ternaria se espera a que el resultado de la condición sea un valor booleano.", fila, columna);
        }

        return response;
    }

    public TipoDato GetTipo(Entorno ent)
    {
        object valor = Ejecutar(ent);

        if (valor is int)
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
        else if (valor is bool)
        {
            return new TipoDato(TipoDato.Tipo.BOOLEAN);
        }
        else if (valor is Date)
        {
            return new TipoDato(TipoDato.Tipo.DATE);
        }
        else if (valor is Time)
        {
            return new TipoDato(TipoDato.Tipo.TIME);
        }
        else if (valor is Map)
        {
            return new TipoDato(TipoDato.Tipo.MAP);
        }
        else if (valor is XList)
        {
            return new TipoDato(TipoDato.Tipo.LIST);
        }
        else if (valor is XSet)
        {
            return new TipoDato(TipoDato.Tipo.SET);
        }
        else
        {
            return new TipoDato(TipoDato.Tipo.OBJECT);
        }
    }
}