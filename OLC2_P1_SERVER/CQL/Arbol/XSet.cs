using System;
using System.Web;
using System.Linq;

using System.Collections.Generic;

public class XSet
{
    private readonly int fila;
    private readonly int columna;
    public TipoDato TipoDatoSet { get; set; }
    public List<object> ListaElementos { get; set; }

    public XSet(TipoDato tipo_dato_set, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        TipoDatoSet = tipo_dato_set;
        ListaElementos = new List<object>();
    }

    public bool Insert(object valor)
    {
        if (ValidarTipoElemento(valor))
        {
            if (!Contains(valor))
            {
                ListaElementos.Add(valor);
                SortList();
                return true;
            }
            else
            {
                CQL.AddLUPError("Semántico", "[SET]", "Error de colección. No es permitido insertar valores repetidos en una colección de tipo SET.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[SET]", "Error de tipos.  El tipo del elemento a insertar no corresponde con el tipo con el que fue declarado el objeto SET (Recibido: " + valor.GetType().FullName + " | Declarado: " + TipoDatoSet.GetRealTipo().ToString() + ")", fila, columna);
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
            CQL.AddLUPError("Semántico", "[SET]", "No se puede obtener un elemento en una posición inexistente.", fila, columna);
        }

        return new Nulo();
    }

    public bool Set(int posicion, object valor)
    {
        if (ListaElementos.ElementAtOrDefault(posicion) != null)
        {
            if (ValidarTipoElemento(valor))
            {
                if(!Contains(valor))
                {
                    ListaElementos[posicion] = valor;
                    SortList();
                    return true;
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[SET]", "Error de colección. No es permitido insertar valores repetidos en una colección de tipo SET.", fila, columna);
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[SET]", "Error de tipos.  El tipo del elemento a insertar no corresponde con el tipo con el que fue declarado el objeto LIST (Recibido: " + valor.GetType().FullName + " | Declarado: " + TipoDatoSet.GetRealTipo().ToString() + ")", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[SET]", "No se puede actualizar un elemento en una posición inexistente.", fila, columna);
        }

        return false;
    }

    public bool Remove(int posicion)
    {
        if (ListaElementos.ElementAtOrDefault(posicion) != null)
        {
            ListaElementos.RemoveAt(posicion);
            SortList();
            return true;
        }
        else
        {
            CQL.AddLUPError("Semántico", "[SET]", "No se puede eliminar un elemento en una posición inexistente.", fila, columna);
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
        if (TipoDatoSet.GetRealTipo().Equals(TipoDato.Tipo.INT))
        {
            return (elemento is int);
        }
        else if (TipoDatoSet.GetRealTipo().Equals(TipoDato.Tipo.DOUBLE))
        {
            return (elemento is double);
        }
        else if (TipoDatoSet.GetRealTipo().Equals(TipoDato.Tipo.STRING))
        {
            return (elemento is string);
        }
        else if (TipoDatoSet.GetRealTipo().Equals(TipoDato.Tipo.BOOLEAN))
        {
            return (elemento is bool);
        }
        else if (TipoDatoSet.GetRealTipo().Equals(TipoDato.Tipo.DATE))
        {
            return (elemento is Date);
        }
        else if (TipoDatoSet.GetRealTipo().Equals(TipoDato.Tipo.TIME))
        {
            return (elemento is Time);
        }
        else if (TipoDatoSet.GetRealTipo().Equals(TipoDato.Tipo.MAP))
        {
            //TODO | XSet | validar los tipos internos del valor cuando es MAP
            return (elemento is Map);
        }
        else if (TipoDatoSet.GetRealTipo().Equals(TipoDato.Tipo.SET))
        {
            //TODO | XSet | validar los tipos internos del valor cuando es SET
            return (elemento is XSet);
        }
        else if (TipoDatoSet.GetRealTipo().Equals(TipoDato.Tipo.LIST))
        {
            //TODO | XSet | validar los tipos internos del valor cuando es LIST
            return (elemento is XList);
        }
        else if (TipoDatoSet.GetRealTipo().Equals(TipoDato.Tipo.OBJECT))
        {
            //TODO | XSet | validar los tipos internos del valor cuando es OBJECT
            return (elemento is Objeto);
        }

        return false;
    }

    private void SortList()
    {
        ListaElementos.Sort();
    }
}