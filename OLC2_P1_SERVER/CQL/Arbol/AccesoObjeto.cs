using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class AccesoObjeto : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public string Variable { get; set; }
    public List<Expresion> ListaAcceso { get; set; }
    
    public AccesoObjeto(string variable, List<Expresion> lista_acceso, int fila, int columna)
    {
        this.fila = fila;
        Variable = variable;
        this.columna = columna;
        ListaAcceso = lista_acceso;
    }

    public TipoDato GetTipo(Entorno ent)
    {
        throw new NotImplementedException();
    }

    public object Ejecutar(Entorno ent)
    {

        // +----------------------------------------------------------------------------------------------------------------------------+
        // |                                                            NOTA                                                            |
        // +----------------------------------------------------------------------------------------------------------------------------+
        // | Para realizar de forma correcta el acceso al valor de un objeto, se deben seguir los siguientes pasos:                     |
        // | 1. Validar que la variable de la cual parte el acceso exista en el entorno.                                                |
        // | 2. Recorrer la lista de acceso de tal forma en la que se sepa que tipo de acceso es.  Pueden existir los siguientes tipos: |
        // |    a. Funciones propias de collections (se debe de validar que el valor anterior sea un Collection).                       |
        // |    b. Funciones propias de string (se debe validar que el valor anterior sea de tipo String).                              |
        // |    c. Funciones propias de Date (se debe validar que el valor anterior sea de tipo Date).                                  |
        // |    d. Funciones propias de Time (se debe validar que el valor anterior sea de tipo Time).                                  |
        // |    e. Nombre de un atributo (se debe validar que el valor anterior sea de tipo Objeto).                                    |
        // +----------------------------------------------------------------------------------------------------------------------------+

        object response = new Nulo();

        // Primero verifico que la variable exista en el entorno.
        object simbolo = ent.ObtenerVariable(Variable);

        if(!(simbolo is Nulo))
        {
            object padre = (Variable)simbolo;

            foreach (Expresion exp in ListaAcceso)
            {
                object access_resp = ValidarAcceso(padre, exp, ent);
                
                if(access_resp is Nulo)
                {
                    break;
                }
                else
                {
                    padre = access_resp;
                }
            }

            response = padre;
        }
        else
        {
            Error.AgregarError("Semántico", "[ACCESO_OBJETO]", "El símbolo con el identificador '"+ Variable +"' no existe en el entorno.", fila, columna);
        }
        
        return response;
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
            Error.AgregarError("Semántico", "[ACCESO_OBJETO]", "Error de acceso.  El tipo de dato '"+ acceso.GetType().ToString() +"' no es permitido en el acceso a elementos.", fila, columna);
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
            Error.AgregarError("Semántico", "[ACCESO_OBJETO]", "Error de acceso.  La función 'GetSeconds' solo es válida cuando el elemento es de tipo Time.", fila, columna);
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
            Error.AgregarError("Semántico", "[ACCESO_OBJETO]", "Error de acceso.  La función 'GetMinutes' solo es válida cuando el elemento es de tipo Time.", fila, columna);
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
            Error.AgregarError("Semántico", "[ACCESO_OBJETO]", "Error de acceso.  La función 'GetHour' solo es válida cuando el elemento es de tipo Time.", fila, columna);
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
            Error.AgregarError("Semántico", "[ACCESO_OBJETO]", "Error de acceso.  La función 'GetYear' solo es válida cuando el elemento es de tipo Date.", fila, columna);
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
            Error.AgregarError("Semántico", "[ACCESO_OBJETO]", "Error de acceso.  La función 'GetMonth' solo es válida cuando el elemento es de tipo Date.", fila, columna);
        }

        return response;
    }

    private object ValidarDate_Day(object elemento)
    {
        object response = new Nulo();

        if(elemento is Date)
        {
            return ((Date)elemento).GetDay();
        }
        else
        {
            Error.AgregarError("Semántico", "[ACCESO_OBJETO]", "Error de acceso.  La función 'GetDay' solo es válida cuando el elemento es de tipo Date.", fila, columna);
        }

        return response;
    }

    private object ValidarString_Case(bool isUpper, object elemento)
    {
        object response = new Nulo();

        if(elemento is string)
        {
            response = isUpper ? ((string)elemento).ToUpper() : ((string)elemento).ToLower();
        }
        else
        {
            Error.AgregarError("Semántico", "[ACCESO_OBJETO]", "Error de acceso.  La función '"+ (isUpper ? "ToUpperCase" : "ToLowerCase") +"' solo es válida cuando el elemento es de tipo String.", fila, columna);
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
                Error.AgregarError("Semántico", "[ACCESO_OBJETO]", "Error de acceso.  La función 'Substring' necesita que sus dos parámetos (posicion y cantidad) sean de tipo entero.", fila, columna);
            }
        }
        else
        {
            Error.AgregarError("Semántico", "[ACCESO_OBJETO]", "Error de acceso.  La función 'Substring' solo es válida cuando el elemento es de tipo String.", fila, columna);
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
            Error.AgregarError("Semántico", "[ACCESO_OBJETO]", "Error de acceso.  La función 'Length' solo es válida cuando el elemento es de tipo String.", fila, columna);
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
                Error.AgregarError("Semántico", "[ACCESO_OBJETO]", "Error de acceso.  La función '"+ (isStarts ? "StartsWith" : "EndsWith") +"' necesita que su parámeto sea de tipo String.", fila, columna);
            }
        }
        else
        {
            Error.AgregarError("Semántico", "[ACCESO_OBJETO]", "Error de acceso.  La función '" + (isStarts ? "StartsWith" : "EndsWith") + "' solo es válida cuando el elemento es de tipo String.", fila, columna);
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
            response = obj.GetAtributo(nombre_atributo);
        }
        else
        {
            Error.AgregarError("Semántico", "[ACCESO_OBJETO]", "Error en acceso.  No se puede acceder a un atributo si el elemento no es de tipo Objeto.", fila, columna);
        }

        return response;
    }

    private object ValidarCollection_Contains(object elemento, Expresion key, Entorno ent)
    {
        object response = new Nulo();

        if(elemento is Map)
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
            Error.AgregarError("Semántico", "[ACCESO_OBJETO]", "Error en acceso.  No se puede aplicar la función Contains a un elemento que no es de tipo Collection.", fila, columna);
        }

        return response;
    }

    private object ValidarCollection_Get(object elemento, Expresion posicion, Entorno ent)
    {
        object response = new Nulo();

        if(elemento is Map)
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
                Error.AgregarError("Semántico", "[ACCESO_OBJETO]", "Error en acceso.  La función Get para una colección de tipo List debe ser de tipo entero.", fila, columna);
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
                Error.AgregarError("Semántico", "[ACCESO_OBJETO]", "Error en acceso.  La función Get para una colección de tipo Set debe ser de tipo entero.", fila, columna);
            }
        }
        else
        {
            Error.AgregarError("Semántico", "[ACCESO_OBJETO]", "Error de acceso.  No se puede aplicar la función Get a un elemento que no sea de tipo Collection.", fila, columna);
        }

        return response;
    }

    private object ValidarCollection_Size(object elemento)
    {
        object response = new Nulo();

        if(elemento is Map)
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
            Error.AgregarError("Semántico", "[ACCESO_OBJETO]", "Error de acceso.  No se puede aplicar la función Size a un elemento que no sea de tipo Collection.", fila, columna);
        }

        return response;
    }
}