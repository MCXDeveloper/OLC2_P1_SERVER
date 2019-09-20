
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class AccesoColumna : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public string NombreColumna { get; set; }
    public List<Expresion> ListaAcceso { get; set; }
    
    public AccesoColumna(string nombre_columna, List<Expresion> lista_acceso, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        ListaAcceso = lista_acceso;
        NombreColumna = nombre_columna;
    }
    
    public TipoDato GetTipo(Entorno ent)
    {
        throw new NotImplementedException();
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Valido si estoy en una instrucción WHERE o una SELECT ya que ambas retornan valores diferentes.
        if (CQL.WhereFlag)
        {
            // 2. Para este punto debe de existir una tupla estática de la cual se obtendrá su valor en base al nombre de la columna proporcionado en esta clase.
            if (!(CQL.TuplaEnUso is null))
            {
                // 3. Itero sobre la lista de acceso hasta que devuelva un valor final.
                return ObtenerValorFinal(CQL.TuplaEnUso[NombreColumna], ent);
            }
            else
            {
                CQL.AddLUPError("Semántico", "[ACCESO_COLUMNA]", "Error.  No existe una tupla actual sobre la cual validar su expresión.", fila, columna);
            }
        }
        else
        {
            // Cuando se hace un select y en los campos viene un acceso a columna, se procede a iterar sobre cada row de la tabla correspondiente, en la
            // columna indicada, y se le aplica lo que la lista de acceso indique.  Al final, se retorna un nuevo DataTable con esa unica columna para
            // hacer merge con la tabla final.
            
            // 1. Primero valido que exista una TablaEnUso.
            if(!(CQL.TablaEnUso is null))
            {
                DataTable dt = new DataTable();
                dt.Clear();
                dt.Columns.Add(NombreColumna);

                // 2. Itero por todas las filas de la tabla devolviendo un nuevo row con el valor de la columna alterado por la lista de acceso.
                foreach (DataRow row in CQL.TablaEnUso.Tabla.Rows)
                {
                    DataRow rowsito = dt.NewRow();

                    object result = ObtenerValorFinal(row[NombreColumna], ent);

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
                CQL.AddLUPError("Semántico", "[ACCESO_COLUMNA]", "Error.  No existe una tabla actual sobre la cual obtener los campos para el SELECT.", fila, columna);
            }
        }

        return new Nulo();
    }

    private object ObtenerValorFinal(object elemento, Entorno ent)
    {
        object padre = elemento;

        foreach (Expresion exp in ListaAcceso)
        {
            object access_resp = ValidarAcceso(padre, exp, ent);
            padre = access_resp;

            if (access_resp is Nulo)
            {
                break;
            }
        }

        return padre;
    }

    private object ValidarAcceso(object elemento, Expresion acceso, Entorno ent)
    {
        object response = new Nulo();

        if (acceso is Atributo)
        {
            response = ValidarObjeto(elemento, ((Atributo)acceso).NombreAtributo);
        }
        else if (acceso is FuncionContains)
        {
            response = ValidarCollection_Contains(elemento, ((FuncionContains)acceso).Elemento, ent);
        }
        else if (acceso is FuncionGet)
        {
            response = ValidarCollection_Get(elemento, ((FuncionGet)acceso).Posicion, ent);
        }
        else if (acceso is FuncionSize)
        {
            response = ValidarCollection_Size(elemento);
        }
        else if (acceso is FuncionEndsWith)
        {
            response = ValidarString_With(false, elemento, ((FuncionEndsWith)acceso).Parametro, ent);
        }
        else if (acceso is FuncionStartsWith)
        {
            response = ValidarString_With(true, elemento, ((FuncionStartsWith)acceso).Parametro, ent);
        }
        else if (acceso is FuncionLength)
        {
            response = ValidarString_Length(elemento);
        }
        else if (acceso is FuncionSubstring)
        {
            response = ValidarString_Substring(elemento, ((FuncionSubstring)acceso).Posicion, ((FuncionSubstring)acceso).Cantidad, ent);
        }
        else if (acceso is FuncionToLowerCase)
        {
            response = ValidarString_Case(false, elemento);
        }
        else if (acceso is FuncionToUpperCase)
        {
            response = ValidarString_Case(true, elemento);
        }
        else if (acceso is FuncionGetDay)
        {
            response = ValidarDate_Day(elemento);
        }
        else if (acceso is FuncionGetMonth)
        {
            response = ValidarDate_Month(elemento);
        }
        else if (acceso is FuncionGetYear)
        {
            response = ValidarDate_Year(elemento);
        }
        else if (acceso is FuncionGetHour)
        {
            response = ValidarTime_Hour(elemento);
        }
        else if (acceso is FuncionGetMinutes)
        {
            response = ValidarTime_Minutes(elemento);
        }
        else if (acceso is FuncionGetSeconds)
        {
            response = ValidarTime_Seconds(elemento);
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ACCESO_COLUMNA]", "Error de acceso.  El tipo de dato '" + acceso.GetType().ToString() + "' no es permitido en el acceso a elementos.", fila, columna);
        }

        return response;
    }

    private object ValidarTime_Seconds(object elemento)
    {
        object response = new Nulo();

        if (elemento is Time)
        {
            return ((Time)elemento).GetSeconds();
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ACCESO_COLUMNA]", "Error de acceso.  La función 'GetSeconds' solo es válida cuando el elemento es de tipo Time.", fila, columna);
        }

        return response;
    }

    private object ValidarTime_Minutes(object elemento)
    {
        object response = new Nulo();

        if (elemento is Time)
        {
            return ((Time)elemento).GetMinutes();
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ACCESO_COLUMNA]", "Error de acceso.  La función 'GetMinutes' solo es válida cuando el elemento es de tipo Time.", fila, columna);
        }

        return response;
    }

    private object ValidarTime_Hour(object elemento)
    {
        object response = new Nulo();

        if (elemento is Time)
        {
            return ((Time)elemento).GetHours();
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ACCESO_COLUMNA]", "Error de acceso.  La función 'GetHour' solo es válida cuando el elemento es de tipo Time.", fila, columna);
        }

        return response;
    }

    private object ValidarDate_Year(object elemento)
    {
        object response = new Nulo();

        if (elemento is Date)
        {
            return ((Date)elemento).GetYear();
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ACCESO_COLUMNA]", "Error de acceso.  La función 'GetYear' solo es válida cuando el elemento es de tipo Date.", fila, columna);
        }

        return response;
    }

    private object ValidarDate_Month(object elemento)
    {
        object response = new Nulo();

        if (elemento is Date)
        {
            return ((Date)elemento).GetMonth();
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ACCESO_COLUMNA]", "Error de acceso.  La función 'GetMonth' solo es válida cuando el elemento es de tipo Date.", fila, columna);
        }

        return response;
    }

    private object ValidarDate_Day(object elemento)
    {
        object response = new Nulo();

        if (elemento is Date)
        {
            return ((Date)elemento).GetDay();
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ACCESO_COLUMNA]", "Error de acceso.  La función 'GetDay' solo es válida cuando el elemento es de tipo Date.", fila, columna);
        }

        return response;
    }

    private object ValidarString_Case(bool isUpper, object elemento)
    {
        object response = new Nulo();

        if (elemento is string)
        {
            response = isUpper ? ((string)elemento).ToUpper() : ((string)elemento).ToLower();
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ACCESO_COLUMNA]", "Error de acceso.  La función '" + (isUpper ? "ToUpperCase" : "ToLowerCase") + "' solo es válida cuando el elemento es de tipo String.", fila, columna);
        }

        return response;
    }

    private object ValidarString_Substring(object elemento, Expresion posicion, Expresion cantidad, Entorno ent)
    {
        object response = new Nulo();

        if (elemento is string)
        {
            object pos = posicion.Ejecutar(ent);
            object cant = cantidad.Ejecutar(ent);

            if (pos is int && cant is int)
            {
                response = ((string)elemento).Substring((int)pos, (int)cant);
            }
            else
            {
                CQL.AddLUPError("Semántico", "[ACCESO_COLUMNA]", "Error de acceso.  La función 'Substring' necesita que sus dos parámetos (posicion y cantidad) sean de tipo entero.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ACCESO_COLUMNA]", "Error de acceso.  La función 'Substring' solo es válida cuando el elemento es de tipo String.", fila, columna);
        }

        return response;
    }

    private object ValidarString_Length(object elemento)
    {
        object response = new Nulo();

        if (elemento is string)
        {
            response = ((string)elemento).Length;
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ACCESO_COLUMNA]", "Error de acceso.  La función 'Length' solo es válida cuando el elemento es de tipo String.", fila, columna);
        }

        return response;
    }

    private object ValidarString_With(bool isStarts, object elemento, Expresion parametro, Entorno ent)
    {
        object response = new Nulo();

        if (elemento is string)
        {
            object cadena = parametro.Ejecutar(ent);

            if (cadena is string)
            {
                response = isStarts ? ((string)elemento).StartsWith((string)cadena) : ((string)elemento).EndsWith((string)cadena);
            }
            else
            {
                CQL.AddLUPError("Semántico", "[ACCESO_COLUMNA]", "Error de acceso.  La función '" + (isStarts ? "StartsWith" : "EndsWith") + "' necesita que su parámeto sea de tipo String.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ACCESO_COLUMNA]", "Error de acceso.  La función '" + (isStarts ? "StartsWith" : "EndsWith") + "' solo es válida cuando el elemento es de tipo String.", fila, columna);
        }

        return response;
    }

    private object ValidarObjeto(object elemento, string nombre_atributo)
    {
        object response = new Nulo();

        // Valido que elemento sea de tipo Objeto
        if (elemento is Objeto)
        {
            Objeto obj = (Objeto)elemento;

            // Obtengo el valor del atributo.  Si no existiese, el resultado sería Nulo().
            response = obj.GetAtributo(true, nombre_atributo);
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ACCESO_COLUMNA]", "Error en acceso.  No se puede acceder a un atributo si el elemento no es de tipo Objeto.", fila, columna);
        }

        return response;
    }

    private object ValidarCollection_Contains(object elemento, Expresion key, Entorno ent)
    {
        object response = new Nulo();

        if (elemento is Map)
        {
            Map coleccion = (Map)elemento;
            response = coleccion.Contains(key.Ejecutar(ent));
        }
        else if (elemento is XList)
        {
            XList coleccion = (XList)elemento;
            response = coleccion.Contains(key.Ejecutar(ent));
        }
        else if (elemento is XSet)
        {
            XSet coleccion = (XSet)elemento;
            response = coleccion.Contains(key.Ejecutar(ent));
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ACCESO_COLUMNA]", "Error en acceso.  No se puede aplicar la función Contains a un elemento que no es de tipo Collection.", fila, columna);
        }

        return response;
    }

    private object ValidarCollection_Get(object elemento, Expresion posicion, Entorno ent)
    {
        object response = new Nulo();

        if (elemento is Map)
        {
            Map coleccion = (Map)elemento;
            response = coleccion.Get(posicion.Ejecutar(ent));
        }
        else if (elemento is XList)
        {
            XList coleccion = (XList)elemento;
            object pos = posicion.Ejecutar(ent);

            if (pos is int)
            {
                response = coleccion.Get((int)pos);
            }
            else
            {
                CQL.AddLUPError("Semántico", "[ACCESO_COLUMNA]", "Error en acceso.  La función Get para una colección de tipo List debe ser de tipo entero.", fila, columna);
            }
        }
        else if (elemento is XSet)
        {
            XSet coleccion = (XSet)elemento;
            object pos = posicion.Ejecutar(ent);

            if (pos is int)
            {
                response = coleccion.Get((int)pos);
            }
            else
            {
                CQL.AddLUPError("Semántico", "[ACCESO_COLUMNA]", "Error en acceso.  La función Get para una colección de tipo Set debe ser de tipo entero.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ACCESO_COLUMNA]", "Error de acceso.  No se puede aplicar la función Get a un elemento que no sea de tipo Collection.", fila, columna);
        }

        return response;
    }

    private object ValidarCollection_Size(object elemento)
    {
        object response = new Nulo();

        if (elemento is Map)
        {
            Map coleccion = (Map)elemento;
            response = coleccion.Size();
        }
        else if (elemento is XList)
        {
            XList coleccion = (XList)elemento;
            response = coleccion.Size();
        }
        else if (elemento is XSet)
        {
            XSet coleccion = (XSet)elemento;
            response = coleccion.Size();
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ACCESO_COLUMNA]", "Error de acceso.  No se puede aplicar la función Size a un elemento que no sea de tipo Collection.", fila, columna);
        }

        return response;
    }
}