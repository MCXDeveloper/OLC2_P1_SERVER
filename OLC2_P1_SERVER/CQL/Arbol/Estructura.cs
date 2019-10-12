using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static TipoDato;

public class Estructura : Expresion
{
    public TipoDato TipoDatoEstructura { get; set; }
    
    public Estructura(TipoDato tipo_dato)
    {
        TipoDatoEstructura = tipo_dato;
    }

    public object Ejecutar(Entorno ent)
    {
        if (TipoDatoEstructura.GetRealTipo().Equals(Tipo.MAP))
        {
            if (TipoDatoEstructura.GetElemento() is MapType)
            {
                MapType mt = (MapType)TipoDatoEstructura.GetElemento();
                return new Map(mt.TipoIzq, mt.TipoDer, 0, 0);
            }
            else
            {
                return TipoDatoEstructura;
            }
        }
        else
        {
            return TipoDatoEstructura;
        }
    }

    public TipoDato GetTipo(Entorno ent)
    {
        return TipoDatoEstructura;
    }
    
    // Esta funcion se encarga de validar que la estructura sea únicamente de tipo UserType/Map/List/Set.
    // En caso fuera otro tipo de dato, es error.

    public bool ValidateStructType()
    {
        if(TipoDatoEstructura.GetRealTipo().Equals(TipoDato.Tipo.OBJECT) || TipoDatoEstructura.GetRealTipo().Equals(TipoDato.Tipo.MAP) || TipoDatoEstructura.GetRealTipo().Equals(TipoDato.Tipo.SET) || TipoDatoEstructura.GetRealTipo().Equals(TipoDato.Tipo.LIST))
        {
            return true;
        }

        return false;
    }
}