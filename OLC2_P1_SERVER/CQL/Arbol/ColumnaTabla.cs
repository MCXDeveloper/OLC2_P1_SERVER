using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ColumnaTabla : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public string NombreColumna { get; set; }

    public ColumnaTabla(string nombre_columna, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        NombreColumna = nombre_columna;
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Valido si estoy en una instrucción WHERE o una SELECT ya que ambas retornan valores diferentes.
        if (CQL.WhereFlag)
        {
            // 2. Para este punto debe de existir una tupla estática de la cual se obtendrá su valor en base al nombre de la columna proporcionado en esta clase.
            if (!(CQL.TuplaEnUso is null))
            {
                // 3. Retorno el valor de la columna de la tupla actual.
                return CQL.TuplaEnUso[NombreColumna];
            }
            else
            {
                Error.AgregarError("Semántico", "[COLUMNA_TABLA]", "Error.  No existe una tupla actual sobre la cual validar su expresión.", fila, columna);
            }
        }
        else
        {
            return NombreColumna;
        }
        
        return new Nulo();
    }

    public TipoDato GetTipo(Entorno ent)
    {
        throw new NotImplementedException();
    }
}