
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class AccesoCollection : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public Expresion Posicion { get; set; }
    public string NombreColumna { get; set; }

    public AccesoCollection(string nombre_columna, Expresion posicion, int fila, int columna)
    {
        this.fila = fila;
        Posicion = posicion;
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
                return ObtenerValorDePosicion(CQL.TuplaEnUso[NombreColumna], ent);
            }
            else
            {
                CQL.AddLUPError("Semántico", "[ACCESO_COLLECTION]", "Error.  No existe una tupla actual sobre la cual validar su expresión.", fila, columna);
            }
        }
        else
        {
            // Cuando se hace un select y en los campos viene un acceso a collection, se procede a iterar sobre cada row de la tabla correspondiente, en la
            // columna indicada, y se devuelve el valor obtenido.  Al final, se retorna un nuevo DataTable con esa unica columna para hacer merge con la tabla final.

            // 1. Primero valido que exista una TablaEnUso.
            if (!(CQL.TablaEnUso is null))
            {
                DataTable dt = new DataTable();
                dt.Clear();
                dt.Columns.Add(NombreColumna);

                // 2. Itero por todas las filas de la tabla devolviendo un nuevo row con el valor de la columna en base a la posicion proporcionada.
                foreach (DataRow row in CQL.TablaEnUso.Tabla.Rows)
                {
                    DataRow rowsito = dt.NewRow();

                    object result = ObtenerValorDePosicion(row[NombreColumna], ent);

                    if (result is Nulo)
                    {
                        return new Nulo();
                    }
                    else
                    {
                        rowsito[NombreColumna] = result;
                        dt.Rows.Add(rowsito);
                    }
                }

                return dt;
            }
            else
            {
                CQL.AddLUPError("Semántico", "[ACCESO_COLLECTION]", "Error.  No existe una tabla actual sobre la cual obtener los campos para el SELECT.", fila, columna);
            }
        }

        return new Nulo();
    }
    
    private object ObtenerValorDePosicion(object elemento, Entorno ent)
    {
        object response = new Nulo();

        if (elemento is Map)
        {
            response = ((Map)elemento).Get(Posicion.Ejecutar(ent));
        }
        else if (elemento is XList || elemento is XSet)
        {
            // Valido que la expresión que representa la posición sea de tipo int.
            object pos = Posicion.Ejecutar(ent);

            if (pos is int)
            {
                response = (elemento is XList) ? ((XList)elemento).Get((int)pos) : ((XSet)elemento).Get((int)pos);
            }
            else
            {
                CQL.AddLUPError("Semántico", "[ACCESO_COLLECTION]", "Error.  El valor de la posición para acceder a un elemento de la lista/set debe ser de tipo entero.", fila, columna);
            }

        }

        return response;
    }

    public TipoDato GetTipo(Entorno ent)
    {
        throw new NotImplementedException();
    }
}