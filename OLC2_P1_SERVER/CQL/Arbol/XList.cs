﻿using System;
using System.Web;
using System.Linq;
using OLC2_P1_SERVER.CQL.Arbol;
using System.Collections.Generic;

public class XList
{
    private readonly int fila;
    private readonly int columna;
    public TipoDato TipoDatoList { get; set; }
    public List<object> ListaElementos { get; set; }

    public XList(TipoDato tipo_dato_list, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        TipoDatoList = tipo_dato_list;
        ListaElementos = new List<object>();
    }

    public bool Insert(object valor)
    {
        if (ValidarTipoElemento(valor))
        {
            ListaElementos.Add(valor);
            return true;
        }
        else
        {
            Error.AgregarError("Semántico", "[LIST]", "Error de tipos.  El tipo del elemento a insertar no corresponde con el tipo con el que fue declarado el objeto LIST (Recibido: " + valor.GetType().FullName + " | Declarado: " + TipoDatoList.GetRealTipo().ToString() + ")", fila, columna);
        }

        return false;
    }

    public object Get(int posicion)
    {
        if (ListaElementos.ElementAtOrDefault(posicion) != null)
        {
            return ListaElementos[posicion];
        }
        else
        {
            Error.AgregarError("Semántico", "[LIST]", "No se puede obtener un elemento en una posición inexistente.", fila, columna);
        }

        return new Nulo();
    }

    public bool Set(int posicion, object valor)
    {
        if (ListaElementos.ElementAtOrDefault(posicion) != null)
        {
            if (ValidarTipoElemento(valor))
            {
                ListaElementos[posicion] = valor;
                return true;
            }
            else
            {
                Error.AgregarError("Semántico", "[LIST]", "Error de tipos.  El tipo del elemento a insertar no corresponde con el tipo con el que fue declarado el objeto LIST (Recibido: " + valor.GetType().FullName + " | Declarado: " + TipoDatoList.GetRealTipo().ToString() + ")", fila, columna);
            }
        }
        else
        {
            Error.AgregarError("Semántico", "[LIST]", "No se puede actualizar un elemento en una posición inexistente.", fila, columna);
        }

        return false;
    }

    public bool Remove(int posicion)
    {
        if(ListaElementos.ElementAtOrDefault(posicion) != null)
        {
            ListaElementos.RemoveAt(posicion);
            return true;
        }
        else
        {
            Error.AgregarError("Semántico", "[LIST]", "No se puede eliminar un elemento en una posición inexistente.", fila, columna);
        }

        return false;
    }

    public int Size()
    {
        return ListaElementos.Count;
    }

    public void Clear()
    {
        ListaElementos.Clear();
    }

    public bool Contains(object elemento)
    {
        return ListaElementos.Contains(elemento);
    }

    private bool ValidarTipoElemento(object elemento)
    {
        if (TipoDatoList.Equals(TipoDato.Tipo.INT))
        {
            return (elemento is int);
        }
        else if (TipoDatoList.Equals(TipoDato.Tipo.DOUBLE))
        {
            return (elemento is double);
        }
        else if (TipoDatoList.Equals(TipoDato.Tipo.STRING))
        {
            return (elemento is string);
        }
        else if (TipoDatoList.Equals(TipoDato.Tipo.BOOLEAN))
        {
            return (elemento is bool);
        }
        else if (TipoDatoList.Equals(TipoDato.Tipo.DATE))
        {
            return (elemento is Date);
        }
        else if (TipoDatoList.Equals(TipoDato.Tipo.TIME))
        {
            return (elemento is Time);
        }
        else if (TipoDatoList.Equals(TipoDato.Tipo.MAP))
        {
            //TODO | XList | validar los tipos internos del valor cuando es MAP
            return (elemento is Map);
        }
        else if (TipoDatoList.Equals(TipoDato.Tipo.SET))
        {
            //TODO | XList | validar los tipos internos del valor cuando es SET
            return (elemento is XSet);
        }
        else if (TipoDatoList.Equals(TipoDato.Tipo.LIST))
        {
            //TODO | XList | validar los tipos internos del valor cuando es LIST
            return (elemento is XList);
        }
        else if (TipoDatoList.Equals(TipoDato.Tipo.OBJECT))
        {
            //TODO | XList | validar los tipos internos del valor cuando es OBJECT
            return (elemento is Objeto);
        }

        return false;
    }
}