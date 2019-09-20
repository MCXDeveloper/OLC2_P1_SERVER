
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FuncionInsert : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public object Padre { get; set; }
    public Expresion Clave { get; set; }
    public Expresion Valor { get; set; }

    public FuncionInsert(Expresion valor, int fila, int columna)
    {
        Valor = valor;
        this.fila = fila;
        Clave = new Nulo();
        this.columna = columna;
    }

    public FuncionInsert(Expresion clave, Expresion valor, int fila, int columna)
    {
        Clave = clave;
        Valor = valor;
        this.fila = fila;
        this.columna = columna;
    }

    public object Ejecutar(Entorno ent)
    {
        // +--------------------------------------------------------------------------------------------+
        // |                                            Nota                                            |
        // +--------------------------------------------------------------------------------------------+
        // | Antes de comenzar con la inserción, en la clase donde se mande a llamar el metodo Ejecutar |
        // | de la clase FuncionInsert se debe definir la variable Padre.                               |
        // +--------------------------------------------------------------------------------------------+

        if (Padre != null)
        {
            if (Padre is Map)
            {
                if (!(Clave is Nulo))
                {
                    Map mapita = (Map)Padre;
                    mapita.Insert(Clave.Ejecutar(ent), Valor.Ejecutar(ent));
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[FUNCION_INSERT]", "Error de parámetros.  La función Insert aplicada a un Map necesita los parámetros clave-valor.", fila, columna);
                }
            }
            else if (Padre is XList)
            {
                XList listita = (XList)Padre;
                listita.Insert(Valor.Ejecutar(ent));
            }
            else if (Padre is XSet)
            {
                XSet setsito = (XSet)Padre;
                setsito.Insert(Valor.Ejecutar(ent));
            }
            else
            {
                CQL.AddLUPError("Semántico", "[FUNCION_INSERT]", "Error de acceso.  No se puede aplicar la función Insert a un elemento que no sea de tipo Collection.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[FUNCION_INSERT]", "Error de jerarquía.  El valor al que se le desea aplicar la función Insert (Padre) no ha sido definido.", fila, columna);
        }

        return new Nulo();
    }

    public TipoDato GetTipo(Entorno ent)
    {
        return new TipoDato(TipoDato.Tipo.NULO);
    }
}