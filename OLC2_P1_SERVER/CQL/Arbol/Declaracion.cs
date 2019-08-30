using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Entorno;

public class Declaracion : Instruccion
{
    private readonly int fila;
    private readonly Tipo tipo;
    private readonly int columna;
    private readonly string nombreObj;
    private readonly Expresion valor;
    private readonly List<string> lista_variables;
    
    public Declaracion(Tipo tipo, List<string> lista_variables, int fila, int columna)
    {
        this.tipo = tipo;
        this.fila = fila;
        this.nombreObj = null;
        this.columna = columna;
        this.valor = new Nulo();
        this.lista_variables = lista_variables;
    }

    public Declaracion(Tipo tipo, List<string> lista_variables, Expresion valor, int fila, int columna)
    {
        this.tipo = tipo;
        this.fila = fila;
        this.valor = valor;
        this.nombreObj = null;
        this.columna = columna;
        this.lista_variables = lista_variables;
    }

    public Declaracion(string nombreObj, List<string> lista_variables, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        this.valor = new Nulo();
        this.nombreObj = nombreObj;
        this.lista_variables = lista_variables;
    }

    public Declaracion(string nombreObj, List<string> lista_variables, Expresion valor, int fila, int columna)
    {
        this.fila = fila;
        this.valor = valor;
        this.columna = columna;
        this.nombreObj = nombreObj;
        this.lista_variables = lista_variables;
    }

    public object Ejecutar(Entorno ent)
    {
        // Si el valor brindado en el constructor es igual a null significa que la declaración no termino con la igualacion a una expresion
        // por lo que a todos se les asignaría valor Nulo().

        if (valor is Nulo)
        {
            foreach(string variable in lista_variables)
            {
                object simbolo = ent.ObtenerVariable(variable);

                if (!(simbolo is Nulo))
                {
                    if (nombreObj is null)
                    {
                        ent.Agregar(variable, new Variable(tipo, variable, new Nulo()));
                    }
                    else
                    {
                        ent.Agregar(variable, new Variable(nombreObj, variable, new Nulo()));
                    }
                    
                }
                else
                {
                    Error.AgregarError("Semántico", "[DECLARACION]", "Se intento declarar '" + variable + "'.  Una variable con el mismo nombre ya se encuentra en el entorno actual.", fila, columna);
                }
            }
        }
        else
        {
            for(int i = 0; i < lista_variables.Count; i++)
            {
                string nombre_variable = lista_variables.ElementAt(i);
                object simbolo = ent.ObtenerVariable(nombre_variable);

                if (!(simbolo is Nulo))
                {
                    //TODO falta declarar los 3 tipos de objeto posibles: Expresiones, Objetos (UserTypes) & Collections
                }
                else
                {
                    Error.AgregarError("Semántico", "[DECLARACION]", "Se intento declarar '" + nombre_variable + "'.  Una variable con el mismo nombre ya se encuentra en el entorno actual.", fila, columna);
                }
            }
        }

        return new Nulo();

    }


}