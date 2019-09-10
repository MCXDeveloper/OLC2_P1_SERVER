using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class LlamadaFuncion : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public string NombreFuncion { get; set; }
    public List<Expresion> ListaValores { get; set; }

    public LlamadaFuncion(string nombre_funcion, List<Expresion> lista_valores, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        ListaValores = lista_valores;
        NombreFuncion = nombre_funcion;
    }

    public TipoDato GetTipo(Entorno ent)
    {
        throw new NotImplementedException();
    }

    public object Ejecutar(Entorno ent)
    {
        object response = new Nulo();
        
        // 1. Primero genero la llave única que representa la llamada a esta función.
        string key = GenerateUniqueKey(ent);

        // 2. Luego, verifico que la función, en base a su llave única, exista en el entorno.
        object simbolo = ent.ObtenerFuncion(key);

        if(!(simbolo is Nulo))
        {
            Funcion func = (Funcion)simbolo;
            Entorno local = new Entorno(ent);

            if(VerificarParametros(func.ListaParametros, local))
            {
                foreach (Instruccion ins in func.ListaInstrucciones)
                {
                    object exec = ins.Ejecutar(local);
                    
                    if (exec is Return)
                    {
                        return ((Return)exec).Ejecutar(local);
                    }

                }
            }
        }
        else
        {
            Error.AgregarError("Semántico", "[LLAMADA_FUNCION]", "Error.  No existe la función con el nombre de '"+ NombreFuncion +"' (Key: "+ key +") en el entorno.", fila, columna);
        }

        return response;
    }

    private bool VerificarParametros(List<Parametro> ListaParametros, Entorno ent)
    {
        // 1. Primero verifico que la cantidad de parámetros en ListaParametros sea la misma que la cantidad de valores en ListaValores.
        if(ListaValores.Count == ListaParametros.Count)
        {
            // 2. Por cada uno de los de los parámetros en ListaParametros se va a verificar si el tipo coincide con el tipo del valor correspondiente a su posición en ListaValores.
            for(int i = 0; i < ListaParametros.Count; i++)
            {
                // 3. Si el tipo de dato del parámetro coincide con el tipo de dato del valor, se registra en un nuevo entorno.
                if (ListaParametros[i].TipoDatoParametro.Equals(ListaValores[i].GetTipo(ent)))
                {
                    ent.Agregar(ListaParametros[i].NombreParametro, new Variable(ListaParametros[i].TipoDatoParametro, ListaParametros[i].NombreParametro, ListaValores[i].Ejecutar(ent)));
                }
                else
                {
                    Error.AgregarError("Semántico", "[LLAMADA_FUNCION]", "Error en la llamada a función '" + NombreFuncion + "'.  El tipo de los parámetros no coincide con el valor correspondiente utilizado en la llamada.  Parámetro: "+ ListaParametros[i].NombreParametro + ".", fila, columna);
                }
            }

            return true;

        }
        else
        {
            Error.AgregarError("Semántico", "[LLAMADA_FUNCION]", "Error en la llamada a función '"+ NombreFuncion +"'.  La cantidad de parámetros no coincide.", fila, columna);
        }

        return false;
    }

    private string GenerateUniqueKey(Entorno ent)
    {
        string key = NombreFuncion + "_";

        foreach (Expresion val in ListaValores)
        {
            key += (ListaValores.Last().Equals(val) ? val.GetTipo(ent).GetRealTipo().ToString() : val.GetTipo(ent).GetRealTipo().ToString() + "_");
        }

        return key;
    }
}