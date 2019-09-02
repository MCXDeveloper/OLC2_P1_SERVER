using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Entorno;

public class Declaracion : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    private readonly TipoDato tipo;
    private readonly string nombreObj;
    private readonly Expresion valor;
    private readonly List<string> lista_variables;
    
    public Declaracion(TipoDato tipo, List<string> lista_variables, int fila, int columna)
    {
        this.tipo = tipo;
        this.fila = fila;
        this.nombreObj = null;
        this.columna = columna;
        this.valor = new Nulo();
        this.lista_variables = lista_variables;
    }

    public Declaracion(TipoDato tipo, List<string> lista_variables, Expresion valor, int fila, int columna)
    {
        this.tipo = tipo;
        this.fila = fila;
        this.valor = valor;
        this.nombreObj = null;
        this.columna = columna;
        this.lista_variables = lista_variables;
    }

    public object Ejecutar(Entorno ent)
    {
        // Si el valor brindado en el constructor es igual a null significa que la declaración no termino con la igualacion a una expresion
        // por lo que a todos se les asignaría valor Nulo().

        if (valor is Nulo)
        {
            foreach(string variable in lista_variables)
            {
                object simbolo = ent.ObtenerVariable(variable);

                if (!(simbolo is Nulo))
                {
                    if (nombreObj is null)
                    {
                        ent.Agregar(variable, new Variable(tipo, variable, new Nulo()));
                    }
                    else
                    {
                        ent.Agregar(variable, new Variable(nombreObj, variable, new Nulo()));
                    }   
                }
                else
                {
                    Error.AgregarError("Semántico", "[DECLARACION]", "Se intento declarar '" + variable + "'.  Una variable con el mismo nombre ya se encuentra en el entorno actual.", fila, columna);
                }
            }
        }
        else
        {
            for(int i = 0; i < lista_variables.Count; i++)
            {
                string nombre_variable = lista_variables.ElementAt(i);
                object simbolo = ent.ObtenerVariable(nombre_variable);

                if (!(simbolo is Nulo))
                {
                    // Valido que tipo de declaración viene.  Existen 3 tipos:
                    // 1. Expresion
                    // 2. Collections (Map, Set, List)
                    // 3. Objetos (UserTypes)

                    if(i.Equals(lista_variables.Count - 1))
                    {
                        // Verifico si la Expresión que equivale a 'valor' es de tipo Estructura.  Esto indica que la variable esta siendo construida por medio de la palabra 'new'.
                        if(valor is Estructura)
                        {
                            // Valido que el tipo de la Expresión sea el mismo del que fue declarado.
                            if (valor.GetTipo(ent).GetRealTipo().Equals(tipo.GetRealTipo()))
                            {
                                // Verifico si el tipo es OBJECT, ya que requiere una validación adicional que sería el nombre del objeto.
                                if (tipo.GetRealTipo().Equals(TipoDato.Tipo.OBJECT))
                                {
                                    // Valido que el nombre del objeto sea el mismo del que fue declarado.
                                    if (valor.GetTipo(ent).GetElemento().Equals(valor.GetTipo(ent).GetElemento()))
                                    {
                                        // Una vez ha sido validado todo lo anterior, se procede a verificar si existe el UserType correspondiente para el objeto que se está creando.
                                        string nombre_user_type = (string)tipo.GetElemento();
                                        object user_type = ent.ObtenerUserType(nombre_user_type);

                                        // Si el UserType si existe, se procede a copiar la lista de atributos del UserType original para esta nueva instancia y se almacena en el entorno.
                                        if (!(user_type is Nulo))
                                        {
                                            UserType objeto = (UserType)user_type;
                                            
                                            // Creo la lista que va a representar los diferentes atributos del objeto que se está creando.
                                            List<AtributoObjeto> listaAtrObj = BuildObjectAttributeList(objeto);
                                            
                                            // Agrego el objeto al entorno
                                            ent.Agregar(nombre_variable, new Variable(tipo, nombre_variable, new Objeto(tipo, listaAtrObj)));
                                        }
                                        else
                                        {
                                            Error.AgregarError("Semántico", "[DECLARACION]", "El UserType '"+ nombre_user_type +"' con el que se está declarando la variable no existe en el sistema.", fila, columna);
                                        }
                                    }
                                    else
                                    {
                                        Error.AgregarError("Semántico", "[DECLARACION]", "Error de tipos.  Se está declarando un objeto de tipo '" + tipo.GetRealTipo().ToString() + "', el cual no concuerda con el tipo de dato que devuelve la expresión ('" + valor.GetTipo(ent).GetRealTipo().ToString() + "').", fila, columna);
                                    }
                                }
                                // De lo contrario, si el tipo es un MAP
                                else if (tipo.GetRealTipo().Equals(TipoDato.Tipo.MAP))
                                {
                                    // Valido que GetElemento sea de tipo MapType, ya que de lo contrario, sería un error de declaración con new.
                                    if(valor.GetTipo(ent).GetElemento() is MapType)
                                    {
                                        MapType tipoMap = (MapType)valor.GetTipo(ent).GetElemento();

                                        // Valido que el tipo de dato de la clave (TipoIzq) sea de tipo primitivo, si no, hay que arrojar un error.
                                        if(tipoMap.ValidarTipoPrimitivoEnClave())
                                        {
                                            //TODO Verificar si no hay que hacer alguna otra validación con respecto al tipo de dato de la derecha.
                                            
                                            // Agrego el Map al entorno
                                            ent.Agregar(nombre_variable, new Variable(tipo, nombre_variable, new Map(tipoMap.TipoIzq, tipoMap.TipoDer, fila, columna)));
                                        }
                                        else
                                        {
                                            Error.AgregarError("Semántico", "[DECLARACION]", "Error de tipos.  En la declaración de tipo 'new' con MAP, el tipo de dato de la clave debe de ser de tipo primitivo.", fila, columna);
                                        }
                                    }
                                    else
                                    {
                                        Error.AgregarError("Semántico", "[DECLARACION]", "Error de tipos.  La declaración de tipo 'new' con MAP recibe como parámetro dos tipos de dato.", fila, columna);
                                    }
                                }
                                // De lo contrario, si el tipo es un LIST
                                else if (tipo.GetRealTipo().Equals(TipoDato.Tipo.LIST))
                                {
                                    // Valido que GetElemento sea de tipo ListType, ya que de lo contrario, sería un error de declaración con new.
                                    if (valor.GetTipo(ent).GetElemento() is ListType)
                                    {
                                        ListType tipoList = (ListType)valor.GetTipo(ent).GetElemento();
                                        // Agrego la List al entorno
                                        // TODO Verificar validaciones adicionales al tipo de dato de la lista
                                        ent.Agregar(nombre_variable, new Variable(tipo, nombre_variable, new XList(tipoList.TipoDatoList, fila, columna)));
                                    }
                                    else
                                    {
                                        Error.AgregarError("Semántico", "[DECLARACION]", "Error de tipos.  La declaración de tipo 'new' con LIST recibe como parámetro un tipo de dato.", fila, columna);
                                    }
                                }
                                // De lo contrario, si el tipo es un SET
                                else if (tipo.GetRealTipo().Equals(TipoDato.Tipo.SET))
                                {
                                    // Valido que GetElemento sea de tipo SetType, ya que de lo contrario, sería un error de declaración con new.
                                    if (valor.GetTipo(ent).GetElemento() is SetType)
                                    {
                                        SetType tipoSet = (SetType)valor.GetTipo(ent).GetElemento();
                                        // Agrego la Set al entorno
                                        // TODO Verificar validaciones adicionales al tipo de dato de la lista
                                        ent.Agregar(nombre_variable, new Variable(tipo, nombre_variable, new XSet(tipoSet.TipoDatoSet, fila, columna)));
                                    }
                                    else
                                    {
                                        Error.AgregarError("Semántico", "[DECLARACION]", "Error de tipos.  La declaración de tipo 'new' con SET recibe como parámetro un tipo de dato.", fila, columna);
                                    }
                                }
                                // De lo contrario, si es una expresión normal.
                                else
                                {
                                    //TODO validar esta mierda en la declaracion
                                }
                            }
                            else
                            {
                                Error.AgregarError("Semántico", "[DECLARACION]", "Error de tipos.  El tipo de dato de la expresión no es del mismo tipo del que fue declarada la variable.  (Recibido: " + valor.GetTipo(ent).GetRealTipo().ToString() + " | Declarado: " + tipo.GetRealTipo().ToString() + ")", fila, columna);
                            }
                        }
                        // De lo contrario, puede ser una expresión normal ó un objeto/map/list/set que está siendo igualado a un arreglo como tal.
                        else
                        {
                            // TODO hacer la declaracion cuando las mierdas estas vienen igualadas a una mierda entre llave toda culera :v
                        }
                    }
                    else
                    {
                        if (tipo.GetRealTipo().Equals(TipoDato.Tipo.INT))
                        {
                            ent.Agregar(nombre_variable, new Variable(tipo, nombre_variable, new Primitivo(0)));
                        }
                        else if (tipo.GetRealTipo().Equals(TipoDato.Tipo.DOUBLE))
                        {
                            ent.Agregar(nombre_variable, new Variable(tipo, nombre_variable, new Primitivo(0.0)));
                        }
                        else if (tipo.GetRealTipo().Equals(TipoDato.Tipo.BOOLEAN))
                        {
                            ent.Agregar(nombre_variable, new Variable(tipo, nombre_variable, new Primitivo(false)));
                        }
                        else
                        {
                            ent.Agregar(nombre_variable, new Variable(tipo, nombre_variable, new Nulo()));
                        }
                    }
                }
                else
                {
                    Error.AgregarError("Semántico", "[DECLARACION]", "Se intento declarar '" + nombre_variable + "'.  Una variable con el mismo nombre ya se encuentra en el entorno actual.", fila, columna);
                }
            }
        }

        return new Nulo();

    }

    private List<AtributoObjeto> BuildObjectAttributeList(UserType objeto)
    {
        List<AtributoObjeto> listaAtrObj = new List<AtributoObjeto>();

        foreach (AtributoUT ao in objeto.GetListaAtributos())
        {
            if (ao.GetTipoAtributo().Equals(TipoDato.Tipo.INT))
            {
                listaAtrObj.Add(new AtributoObjeto(ao.Tipo, ao.Identificador, 0));
            }
            else if (ao.GetTipoAtributo().Equals(TipoDato.Tipo.DOUBLE))
            {
                listaAtrObj.Add(new AtributoObjeto(ao.Tipo, ao.Identificador, 0.0));
            }
            else if (ao.GetTipoAtributo().Equals(TipoDato.Tipo.BOOLEAN))
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
}