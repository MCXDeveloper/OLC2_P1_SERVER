using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Entorno
{

    public enum Tipo
    {
        INT,
        DOUBLE,
        STRING,
        BOOLEAN,
        DATE,
        TIME,
        MAP,
        SET,
        LIST,
        OBJECT
    }

    private readonly Entorno anterior;
    private readonly Hashtable tablaVariables;
    private readonly Hashtable tablaFunciones;
        
    public Entorno(Entorno anterior)
    {
        this.anterior = anterior;
        this.tablaVariables = new Hashtable();
        this.tablaFunciones = new Hashtable();
    }

    public void Agregar(string id, object simbolo)
    {
        if(simbolo is Variable)
        {
            this.tablaVariables.Add(id, (Variable)simbolo);
        }
        else
        {
            this.tablaFunciones.Add(id, (Funcion)simbolo);
        }
    }

    public object ObtenerVariable(string id)
    {
        for(Entorno e = this; e != null; e = e.anterior)
        {
            Variable encontrado = (Variable)(e.tablaVariables[id]);
            if(encontrado != null)
            {
                return encontrado;
            }
        }

        return new Nulo();
    }

    public object ObtenerFuncion(Tipo tipo, string id)
    {
        for (Entorno e = this; e != null; e = e.anterior)
        {
            Funcion encontrado = (Funcion)(e.tablaFunciones[id]);
            if (encontrado != null)
            {
                return encontrado;
            }
        }

        return new Nulo();
    }

    public void ReemplazarVariable(Tipo tipo, string id, Variable nuevoValor)
    {
        bool flag = false;

        for (Entorno e = this; e != null; e = e.anterior)
        {
            Variable encontrado = (Variable)(e.tablaVariables[id]);
            if (encontrado != null)
            {
                e.tablaVariables[id] = nuevoValor;
                flag = true;
            }
        }

        if(!flag)
        {
            Console.WriteLine("El simbolo '" + id + "' no ha sido declarado en el entorno actual ni en alguno externo.");
        }

    }

    public void ReemplazarFuncion(Tipo tipo, string id, Funcion nuevoValor)
    {
        bool flag = false;

        for (Entorno e = this; e != null; e = e.anterior)
        {
            Funcion encontrado = (Funcion)(e.tablaFunciones[id]);
            if (encontrado != null)
            {
                e.tablaFunciones[id] = nuevoValor;
                flag = true;
            }
        }

        if (!flag)
        {
            Console.WriteLine("El simbolo '" + id + "' no ha sido declarado en el entorno actual ni en alguno externo.");
        }

    }

}