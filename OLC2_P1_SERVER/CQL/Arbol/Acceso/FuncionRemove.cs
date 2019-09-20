
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FuncionRemove : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public object Padre { get; set; }
    public Expresion Elemento { get; set; }

    public FuncionRemove(Expresion elemento, int fila, int columna)
    {
        this.fila = fila;
        Elemento = elemento;
        this.columna = columna;
    }

    public object Ejecutar(Entorno ent)
    {
        // +-------------------------------------------------------------------------------------------------------+
        // |                                                 Nota                                                  |
        // +-------------------------------------------------------------------------------------------------------+
        // | Antes de comenzar con el eliminado de un valor de una Collection, en la clase donde se mande a llamar |
        // | el método Ejecutar de la clase FuncionRemove se debe definir la variable Padre.                       |
        // +-------------------------------------------------------------------------------------------------------+

        if (Padre != null)
        {
            if (Padre is Map)
            {
                Map coleccion = (Map)Padre;
                coleccion.Remove(Elemento.Ejecutar(ent));
            }
            else if (Padre is XList)
            {
                XList coleccion = (XList)Padre;
                object posicion = Elemento.Ejecutar(ent);

                if (posicion is int)
                {
                    coleccion.Remove((int)posicion);
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[FUNCION_REMOVE]", "Error en acceso.  La función Remove para una colección de tipo List debe ser de tipo entero.", fila, columna);
                }
            }
            else if (Padre is XSet)
            {
                XSet coleccion = (XSet)Padre;
                object posicion = Elemento.Ejecutar(ent);

                if (posicion is int)
                {
                    coleccion.Remove((int)posicion);
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[FUNCION_REMOVE]", "Error en acceso.  La función Remove para una colección de tipo Set debe ser de tipo entero.", fila, columna);
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[FUNCION_REMOVE]", "Error de acceso.  No se puede aplicar la función Remove a un elemento que no sea de tipo Collection.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[FUNCION_REMOVE]", "Error de jerarquía.  El valor al que se le desea aplicar la función Remove (Padre) no ha sido definido.", fila, columna);
        }

        return new Nulo();
    }

    public TipoDato GetTipo(Entorno ent)
    {
        return new TipoDato(TipoDato.Tipo.NULO);
    }
}