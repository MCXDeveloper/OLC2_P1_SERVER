using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class TError
{
    private readonly int fila;
    private readonly int columna;
    private readonly string tipo;
    private readonly string ubicacion;
    private readonly string descripcion;

    public TError(string tipo, string ubicacion, string descripcion, int fila, int columna)
    {
        this.fila = fila;
        this.tipo = tipo;
        this.columna = columna;
        this.ubicacion = ubicacion;
        this.descripcion = descripcion;
    }

    public string GetGeneratedError()
    {
        return "Error " + tipo + " <<<>>> Ubicación: " + ubicacion + " <<<>>> " + descripcion + " <<<>>> Fila: " + fila + " | Columna: " + columna;
    }

}