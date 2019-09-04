using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Funcion
{
    public string NombreFuncion { get; set; }
    public TipoDato TipoDatoFuncion { get; set; }
    public List<Instruccion> ListaInstrucciones { get; set; }
    public List<ParametroFuncion> ListaParametros { get; set; }

    public Funcion(TipoDato tipo_dato_funcion, string nombre_funcion, List<ParametroFuncion> lista_parametros, List<Instruccion> lista_instrucciones)
    {
        NombreFuncion = nombre_funcion;
        ListaParametros = lista_parametros;
        TipoDatoFuncion = tipo_dato_funcion;
        ListaInstrucciones = lista_instrucciones;
    }
}