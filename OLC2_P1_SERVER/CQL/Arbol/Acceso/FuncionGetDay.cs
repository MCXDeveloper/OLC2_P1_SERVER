﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FuncionGetDay : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public object Padre { get; set; }

    public FuncionGetDay(int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
    }

    public object Ejecutar(Entorno ent)
    {
        // +----------------------------------------------------------------------------------------+
        // |                                          Nota                                          |
        // +----------------------------------------------------------------------------------------+
        // | Antes de comenzar con la obtención del día, en la clase donde se mande a llamar        |
        // | el metodo Ejecutar de la clase FuncionGetDay se debe definir la variable Padre.        |
        // +----------------------------------------------------------------------------------------+

        if (Padre != null)
        {
            if (Padre is Date)
            {
                return ((Date)Padre).GetDay();
            }
            else
            {
                CQL.AddLUPError("Semántico", "[FUNCION_GET_DAY]", "Error de acceso.  La función 'GetDay' solo es válida cuando el elemento es de tipo Date.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[FUNCION_GET_DAY]", "Error de jerarquía.  El valor al que se le desea aplicar la función GetDay (Padre) no ha sido definido.", fila, columna);
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