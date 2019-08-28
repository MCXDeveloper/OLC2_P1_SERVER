using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Operacion : Instruccion
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
    private readonly Instruccion opIzq;
    private readonly Instruccion opDer;
    private readonly int fila;
    private readonly int columna;

    public Operacion(Instruccion opIzq, TipoOperacion tipo, int fila, int columna)
    {
        this.tipo = tipo;
        this.fila = fila;
        this.opIzq = opIzq;
        this.columna = columna;
    }

    public Operacion(Instruccion opIzq, Instruccion opDer, TipoOperacion tipo, int fila, int columna)
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

    public object Ejecutar(Entorno ent, AST arbol)
    {

        object op1 = opIzq?.Ejecutar(ent, arbol);
        object op2 = opDer?.Ejecutar(ent, arbol);
        object respuesta = new Nulo();

        switch (tipo)
        {
            case TipoOperacion.INCREMENTO:
                respuesta = EjecutarIncremento(op1);
                break;
            case TipoOperacion.DECREMENTO:
                respuesta = EjecutarDecremento(op1);
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
            Error.AgregarError("Semántico", "[OPERACION]", "La operación XOR no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.", fila, columna);
        }

        return new Nulo();
    }

    private object EjecutarNOT(object op1)
    {
        if (op1 is bool)
        {
            return !((bool)op1);
        }
        else
        {
            Error.AgregarError("Semántico", "[OPERACION]", "El operador NOT debe hacerse a una expresión booleana.", fila, columna);
        }

        return new Nulo();
    }

    private object EjecutarOR(object op1, object op2)
    {
        if (op1 is bool && op2 is bool)
        {
            return ((bool)op1) || ((bool)op2);
        }
        else
        {
            Error.AgregarError("Semántico", "[OPERACION]", "La operación OR no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.", fila, columna);
        }

        return new Nulo();
    }

    private object EjecutarAND(object op1, object op2)
    {
        if (op1 is bool && op2 is bool)
        {
            return ((bool)op1) && ((bool)op2);
        }
        else
        {
            Error.AgregarError("Semántico", "[OPERACION]", "La operación AND no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.", fila, columna);
        }

        return new Nulo();
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
            Error.AgregarError("Semántico", "[OPERACION]", "La operación DIFERENTE QUE no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.", fila, columna);
        }

        return new Nulo();
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
            Error.AgregarError("Semántico", "[OPERACION]", "La operación IGUAL QUE no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.", fila, columna);
        }

        return new Nulo();
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
            Error.AgregarError("Semántico", "[OPERACION]", "La operación MENOR IGUAL no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.", fila, columna);
        }

        return new Nulo();
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
            Error.AgregarError("Semántico", "[OPERACION]", "La operación MAYOR IGUAL no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.", fila, columna);
        }

        return new Nulo();
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
            Error.AgregarError("Semántico", "[OPERACION]", "La operación MENOR no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.", fila, columna);
        }

        return new Nulo();
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
            Error.AgregarError("Semántico", "[OPERACION]", "La operación MAYOR no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.", fila, columna);
        }

        return new Nulo();
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
            Error.AgregarError("Semántico", "[OPERACION]", "La operación MODULO no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.", fila, columna);
        }

        return new Nulo();
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
            Error.AgregarError("Semántico", "[OPERACION]", "La operación POTENCIA no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.", fila, columna);
        }

        return new Nulo();
    }

    private object EjecutarDivision(object op1, object op2)
    {
        if(Math.Round(Convert.ToDouble(op2.ToString())) == 0)
        {
            Error.AgregarError("Semántico", "[OPERACION]", "No se puede realizar una división entre 0.", fila, columna);
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
                Error.AgregarError("Semántico", "[OPERACION]", "La operación DIVISION no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.", fila, columna);
            }
        }

        return new Nulo();
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
            Error.AgregarError("Semántico", "[OPERACION]", "La operación MULTIPLICACION no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.", fila, columna);
        }

        return new Nulo();
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
            Error.AgregarError("Semántico", "[OPERACION]", "La operación RESTA no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.", fila, columna);
        }

        return new Nulo();
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
            Error.AgregarError("Semántico", "[OPERACION]", "La operación SUMA no se puede realizar entre variables de tipo '" + op1.GetType().ToString() + "' & '" + op2.GetType().ToString() + "'.", fila, columna);
        }

        return new Nulo();
    }

    private object EjecutarNegativo(object op1)
    {
        if (op1 is bool)
        {
            return !(bool)op1;
        }
        else
        {
            Error.AgregarError("Semántico", "[OPERACION]", "El operador negativo debe aplicarse a un booleano.", fila, columna);
        }

        return new Nulo();
    }

    private object EjecutarDecremento(object op1)
    {
        throw new NotImplementedException();
    }

    private object EjecutarIncremento(object op1)
    {
        throw new NotImplementedException();
    }
}