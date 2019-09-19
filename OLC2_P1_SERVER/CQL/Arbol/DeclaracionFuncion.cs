using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class DeclaracionFuncion : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public string NombreFuncion { get; set; }
    public TipoDato TipoDatoFuncion { get; set; }
    public List<Parametro> ListaParametros { get; set; }
    public List<Instruccion> ListaInstrucciones { get; set; }

    public DeclaracionFuncion(TipoDato tipo_dato_funcion, string nombre_funcion, List<Parametro> lista_parametros, List<Instruccion> lista_instrucciones, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        NombreFuncion = nombre_funcion;
        ListaParametros = lista_parametros;
        TipoDatoFuncion = tipo_dato_funcion;
        ListaInstrucciones = lista_instrucciones;
    }

    public object Ejecutar(Entorno ent)
    {

        // +-------------------------------------------------------------------------------------------------------+
        // |                                                 Nota                                                  |
        // +-------------------------------------------------------------------------------------------------------+
        // | Para realizar de forma correcta la declaración de funciones, se deben seguir los siguientes pasos:    |
        // | 1. Se debe de generar una llave única, de tipo string, que represente la sobrecarga en las funciones. |
        // |    a. Esta llave única se genera utilizando el nombre de la función seguido de cada uno de los        |
        // |    parámetros que ésta tenga, separados por un guión bajo.                                            |
        // | 2. Validar que la llave única generada no exista en el entorno de funciones.                          |
        // | 3. Crear el símbolo función y almacenarlo en el entorno.                                              |
        // +-------------------------------------------------------------------------------------------------------+

        // 1. Genero la llave única que representa la función.
        string key = GenerateUniqueKey();

        // 2. Valido que la llave única generada no exista en el entorno de funciones.
        object simbolo = ent.ObtenerFuncion(key);

        if (simbolo is Nulo)
        {
            // 3. Agrego el símbolo de función en el entorno.
            ent.Agregar(key, new Funcion(TipoDatoFuncion, NombreFuncion, ListaParametros, ListaInstrucciones));
        }
        else
        {
            CQL.AddLUPError("Semántico", "[DECLARACION_FUNCION]", "Se intentó declarar una función con el nombre de '" + key + "'.  Una con el mismo nombre ya existe en el entorno.", fila, columna);
        }

        return new Nulo();
    }

    private string GenerateUniqueKey()
    {
        string id = "_" + NombreFuncion + "(";

        foreach (Parametro p in ListaParametros)
        {
            TipoDato.Tipo type = p.TipoDatoParametro.GetRealTipo();
            id += "_" + type.ToString();

            if (type.Equals(TipoDato.Tipo.OBJECT))
            {
                 id += "_" + (string)p.TipoDatoParametro.GetElemento();
            }
        }

        id += ")";

        return id;
    }
}