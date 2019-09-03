﻿using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Map
{
    private readonly int fila;
    private readonly int columna;
    public TipoDato TipoDatoClave { get; set; }
    public TipoDato TipoDatoValor { get; set; }
    public Dictionary<object, object> ListaElementos { get; set; }

    public Map(TipoDato tipo_dato_clave, TipoDato tipo_dato_valor, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        TipoDatoClave = tipo_dato_clave;
        TipoDatoValor = tipo_dato_valor;
        ListaElementos = new Dictionary<object, object>();
    }

    public bool Insert(object clave, object valor)
    {
        // Primero valido que el tipo de dato de la clave coincida con el tipo de dato del parámetro clave.
        if(ValidarTipoDatoClave(clave))
        {
            ListaElementos.Add(clave, valor);
        }
        else
        {
            Error.AgregarError("Semántico", "[MAP]", "Error de tipos.  El tipo de la clave no corresponde con el tipo con el que fue declarado el objeto MAP (Recibido: "+ clave.GetType().FullName +" | Declarado: "+ TipoDatoClave.GetRealTipo().ToString() +")", fila, columna);
        }

        return false;
    }

    public object Get(object clave)
    {
        if (ValidarTipoDatoClave(clave))
        {
            if (Contains(clave))
            {
                return ListaElementos[clave];
            }
            else
            {
                Error.AgregarError("Semántico", "[MAP]", "No se pudo obtener la clave especificada ya que no existe en la colección.", fila, columna);
            }
        }
        else
        {
            Error.AgregarError("Semántico", "[MAP]", "Error de tipos.  El tipo de la clave no corresponde con el tipo con el que fue declarado el objeto MAP (Recibido: " + clave.GetType().FullName + " | Declarado: " + TipoDatoClave.GetRealTipo().ToString() + ")", fila, columna);
        }

        return new Nulo();
    }

    public bool Set(object clave, object valor)
    {
        if (ValidarTipoDatoClave(clave))
        {
            if (Contains(clave))
            {
                // TODO Validar el tipo de dato del atributo valor en SET
                ListaElementos[clave] = valor;
                return true;
            }
            else
            {
                Error.AgregarError("Semántico", "[MAP]", "No se pudo actualizar el valor de la clave ya que no existe en la colección.", fila, columna);
            }
        }
        else
        {
            Error.AgregarError("Semántico", "[MAP]", "Error de tipos.  El tipo de la clave no corresponde con el tipo con el que fue declarado el objeto MAP (Recibido: " + clave.GetType().FullName + " | Declarado: " + TipoDatoClave.GetRealTipo().ToString() + ")", fila, columna);
        }

        return false;
    }

    public bool Remove(object clave)
    {
        if (ValidarTipoDatoClave(clave))
        {
            if (Contains(clave))
            {
                ListaElementos.Remove(clave);
                return true;
            }
            else
            {
                Error.AgregarError("Semántico", "[MAP]", "No se pudo remover la clave especificada ya que no existe en la colección.", fila, columna);
            }
        }
        else
        {
            Error.AgregarError("Semántico", "[MAP]", "Error de tipos.  El tipo de la clave no corresponde con el tipo con el que fue declarado el objeto MAP (Recibido: " + clave.GetType().FullName + " | Declarado: " + TipoDatoClave.GetRealTipo().ToString() + ")", fila, columna);
        }

        return false;
    }

    public int Size()
    {
        return ListaElementos.Count();
    }

    public void Clear()
    {
        ListaElementos.Clear();
    }

    public bool Contains(object clave)
    {
        if (ValidarTipoDatoClave(clave))
        {
            return ListaElementos.ContainsKey(clave);
        }
        else
        {
            Error.AgregarError("Semántico", "[MAP]", "Error de tipos.  El tipo de la clave no corresponde con el tipo con el que fue declarado el objeto MAP (Recibido: " + clave.GetType().FullName + " | Declarado: " + TipoDatoClave.GetRealTipo().ToString() + ")", fila, columna);
        }

        return false;
    }

    private bool ValidarTipoDatoClave(object key)
    {
        if(TipoDatoClave.Equals(TipoDato.Tipo.INT))
        {
            return (key is int);
        }
        else if (TipoDatoClave.Equals(TipoDato.Tipo.DOUBLE))
        {
            return (key is double);
        }
        else if (TipoDatoClave.Equals(TipoDato.Tipo.STRING))
        {
            return (key is string);
        }
        else if (TipoDatoClave.Equals(TipoDato.Tipo.BOOLEAN))
        {
            return (key is bool);
        }
        else if (TipoDatoClave.Equals(TipoDato.Tipo.DATE))
        {
            return (key is Date);
        }
        else if (TipoDatoClave.Equals(TipoDato.Tipo.TIME))
        {
            return (key is Time);
        }

        return false;
    }

    private bool ValidarTipoDatoValor(object val)
    {
        if (TipoDatoValor.Equals(TipoDato.Tipo.INT))
        {
            return (val is int);
        }
        else if (TipoDatoValor.Equals(TipoDato.Tipo.DOUBLE))
        {
            return (val is double);
        }
        else if (TipoDatoValor.Equals(TipoDato.Tipo.STRING))
        {
            return (val is string);
        }
        else if (TipoDatoValor.Equals(TipoDato.Tipo.BOOLEAN))
        {
            return (val is bool);
        }
        else if (TipoDatoValor.Equals(TipoDato.Tipo.DATE))
        {
            return (val is Date);
        }
        else if (TipoDatoValor.Equals(TipoDato.Tipo.TIME))
        {
            return (val is Time);
        }
        else if (TipoDatoValor.Equals(TipoDato.Tipo.MAP))
        {
            //TODO validar los tipos internos del valor cuando es MAP
            return (val is Map);
        }
        else if (TipoDatoValor.Equals(TipoDato.Tipo.SET))
        {
            //TODO validar los tipos internos del valor cuando es SET
            return (val is XSet);
        }
        else if (TipoDatoValor.Equals(TipoDato.Tipo.LIST))
        {
            //TODO validar los tipos internos del valor cuando es LIST
            return (val is XList);
        }
        else if (TipoDatoValor.Equals(TipoDato.Tipo.OBJECT))
        {
            //TODO validar los tipos internos del valor cuando es OBJECT
            return (val is Objeto);
        }

        return false;
    }
}