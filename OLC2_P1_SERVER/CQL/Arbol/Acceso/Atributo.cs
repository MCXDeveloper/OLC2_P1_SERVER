
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Atributo : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public bool IsValor { get; set; }
    public object Padre { get; set; }
    public string NombreAtributo { get; set; }

    public Atributo(string nombre_atributo, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        NombreAtributo = nombre_atributo;
    }

    public object Ejecutar(Entorno ent)
    {
        // +----------------------------------------------------------------------------------------------------+
        // |                                                Nota                                                |
        // +----------------------------------------------------------------------------------------------------+
        // | Antes de comenzar con la obtención del valor dentro un objeto, en la clase donde se mande a llamar |
        // | el método Ejecutar de la clase Atributo se debe definir la variable Padre y la de IsValor.         |
        // +----------------------------------------------------------------------------------------------------+

        if (Padre != null)
        {
            // Valido que elemento sea de tipo Objeto
            if (Padre is Objeto)
            {
                Objeto obj = (Objeto)Padre;

                // Obtengo el valor del atributo.  Si no existiese, el resultado sería Nulo().
                return obj.GetAtributo(IsValor, NombreAtributo);
            }
            else
            {
                CQL.AddLUPError("Semántico", "[ATRIBUTO]", "Error en acceso.  No se puede acceder a un atributo si el elemento no es de tipo Objeto.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ATRIBUTO]", "Error de jerarquía.  El valor al que se le desea aplicar la función Atributo (Padre) no ha sido definido.", fila, columna);
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
        else if (valor is Objeto)
        {
            return new TipoDato(TipoDato.Tipo.OBJECT);
        }
        else if (valor is Map)
        {
            return new TipoDato(TipoDato.Tipo.MAP);
        }
        else if (valor is XSet)
        {
            return new TipoDato(TipoDato.Tipo.SET);
        }
        else if (valor is XList)
        {
            return new TipoDato(TipoDato.Tipo.LIST);
        }
        else
        {
            return new TipoDato(TipoDato.Tipo.NULO);
        }
    }
}