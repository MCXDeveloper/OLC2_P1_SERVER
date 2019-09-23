using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class AccionCursor : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public string NombreCursor { get; set; }
    public TipoAccionCursor TipoAccion { get; set; }

    public AccionCursor(string nombre_cursor, TipoAccionCursor accion, int fila, int columna)
    {
        this.fila = fila;
        TipoAccion = accion;
        this.columna = columna;
        NombreCursor = nombre_cursor;
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Valido que el nombre de variable del cursor proporcionado exista en el entorno.
        object simbolo = ent.ObtenerVariable(NombreCursor);

        if (!(simbolo is Nulo))
        {
            Variable sim = (Variable)simbolo;

            // 2. Verifico que la variable si sea de tipo CURSOR para poder aplicar su acción.
            if (sim.Tipo.GetRealTipo().Equals(TipoDato.Tipo.CURSOR))
            {
                // 3. Actualizo el valor de su acción.
                sim.Tipo = new TipoDato(TipoDato.Tipo.CURSOR, TipoAccion);
            }
            else
            {
                CQL.AddLUPError("Semántico", "[ACCION_CURSOR]", "Error. No se puede hacer la acción '" + TipoAccion.ToString() + "' a '" + NombreCursor + "'.  La variable no es de tipo CURSOR.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ACCION_CURSOR]", "Error. No se puede hacer la acción '"+ TipoAccion.ToString() +"' a '"+ NombreCursor +"'.  La variable no existe en el entorno.", fila, columna);
        }

        return new Nulo();
    }
}