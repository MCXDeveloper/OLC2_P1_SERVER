using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public static class TipoDatoExtensions
{
    public static string ToFriendlyString(TipoDato td)
    {
        if (td.GetRealTipo().Equals(TipoDato.Tipo.INT))
        {
            return "Int";
        }
        else if (td.GetRealTipo().Equals(TipoDato.Tipo.DOUBLE))
        {
            return "Double";
        }
        else if (td.GetRealTipo().Equals(TipoDato.Tipo.STRING))
        {
            return "String";
        }
        else if (td.GetRealTipo().Equals(TipoDato.Tipo.BOOLEAN))
        {
            return "Boolean";
        }
        else if (td.GetRealTipo().Equals(TipoDato.Tipo.DATE))
        {
            return "Date";
        }
        else if (td.GetRealTipo().Equals(TipoDato.Tipo.TIME))
        {
            return "Time";
        }
        else if (td.GetRealTipo().Equals(TipoDato.Tipo.CURSOR))
        {
            return "Cursor";
        }
        else if (td.GetRealTipo().Equals(TipoDato.Tipo.MAP))
        {
            if (td.GetElemento() is MapType)
            {
                return "Map<" + ToFriendlyString(((MapType)td.GetElemento()).TipoIzq) + ", " + ToFriendlyString(((MapType)td.GetElemento()).TipoDer) + ">";
            }
            else
            {
                return "Map";
            }
        }
        else if (td.GetRealTipo().Equals(TipoDato.Tipo.LIST))
        {
            if (td.GetElemento() is ListType)
            {
                return "List<" + ToFriendlyString(((ListType)td.GetElemento()).TipoDatoList) + ">";
            }
            else
            {
                return "List";
            }
        }
        else if (td.GetRealTipo().Equals(TipoDato.Tipo.SET))
        {
            if (td.GetElemento() is SetType)
            {
                return "Set<" + ToFriendlyString(((SetType)td.GetElemento()).TipoDatoSet) + ">";
            }
            else
            {
                return "Set";
            }
        }
        else
        {
            return (string)td.GetElemento();
        }
    }
}