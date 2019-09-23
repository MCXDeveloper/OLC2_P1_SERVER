
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Operacion : Exception, Expresion, Instruccion
{
    public enum TipoOperacion
    {
        SUMA,
        RESTA,
        MULTIPLICACION,
        DIVISION,
        POTENCIA,
        MODULO,
        NEGATIVO,
        MAYOR,
        MENOR,
        MAYOR_IGUAL,
        MENOR_IGUAL,
        IGUALDAD,
        DIFERENTE,
        NOT,
        AND,
        OR,
        XOR,
        INCREMENTO,
        DECREMENTO,
        DESCONOCIDO
    }

    private readonly TipoOperacion tipo;
    private readonly Expresion opIzq;
    private readonly Expresion opDer;
    private readonly int fila;
    private readonly int columna;
    private bool typeFlag = false; //Esta bandera me sirve para determinar si se esta obteniendo el tipo del resultado para que en las instrucciones inc/dec no las cambie en el entorno.

    public Operacion(Expresion opIzq, TipoOperacion tipo, int fila, int columna)
    {
        this.tipo = tipo;
        this.fila = fila;
        this.opIzq = opIzq;
        this.columna = columna;
    }

    public Operacion(Expresion opIzq, Expresion opDer, TipoOperacion tipo, int fila, int columna)
    {
        this.tipo = tipo;
        this.fila = fila;
        this.opIzq = opIzq;
        this.opDer = opDer;
        this.columna = columna;
    }

    public static TipoOperacion GetTipoOperacion(string op)
    {
        switch (op)
        {
            case "+":
                return TipoOperacion.SUMA;
            case "-":
                return TipoOperacion.RESTA;
            case "*":
                return TipoOperacion.MULTIPLICACION;
            case "/":
                return TipoOperacion.DIVISION;
            case "**":
                return TipoOperacion.POTENCIA;
            case "%":
                return TipoOperacion.MODULO;
            case ">":
                return TipoOperacion.MAYOR;
            case "<":
                return TipoOperacion.MENOR;
            case ">=":
                return TipoOperacion.MAYOR_IGUAL;
            case "<=":
                return TipoOperacion.MENOR_IGUAL;
            case "==":
                return TipoOperacion.IGUALDAD;
            case "!=":
                return TipoOperacion.DIFERENTE;
            case "||":
                return TipoOperacion.OR;
            case "&&":
                return TipoOperacion.AND;
            case "^":
                return TipoOperacion.XOR;
            case "!":
                return TipoOperacion.NOT;
            case "++":
                return TipoOperacion.INCREMENTO;
            case "--":
                return TipoOperacion.DECREMENTO;
            default:
                return TipoOperacion.DESCONOCIDO;
        }
    }

    public TipoDato GetTipo(Entorno ent)
    {
        typeFlag = true;
        object valor = Ejecutar(ent);
        
        if(valor is int)
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
        else if (valor is Exception)
        {
            return new TipoDato(TipoDato.Tipo.EXCEPCION);
        }
        else
        {
            return new TipoDato(TipoDato.Tipo.NULO);
        }
    }

    public object Ejecutar(Entorno ent)
    {
        object op1 = opIzq?.Ejecutar(ent);
        object op2 = opDer?.Ejecutar(ent);
        object respuesta = new Nulo();

        switch (tipo)
        {
            case TipoOperacion.INCREMENTO:
                respuesta = EjecutarIncremento(op1, ent);
                break;
            case TipoOperacion.DECREMENTO:
                respuesta = EjecutarDecremento(op1, ent);
                break;
            case TipoOperacion.NEGATIVO:
                respuesta = EjecutarNegativo(op1);
                break;
            case TipoOperacion.SUMA:
                respuesta = EjecutarSuma(op1, op2);
                break;
            case TipoOperacion.RESTA:
                respuesta = EjecutarResta(op1, op2);
                break;
            case TipoOperacion.MULTIPLICACION:
                respuesta = EjecutarMultiplicacion(op1, op2);
                break;
            case TipoOperacion.DIVISION:
                respuesta = EjecutarDivision(op1, op2);
                break;
            case TipoOperacion.POTENCIA:
                respuesta = EjecutarPotencia(op1, op2);
                break;
            case TipoOperacion.MODULO:
                respuesta = EjecutarModulo(op1, op2);
                break;
            case TipoOperacion.MAYOR:
                respuesta = EjecutarMayor(op1, op2);
                break;
            case TipoOperacion.MENOR:
                respuesta = EjecutarMenor(op1, op2);
                break;
            case TipoOperacion.MAYOR_IGUAL:
                respuesta = EjecutarMayorIgual(op1, op2);
                break;
            case TipoOperacion.MENOR_IGUAL:
                respuesta = EjecutarMenorIgual(op1, op2);
                break;
            case TipoOperacion.IGUALDAD:
                respuesta = EjecutarIgualdad(op1, op2);
                break;
            case TipoOperacion.DIFERENTE:
                respuesta = EjecutarDiferente(op1, op2);
                break;
            case TipoOperacion.AND:
                respuesta = EjecutarAND(op1, op2);
                break;
            case TipoOperacion.OR:
                respuesta = EjecutarOR(op1, op2);
                break;
            case TipoOperacion.XOR:
                respuesta = EjecutarXOR(op1, op2);
                break;
            case TipoOperacion.NOT:
                respuesta = EjecutarNOT(op1);
                break;
            default:
                break;
        }

        return respuesta;

    }

    private object EjecutarXOR(object op1, object op2)
    {
        if (op1 is bool && op2 is bool)
        {
            return ((bool)op1) ^ ((bool)op2);
        }
        else
        {
            string mensaje = "La operación XOR no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.";
            SendStatusMessages(mensaje);
            return new ArithmeticException(mensaje);
        }
    }

    private object EjecutarNOT(object op1)
    {
        if (op1 is bool)
        {
            return !((bool)op1);
        }
        else
        {
            string mensaje = "El operador NOT debe hacerse a una expresión booleana.";
            SendStatusMessages(mensaje);
            return new ArithmeticException(mensaje);
        }
    }

    private object EjecutarOR(object op1, object op2)
    {
        if (op1 is bool && op2 is bool)
        {
            return ((bool)op1) || ((bool)op2);
        }
        else
        {
            string mensaje = "La operación OR no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.";
            SendStatusMessages(mensaje);
            return new ArithmeticException(mensaje);
        }
    }

    private object EjecutarAND(object op1, object op2)
    {
        if (op1 is bool && op2 is bool)
        {
            return ((bool)op1) && ((bool)op2);
        }
        else
        {
            string mensaje = "La operación AND no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.";
            SendStatusMessages(mensaje);
            return new ArithmeticException(mensaje);
        }
    }

    private object EjecutarDiferente(object op1, object op2)
    {
        if (op1 is int && op2 is int)
        {
            return (int)op1 != (int)op2;
        }
        else if (op1 is int && op2 is double)
        {
            return ((int)op1 != (double)op2);
        }
        else if (op1 is double && op2 is int)
        {
            return ((double)op1 != (int)op2);
        }
        else if (op1 is double && op2 is double)
        {
            return (double)op1 != (double)op2;
        }
        else if (op1 is string && op2 is string)
        {
            return !op1.ToString().Equals(op2.ToString());
        }
        else if (op1 is bool && op2 is bool)
        {
            return (bool)op1 != (bool)op2;
        }
        else
        {
            string mensaje = "La operación DIFERENTE QUE no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.";
            SendStatusMessages(mensaje);
            return new ArithmeticException(mensaje);
        }
    }

    private object EjecutarIgualdad(object op1, object op2)
    {
        if (op1 is int && op2 is int)
        {
            return (int)op1 == (int)op2;
        }
        else if (op1 is int && op2 is double)
        {
            return ((int)op1 == (double)op2);
        }
        else if (op1 is double && op2 is int)
        {
            return ((double)op1 == (int)op2);
        }
        else if (op1 is double && op2 is double)
        {
            return (double)op1 == (double)op2;
        }
        else if (op1 is string && op2 is string)
        {
            return op1.ToString().Equals(op2.ToString());
        }
        else if (op1 is bool && op2 is bool)
        {
            return (bool)op1 == (bool)op2;
        }
        else
        {
            string mensaje = "La operación IGUAL QUE no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.";
            SendStatusMessages(mensaje);
            return new ArithmeticException(mensaje);
        }
    }

    private object EjecutarMenorIgual(object op1, object op2)
    {
        if (op1 is int && op2 is int)
        {
            return ((int)op1 <= (int)op2);
        }
        else if (op1 is int && op2 is double)
        {
            return ((int)op1 <= (double)op2);
        }
        else if (op1 is double && op2 is int)
        {
            return ((double)op1 <= (int)op2);
        }
        else if (op1 is double && op2 is double)
        {
            return ((double)op1 <= (double)op2);
        }
        else if(op1 is Date && op2 is Date)
        {
            return ((Date)op1).GetParsedDate() <= ((Date)op2).GetParsedDate();
        }
        else if (op1 is Time && op2 is Time)
        {
            return ((Time)op1).GetParsedTime() <= ((Time)op2).GetParsedTime();
        }
        else
        {
            string mensaje = "La operación MENOR IGUAL no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.";
            SendStatusMessages(mensaje);
            return new ArithmeticException(mensaje);
        }
    }

    private object EjecutarMayorIgual(object op1, object op2)
    {
        if (op1 is int && op2 is int)
        {
            return ((int)op1 >= (int)op2);
        }
        else if (op1 is int && op2 is double)
        {
            return ((int)op1 >= (double)op2);
        }
        else if (op1 is double && op2 is int)
        {
            return ((double)op1 >= (int)op2);
        }
        else if (op1 is double && op2 is double)
        {
            return ((double)op1 >= (double)op2);
        }
        else if (op1 is Date && op2 is Date)
        {
            return ((Date)op1).GetParsedDate() >= ((Date)op2).GetParsedDate();
        }
        else if (op1 is Time && op2 is Time)
        {
            return ((Time)op1).GetParsedTime() >= ((Time)op2).GetParsedTime();
        }
        else
        {
            string mensaje = "La operación MAYOR IGUAL no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.";
            SendStatusMessages(mensaje);
            return new ArithmeticException(mensaje);
        }
    }

    private object EjecutarMenor(object op1, object op2)
    {
        if (op1 is int && op2 is int)
        {
            return ((int)op1 < (int)op2);
        }
        else if (op1 is int && op2 is double)
        {
            return ((int)op1 < (double)op2);
        }
        else if (op1 is double && op2 is int)
        {
            return ((double)op1 < (int)op2);
        }
        else if (op1 is double && op2 is double)
        {
            return ((double)op1 < (double)op2);
        }
        else if (op1 is Date && op2 is Date)
        {
            return ((Date)op1).GetParsedDate() < ((Date)op2).GetParsedDate();
        }
        else if (op1 is Time && op2 is Time)
        {
            return ((Time)op1).GetParsedTime() < ((Time)op2).GetParsedTime();
        }
        else
        {
            string mensaje = "La operación MENOR no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.";
            SendStatusMessages(mensaje);
            return new ArithmeticException(mensaje);
        }
    }

    private object EjecutarMayor(object op1, object op2)
    {
        if (op1 is int && op2 is int)
        {
            return ((int)op1 > (int)op2);
        }
        else if (op1 is int && op2 is double)
        {
            return ((int)op1 > (double)op2);
        }
        else if (op1 is double && op2 is int)
        {
            return ((double)op1 > (int)op2);
        }
        else if (op1 is double && op2 is double)
        {
            return ((double)op1 > (double)op2);
        }
        else if (op1 is Date && op2 is Date)
        {
            return ((Date)op1).GetParsedDate() > ((Date)op2).GetParsedDate();
        }
        else if (op1 is Time && op2 is Time)
        {
            return ((Time)op1).GetParsedTime() > ((Time)op2).GetParsedTime();
        }
        else
        {
            string mensaje = "La operación MAYOR no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.";
            SendStatusMessages(mensaje);
            return new ArithmeticException(mensaje);
        }
    }

    private object EjecutarModulo(object op1, object op2)
    {
        if (op1 is int && op2 is int)
        {
            return (int)op1 % (int)op2;
        }
        else if (op1 is int && op2 is double)
        {
            return (int)op1 % (double)op2;
        }
        else if (op1 is double && op2 is int)
        {
            return (double)op1 % (int)op2;
        }
        else if (op1 is double && op2 is double)
        {
            return (double)op1 % (double)op2;
        }
        else
        {
            string mensaje = "La operación MODULO no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.";
            SendStatusMessages(mensaje);
            return new ArithmeticException(mensaje);
        }
    }

    private object EjecutarPotencia(object op1, object op2)
    {
        if (op1 is int && op2 is int)
        {
            return Math.Pow((int)op1, (int)op2) * 1.0;
        }
        else if (op1 is int && op2 is double)
        {
            return Math.Pow((int)op1, (double)op2) * 1.0;
        }
        else if (op1 is double && op2 is int)
        {
            return Math.Pow((double)op1, (int)op2) * 1.0;
        }
        else if (op1 is double && op2 is double)
        {
            return Math.Pow((double)op1, (double)op2) * 1.0;
        }
        else
        {
            string mensaje = "La operación POTENCIA no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.";
            SendStatusMessages(mensaje);
            return new ArithmeticException(mensaje);
        }
    }

    private object EjecutarDivision(object op1, object op2) 
    {
        if(Math.Round(Convert.ToDouble(op2.ToString())) == 0)
        {
            string mensaje = "No se puede realizar una división entre 0.";
            SendStatusMessages(mensaje);
            return new ArithmeticException(mensaje);
        }
        else
        {
            if (op1 is int && op2 is int)
            {
                return (int)op1 / (int)op2;
            }
            else if (op1 is int && op2 is double)
            {
                return (int)op1 / (double)op2;
            }
            else if (op1 is double && op2 is int)
            {
                return (double)op1 / (int)op2;
            }
            else if (op1 is double && op2 is double)
            {
                return (double)op1 / (double)op2;
            }
            else
            {
                string mensaje = "La operación DIVISION no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.";
                SendStatusMessages(mensaje);
                return new ArithmeticException(mensaje);
            }
        }
    }

    private object EjecutarMultiplicacion(object op1, object op2)
    {
        if (op1 is int && op2 is int)
        {
            return (int)op1 * (int)op2;
        }
        else if (op1 is int && op2 is double)
        {
            return (int)op1 * (double)op2;
        }
        else if (op1 is double && op2 is int)
        {
            return (double)op1 * (int)op2;
        }
        else if (op1 is double && op2 is double)
        {
            return (double)op1 * (double)op2;
        }
        else
        {
            string mensaje = "La operación MULTIPLICACION no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.";
            SendStatusMessages(mensaje);
            return new ArithmeticException(mensaje);
        }
    }

    private object EjecutarResta(object op1, object op2)
    {
        if (op1 is int && op2 is int)
        {
            return (int)op1 - (int)op2;
        }
        else if (op1 is int && op2 is double)
        {
            return (int)op1 - (double)op2;
        }
        else if (op1 is double && op2 is int)
        {
            return (double)op1 - (int)op2;
        }
        else if (op1 is double && op2 is double)
        {
            return (double)op1 - (double)op2;
        }
        else
        {
            string mensaje = "La operación RESTA no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.";
            SendStatusMessages(mensaje);
            return new ArithmeticException(mensaje);
        }
    }

    private object EjecutarSuma(object op1, object op2)
    {
        if (op1 is int && op2 is int)
        {
            return (int)op1 + (int)op2;
        }
        else if (op1 is int && op2 is double)
        {
            return (int)op1 + (double)op2;
        }
        else if (op1 is double && op2 is int)
        {
            return (double)op1 + (int)op2;
        }
        else if (op1 is double && op2 is double)
        {
            return (double)op1 + (double)op2;
        }
        else if (op1 is int && op2 is string)
        {
            return ((int)op1 + (string)op2);
        }
        else if (op1 is double && op2 is string)
        {
            return ((double)op1 + (string)op2);
        }
        else if (op1 is string && op2 is int)
        {
            return ((string)op1 + (int)op2);
        }
        else if (op1 is string && op2 is double)
        {
            return ((string)op1 + (double)op2);
        }
        else if (op1 is string && op2 is string)
        {
            return ((string)op1 + (string)op2);
        }
        else if (op1 is string && op2 is bool)
        {
            if ((bool)op2)
            {
                return ((string)op1 + "true");
            }
            else
            {
                return ((string)op1 + "false");
            }
        }
        else if (op1 is bool && op2 is string)
        {
            if ((bool)op1)
            {
                return ("true" + (string)op2);
            }
            else
            {
                return ("false" + (string)op2);
            }
        }
        else
        {
            string mensaje = "La operación SUMA no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.";
            SendStatusMessages(mensaje);
            return new ArithmeticException(mensaje);
        }
    }

    private object EjecutarNegativo(object op1)
    {
        if (op1 is int)
        {
            return (int)op1 * -1;
        }
        else if (op1 is double)
        {
            return (double)op1 * -1;
        }
        else
        {
            string mensaje = "El operador negativo debe aplicarse a un valor numérico.";
            SendStatusMessages(mensaje);
            return new ArithmeticException(mensaje);
        }
    }

    private object EjecutarDecremento(object op1, Entorno ent)
    {
        if (op1 is int)
        {
            int temp = (int)op1;
            if (!typeFlag)
            {
                Asignacion inc = new Asignacion(((Identificador)opIzq).Id, Asignacion.TipoAsignacion.AS_NORMAL, new Primitivo((int)op1 - 1), fila, columna);
                inc.Ejecutar(ent);
                typeFlag = false;
            }
            return temp;
        }
        else if (op1 is double)
        {
            double temp = (double)op1;
            if (!typeFlag)
            {
                Asignacion inc = new Asignacion(((Identificador)opIzq).Id, Asignacion.TipoAsignacion.AS_NORMAL, new Primitivo((double)op1 - 1), fila, columna);
                inc.Ejecutar(ent);
                typeFlag = false;
            }
            return temp;
        }
        else
        {
            string mensaje = "El decremento se debe hacerse a una expresión numérica.";
            SendStatusMessages(mensaje);
            return new ArithmeticException(mensaje);
        }
    }

    private object EjecutarIncremento(object op1, Entorno ent)
    {
        if (op1 is int)
        {
            int temp = (int)op1;
            if (!typeFlag)
            {
                Asignacion inc = new Asignacion(((Identificador)opIzq).Id, Asignacion.TipoAsignacion.AS_NORMAL, new Primitivo((int)op1 + 1), fila, columna);
                inc.Ejecutar(ent);
                typeFlag = false;
            }
            return temp;
        }
        else if (op1 is double)
        {
            double temp = (double)op1;
            if (!typeFlag)
            {
                Asignacion inc = new Asignacion(((Identificador)opIzq).Id, Asignacion.TipoAsignacion.AS_NORMAL, new Primitivo((double)op1 + 1), fila, columna);
                inc.Ejecutar(ent);
                typeFlag = false;
            }
            return temp;
        }
        else
        {
            string mensaje = "El incremento se debe hacerse a una expresión numérica.";
            SendStatusMessages(mensaje);
            return new ArithmeticException(mensaje);
        }
    }

    private void SendStatusMessages(string mensaje)
    {
        if (!typeFlag)
        {
            CQL.AddLUPError("Semántico", "[OPERACION]", mensaje, fila, columna);
            if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'ArithmeticException' no capturada.  " + mensaje); }
        }
    }

}