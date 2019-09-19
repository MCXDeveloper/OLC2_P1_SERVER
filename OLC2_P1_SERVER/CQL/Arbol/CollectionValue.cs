using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CollectionValue : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public bool IsList { get; set; } // Bandera que me indica si el valor a retornar debería de ser una List (true) o un Set (false).
    public List<AtributosMap> MapArray { get; set; }
    public List<Expresion> ListSetArray { get; set; }

    public CollectionValue(List<AtributosMap> map_array, int fila, int columna)
    {
        this.fila = fila;
        ListSetArray = null;
        MapArray = map_array;
        this.columna = columna;
    }

    public CollectionValue(List<Expresion> list_set_array, int fila, int columna)
    {
        MapArray = null;
        this.fila = fila;
        this.columna = columna;
        ListSetArray = list_set_array;
    }

    public object Ejecutar(Entorno ent)
    {
        if (!(MapArray is null))
        {
            // 1. Verifico que todas y cada una de las validaciones correspondientes a Map sean satisfactorias.
            if (ValidarTiposDeMap(MapArray, ent, fila, columna))
            {
                // 2. Si todo se cumple, procedo a armar la Collection Map y lo retorno.
                MapType tipoMap = ObtenerMapType(ent);
                Map mapita = new Map(tipoMap.TipoIzq, tipoMap.TipoDer, fila, columna);

                foreach (AtributosMap amap in MapArray)
                {
                    mapita.Insert(amap.Key.Ejecutar(ent), amap.Value.Ejecutar(ent));
                }

                return mapita;
            }
            else
            {
                CQL.AddLUPError("Semántico", "[COLLECTION_VALUE]", "Error de tipos.  El arreglo que representa el valor de la declaración del MAP difiere en los tipos de dato de los elementos que contiene.", fila, columna);
            }
        }
        else if (!(ListSetArray is null))
        {
            if(IsList)
            {
                if (ValidarTiposList(ListSetArray, ent, fila, columna))
                {
                    ListType tipoList = ObtenerListType(ent);
                    XList listita = new XList(tipoList.TipoDatoList, fila, columna);

                    foreach (Expresion ex in ListSetArray)
                    {
                        listita.Insert(ex.Ejecutar(ent));
                    }

                    return listita;
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[COLLECTION_VALUE]", "Error de tipos.  El arreglo que representa el valor de la declaración del LIST difiere en los tipos de dato de los elementos que contiene.", fila, columna);
                }
            }
            else
            {
                if (ValidarTiposSet(ListSetArray, ent, fila, columna))
                {
                    SetType tipoSet = ObtenerSetType(ent);
                    XSet setsito = new XSet(tipoSet.TipoDatoSet, fila, columna);

                    foreach (Expresion ex in ListSetArray)
                    {
                        setsito.Insert(ex.Ejecutar(ent));
                    }

                    return setsito;
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[COLLECTION_VALUE]", "Error de tipos.  El arreglo que representa el valor de la declaración del SET difiere en los tipos de dato de los elementos que contiene.", fila, columna);
                }
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[COLLECTION_VALUE]", "Error. No se especificó el tipo de CollectionValue a definir.", fila, columna);
        }

        return new Nulo();
    }

    public TipoDato GetTipo(Entorno ent)
    {
        object valor = Ejecutar(ent);

        if (valor is Map)
        {
            return new TipoDato(TipoDato.Tipo.MAP);
        }
        else if (valor is XList)
        {
            return new TipoDato(TipoDato.Tipo.LIST);
        }
        else if (valor is XSet)
        {
            return new TipoDato(TipoDato.Tipo.SET);
        }
        else
        {
            return new TipoDato(TipoDato.Tipo.NULO);
        }
    }

    // El primer atributo de 'ValidarTiposDeMap', map_elements, es exactamente igual a MapArray.  Se 
    // agrego como parametro ya que esta funcion es recursiva para cuando existen colecciones dentro de
    // colecciones.

    public MapType ObtenerMapType(Entorno ent)
    {
        TipoDato KeyType = MapArray[0].Key.GetTipo(ent);
        TipoDato ValueType = MapArray[0].Value.GetTipo(ent);
        return new MapType(KeyType, ValueType);
    }

    public ListType ObtenerListType(Entorno ent)
    {
        TipoDato ExpType = ListSetArray[0].GetTipo(ent);
        return new ListType(ExpType);
    }

    public SetType ObtenerSetType(Entorno ent)
    {
        TipoDato ExpType = ListSetArray[0].GetTipo(ent);
        return new SetType(ExpType);
    }

    public bool ValidarTiposDeMap(List<AtributosMap> map_elements, Entorno ent, int fila, int columna)
    {
        // En esta función se tiene que validar lo siguiente:
        // 1. Que todas los keys sean de tipo primitivo (todas del mismo tipo) y que no se repitan.
        // 2. Que todos los values sean del mismo tipo (tomar como referencia el primero) y si son 
        // de tipo Map/List/Set, verificar sus tipos internos para que se cumpla con el tipado.

        // Primera validación:
        if(ValidateKeysOfMap(map_elements, ent, fila, columna))
        {
            // Segunda validación:
            if(ValidateValuesOfMap(map_elements, ent, fila, columna))
            {
                return true;
            }
        }

        return false;
    }

    public bool ValidarTiposList(List<Expresion> list_elements, Entorno ent, int fila, int columna)
    {
        // Como primer punto vamos a tomar el primer elemento de la lista para que nos indique el tipo de la misma.
        TipoDato type = list_elements[0].GetTipo(ent);

        // Se procede a verificar que todos los elementos de la lista sean del mismo tipo.
        if(ValidateSameType(list_elements, type, ent))
        {
            return true;
        }
        else
        {
            CQL.AddLUPError("Semántico", "[COLLECTION_VALUE]", "Error de tipos.  Todos los elementos correspondientes deben ser del mismo tipo en la instrucción LIST.", fila, columna);
        }

        return false;
    }

    public bool ValidarTiposSet(List<Expresion> list_elements, Entorno ent, int fila, int columna)
    {
        // Como primer punto vamos a tomar el primer elemento de la lista para que nos indique el tipo de la misma.
        TipoDato type = list_elements[0].GetTipo(ent);

        // Se procede a verificar que todos los elementos de la lista sean del mismo tipo.
        if (ValidateSameType(list_elements, type, ent))
        {
            // Al ser la instruccion SET se necesita que ningún valor esté repetido en la lista, por lo que se procede a validar esa cuestión.
            object _valor_ = list_elements[0].Ejecutar(ent);

            if(ValidateUniqueValues(list_elements, _valor_, ent))
            {
                return true;
            }
            else
            {
                CQL.AddLUPError("Semántico", "[COLLECTION_VALUE]", "Los valores correspondientes dentro de la colección SET no se pueden repetir.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[COLLECTION_VALUE]", "Error de tipos.  Todos los elementos correspondientes deben ser del mismo tipo en la instrucción SET.", fila, columna);
        }

        return false;
    }

    private bool ValidateKeysOfMap(List<AtributosMap> RepresentacionMap, Entorno ent, int fila, int columna)
    {
        // Se tomará como punto de partida el primer elemento de la lista para verificar el resto de tipos y valores para las 'keys'.
        object KeyValue = RepresentacionMap[0].Key.Ejecutar(ent);
        TipoDato KeyType = RepresentacionMap[0].Key.GetTipo(ent);

        // Una vez teniendo el tipo de dato del primer elemento, se procede a verificar que por lo menos ese primer elemento
        // sea de tipo primitivo, si no, se debé mostrar un mensaje de error.
        if (MAP_ValidatePrimitiveKey(KeyType.GetRealTipo()))
        {
            // Si por lo menos el primer elemento de la lista es de tipo de dato primitivo, se procede a verificar que todos
            // los 'keys' de esa lista sean del mismo tipo.
            if (MAP_ValidateSameKeyType(RepresentacionMap, KeyType, ent))
            {
                // Una vez validado que todos sean del mismo tipo, hay que verificar que ningún valor correspondiente a las 'keys'
                // se repita dentro de la lista. Esto es un error ya que Map una colección de clave-valor donde la clave no se puede repetir.
                if (MAP_ValidateUniqueKeyValues(RepresentacionMap, KeyValue, ent))
                {
                    // Aquí finaliza la primera etapa de validación de Maps: Keys.
                    return true;
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[COLLECTION_VALUE]", "Los valores correspondientes a las 'claves' dentro de la colección MAP no se pueden repetir.", fila, columna);
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[COLLECTION_VALUE]", "Error de tipos.  Todos los elementos correspondientes a la 'clave' deben ser del mismo tipo en la instrucción MAP.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[COLLECTION_VALUE]", "Error de tipos.  El atributo clave de una colección MAP debe ser de tipo primitivo.  El valor que actualmente tiene es: '" + KeyType.GetRealTipo().ToString() + "'.", fila, columna);
        }

        return false;
    }

    private bool ValidateValuesOfMap(List<AtributosMap> RepresentacionMap, Entorno ent, int fila, int columna)
    {
        // Se tomará como punto de partida el primer elemento de la lista para verificar el resto de tipos y valores para los 'values'.
        object ValueVal = RepresentacionMap[0].Value.Ejecutar(ent);
        TipoDato ValueType = RepresentacionMap[0].Value.GetTipo(ent);

        // Primero se procede a verificar si el primer elemento es de tipo Collection, ya que requiere unas validaciones adicionales.
        // En el caso de que no lo fuese, únicamente se valida que todos los elementos sean del mismo tipo.
        if (ValueType.GetRealTipo().Equals(TipoDato.Tipo.MAP))
        {
            // Una vez validado lo anterior, existen 2 vías por las que puede venir un Collection como Expresion:
            // 1. Como una instancia de CollectionValue (usando arreglo de valores)
            // 2. Como una instancia de Estructura (usando new)

            // En el caso de que sea un CollectionValue, únicamente se debe de obtener el valor de MapArray y reutilizar
            // la función 'ValidarTiposDeMap' recursivamente.  Al finalizar la recursividad, devolverá un valor y a sea
            // verdadero o falso si se cumplió con los tipados en los datos.
            if (ValueVal is CollectionValue)
            {
                return ValidarTiposDeMap(((CollectionValue)ValueVal).MapArray, ent, fila, columna);
            }
            // En el caso de que sea una Estructura, se deben de validar que los tipos de datos de MAP definidos en Estructura
            // concuerden con los que ha sido declarado el primer elemento de la lista.
            else if (ValueVal is Estructura)
            {
                // TODO | CollectionValue | Validar que sucede cuando viene una estructura dentro de las llaves
                return true;
            }
        }
        else if (ValueType.GetRealTipo().Equals(TipoDato.Tipo.LIST))
        {
            // TODO | CollectionValue | Validar que sucede cuando vienen LISTAS dentro de las llaves
        }
        else if (ValueType.GetRealTipo().Equals(TipoDato.Tipo.SET))
        {
            // TODO | CollectionValue | Validar que sucede cuando vienen SETS dentro de las llaves
        }
        else
        {
            if (MAP_ValidateSameValueType(RepresentacionMap, ValueType, ent))
            {
                return true;
            }
            else
            {
                CQL.AddLUPError("Semántico", "[COLLECTION_VALUE]", "Los valores correspondientes a los 'values' dentro de la colección MAP no pueden ser de diferentes tipos.", fila, columna);
            }
        }
        
        return false;
    }

    private bool MAP_ValidateCollectionValue(TipoDato.Tipo FirstValueType)
    {
        return (
            FirstValueType.Equals(TipoDato.Tipo.MAP)  ||
            FirstValueType.Equals(TipoDato.Tipo.LIST) ||
            FirstValueType.Equals(TipoDato.Tipo.SET)
        );
    }

    private bool MAP_ValidatePrimitiveKey(TipoDato.Tipo FirstKeyType)
    {
        return (
            FirstKeyType.Equals(TipoDato.Tipo.INT)      ||
            FirstKeyType.Equals(TipoDato.Tipo.DOUBLE)   ||
            FirstKeyType.Equals(TipoDato.Tipo.STRING)   ||
            FirstKeyType.Equals(TipoDato.Tipo.BOOLEAN)  ||
            FirstKeyType.Equals(TipoDato.Tipo.DATE)     ||
            FirstKeyType.Equals(TipoDato.Tipo.TIME)
        );
    }

    private bool MAP_ValidateSameKeyType(List<AtributosMap> RepresentacionMap, TipoDato FirstKeyDataType, Entorno ent)
    {
        return !(RepresentacionMap.Any(rm => !rm.Key.GetTipo(ent).GetRealTipo().Equals(FirstKeyDataType.GetRealTipo())));
    }

    private bool MAP_ValidateSameValueType(List<AtributosMap> RepresentacionMap, TipoDato FirstValueDataType, Entorno ent)
    {
        return !(RepresentacionMap.Any(rm => !rm.Value.GetTipo(ent).GetRealTipo().Equals(FirstValueDataType.GetRealTipo())));
    }

    private bool MAP_ValidateUniqueKeyValues(List<AtributosMap> RepresentacionMap, object FirstKeyValue, Entorno ent)
    {
        return !RepresentacionMap.GroupBy(x => x.Key.Ejecutar(ent)).Any(e => e.Count() > 1);
    }

    private bool ValidateSameType(List<Expresion> RepresentacionListSet, TipoDato FirstElementDataType, Entorno ent)
    {
        return !(RepresentacionListSet.Any(rm => !rm.GetTipo(ent).GetRealTipo().Equals(FirstElementDataType.GetRealTipo())));
    }

    private bool ValidateUniqueValues(List<Expresion> RepresentacionListSet, object FirstElementDataValue, Entorno ent)
    {
        return !RepresentacionListSet.GroupBy(x => x.Ejecutar(ent)).Any(e => e.Count() > 1);
    }
}