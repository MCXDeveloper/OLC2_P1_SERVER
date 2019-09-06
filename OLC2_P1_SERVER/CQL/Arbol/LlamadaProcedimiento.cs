using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class LlamadaProcedimiento : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public string NombreProcedimiento { get; set; }
    public List<Expresion> ListaExpresiones { get; set; }

    public LlamadaProcedimiento(string nombre_procedimiento, List<Expresion> lista_expresiones, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        ListaExpresiones = lista_expresiones;
        NombreProcedimiento = nombre_procedimiento;
    }

    public TipoDato GetTipo(Entorno ent)
    {
        // TODO | LlamadaProcedimiento | Realizar funcion de obtener tipo de dato de retorno de procedimiento.
        throw new NotImplementedException();
    }

    public object Ejecutar(Entorno ent)
    {
        // TODO | LlamadaProcedimiento | Realizar funcion de ejecucion de procedimiento.
        throw new NotImplementedException();
    }
}