using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class DeclaracionCursor : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public string VariableCursor { get; set; }
    public Select InstruccionSelect { get; set; }

    public DeclaracionCursor(string variable, Select ins_select, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        VariableCursor = variable;
        InstruccionSelect = ins_select;
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Primero valido que la variable en la cual se va a guardar el cursor no exista en el entorno.
        object simbolo = ent.ObtenerVariable(VariableCursor);

        if (simbolo is Nulo)
        {
            // 2. Ejecuto la instrucción SELECT e interpreto su salida ya que esperamos un Table.
            object execSelect = InstruccionSelect.Ejecutar(ent);

            if (execSelect is Nulo || execSelect is Exception)
            {
                return execSelect;
            }
            else
            {
                // 3. Procedo a almacenar el Table en el entorno.  El tipo de dato de la variable será de tipo CURSOR y su Elemento corresponderá al estado del Cursor (OPEN or CLOSE).
                ent.Agregar(VariableCursor, new Variable(new TipoDato(TipoDato.Tipo.CURSOR, TipoAccionCursor.CLOSE), VariableCursor, execSelect));
            }
        }
        else
        {
            string mensaje = "Se intento declarar '" + VariableCursor + "'.  Una variable con el mismo nombre ya se encuentra en el entorno actual.";
            CQL.AddLUPError("Semántico", "[ACCION_CURSOR]", mensaje, fila, columna);
            if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'ObjectAlreadyExists' no capturada.  " + mensaje); }
            return new ObjectAlreadyExists(mensaje);
        }

        return new Nulo();
    }
}