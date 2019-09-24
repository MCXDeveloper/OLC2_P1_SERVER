using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Return : Instruccion
{
    public List<Expresion> ListaValores { get; set; }
    
    public Return()
    {
        ListaValores = new List<Expresion>();
    }

    public Return(List<Expresion> lista_valores)
    {
        ListaValores = lista_valores;
    }

    public int CantidadRetornos()
    {
        return ListaValores.Count;
    }

    public object Ejecutar(Entorno ent)
    {
        switch (CantidadRetornos())
        {
            case 0:
                return new Nulo();
            case 1:
                return (ListaValores[0] is Nulo) ? ListaValores[0] : ListaValores[0].Ejecutar(ent);
            default:
                return ObtenerRetornosEjecutados(ent);
        }        
    }

    public List<object> ObtenerRetornosEjecutados(Entorno ent)
    {
        List<object> ValoresRetorno = new List<object>();

        foreach (Expresion exp in ListaValores)
        {
            ValoresRetorno.Add(exp.Ejecutar(ent));
        }

        return ValoresRetorno;
    }
}