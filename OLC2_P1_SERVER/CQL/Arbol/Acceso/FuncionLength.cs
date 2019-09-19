using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FuncionLength : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public object Padre { get; set; }

    public FuncionLength(int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
    }

    public object Ejecutar(Entorno ent)
    {
        // +----------------------------------------------------------------------------------------+
        // |                                          Nota                                          |
        // +----------------------------------------------------------------------------------------+
        // | Antes de comenzar con la obtención del largo, en la clase donde se mande a llamar      |
        // | el metodo Ejecutar de la clase FuncionLength se debe definir la variable Padre.        |
        // +----------------------------------------------------------------------------------------+

        if (Padre != null)
        {
            if (Padre is string)
            {
                return ((string)Padre).Length;
            }
            else
            {
                CQL.AddLUPError("Semántico", "[FUNCION_LENGTH]", "Error de acceso.  La función 'Length' solo es válida cuando el elemento es de tipo String.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[FUNCION_LENGTH]", "Error de jerarquía.  El valor al que se le desea aplicar la función Length (Padre) no ha sido definido.", fila, columna);
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