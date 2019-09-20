
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FuncionToUpperCase : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public object Padre { get; set; }

    public FuncionToUpperCase(int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
    }

    public object Ejecutar(Entorno ent)
    {
        // +---------------------------------------------------------------------------------------------------+
        // |                                               Nota                                                |
        // +---------------------------------------------------------------------------------------------------+
        // | Antes de comenzar con la obtención de la cadena en mayúscula, en la clase donde se mande a llamar |
        // | el metodo Ejecutar de la clase FuncitonToUpperCase se debe definir la variable Padre.             |
        // +---------------------------------------------------------------------------------------------------+

        if (Padre != null)
        {
            if (Padre is string)
            {
                return ((string)Padre).ToUpper();
            }
            else
            {
                CQL.AddLUPError("Semántico", "[FUNCION_TO_UPPER_CASE]", "Error de acceso.  La función 'ToUpperCase' solo es válida cuando el elemento es de tipo String.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[FUNCION_TO_UPPER_CASE]", "Error de jerarquía.  El valor al que se le desea aplicar la función ToUpperCase (Padre) no ha sido definido.", fila, columna);
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