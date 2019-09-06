using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class Columna : DataColumn
{
    public bool IsPK { get; set; }
    public string NombreColumna { get; set; }
    public TipoDato TipoDatoColumna { get; set; }

    public Columna(bool is_pk, string nombre_columna, TipoDato tipo_dato)
    {
        IsPK = is_pk;
        TipoDatoColumna = tipo_dato;
        NombreColumna = nombre_columna;
    }
}