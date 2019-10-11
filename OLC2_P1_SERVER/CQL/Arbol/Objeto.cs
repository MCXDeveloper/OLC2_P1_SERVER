
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

    public string GetChisonRepresentation()
    {
        List<string> ValoresEnString = new List<string>();

        foreach (AtributoObjeto ao in ListaAtributosObjeto)
        {
            ValoresEnString.Add("\"" + ao.Nombre + "\" = " + GetChisonValue(ao.Valor));
        }

        return ValoresEnString.Count > 0 ? "<" + string.Join(", ", ValoresEnString) + ">" : "NULL";
    }

    private string GetChisonValue(object val)
    {
        if (val is string)
        {
            return "\"" + val.ToString() + "\"";
        }
        else if (val is Date)
        {
            return "'" + ((Date)val).Fecha + "'";
        }
        else if (val is Time)
        {
            return "'" + ((Time)val).Tiempo + "'";
        }
        else if (val is Map)
        {
            return ((Map)val).GetChisonRepresentation();
        }
        else if (val is XList)
        {
            return ((XList)val).GetChisonRepresentation();
        }
        else if (val is XSet)
        {
            return ((XSet)val).GetChisonRepresentation();
        }
        else if (val is Objeto)
        {
            return ((Objeto)val).GetChisonRepresentation();
        }
        else
        {
            return val.ToString();
        }
    }
}