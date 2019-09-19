using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Switch : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    private readonly Expresion condicion;
    private readonly List<Instruccion> listaCases;
    private readonly List<Instruccion> sentenciasDefault;
    
    public Switch(Expresion condicion, List<Instruccion> listaCases, List<Instruccion> sentenciasDefault, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        this.condicion = condicion;
        this.listaCases = listaCases;
        this.sentenciasDefault = sentenciasDefault;
    }
    
    public object Ejecutar(Entorno ent)
    {
        // Consideraciones a tomar en cuenta para ejecutar una sentencia Selecciona:
        // 1. La expresión proporcionada como condicion debe de ser de tipo Expresion.
        // 2. Cada uno de los casos establecidos contiene una expresion que es la que tiene que ser comparada con el punto 1.
        // 3. En caso de que se cumpla un caso, pero no exista sentencia detener, se activa una bandera de señal que indica que la expresión ya fue evaluada para que no entre en el defecto.
        // 4. Si la expresión proporcionada no cumple ninguno de los casos, debe de ejecutarse el caso defecto.

        // Bandera que me indica si el selecciona entro por alguno de los casos.
        bool caseFlag = false;

        // Primero: Valido la expresion proporcionada como condicion.
        object expresion_condicion = condicion.Ejecutar(ent);

        // Segundo: Por cada uno de los casos establecidos en lista_casos verifico si la expresion proporcionada en la clase Caso es la misma que la expresion_condicion definida en el primer paso.
        foreach(Instruccion ins in listaCases)
        {
            if(ins is Case)
            {
                // Evalúo la expresion del caso
                Case target = (Case)ins;
                object exp = target.GetCondicion().Ejecutar(ent);

                // Si el valor de la expresion definida en el selecciona hace match con la expresion definida por el Caso
                if (expresion_condicion.Equals(exp))
                {
                    object resp = target.Ejecutar(ent);

                    // Si la respuesta de la ejecución de las sentencias del Caso devuelve un detener, se para la evaluación de los demás casos y de igual forma activo caseFlag = true
                    // Si no viene el detener, de igual forma activo caseFlag = true para que ya no se ejecute la sentencia defecto aunque se itere por los demás casos.
                    // Si entre las sentencias ejecutadas devuelve un Retornar, entonces paro el ciclo del Selecciona y devuelvo el valor del Retornar.
                    caseFlag = true;

                    if (resp is Break) {
                        break;
                    }
                    else if (resp is Return) {
                        return ((Return)resp).Ejecutar(ent);
                    }
                }

            }
            else
            {
                CQL.AddLUPError("Semántico", "[SWITCH]", "No se puede evaluar una sentencia que no sea un Case dentro de la sentencia Switch.", fila, columna);
            }
        }

        // Tercero: Si caseFlag = false, significa que ya se iteró por cada uno de los casos y ninguno hizo match, por lo que se procede a ejecutar las sentencias de defecto.
        if (!caseFlag)
        {
            Entorno local = new Entorno(ent);

            foreach(Instruccion ins in sentenciasDefault)
            {
                if(ins is Return)
                {
                    return ins.Ejecutar(local);
                }
                else if (ins is Break)
                {
                    break;
                }
                else
                {
                    ins.Ejecutar(local);
                }
            }

        }

        return new Nulo();

    }

}