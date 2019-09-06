using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Parametro
{
    public string NombreParametro { get; set; }
    public TipoDato TipoDatoParametro { get; set; }
    
    public Parametro(TipoDato tipo_dato_parametro, string nombre_parametro)
    {
        NombreParametro = nombre_parametro;
        TipoDatoParametro = tipo_dato_parametro;
    }
}