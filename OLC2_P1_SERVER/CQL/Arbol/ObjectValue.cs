
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ObjectValue : Expresion
{
    private readonly int fila;
    private readonly int columna;
    public string NombreObjeto { get; set; }
    public List<Expresion> ListaExpresiones { get; set; }
    
    public ObjectValue(List<Expresion> lista_expresiones, string nombre_objeto, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        NombreObjeto = nombre_objeto;
        ListaExpresiones = lista_expresiones;
    }

    public object Ejecutar(Entorno ent)
    {
        // +-----------------------------------------------------------------------------------+
        // |                                       Nota                                        |
        // +-----------------------------------------------------------------------------------+
        // | La función Ejecutar debe retornar una clase Objeto que contenga todos los valores |
        // | pertenecientes a la ListaExpresiones siempre y cuando sean validos para el objeto |
        // | al que se desea colocar.                                                          |
        // +-----------------------------------------------------------------------------------+

        // 1. Primero obtengo la estructura original del objeto que se puede obtener de la lista de UserTypes.
        UserType ut = (UserType)CQL.ObtenerUserType(NombreObjeto);

        // 2. Valido que los tipos de dato obtenidos de la lista de expresiones concuerde con el
        //    tipo de dato que necesita el objeto.
        if (ValidateValuesOfObject(ut, ent))
        {
            // 3. Si todos los tipos de dato concuerdan, se procede a crear la lista de AtributoObjeto y posteriormente
            //    colocarlo dentro de una clase Objeto y luego retornarlo como respuesta.

            List<AtributoObjeto> listAtrObj = new List<AtributoObjeto>();

            for (int i = 0; i < ut.ListaAtributos.Count; i++)
            {
                Expresion exp = ListaExpresiones[i];
                AtributoUT aut = ut.ListaAtributos[i];
                listAtrObj.Add(new AtributoObjeto(exp.GetTipo(ent), aut.Identificador, exp.Ejecutar(ent)));
            }

            return new Objeto(GetTipo(ent), listAtrObj);
        }
        else
        {
            CQL.AddLUPError("Semántico", "[OBJECT_VALUE]", "Error de tipos.  Alguno de los tipos definidos en la lista de expresiones no concuerda con los tipos del objeto.", fila, columna);
        }

        return new Nulo();
    }

    public TipoDato GetTipo(Entorno ent)
    {
        return new TipoDato(TipoDato.Tipo.OBJECT, NombreObjeto);
    }

    public bool ValidateValuesOfObject(UserType ut, Entorno ent)
    {
        // 1. Itero sobre cada uno de los atributos para verificar los tipos con los definidos en la lista de expresiones.
        for (int i = 0; i < ut.ListaAtributos.Count; i++)
        {
            TipoDato atrType = ut.ListaAtributos[i].Tipo;
            TipoDato valType = ListaExpresiones[i].GetTipo(ent);
            object valValue = ListaExpresiones[i].Ejecutar(ent);

            // 2. Se valida si el tipo de dato del atributo es cualquiera de los detallados abajo ya que estos pueden recibir dos valores:
            // - Uno del mismo tipo
            // - Un valor nulo.
            if
            (
                atrType.GetRealTipo().Equals(TipoDato.Tipo.STRING) ||
                atrType.GetRealTipo().Equals(TipoDato.Tipo.DATE) ||
                atrType.GetRealTipo().Equals(TipoDato.Tipo.TIME) ||
                atrType.GetRealTipo().Equals(TipoDato.Tipo.MAP) ||
                atrType.GetRealTipo().Equals(TipoDato.Tipo.SET) ||
                atrType.GetRealTipo().Equals(TipoDato.Tipo.LIST)
            )
            {
                if (!(valType.GetRealTipo().Equals(atrType.GetRealTipo()) || valType.GetRealTipo().Equals(TipoDato.Tipo.NULO)))
                {
                    return false;
                }
            }

            // 2. Se valida si el tipo de dato del atributo es de tipo Objeto ya que se tiene que validar que se necesita validar adicionalmente
            // el tipo de UserType que es y también puede recibir un valor nulo.
            else if (atrType.GetRealTipo().Equals(TipoDato.Tipo.OBJECT))
            {
                if (!((valType.GetRealTipo().Equals(atrType.GetRealTipo()) && CQL.CompararTiposDeObjeto(valType, atrType)) || valType.GetRealTipo().Equals(TipoDato.Tipo.NULO)))
                {
                    return false;
                }
            }

            // 2. Para los demás valores, solo se valida que el tipo coincida.
            else
            {
                if (!valType.GetRealTipo().Equals(atrType.GetRealTipo()))
                {
                    object value = CasteoImplicito(atrType, valType, valValue);

                    if (value is Nulo || value is Exception)
                    {
                        return false;
                    }
                    else
                    {
                        ListaExpresiones[i] = new Primitivo(value);
                    }
                }
            }
        }

        return true;
    }

    private object CasteoImplicito(TipoDato tipoDeclaracion, TipoDato tipoValor, object valor)
    {
        if (tipoDeclaracion.GetRealTipo().Equals(TipoDato.Tipo.INT) && tipoValor.GetRealTipo().Equals(TipoDato.Tipo.DOUBLE))
        {
            return Convert.ToInt32((double)valor);
        }
        else if (tipoDeclaracion.GetRealTipo().Equals(TipoDato.Tipo.DOUBLE) && tipoValor.GetRealTipo().Equals(TipoDato.Tipo.INT))
        {
            return Convert.ToDouble((int)valor);
        }

        return new Nulo();
    }
}