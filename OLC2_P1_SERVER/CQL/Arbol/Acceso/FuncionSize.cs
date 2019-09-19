using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FuncionSize : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public object Padre { get; set; }

    public FuncionSize(int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
    }

    public object Ejecutar(Entorno ent)
    {
        // +----------------------------------------------------------------------------------------+
        // |                                          Nota                                          |
        // +----------------------------------------------------------------------------------------+
        // | Antes de comenzar con la obtención del tamaño, en la clase donde se mande a llamar     |
        // | el metodo Ejecutar de la clase FuncionSize se debe definir la variable Padre.          |
        // +----------------------------------------------------------------------------------------+

        if (Padre != null)
        {
            if (Padre is Map)
            {
                Map coleccion = (Map)Padre;
                return coleccion.Size();
            }
            else if (Padre is XList)
            {
                XList coleccion = (XList)Padre;
                return coleccion.Size();
            }
            else if (Padre is XSet)
            {
                XSet coleccion = (XSet)Padre;
                return coleccion.Size();
            }
            else
            {
                CQL.AddLUPError("Semántico", "[FUNCION_SIZE]", "Error de acceso.  No se puede aplicar la función Size a un elemento que no sea de tipo Collection.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[FUNCION_SIZE]", "Error de jerarquía.  El valor al que se le desea aplicar la función Size (Padre) no ha sido definido.", fila, columna);
        }

        return new Nulo();
    }

    public TipoDato GetTipo(Entorno ent)
    {
        object valor = Ejecutar(ent);

        if (valor is int)
        {
            return new TipoDato(TipoDato.Tipo.INT);
        }
        else
        {
            return new TipoDato(TipoDato.Tipo.NULO);
        }
    }
}