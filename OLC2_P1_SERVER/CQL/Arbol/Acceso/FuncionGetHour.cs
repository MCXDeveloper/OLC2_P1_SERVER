﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FuncionGetHour : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public object Padre { get; set; }

    public FuncionGetHour(int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
    }

    public object Ejecutar(Entorno ent)
    {
        // +--------------------------------------------------------------------------------------+
        // |                                         Nota                                         |
        // +--------------------------------------------------------------------------------------+
        // | Antes de comenzar con la obtención de las horas, en la clase donde se mande a llamar |
        // | el metodo Ejecutar de la clase FuncionGetHour se debe definir la variable Padre.     |
        // +--------------------------------------------------------------------------------------+

        if (Padre != null)
        {
            if (Padre is Time)
            {
                return ((Time)Padre).GetHours();
            }
            else
            {
                CQL.AddLUPError("Semántico", "[FUNCION_GET_HOUR]", "Error de acceso.  La función 'GetHour' solo es válida cuando el elemento es de tipo Time.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[FUNCION_GET_HOUR]", "Error de jerarquía.  El valor al que se le desea aplicar la función GetHour (Padre) no ha sido definido.", fila, columna);
        }

        return new Nulo();
    }

    public TipoDato GetTipo(Entorno ent)
    {
        object valor = Ejecutar(ent);

        if (valor is int)
        {
            return new TipoDato(TipoDato.Tipo.INT);
        }
        else
        {
            return new TipoDato(TipoDato.Tipo.NULO);
        }
    }
}