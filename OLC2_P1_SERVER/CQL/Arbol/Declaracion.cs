
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Declaracion : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public TipoDato Tipo { get; set; }
    public Expresion Valor { get; set; }
    public List<string> ListaVariables { get; set; }

    public Declaracion(TipoDato tipo, List<string> lista_variables, int fila, int columna)
    {
        Tipo = tipo;
        this.fila = fila;
        Valor = new Nulo();
        this.columna = columna;
        ListaVariables = lista_variables;
    }

    public Declaracion(TipoDato tipo, List<string> lista_variables, Expresion valor, int fila, int columna)
    {
        Tipo = tipo;
        Valor = valor;
        this.fila = fila;
        this.columna = columna;
        ListaVariables = lista_variables;
    }

    public object Ejecutar(Entorno ent)
    {
        // Si el valor brindado en el constructor es igual a null significa que la declaración no termino con la igualacion a una expresion
        // por lo que a todos se les asignaría valor Nulo().

        if (Valor is Nulo)
        {
            foreach(string variable in ListaVariables)
            {
                object simbolo = ent.ObtenerVariable(variable);

                if (!(simbolo is Nulo))
                {
                    ent.Agregar(variable, new Variable(Tipo, variable, new Nulo()));
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[DECLARACION]", "Se intento declarar '" + variable + "'.  Una variable con el mismo nombre ya se encuentra en el entorno actual.", fila, columna);
                }
            }
        }
        else
        {
            for(int i = 0; i < ListaVariables.Count; i++)
            {
                string nombre_variable = ListaVariables.ElementAt(i);
                object simbolo = ent.ObtenerVariable(nombre_variable);

                if (simbolo is Nulo)
                {
                    // Valido que tipo de declaración viene.  Existen 3 tipos:
                    // 1. Expresion
                    // 2. Collections (Map, Set, List)
                    // 3. Objetos (UserTypes)

                    if(i.Equals(ListaVariables.Count - 1))
                    {
                        
                        
                        // Verifico si la Expresión que equivale a 'valor' es de tipo Estructura.  Esto indica que la variable esta siendo construida por medio de la palabra 'new'.
                        if (Valor is Estructura)
                        {
                            // Valido que el tipo de la Expresión sea el mismo del que fue declarado.
                            if (Valor.GetTipo(ent).GetRealTipo().Equals(Tipo.GetRealTipo()))
                            {
                                // Verifico si el tipo es OBJECT, ya que requiere una validación adicional que sería el nombre del objeto.
                                if (Tipo.GetRealTipo().Equals(TipoDato.Tipo.OBJECT))
                                {
                                    // Valido que el nombre del objeto sea el mismo del que fue declarado.
                                    if (Valor.GetTipo(ent).GetElemento().Equals(Valor.GetTipo(ent).GetElemento()))
                                    {
                                        // Una vez ha sido validado todo lo anterior, se procede a verificar si existe el UserType correspondiente para el objeto que se está creando.
                                        string nombre_user_type = (string)Tipo.GetElemento();
                                        object user_type = CQL.ObtenerUserType(nombre_user_type);

                                        // Si el UserType si existe, se procede a copiar la lista de atributos del UserType original para esta nueva instancia y se almacena en el entorno.
                                        if (!(user_type is null))
                                        {
                                            UserType objeto = (UserType)user_type;

                                            // Creo la lista que va a representar los diferentes atributos del objeto que se está creando.
                                            List<AtributoObjeto> listaAtrObj = BuildObjectAttributeList(objeto);

                                            // Agrego el objeto al entorno
                                            ent.Agregar(nombre_variable, new Variable(Tipo, nombre_variable, new Objeto(Tipo, listaAtrObj)));
                                        }
                                        else
                                        {
                                            CQL.AddLUPError("Semántico", "[DECLARACION]", "El UserType '" + nombre_user_type + "' con el que se está declarando la variable no existe en el sistema.", fila, columna);
                                        }
                                    }
                                    else
                                    {
                                        CQL.AddLUPError("Semántico", "[DECLARACION]", "Error de tipos.  Se está declarando un objeto de tipo '" + Tipo.GetRealTipo().ToString() + "', el cual no concuerda con el tipo de dato que devuelve la expresión ('" + Valor.GetTipo(ent).GetRealTipo().ToString() + "').", fila, columna);
                                    }
                                }
                                // De lo contrario, si el tipo es un MAP
                                else if (Tipo.GetRealTipo().Equals(TipoDato.Tipo.MAP))
                                {
                                    // Valido que GetElemento sea de tipo MapType, ya que de lo contrario, sería un error de declaración con new.
                                    if (Valor.GetTipo(ent).GetElemento() is MapType)
                                    {
                                        MapType tipoMap = (MapType)Valor.GetTipo(ent).GetElemento();

                                        // Valido que el tipo de dato de la clave (TipoIzq) sea de tipo primitivo, si no, hay que arrojar un error.
                                        if (tipoMap.ValidarTipoPrimitivoEnClave())
                                        {
                                            //TODO Verificar si no hay que hacer alguna otra validación con respecto al tipo de dato de la derecha.

                                            // Agrego el Map al entorno
                                            ent.Agregar(nombre_variable, new Variable(Tipo, nombre_variable, new Map(tipoMap.TipoIzq, tipoMap.TipoDer, fila, columna)));
                                        }
                                        else
                                        {
                                            CQL.AddLUPError("Semántico", "[DECLARACION]", "Error de tipos.  En la declaración de tipo 'new' con MAP, el tipo de dato de la clave debe de ser de tipo primitivo.", fila, columna);
                                        }
                                    }
                                    else
                                    {
                                        CQL.AddLUPError("Semántico", "[DECLARACION]", "Error de tipos.  La declaración de tipo 'new' con MAP recibe como parámetro dos tipos de dato.", fila, columna);
                                    }
                                }
                                // De lo contrario, si el tipo es un LIST
                                else if (Tipo.GetRealTipo().Equals(TipoDato.Tipo.LIST))
                                {
                                    // Valido que GetElemento sea de tipo ListType, ya que de lo contrario, sería un error de declaración con new.
                                    if (Valor.GetTipo(ent).GetElemento() is ListType)
                                    {
                                        ListType tipoList = (ListType)Valor.GetTipo(ent).GetElemento();
                                        // Agrego la List al entorno
                                        // TODO Verificar validaciones adicionales al tipo de dato de la lista
                                        ent.Agregar(nombre_variable, new Variable(Tipo, nombre_variable, new XList(tipoList.TipoDatoList, fila, columna)));
                                    }
                                    else
                                    {
                                        CQL.AddLUPError("Semántico", "[DECLARACION]", "Error de tipos.  La declaración de tipo 'new' con LIST recibe como parámetro un tipo de dato.", fila, columna);
                                    }
                                }
                                // De lo contrario, si el tipo es un SET
                                else if (Tipo.GetRealTipo().Equals(TipoDato.Tipo.SET))
                                {
                                    // Valido que GetElemento sea de tipo SetType, ya que de lo contrario, sería un error de declaración con new.
                                    if (Valor.GetTipo(ent).GetElemento() is SetType)
                                    {
                                        SetType tipoSet = (SetType)Valor.GetTipo(ent).GetElemento();
                                        // Agrego la Set al entorno
                                        // TODO Verificar validaciones adicionales al tipo de dato de la lista
                                        ent.Agregar(nombre_variable, new Variable(Tipo, nombre_variable, new XSet(tipoSet.TipoDatoSet, fila, columna)));
                                    }
                                    else
                                    {
                                        CQL.AddLUPError("Semántico", "[DECLARACION]", "Error de tipos.  La declaración de tipo 'new' con SET recibe como parámetro un tipo de dato.", fila, columna);
                                    }
                                }
                            }
                            else
                            {
                                CQL.AddLUPError("Semántico", "[DECLARACION]", "Error de tipos.  El tipo de dato de la expresión no es del mismo tipo del que fue declarada la variable.  (Recibido: " + Valor.GetTipo(ent).GetRealTipo().ToString() + " | Declarado: " + Tipo.GetRealTipo().ToString() + ")", fila, columna);
                            }
                        }
                        
                        

                        // De lo contrario, puede ser un map/list/set que está siendo igualado a un arreglo como tal.
                        // Valido si valor es de tipo CollectionValue, lo cual representa cuando un List/Map/Set esta siendo creado por medio de un arreglo de elementos.
                        else if (Valor is CollectionValue)
                        {
                            CollectionValue cv = (CollectionValue)Valor;

                            // Si el tipo de declaración fue MAP, CollectionValue debería contener MapArray distinto de null.
                            if (Tipo.GetRealTipo().Equals(TipoDato.Tipo.MAP))
                            {
                                object cvResp = cv.Ejecutar(ent);

                                if (cvResp is Map)
                                {
                                    ent.Agregar(nombre_variable, new Variable(Tipo, nombre_variable, (Map)cvResp));
                                }
                                else
                                {
                                    CQL.AddLUPError("Semántico", "[DECLARACION]", "Error de declaración.  La variable fue declarada de tipo MAP y la expresión recibida no concuerda con la estructura de la colección MAP.", fila, columna);
                                }
                            }


                            // Si el tipo de declaración fue un LIST, CollectionValue debería contener ListSetArray distinto de null.
                            else if (Tipo.GetRealTipo().Equals(TipoDato.Tipo.LIST))
                            {
                                cv.IsList = true;
                                object cvResp = cv.Ejecutar(ent);

                                if (cvResp is XList)
                                {
                                    ent.Agregar(nombre_variable, new Variable(Tipo, nombre_variable, (XList)cvResp));
                                }
                                else
                                {
                                    CQL.AddLUPError("Semántico", "[DECLARACION]", "Error de declaración.  La variable fue declarada de tipo LIST y la expresión recibida no concuerda con la estructura de la colección LIST.", fila, columna);
                                }
                            }


                            // Si el tipo de declaración fue un SET, CollectionValue debería contener ListSetArray distinto de null.
                            else if (Tipo.GetRealTipo().Equals(TipoDato.Tipo.SET))
                            {
                                cv.IsList = false;
                                object cvResp = cv.Ejecutar(ent);

                                if (cvResp is XSet)
                                {
                                    ent.Agregar(nombre_variable, new Variable(Tipo, nombre_variable, (XSet)cvResp));
                                }
                                else
                                {
                                    CQL.AddLUPError("Semántico", "[DECLARACION]", "Error de declaración.  La variable fue declarada de tipo SET y la expresión recibida no concuerda con la estructura de la colección SET.", fila, columna);
                                }
                            }

                            // De lo contrario, se debe arrojar un mensaje de error ya que no se puede declarar un CollectionValue a una variable que no sea de tipo Collection.
                            else
                            {
                                CQL.AddLUPError("Semántico", "[DECLARACION]", "Error de declaración.  No se puede asignar una estructura de tipo Collection a una variable que no fue declarada como Collection.", fila, columna);
                            }

                        }



                        // De lo contrario, puede ser un objeto que está siendo igualado a un arreglo como tal.
                        // Valido si valor es de tipo ObjectValue, lo cual representa cuando un objecto esta siendo creado por medio de un arreglo de elementos.
                        else if (Valor is ObjectValue)
                        {
                            ObjectValue object_value = (ObjectValue)Valor;

                            // 1. Verifico que ambos tipos sean iguales a Object.
                            if (Tipo.GetRealTipo().Equals(object_value.GetTipo(ent).GetRealTipo()))
                            {
                                // 2. Verifico que el tipo de objeto que fue declarado exista en la lista de UserTypes.
                                if (CQL.ExisteUserTypeEnBD(object_value.NombreObjeto))
                                {
                                    // 3. Verifico que el identificador despues del 'as' corresponda con el tipo de la declaración.
                                    if (CQL.CompararTiposDeObjeto(Tipo, object_value.GetTipo(ent)))
                                    {
                                        ent.Agregar(nombre_variable, new Variable(Tipo, nombre_variable, object_value.Ejecutar(ent)));
                                    }
                                }
                                else
                                {
                                    CQL.AddLUPError("Semántico", "[DECLARACION]", "Error de declaración.  El UserType con el que fue declarado el objeto no existe en la base de datos.", fila, columna);
                                }
                            }
                            else
                            {
                                CQL.AddLUPError("Semántico", "[DECLARACION]", "Error de tipos.  La variable fue declarada de tipo OBJETO y la expresión recibida no es de tipo Objeto.", fila, columna);
                            }
                        }



                        // Si no es ninguno de los anteriores, no queda otra opción que no sea una Expresión.
                        else
                        {
                            object value = Valor.Ejecutar(ent);
                            TipoDato valueType = Valor.GetTipo(ent);

                            if (Tipo.GetRealTipo().Equals(TipoDato.Tipo.STRING) || Tipo.GetRealTipo().Equals(TipoDato.Tipo.MAP) || Tipo.GetRealTipo().Equals(TipoDato.Tipo.SET) || Tipo.GetRealTipo().Equals(TipoDato.Tipo.LIST) || Tipo.GetRealTipo().Equals(TipoDato.Tipo.OBJECT))
                            {
                                if (valueType.GetRealTipo().Equals(Tipo.GetRealTipo()) || valueType.GetRealTipo().Equals(TipoDato.Tipo.NULO))
                                {
                                    ent.Agregar(nombre_variable, new Variable(Tipo, nombre_variable, value));
                                }
                                else
                                {
                                    CQL.AddLUPError("Semántico", "[DECLARACION]", "Error de tipos.  Un valor de tipo '" + valueType.GetRealTipo().ToString() + "' no puede ser asignado a una variable de tipo '" + Tipo.GetRealTipo().ToString() + "'.", fila, columna);
                                }
                            }
                            else
                            {
                                if (Tipo.GetRealTipo().Equals(valueType.GetRealTipo()))
                                {
                                    ent.Agregar(nombre_variable, new Variable(Tipo, nombre_variable, value));
                                }
                                else
                                {
                                    value = CasteoImplicito(Tipo, valueType, value);

                                    if (value is Nulo)
                                    {
                                        CQL.AddLUPError("Semántico", "[DECLARACION]", "Error de tipos.  El tipo de la variable no concuerda con el tipo de dato del valor de la expresión. (Recibido: " + valueType.GetRealTipo().ToString() + " | Declarado: " + Tipo.GetRealTipo().ToString() + ")", fila, columna);
                                    }
                                    else
                                    {
                                        ent.Agregar(nombre_variable, new Variable(Tipo, nombre_variable, value));
                                    }
                                }
                            }  
                        }
                    }
                    else
                    {
                        if (Tipo.GetRealTipo().Equals(TipoDato.Tipo.INT))
                        {
                            ent.Agregar(nombre_variable, new Variable(Tipo, nombre_variable, new Primitivo(0)));
                        }
                        else if (Tipo.GetRealTipo().Equals(TipoDato.Tipo.DOUBLE))
                        {
                            ent.Agregar(nombre_variable, new Variable(Tipo, nombre_variable, new Primitivo(0.0)));
                        }
                        else if (Tipo.GetRealTipo().Equals(TipoDato.Tipo.BOOLEAN))
                        {
                            ent.Agregar(nombre_variable, new Variable(Tipo, nombre_variable, new Primitivo(false)));
                        }
                        else
                        {
                            ent.Agregar(nombre_variable, new Variable(Tipo, nombre_variable, new Nulo()));
                        }
                    }
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[DECLARACION]", "Se intento declarar '" + nombre_variable + "'.  Una variable con el mismo nombre ya se encuentra en el entorno actual.", fila, columna);
                }
            }
        }

        return new Nulo();

    }

    private List<AtributoObjeto> BuildObjectAttributeList(UserType objeto)
    {
        List<AtributoObjeto> listaAtrObj = new List<AtributoObjeto>();

        foreach (AtributoUT ao in objeto.ListaAtributos)
        {
            if (ao.Tipo.GetRealTipo().Equals(TipoDato.Tipo.INT))
            {
                listaAtrObj.Add(new AtributoObjeto(ao.Tipo, ao.Identificador, 0));
            }
            else if (ao.Tipo.GetRealTipo().Equals(TipoDato.Tipo.DOUBLE))
            {
                listaAtrObj.Add(new AtributoObjeto(ao.Tipo, ao.Identificador, 0.0));
            }
            else if (ao.Tipo.GetRealTipo().Equals(TipoDato.Tipo.BOOLEAN))
            {
                listaAtrObj.Add(new AtributoObjeto(ao.Tipo, ao.Identificador, false));
            }
            else
            {
                listaAtrObj.Add(new AtributoObjeto(ao.Tipo, ao.Identificador, new Nulo()));
            }
        }

        return listaAtrObj;
    }

    private object CasteoImplicito(TipoDato tipoDeclaracion, TipoDato tipoValor, object valor)
    {
        if (Tipo.GetRealTipo().Equals(TipoDato.Tipo.INT) && tipoValor.GetRealTipo().Equals(TipoDato.Tipo.DOUBLE))
        {
            return Convert.ToInt32((double)valor);
        }
        else if (Tipo.GetRealTipo().Equals(TipoDato.Tipo.DOUBLE) && tipoValor.GetRealTipo().Equals(TipoDato.Tipo.INT))
        {
            return ((int)valor) * 1.0;
        }
        
        return new Nulo();
    }

}