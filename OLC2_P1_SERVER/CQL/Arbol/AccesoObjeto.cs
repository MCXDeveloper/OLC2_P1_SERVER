using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class AccesoObjeto : Expresion
{
    private readonly string variable;
    private readonly List<string> lista_atributos;

    public AccesoObjeto(string variable, List<string> lista_atributos)
    {
        this.variable = variable;
        this.lista_atributos = lista_atributos;
    }

    public object Ejecutar(Entorno ent)
    {
        throw new NotImplementedException();
    }

    public Entorno.Tipo GetTipo(Entorno ent)
    {
        throw new NotImplementedException();
    }
}