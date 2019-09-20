using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Entorno
{
    public Entorno Anterior { get; private set; }
    public Hashtable TablaVariables { get; private set; }
    public Hashtable TablaFunciones { get; private set; }

    public Entorno(Entorno anterior)
    {
        Anterior = anterior;
        TablaVariables = new Hashtable();
        TablaFunciones = new Hashtable();
    }

    public void Agregar(string id, object simbolo)
    {
        if(simbolo is Variable)
        {
            TablaVariables.Add(id, (Variable)simbolo);
        }
        else
        {
            TablaFunciones.Add(id, (Funcion)simbolo);
        }
    }

    public object ObtenerVariable(string id)
    {
        for(Entorno e = this; e != null; e = e.Anterior)
        {
            Variable encontrado = (Variable)(e.TablaVariables[id]);
            if(encontrado != null)
            {
                return encontrado;
            }
        }

        return new Nulo();
    }

    public object ObtenerFuncion(string id)
    {
        for (Entorno e = this; e != null; e = e.Anterior)
        {
            Funcion encontrado = (Funcion)(e.TablaFunciones[id]);
            if (encontrado != null)
            {
                return encontrado;
            }
        }

        return new Nulo();
    }

    public void ReemplazarVariable(string id, Variable nuevoValor)
    {
        bool flag = false;

        for (Entorno e = this; e != null; e = e.Anterior)
        {
            Variable encontrado = (Variable)(e.TablaVariables[id]);
            if (encontrado != null)
            {
                e.TablaVariables[id] = nuevoValor;
                flag = true;
            }
        }

        if(!flag)
        {
            CQL.AddLUPMessage("El simbolo '" + id + "' no ha sido declarado en el entorno actual ni en alguno externo.");
        }

    }

}