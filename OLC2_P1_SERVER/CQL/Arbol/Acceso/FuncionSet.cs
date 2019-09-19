using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FuncionSet : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public object Padre { get; set; }
    public Expresion Clave { get; set; }
    public Expresion Valor { get; set; }

    public FuncionSet(Expresion clave, Expresion valor, int fila, int columna)
    {
        Clave = clave;
        Valor = valor;
        this.fila = fila;
        this.columna = columna;
    }

    public object Ejecutar(Entorno ent)
    {
        // +----------------------------------------------------------------------------------------------------+
        // |                                                Nota                                                |
        // +----------------------------------------------------------------------------------------------------+
        // | Antes de comenzar con el seteado de un valor en un Collection, en la clase donde se mande a llamar |
        // | el método Ejecutar de la clase FuncionSet se debe definir la variable Padre.                       |
        // +----------------------------------------------------------------------------------------------------+

        if (Padre != null)
        {
            if (Padre is Map)
            {
                Map coleccion = (Map)Padre;
                coleccion.Set(Clave.Ejecutar(ent), Valor.Ejecutar(ent));
            }
            else if (Padre is XList)
            {
                XList coleccion = (XList)Padre;
                object posicion = Clave.Ejecutar(ent);

                if (posicion is int)
                {
                    coleccion.Set((int)posicion, Valor.Ejecutar(ent));
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[FUNCION_SET]", "Error en acceso.  La función Set para una colección de tipo List debe recibir un valor de tipo entero para la posición.", fila, columna);
                }
            }
            else if (Padre is XSet)
            {
                XSet coleccion = (XSet)Padre;
                object posicion = Clave.Ejecutar(ent);

                if (posicion is int)
                {
                    coleccion.Set((int)posicion, Valor.Ejecutar(ent));
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[FUNCION_SET]", "Error en acceso.  La función Set para una colección de tipo Set debe recibir un valor de tipo entero para la posición.", fila, columna);
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[FUNCION_SET]", "Error de acceso.  No se puede aplicar la función Set a un elemento que no sea de tipo Collection.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[FUNCION_SET]", "Error de jerarquía.  El valor al que se le desea aplicar la función Set (Padre) no ha sido definido.", fila, columna);
        }

        return new Nulo();
    }

    public TipoDato GetTipo(Entorno ent)
    {
        return new TipoDato(TipoDato.Tipo.NULO);
    }
}