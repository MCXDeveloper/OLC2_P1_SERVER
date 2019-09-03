using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CollectionValue : Expresion
{
    public List<AtributosMap> MapArray { get; set; }
    public List<Expresion> ListSetArray { get; set; }

    public CollectionValue(List<AtributosMap> map_array)
    {
        ListSetArray = null;
        MapArray = map_array;
    }

    public CollectionValue(List<Expresion> list_set_array)
    {
        MapArray = null;
        ListSetArray = list_set_array;
    }

    public object Ejecutar(Entorno ent)
    {
        throw new NotImplementedException();
    }

    public TipoDato GetTipo(Entorno ent)
    {
        throw new NotImplementedException();
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
            Error.AgregarError("Semántico", "[COLLECTION_VALUE]", "Error de tipos.  Todos los elementos correspondientes deben ser del mismo tipo en la instrucción LIST.", fila, columna);
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
                Error.AgregarError("Semántico", "[COLLECTION_VALUE]", "Los valores correspondientes dentro de la colección SET no se pueden repetir.", fila, columna);
            }
        }
        else
        {
            Error.AgregarError("Semántico", "[COLLECTION_VALUE]", "Error de tipos.  Todos los elementos correspondientes deben ser del mismo tipo en la instrucción LIST.", fila, columna);
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
                    Error.AgregarError("Semántico", "[COLLECTION_VALUE]", "Los valores correspondientes a las 'claves' dentro de la colección MAP no se pueden repetir.", fila, columna);
                }
            }
            else
            {
                Error.AgregarError("Semántico", "[COLLECTION_VALUE]", "Error de tipos.  Todos los elementos correspondientes a la 'clave' deben ser del mismo tipo en la instrucción MAP.", fila, columna);
            }
        }
        else
        {
            Error.AgregarError("Semántico", "[COLLECTION_VALUE]", "Error de tipos.  El atributo clave de una colección MAP debe ser de tipo primitivo.  El valor que actualmente tiene es: '" + KeyType.GetRealTipo().ToString() + "'.", fila, columna);
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
                Error.AgregarError("Semántico", "[COLLECTION_VALUE]", "Los valores correspondientes a los 'values' dentro de la colección MAP no pueden ser de diferentes tipos.", fila, columna);
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
        return !(RepresentacionMap.Any(rm => !rm.Key.GetTipo(ent).Equals(FirstKeyDataType)));
    }

    private bool MAP_ValidateSameValueType(List<AtributosMap> RepresentacionMap, TipoDato FirstValueDataType, Entorno ent)
    {
        return !(RepresentacionMap.Any(rm => !rm.Value.GetTipo(ent).Equals(FirstValueDataType)));
    }

    private bool MAP_ValidateUniqueKeyValues(List<AtributosMap> RepresentacionMap, object FirstKeyValue, Entorno ent)
    {
        return !(RepresentacionMap.Any(rm => !rm.Key.Ejecutar(ent).Equals(FirstKeyValue)));
    }

    private bool ValidateSameType(List<Expresion> RepresentacionListSet, TipoDato FirstElementDataType, Entorno ent)
    {
        return !(RepresentacionListSet.Any(rm => !rm.GetTipo(ent).Equals(FirstElementDataType)));
    }

    private bool ValidateUniqueValues(List<Expresion> RepresentacionListSet, object FirstElementDataValue, Entorno ent)
    {
        return !(RepresentacionListSet.Any(rm => !rm.Ejecutar(ent).Equals(FirstElementDataValue)));
    }
}