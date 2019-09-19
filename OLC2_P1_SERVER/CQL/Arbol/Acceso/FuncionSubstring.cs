using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FuncionSubstring : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public object Padre { get; set; }
    public Expresion Posicion { get; set; }
    public Expresion Cantidad { get; set; }

    public FuncionSubstring(Expresion posicion, Expresion cantidad, int fila, int columna)
    {
        this.fila = fila;
        Posicion = posicion;
        Cantidad = cantidad;
        this.columna = columna;
    }

    public object Ejecutar(Entorno ent)
    {
        // +-----------------------------------------------------------------------------------------------------+
        // |                                                Nota                                                 |
        // +-----------------------------------------------------------------------------------------------------+
        // | Antes de comenzar con la obtención del substring de una cadena, en la clase donde se mande a llamar |
        // | el metodo Ejecutar de la clase FuncionSubstring se debe definir la variable Padre.                  |
        // +-----------------------------------------------------------------------------------------------------+

        if (Padre != null)
        {
            if (Padre is string)
            {
                object pos = Posicion.Ejecutar(ent);
                object cant = Cantidad.Ejecutar(ent);

                if (pos is int && cant is int)
                {
                    return ((string)Padre).Substring((int)pos, (int)cant);
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[FUNCION_SUBSTRING]", "Error de acceso.  La función 'Substring' necesita que sus dos parámetos (posicion y cantidad) sean de tipo entero.", fila, columna);
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[FUNCION_SUBSTRING]", "Error de acceso.  La función 'Substring' solo es válida cuando el elemento es de tipo String.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[FUNCION_SUBSTRING]", "Error de jerarquía.  El valor al que se le desea aplicar la función Substring (Padre) no ha sido definido.", fila, columna);
        }

        return new Nulo();
    }

    public TipoDato GetTipo(Entorno ent)
    {
        object valor = Ejecutar(ent);

        if (valor is string)
        {
            return new TipoDato(TipoDato.Tipo.STRING);
        }
        else
        {
            return new TipoDato(TipoDato.Tipo.NULO);
        }
    }
}