using System;
using System.Web;
using System.Linq;
using Newtonsoft.Json;
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
            string mensaje = "No se puede obtener un elemento en una posición inexistente.";
            CQL.AddLUPError("Semántico", "[SET]", mensaje, fila, columna);
            if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'IndexOutException' no capturada.  " + mensaje); }
            return new IndexOutException(mensaje);
        }
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

    public object Remove(int posicion)
    {
        if (ListaElementos.ElementAtOrDefault(posicion) != null)
        {
            ListaElementos.RemoveAt(posicion);
            SortList();
            return true;
        }
        else
        {
            if (!CQL.BatchFlag)
            {
                string mensaje = "No se puede eliminar un elemento en una posición inexistente.";
                CQL.AddLUPError("Semántico", "[SET]", mensaje, fila, columna);
                if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'IndexOutException' no capturada.  " + mensaje); }
                return new IndexOutException(mensaje);
            }
            else
            {
                CQL.BatchErrorCounter++;
            }
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
        if (elemento is Date || elemento is Time || elemento is Map || elemento is XList || elemento is XSet || elemento is Objeto)
        {
            foreach (object obj in ListaElementos)
            {
                if (JsonConvert.SerializeObject(obj).Equals(JsonConvert.SerializeObject(elemento), StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
        else
        {
            return ListaElementos.Contains(elemento);
        }
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
            return (elemento is Map);
        }
        else if (TipoDatoSet.GetRealTipo().Equals(TipoDato.Tipo.SET))
        {
            return (elemento is XSet);
        }
        else if (TipoDatoSet.GetRealTipo().Equals(TipoDato.Tipo.LIST))
        {
            return (elemento is XList);
        }
        else if (TipoDatoSet.GetRealTipo().Equals(TipoDato.Tipo.OBJECT))
        {
            return (elemento is Objeto);
        }

        return false;
    }

    private void SortList()
    {
        object pivote = ListaElementos[0];

        if (pivote is int || pivote is double || pivote is string || pivote is bool)
        {
            ListaElementos.Sort();
        }
        else if (ListaElementos[0] is Date)
        {
            List<Date> dl = ListaElementos.ConvertAll(x => (Date)x);
            List<DateTime> dtl = dl.ConvertAll(x => x.GetParsedDate());
            dtl.Sort();
            List<Date> dlConverted = dtl.ConvertAll(x => new Date(x.Date.ToString("yyyy-MM-dd")));
            ListaElementos = dlConverted.ConvertAll(x => (object)x);
        }
        else if (ListaElementos[0] is Time)
        {
            List<Time> dl = ListaElementos.ConvertAll(x => (Time)x);
            List<DateTime> dtl = dl.ConvertAll(x => x.GetTimeInDateTime());
            dtl.Sort();
            List<Time> dlConverted = dtl.ConvertAll(x => new Time(x.TimeOfDay.ToString(@"hh\:mm\:ss")));
            ListaElementos = dlConverted.ConvertAll(x => (object)x);
        }
    }

    public override string ToString()
    {
        if (ListaElementos != null)
        {
            List<string> ValoresEnString = ListaElementos.ConvertAll(x => x.ToString());
            return "{ " + string.Join(", ", ValoresEnString) + " }";
        }

        return "{ }";
    }
}