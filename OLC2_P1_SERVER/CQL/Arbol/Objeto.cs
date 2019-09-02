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
}