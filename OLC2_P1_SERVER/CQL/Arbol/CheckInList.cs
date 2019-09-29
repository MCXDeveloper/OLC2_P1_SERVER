using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CheckInList : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public bool GetTipoFlag { get; set; }
    public Expresion ValorIzq { get; set; }
    public Expresion ValorDer { get; set; }
    public List<Expresion> ListaExpresiones { get; set; }

    public CheckInList(Expresion valIzq, Expresion valDer, int fila, int columna)
    {
        this.fila = fila;
        ValorIzq = valIzq;
        ValorDer = valDer;
        this.columna = columna;
        ListaExpresiones = null;
    }

    public CheckInList(Expresion valIzq, List<Expresion> lista_expresiones, int fila, int columna)
    {
        ValorDer = null;
        this.fila = fila;
        ValorIzq = valIzq;
        this.columna = columna;
        ListaExpresiones = lista_expresiones;
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Ejecuto la expresión del lado izquierdo para obtener su valor.
        object objValIzq = ValorIzq.Ejecutar(ent);

        // 2. Verifico cual de los dos tipos de 'WHERE xxx IN yyy' se realizó.
        if (ListaExpresiones is null)
        {
            // 3. Si ListaExpresiones es null significa que se definio por medio de otra expresión.  Esa expresión tiene que ser un Collection.
            object objValDer = ValorDer.Ejecutar(ent);

            if (objValDer is Map)
            {
                return ((Map)objValDer).Contains(objValIzq);
            }
            else if (objValDer is XSet)
            {
                return ((XSet)objValDer).Contains(objValIzq);
            }
            else if (objValDer is XList)
            {
                return ((XList)objValDer).Contains(objValIzq);
            }
            else
            {
                CQL.AddLUPError("Semántico", "[CHECK_IN_LIST]", "Error. La expresión después de la instrucción 'IN' debe ser de tipo Collection.", fila, columna);
            }
        }
        else
        {
            // 3. Evalúo todas las expresiones contenidas en la lista y las agrego a una nueva y pregunto si contiene el valor buscado.
            List<object> ListaExpresionesEvaluadas = new List<object>();

            foreach (Expresion exp in ListaExpresiones) {
                ListaExpresionesEvaluadas.Add(exp.Ejecutar(ent));
            }

            return ListaExpresionesEvaluadas.Contains(objValIzq);
        }

        return new Nulo();
    }

    public TipoDato GetTipo(Entorno ent)
    {
        GetTipoFlag = true;
        object valor = Ejecutar(ent);

        if (valor is bool)
        {
            return new TipoDato(TipoDato.Tipo.BOOLEAN);
        }
        else
        {
            return new TipoDato(TipoDato.Tipo.NULO);
        }
    }
}