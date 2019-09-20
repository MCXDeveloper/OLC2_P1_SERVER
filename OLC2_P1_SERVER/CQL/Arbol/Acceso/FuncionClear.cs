
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FuncionClear : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public object Padre { get; set; }

    public FuncionClear(int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
    }

    public object Ejecutar(Entorno ent)
    {
        // +----------------------------------------------------------------------------------------+
        // |                                          Nota                                          |
        // +----------------------------------------------------------------------------------------+
        // | Antes de comenzar con el vaciado de la Collection, en la clase donde se mande a llamar |
        // | el método Ejecutar de la clase FuncionClear se debe definir la variable Padre.         |
        // +----------------------------------------------------------------------------------------+

        if (Padre != null)
        {
            if (Padre is Map)
            {
                Map coleccion = (Map)Padre;
                coleccion.Clear();
            }
            else if (Padre is XList)
            {
                XList coleccion = (XList)Padre;
                coleccion.Clear();
            }
            else if (Padre is XSet)
            {
                XSet coleccion = (XSet)Padre;
                coleccion.Clear();
            }
            else
            {
                CQL.AddLUPError("Semántico", "[FUNCION_CLEAR]", "Error de acceso.  No se puede aplicar la función Clear a un elemento que no sea de tipo Collection.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[FUNCION_CLEAR]", "Error de jerarquía.  El valor al que se le desea aplicar la función Clear (Padre) no ha sido definido.", fila, columna);
        }

        return new Nulo();
    }

    public TipoDato GetTipo(Entorno ent)
    {
        return new TipoDato(TipoDato.Tipo.NULO);
    }
}