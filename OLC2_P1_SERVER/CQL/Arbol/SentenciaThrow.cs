using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class SentenciaThrow : Instruccion
{
    public TipoExcepcion TipoDeExcepcion { get; set; }

    public SentenciaThrow(TipoExcepcion tx)
    {
        TipoDeExcepcion = tx;
    }

    public object Ejecutar(Entorno ent)
    {
        switch (TipoDeExcepcion)
        {
            case TipoExcepcion.ARITHMETIC_EXCEPTION:
                throw new ArithmeticException("");

            case TipoExcepcion.TYPE_ALREADY_EXISTS:
                throw new TypeAlreadyExists("");

            case TipoExcepcion.BD_ALREADY_EXISTS:
                throw new BDAlreadyExists("");

            case TipoExcepcion.BD_DONT_EXISTS:
                throw new BDDontExists("");

            case TipoExcepcion.USE_DB_EXCEPTION:
                throw new UseBDException("");

            case TipoExcepcion.TABLE_ALREADY_EXISTS:
                throw new TableAlreadyExists("");

            case TipoExcepcion.TABLE_DONT_EXISTS:
                throw new TableDontExists("");

            case TipoExcepcion.COUNTER_TYPE_EXCEPTION:
                throw new CounterTypeException("");

            case TipoExcepcion.USER_ALREADY_EXISTS:
                throw new UserAlreadyExists("");

            case TipoExcepcion.USER_DONT_EXISTS:
                throw new UserDontExists("");

            case TipoExcepcion.VALUES_EXCEPTION:
                throw new ValuesException("");

            case TipoExcepcion.COLUMN_EXCEPTION:
                throw new ColumnException("");

            case TipoExcepcion.INDEX_OUT_EXCEPTION:
                throw new IndexOutException("");

            case TipoExcepcion.NULL_POINTER_EXCEPTION:
                throw new NullPointerException("");

            case TipoExcepcion.FUNCTION_ALREADY_EXISTS:
                throw new FunctionAlreadyExists("");

            case TipoExcepcion.OBJECT_ALREADY_EXISTS:
                throw new ObjectAlreadyExists("");
        }

        return new Nulo();
    }
}