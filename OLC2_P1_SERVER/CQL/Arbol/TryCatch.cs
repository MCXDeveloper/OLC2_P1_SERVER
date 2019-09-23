using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class TryCatch : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public string ObjetoExcepcion { get; set; }
    public TipoExcepcion TipoDeExcepcion { get; set; }
    public List<Instruccion> ListaInstruccionesTry { get; set; }
    public List<Instruccion> ListaInstruccionesCatch { get; set; }

    public TryCatch(List<Instruccion> lista_try, TipoExcepcion tipo_excepcion, string objeto_excepcion, List<Instruccion> lista_catch, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        TipoDeExcepcion = tipo_excepcion;
        ListaInstruccionesTry = lista_try;
        ObjetoExcepcion = objeto_excepcion;
        ListaInstruccionesCatch = lista_catch;
    }

    public object Ejecutar(Entorno ent)
    {
        CQL.TryCatchFlag = true;
        
        // 1. Primero evalúo el tipo de excepción que se desea arrojar ya que en base a eso se va a definir el catch del try-catch.
        switch (TipoDeExcepcion)
        {
            case TipoExcepcion.ARITHMETIC_EXCEPTION:
                try { return EjecutarInstruccionesTry(ent); } catch (ArithmeticException ex) { return EjecutarInstruccionesCatch(ex.Message, ent); }
            case TipoExcepcion.TYPE_ALREADY_EXISTS:
                try { return EjecutarInstruccionesTry(ent); } catch (TypeAlreadyExists ex) { return EjecutarInstruccionesCatch(ex.Message, ent); }
            case TipoExcepcion.BD_ALREADY_EXISTS:
                try { return EjecutarInstruccionesTry(ent); } catch (BDAlreadyExists ex) { return EjecutarInstruccionesCatch(ex.Message, ent); }
            case TipoExcepcion.BD_DONT_EXISTS:
                try { return EjecutarInstruccionesTry(ent); } catch (BDDontExists ex) { return EjecutarInstruccionesCatch(ex.Message, ent); }
            case TipoExcepcion.USE_DB_EXCEPTION:
                try { return EjecutarInstruccionesTry(ent); } catch (UseBDException ex) { return EjecutarInstruccionesCatch(ex.Message, ent); }
            case TipoExcepcion.TABLE_ALREADY_EXISTS:
                try { return EjecutarInstruccionesTry(ent); } catch (TableAlreadyExists ex) { return EjecutarInstruccionesCatch(ex.Message, ent); }
            case TipoExcepcion.TABLE_DONT_EXISTS:
                try { return EjecutarInstruccionesTry(ent); } catch (TableDontExists ex) { return EjecutarInstruccionesCatch(ex.Message, ent); }
            case TipoExcepcion.COUNTER_TYPE_EXCEPTION:
                try { return EjecutarInstruccionesTry(ent); } catch (CounterTypeException ex) { return EjecutarInstruccionesCatch(ex.Message, ent); }
            case TipoExcepcion.USER_ALREADY_EXISTS:
                try { return EjecutarInstruccionesTry(ent); } catch (UserAlreadyExists ex) { return EjecutarInstruccionesCatch(ex.Message, ent); }
            case TipoExcepcion.USER_DONT_EXISTS:
                try { return EjecutarInstruccionesTry(ent); } catch (UserDontExists ex) { return EjecutarInstruccionesCatch(ex.Message, ent); }
            case TipoExcepcion.VALUES_EXCEPTION:
                try { return EjecutarInstruccionesTry(ent); } catch (ValuesException ex) { return EjecutarInstruccionesCatch(ex.Message, ent); }
            case TipoExcepcion.COLUMN_EXCEPTION:
                try { return EjecutarInstruccionesTry(ent); } catch (ColumnException ex) { return EjecutarInstruccionesCatch(ex.Message, ent); }
            case TipoExcepcion.INDEX_OUT_EXCEPTION:
                try { return EjecutarInstruccionesTry(ent); } catch (IndexOutException ex) { return EjecutarInstruccionesCatch(ex.Message, ent); }
        }

        CQL.TryCatchFlag = false;

        return new Nulo();
    }

    private object EjecutarInstruccionesTry(Entorno ent)
    {
        // 1. Creo un nuevo entorno local para almacenar las variables correspondientes a la ejecución del Try.
        Entorno local = new Entorno(ent);

        // 2. Ejecuto todas las instrucciones dentro de la ListaInstruccionesTry.
        foreach (Instruccion ins in ListaInstruccionesTry)
        {
            object resp = ins.Ejecutar(local);

            if (resp is Return || resp is Break || resp is Continue)
            {
                return resp;
            }
            else if (resp is Exception)
            {
                throw (Exception)resp;
            }
        }

        return new Nulo();
    }

    private object EjecutarInstruccionesCatch(string exMessage, Entorno ent)
    {
        // 1. Creo un nuevo entorno local para almacenar las variables correspondientes a la ejecución del Try.
        Entorno local = new Entorno(ent);

        // 2. Agrego al entorno el valor de la excepción por si el usuario hará uso de la misma.
        AtributoObjeto ao = new AtributoObjeto(new TipoDato(TipoDato.Tipo.STRING), "message", exMessage);
        List<AtributoObjeto> lao = new List<AtributoObjeto> { ao };
        Objeto exObj = new Objeto(new TipoDato(TipoDato.Tipo.OBJECT), lao);
        local.Agregar(ObjetoExcepcion, new Variable(new TipoDato(TipoDato.Tipo.OBJECT), ObjetoExcepcion, exObj));

        // 3. Ejecuto todas las instrucciones dentro de la ListaInstruccionesTry.
        foreach (Instruccion ins in ListaInstruccionesCatch)
        {
            if (ins is Return || ins is Break || ins is Continue)
            {
                return ins;
            }
            else
            {
                ins.Ejecutar(local);
            }
        }

        return new Nulo();
    }
}