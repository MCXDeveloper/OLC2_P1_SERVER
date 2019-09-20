
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FuncionStartsWith : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public object Padre { get; set; }
    public Expresion Parametro { get; set; }

    public FuncionStartsWith(Expresion parametro, int fila, int columna)
    {
        this.fila = fila;
        Parametro = parametro;
        this.columna = columna;
    }

    public object Ejecutar(Entorno ent)
    {
        // +---------------------------------------------------------------------------------------------------+
        // |                                               Nota                                                |
        // +---------------------------------------------------------------------------------------------------+
        // | Antes de comenzar con la obtención de si la cadena comienza, en la clase donde se mande a llamar  |
        // | el metodo Ejecutar de la clase FuncionStartsWith se debe definir la variable Padre.               |
        // +---------------------------------------------------------------------------------------------------+

        if (Padre != null)
        {
            if (Padre is string)
            {
                object cadena = Parametro.Ejecutar(ent);

                if (cadena is string)
                {
                    return ((string)Padre).StartsWith((string)cadena);
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[FUNCION_STARTS_WITH]", "Error de acceso.  La función 'StartsWith' necesita que su parámeto sea de tipo String.", fila, columna);
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[FUNCION_STARTS_WITH]", "Error de acceso.  La función 'StartsWith' solo es válida cuando el elemento es de tipo String.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[FUNCION_STARTS_WITH]", "Error de jerarquía.  El valor al que se le desea aplicar la función StartsWith (Padre) no ha sido definido.", fila, columna);
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
            return new TipoDato(TipoDato.Tipo.DESCONOCIDO);
        }
    }
}