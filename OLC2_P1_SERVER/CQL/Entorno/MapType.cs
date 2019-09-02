using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class MapType
{
    public TipoDato TipoIzq { get; set; }
    public TipoDato TipoDer { get; set; }

    public MapType(TipoDato tipoIzq, TipoDato tipoDer)
    {
        TipoIzq = tipoIzq;
        TipoDer = tipoDer;
    }

    public bool ValidarTipoPrimitivoEnClave()
    {
        return (
            TipoIzq.GetRealTipo().Equals(TipoDato.Tipo.INT)     ||
            TipoIzq.GetRealTipo().Equals(TipoDato.Tipo.DOUBLE)  ||
            TipoIzq.GetRealTipo().Equals(TipoDato.Tipo.STRING)  ||
            TipoIzq.GetRealTipo().Equals(TipoDato.Tipo.BOOLEAN) ||
            TipoIzq.GetRealTipo().Equals(TipoDato.Tipo.DOUBLE)  ||
            TipoIzq.GetRealTipo().Equals(TipoDato.Tipo.TIME)
        );
    }

}