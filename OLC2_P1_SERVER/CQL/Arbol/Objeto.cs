﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Objeto
{
    public TipoDato TipoDatoObjeto { get; set; }
    public List<AtributoObjeto> ListaAtributosObjeto { get; set; }

    public Objeto(TipoDato tipo_dato_objeto, List<AtributoObjeto> lista_atributos_objeto)
    {
        TipoDatoObjeto = tipo_dato_objeto;
        ListaAtributosObjeto = lista_atributos_objeto;
    }

    public object GetAtributo(bool onlyValue, string NombreAtributo)
    {
        AtributoObjeto result = ListaAtributosObjeto.Find(x => x.Nombre.Equals(NombreAtributo));
        return (result is null) ? new Nulo() : (onlyValue ? result.Valor : result);
    }

    public override string ToString()
    {
        string objString = "{ ";
        List<string> listAtr = new List<string>();

        foreach (AtributoObjeto ao in ListaAtributosObjeto)
        {
            listAtr.Add("\""+ ao.Nombre +"\" : " + (ao.Valor is string ? "\""+ ao.Valor.ToString() +"\"" : ao.Valor.ToString()));
        }

        objString += string.Join(", ", listAtr);
        objString += " }";

        return objString;
    }
}