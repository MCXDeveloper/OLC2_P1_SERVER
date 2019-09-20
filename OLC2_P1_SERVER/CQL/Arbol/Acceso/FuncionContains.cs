
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FuncionContains : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public object Padre { get; set; }
    public Expresion Elemento { get; set; }

    public FuncionContains(Expresion elemento, int fila, int columna)
    {
        this.fila = fila;
        Elemento = elemento;
        this.columna = columna;
    }

    public object Ejecutar(Entorno ent)
    {
        // +--------------------------------------------------------------------------------------------------+
        // |                                               Nota                                               |
        // +--------------------------------------------------------------------------------------------------+
        // | Antes de comenzar con la verificación de si existe el valor, en la clase donde se mande a llamar |
        // | el método Ejecutar de la clase FuncionContains se debe definir la variable Padre.                |
        // +--------------------------------------------------------------------------------------------------+

        if (Padre != null)
        {
            if (Padre is Map)
            {
                Map coleccion = (Map)Padre;
                return coleccion.Contains(Elemento.Ejecutar(ent));
            }
            else if (Padre is XList)
            {
                XList coleccion = (XList)Padre;
                return coleccion.Contains(Elemento.Ejecutar(ent));
            }
            else if (Padre is XSet)
            {
                XSet coleccion = (XSet)Padre;
                return coleccion.Contains(Elemento.Ejecutar(ent));
            }
            else
            {
                CQL.AddLUPError("Semántico", "[FUNCION_CONTAINS]", "Error en acceso.  No se puede aplicar la función Contains a un elemento que no es de tipo Collection.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[FUNCION_CONTAINS]", "Error de jerarquía.  El valor al que se le desea aplicar la función Contains (Padre) no ha sido definido.", fila, columna);
        }

        return new Nulo();
    }

    public TipoDato GetTipo(Entorno ent)
    {
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