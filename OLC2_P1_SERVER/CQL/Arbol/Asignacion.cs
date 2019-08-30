using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Entorno;

public class Asignacion : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    private readonly string variable;
    private readonly Expresion valor;
    private readonly TipoAsignacion tipo_asig;

    public enum TipoAsignacion
    {
        AS_SUMA,
        AS_RESTA,
        AS_NORMAL,
        AS_DIVISION,
        AS_MULTIPLICACION
    }

    public Asignacion(TipoAsignacion tipo_asig, string variable, Expresion valor, int fila, int columna)
    {
        this.fila = fila;
        this.valor = valor;
        this.columna = columna;
        this.variable = variable;
        this.tipo_asig = tipo_asig;
    }

    public object Ejecutar(Entorno ent)
    {
        // Primero verifico que la variable a modificar ya haya sido declarada en el entorno.
        object simbolo = ent.ObtenerVariable(variable);

        if(!(simbolo is Nulo))
        {
            Variable sim = (Variable)simbolo;
            Tipo tipo_resultado = valor.GetTipo(ent);
            string nombre_variable = sim.GetNombre();

            if (tipo_asig.Equals(TipoAsignacion.AS_NORMAL))
            {
                if (sim.GetTipo().Equals(tipo_resultado))
                {
                    ent.ReemplazarVariable(nombre_variable, new Variable(tipo_resultado, nombre_variable, valor.Ejecutar(ent)));
                }
                else
                {
                    Error.AgregarError("Semantico", "[ASIGNACION]", "Error de tipos. Se intentó setear un valor a la variable" + nombre_variable + " diferente al que fue declarado.", fila, columna);
                }
            }
            else
            {
                if ((sim.GetTipo().Equals(Tipo.INT) || sim.GetTipo().Equals(Tipo.DOUBLE)) && (tipo_resultado is Tipo.INT || tipo_resultado is Tipo.DOUBLE))
                {
                    switch (tipo_asig)
                    {
                        case TipoAsignacion.AS_SUMA:
                            ent.ReemplazarVariable(nombre_variable, new Variable(tipo_resultado, nombre_variable, new Operacion(new Identificador(nombre_variable), valor, Operacion.TipoOperacion.SUMA, fila, columna)));
                            break;
                        case TipoAsignacion.AS_RESTA:
                            ent.ReemplazarVariable(nombre_variable, new Variable(tipo_resultado, nombre_variable, new Operacion(new Identificador(nombre_variable), valor, Operacion.TipoOperacion.RESTA, fila, columna)));
                            break;
                        case TipoAsignacion.AS_MULTIPLICACION:
                            ent.ReemplazarVariable(nombre_variable, new Variable(tipo_resultado, nombre_variable, new Operacion(new Identificador(nombre_variable), valor, Operacion.TipoOperacion.MULTIPLICACION, fila, columna)));
                            break;
                        case TipoAsignacion.AS_DIVISION:
                            ent.ReemplazarVariable(nombre_variable, new Variable(tipo_resultado, nombre_variable, new Operacion(new Identificador(nombre_variable), valor, Operacion.TipoOperacion.DIVISION, fila, columna)));
                            break;
                        default:
                            Error.AgregarError("Semantico", "[ASIGNACION]", "El tipo de asignacion proporcionado no es reconocido por este lenguaje.", fila, columna);
                            break;
                    }
                }
                else
                {
                    Error.AgregarError("Semantico", "[ASIGNACION]", "La asignación-operación solo se puede realizar con valores numéricos.", fila, columna);
                }
            }
        }
        else
        {
            Error.AgregarError("Semantico", "[ASIGNACION]", "Se intentó asignar un valor a la variable: '" + variable + "', la cual, no se ecuentra definida en el entorno actual.", fila, columna);
        }

        return new Nulo();
    }
}