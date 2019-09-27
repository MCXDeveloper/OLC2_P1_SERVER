using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class RollbackError
{
    public int Fila { get; set; }
    public int Columna { get; set; }
    public string TipoError { get; set; }
    public string Ubicacion { get; set; }
    public string Descripcion { get; set; }

    public RollbackError(string tipo, string ubicacion, string descripcion, int fila, int columna)
    {
        Fila = fila;
        TipoError = tipo;
        Columna = columna;
        Ubicacion = ubicacion;
        Descripcion = descripcion;
    }
}