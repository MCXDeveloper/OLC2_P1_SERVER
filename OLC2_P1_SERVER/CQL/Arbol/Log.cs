
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Log : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public Expresion Elemento { get; set; }

    public Log(Expresion elemento, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        Elemento = elemento;
    }

    public object Ejecutar(Entorno ent)
    {
        object ob = Elemento.Ejecutar(ent);

        if (!(ob is Nulo))
        {
            string salida = String.Empty;

            if (ob is Date)
            {
                salida = ((Date)ob).Fecha;
            }
            else if (ob is Time)
            {
                salida = ((Time)ob).Tiempo;
            }
            else
            {
                salida = ob.ToString();
            }

            CQL.AddLUPMessage(salida);
        }
        else
        {
            CQL.AddLUPError("Semántico", "[LOG]", "Error.  Se intentó imprimir una expresión nula.", fila, columna);
        }

        return new Nulo();
    }
}