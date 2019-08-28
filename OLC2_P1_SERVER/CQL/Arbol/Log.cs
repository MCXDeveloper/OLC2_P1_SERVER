using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Log : Instruccion
{
    
    private readonly int fila;
    private readonly int columna;
    private readonly Instruccion elemento;

    public Log(Instruccion elemento, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        this.elemento = elemento;
    }

    public object Ejecutar(Entorno ent, AST arbol)
    {
        object ob = elemento.Ejecutar(ent, arbol);

        string salida = "Error <<<>>> Se intentó imprimir una expresión nula.";

        if((ob != null))
        {
            if (!(ob is Nulo))
            {
                salida = ob.ToString();
            }
        }

        /* Esta linea debe de ser reemplazada por un paquete de LUP de tipo MESSAGE */
        Console.WriteLine(salida);

        return new Nulo();
        
    }
}