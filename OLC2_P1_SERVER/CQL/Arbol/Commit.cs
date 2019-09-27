using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Commit : Instruccion
{
    private readonly int fila;
    private readonly int columna;

    public Commit(int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
    }

    public object Ejecutar(Entorno ent)
    {
        throw new NotImplementedException();
    }
}