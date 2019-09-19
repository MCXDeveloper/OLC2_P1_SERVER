using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FuncionGet : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public object Padre { get; set; }
    public Expresion Posicion { get; set; }

    public FuncionGet(Expresion posicion, int fila, int columna)
    {
        this.fila = fila;
        Posicion = posicion;
        this.columna = columna;
    }

    public object Ejecutar(Entorno ent)
    {
        // +-----------------------------------------------------------------------------------+
        // |                                       Nota                                        |
        // +-----------------------------------------------------------------------------------+
        // | Antes de comenzar con la obtención del valor, en la clase donde se mande a llamar |
        // | el metodo Ejecutar de la clase FuncionGet se debe definir la variable Padre.      |
        // +-----------------------------------------------------------------------------------+

        if (Padre != null)
        {
            if (Padre is Map)
            {
                Map coleccion = (Map)Padre;
                return coleccion.Get(Posicion.Ejecutar(ent));
            }
            else if (Padre is XList)
            {
                XList coleccion = (XList)Padre;
                object pos = Posicion.Ejecutar(ent);

                if (pos is int)
                {
                    return coleccion.Get((int)pos);
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[FUNCION_GET]", "Error en acceso.  La función Get para una colección de tipo List debe ser de tipo entero.", fila, columna);
                }
            }
            else if (Padre is XSet)
            {
                XSet coleccion = (XSet)Padre;
                object pos = Posicion.Ejecutar(ent);

                if (pos is int)
                {
                    return coleccion.Get((int)pos);
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[FUNCION_GET]", "Error en acceso.  La función Get para una colección de tipo Set debe ser de tipo entero.", fila, columna);
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[FUNCION_GET]", "Error de acceso.  No se puede aplicar la función Get a un elemento que no sea de tipo Collection.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[FUNCION_GET]", "Error de jerarquía.  El valor al que se le desea aplicar la función Get (Padre) no ha sido definido.", fila, columna);
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
        else if (valor is double)
        {
            return new TipoDato(TipoDato.Tipo.DOUBLE);
        }
        else if (valor is string)
        {
            return new TipoDato(TipoDato.Tipo.STRING);
        }
        else if (valor is bool)
        {
            return new TipoDato(TipoDato.Tipo.BOOLEAN);
        }
        else if (valor is Date)
        {
            return new TipoDato(TipoDato.Tipo.DATE);
        }
        else if (valor is Time)
        {
            return new TipoDato(TipoDato.Tipo.TIME);
        }
        else if (valor is Objeto)
        {
            return new TipoDato(TipoDato.Tipo.OBJECT);
        }
        else if (valor is Map)
        {
            return new TipoDato(TipoDato.Tipo.MAP);
        }
        else if (valor is XSet)
        {
            return new TipoDato(TipoDato.Tipo.SET);
        }
        else if (valor is XList)
        {
            return new TipoDato(TipoDato.Tipo.LIST);
        }
        else
        {
            return new TipoDato(TipoDato.Tipo.NULO);
        }
    }
}