using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Batch : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public List<Instruccion> ListaSentenciasDML { get; set; }

    public Batch(List<Instruccion> lista_sentencias, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        ListaSentenciasDML = lista_sentencias;
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Valido que las instrucciones sean las permitidas, de lo contrario se arroja un error.
        if (ValidarInstruccionesDML())
        {
            // 2. Activo la bandera BatchFlag.  Su función es evitar que la instrucción se ejecute completamente ya que es necesario
            // verficar si existió algun error durante la validación de la instrucción.  Eso se puede determinar gracias al aumento
            // del contador BatchErrorCounter el cual llevará el control de errores recibidos al validar la instrucción.
            if (!ValidarSiInstruccionPresentaErrores(ent))
            {
                // 3. Si no hubo errores en la validación de las instrucciones del Batch, se procede a ejecutar.
                EjecutarBatch(ent);
                CQL.AddLUPMessage("Batch aplicado correctamente.");
            }
            else
            {
                string mensaje = "Error.  No se puede ejecutar el Batch ya alguna de las instrucciones internas presentan errores.";
                CQL.AddLUPError("Semántico", "[BATCH]", mensaje, fila, columna);
                if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'BatchException' no capturada.  " + mensaje); }
                return new BatchException(mensaje);
            }
        }
        else
        {
            string mensaje = "Error.  No se puede ejecutar el Batch ya que cuenta con instrucciones que no son de tipo DML.";
            CQL.AddLUPError("Semántico", "[BATCH]", mensaje, fila, columna);
            if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'BatchException' no capturada.  " + mensaje); }
            return new BatchException(mensaje);
        }

        return new Nulo();
    }

    private void EjecutarBatch(Entorno ent)
    {
        foreach (Instruccion ins in ListaSentenciasDML)
        {
            ins.Ejecutar(ent);
        }
    }

    private bool ValidarInstruccionesDML()
    {
        foreach (Instruccion ins in ListaSentenciasDML)
        {
            if (!(ins is InsertTable || ins is UpdateTable || ins is DeleteTable))
            {
                return false;
            }
        }

        return true;
    }

    private bool ValidarSiInstruccionPresentaErrores(Entorno ent)
    {
        CQL.BatchFlag = true;
        
        foreach (Instruccion ins in ListaSentenciasDML)
        {
            CQL.BatchErrorCounter = 0;
            ins.Ejecutar(ent);

            if (CQL.BatchErrorCounter > 0)
            {
                return true;
            }
        }

        CQL.BatchFlag = false;

        return false;
    }
}