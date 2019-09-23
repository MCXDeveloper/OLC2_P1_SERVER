using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Asignacion;

public class AsignacionColumna
{
    public string NombreColumna { get; set; }
    public Expresion ValorColumna { get; set; }
    public TipoAsignacion TipoAsignacionColumna { get; set; }

    public AsignacionColumna(string nombre_columna, TipoAsignacion tipo_asignacion_columna, Expresion valor_columna)
    {
        ValorColumna = valor_columna;
        NombreColumna = nombre_columna;
        TipoAsignacionColumna = tipo_asignacion_columna;
    }
}