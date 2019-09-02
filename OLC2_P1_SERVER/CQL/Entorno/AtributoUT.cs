using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class AtributoUT
{
    public TipoDato Tipo { get; set; }
    public string Identificador { get; set; }

    public AtributoUT(TipoDato tipo, string identificador)
    {
        Tipo = tipo;
        Identificador = identificador;
    }

    public TipoDato.Tipo GetTipoAtributo()
    {
        return Tipo.GetRealTipo();
    }
}