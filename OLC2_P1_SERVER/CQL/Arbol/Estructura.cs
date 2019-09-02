using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static TipoDato;

public class Estructura : Expresion
{
    private readonly TipoDato tipo_dato;

    /* 
     * Este constructor tiene las siguientes particularidades:
     * 1. Recibe como parámetro un object el cual puede ser de los siguientes tipos:
     *    - UserType
     *    - Map
     *    - List
     *    - Set
     * 2. Si viniese otro tipo, debería de ser une error ya que esta clase representa el ejecutar la sentencia "... = new UserType/Map/Set/List".
     */
    public Estructura(TipoDato tipo_dato)
    {
        this.tipo_dato = tipo_dato;
    }

    public object Ejecutar(Entorno ent)
    {
        return tipo_dato;
    }

    public TipoDato GetTipo(Entorno ent)
    {
        return tipo_dato;
    }
}