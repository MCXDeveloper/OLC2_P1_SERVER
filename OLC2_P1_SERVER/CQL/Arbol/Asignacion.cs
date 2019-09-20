
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Asignacion : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public string Variable { get; set; }
    public Expresion Valor { get; set; }
    public AccesoObjeto ListaAcceso { get; set; }
    public TipoAsignacion TipoAsig { get; set; }
    
    public enum TipoAsignacion
    {
        AS_SUMA,
        AS_RESTA,
        AS_NORMAL,
        AS_DIVISION,
        AS_MULTIPLICACION
    }

    public Asignacion(string variable, TipoAsignacion tipo_asignacion, Expresion valor, int fila, int columna)
    {
        Valor = valor;
        this.fila = fila;
        ListaAcceso = null;
        Variable = variable;
        this.columna = columna;
        TipoAsig = tipo_asignacion;
    }

    public Asignacion(AccesoObjeto lista_acceso, TipoAsignacion tipo_asignacion, Expresion valor, int fila, int columna)
    {
        Valor = valor;
        this.fila = fila;
        this.columna = columna;
        TipoAsig = tipo_asignacion;
        ListaAcceso = lista_acceso;
    }
    
    public object Ejecutar(Entorno ent)
    {

        // +-------------------------------------------------------------------------------------------------------+
        // |                                                 Nota                                                  |
        // +-------------------------------------------------------------------------------------------------------+
        // | Para realizar de forma correcta la asignación de variables, se deben seguir los siguientes pasos:     |
        // | 1. Tener en cuenta que hay dos tipos de asignaciones: a variable normal o a lista de acceso.          |
        // | 2. En caso de que fuese una variable normal, tomar en cuenta los siguientes aspectos:                 |
        // |    a. Que la variable a la que se le desea asignar el valor exista en el entorno.                     |
        // |    b. Que la variable a la que se le desea asignar el valor tiene que ser del mismo tipo que el valor |
        // |    de la expresión.                                                                                   |
        // |    c. Validar que el tipo de asignación sea distinto de igual (=) si y solo sí la variable y su valor |
        // |    sean de tipo int/double.                                                                           |
        // | 3. En caso de que fuese una lista de accesos, tomar en cuenta los siguientes aspectos:                |
        // |    a. La variable de la que parte la lista de acceso debe existir en el entorno.                      |
        // |    b. El atributo final de la lista de acceso debe existir y poder aceptar un valor.                  |
        // |    c. El atributo final debe ser del mismo tipo que el valor de la expresión.                         |
        // |    d. Validar que el tipo de asignación sea distinto de igual (=) sí y solo sí la variable y su valor |
        // |    sean de tipo int/double.                                                                           |
        // +-------------------------------------------------------------------------------------------------------+
        
        if (ListaAcceso is null)
        {
            AsignacionVariableNormal(ent);
        }
        else
        {
            AsignacionAccesoObjeto(ent);
        }

        return new Nulo();
    }
    
    private void AsignacionVariableNormal(Entorno ent)
    {
        // 1. Verifico que la variable proporcionada exista en el entorno.
        object simbolo = ent.ObtenerVariable(Variable);

        if (!(simbolo is Nulo))
        {
            Variable elemento = (Variable)simbolo;
            TipoDato ValorType = ObtenerTipoDatoDeValor(elemento, ent);

            if (elemento.GetTipo().Equals(TipoDato.Tipo.STRING) || elemento.GetTipo().Equals(TipoDato.Tipo.MAP) || elemento.GetTipo().Equals(TipoDato.Tipo.SET) || elemento.GetTipo().Equals(TipoDato.Tipo.LIST) || elemento.GetTipo().Equals(TipoDato.Tipo.OBJECT))
            {
                if (ValorType.GetRealTipo().Equals(elemento.GetTipo()) || ValorType.GetRealTipo().Equals(TipoDato.Tipo.NULO))
                {
                    RealizarAsignacion(elemento, ValorType, ent);
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[ASIGNACION]", "Error de tipos.  Un valor de tipo '" + ValorType.GetRealTipo().ToString() + "' no puede ser asignado a una variable de tipo '" + elemento.GetTipo().ToString() + "'.", fila, columna);
                }
            }
            else
            {
                if (ValorType.GetRealTipo().Equals(elemento.GetTipo()))
                {
                    RealizarAsignacion(elemento, ValorType, ent);
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[ASIGNACION]", "Error de tipos.  Un valor de tipo '" + ValorType.GetRealTipo().ToString() + "' no puede ser asignado a una variable de tipo '" + elemento.GetTipo().ToString() + "'.", fila, columna);
                }
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ASIGNACION]", "Se intento asignar un valor a la variable '" + Variable + "', la cual no existe en el entorno.", fila, columna);
        }
    }

    private void AsignacionAccesoObjeto(Entorno ent)
    {
        object VarAccess = ListaAcceso.Ejecutar(ent);

        // 1. Verifico que el acceso retorne un valor válido para asignarle un valor.
        if (!(VarAccess is Nulo))
        {
            TipoDato ValorType = Valor.GetTipo(ent);

            // 2. Verifico que VarAccess sea de tipo AtributoObjeto, ya que dentro éste, se encuentra el valor que queremos cambiar.
            if (VarAccess is AtributoObjeto)
            {
                AtributoObjeto attrObj = (AtributoObjeto)VarAccess;

                // 3. Verifico que el tipo de dato del elemento concuerde con el tipo de dato de la expresión.
                if (ValorType.GetRealTipo().Equals(attrObj.Tipo.GetRealTipo()))
                {
                    // 4. Verifico que el tipo de asignación, si es diferente de igual, se aplique únicamente a valores numéricos.
                    switch (TipoAsig)
                    {
                        case TipoAsignacion.AS_NORMAL:
                            attrObj.Valor = Valor.Ejecutar(ent);
                            break;
                        default:
                            if (ValorType.GetRealTipo().Equals(TipoDato.Tipo.INT))
                            {
                                if (TipoAsig.Equals(TipoAsignacion.AS_SUMA))
                                {
                                    attrObj.Valor = (int)attrObj.Valor + (int)Valor.Ejecutar(ent);
                                }
                                else if (TipoAsig.Equals(TipoAsignacion.AS_RESTA))
                                {
                                    attrObj.Valor = (int)attrObj.Valor - (int)Valor.Ejecutar(ent);
                                }
                                else if (TipoAsig.Equals(TipoAsignacion.AS_MULTIPLICACION))
                                {
                                    attrObj.Valor = (int)attrObj.Valor * (int)Valor.Ejecutar(ent);
                                }
                                else if (TipoAsig.Equals(TipoAsignacion.AS_DIVISION))
                                {
                                    attrObj.Valor = (int)attrObj.Valor / (int)Valor.Ejecutar(ent);
                                }
                            }
                            else if (ValorType.GetRealTipo().Equals(TipoDato.Tipo.DOUBLE))
                            {
                                if (TipoAsig.Equals(TipoAsignacion.AS_SUMA))
                                {
                                    attrObj.Valor = (double)attrObj.Valor + (double)Valor.Ejecutar(ent);
                                }
                                else if (TipoAsig.Equals(TipoAsignacion.AS_RESTA))
                                {
                                    attrObj.Valor = (double)attrObj.Valor - (double)Valor.Ejecutar(ent);
                                }
                                else if (TipoAsig.Equals(TipoAsignacion.AS_MULTIPLICACION))
                                {
                                    attrObj.Valor = (double)attrObj.Valor * (double)Valor.Ejecutar(ent);
                                }
                                else if (TipoAsig.Equals(TipoAsignacion.AS_DIVISION))
                                {
                                    attrObj.Valor = (double)attrObj.Valor / (double)Valor.Ejecutar(ent);
                                }
                            }
                            else
                            {
                                CQL.AddLUPError("Semántico", "[ASIGNACION]", "No se puede realizar una 'Asignación y Operación' a valores que no sean numéricos.", fila, columna);
                            }
                            break;
                    }
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[ASIGNACION]", "Error de tipos.  Un valor de tipo '" + ValorType.GetRealTipo().ToString() + "' no puede ser asignado a una variable de tipo '" + attrObj.Tipo.GetRealTipo().ToString() + "'.", fila, columna);
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[ASIGNACION]", "Error de tipos.  Se espera que el acceso a un objeto devuelva un valor de tipo Objeto.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ASIGNACION]", "Se intento asignar un valor a un acceso a objeto que retornó un valor nulo.", fila, columna);
        }
    }

    private TipoDato ObtenerTipoDatoDeValor(Variable elemento, Entorno ent)
    {
        if (elemento.GetTipo().Equals(TipoDato.Tipo.LIST) && Valor is CollectionValue)
        {
            CollectionValue cv = (CollectionValue)Valor;
            cv.IsList = true;
            return cv.GetTipo(ent);
        }
        else if (elemento.GetTipo().Equals(TipoDato.Tipo.SET) && Valor is CollectionValue)
        {
            CollectionValue cv = (CollectionValue)Valor;
            cv.IsList = false;
            return cv.GetTipo(ent);
        }
        else
        {
            return Valor.GetTipo(ent);
        }
    }

    private void RealizarAsignacion(Variable elemento, TipoDato ValorType, Entorno ent)
    {
        // Verifico que el tipo de asignación, si es diferente de igual, se aplique únicamente a valores numéricos.
        switch (TipoAsig)
        {
            case TipoAsignacion.AS_NORMAL:
                ent.ReemplazarVariable(Variable, new Variable(ValorType, Variable, Valor.Ejecutar(ent)));
                break;
            default:
                if (elemento.GetTipo().Equals(TipoDato.Tipo.INT) || elemento.GetTipo().Equals(TipoDato.Tipo.DOUBLE))
                {
                    if (TipoAsig.Equals(TipoAsignacion.AS_SUMA))
                    {
                        ent.ReemplazarVariable(Variable, new Variable(ValorType, Variable, new Operacion(new Identificador(Variable), Valor, Operacion.TipoOperacion.SUMA, fila, columna).Ejecutar(ent)));
                    }
                    else if (TipoAsig.Equals(TipoAsignacion.AS_RESTA))
                    {
                        ent.ReemplazarVariable(Variable, new Variable(ValorType, Variable, new Operacion(new Identificador(Variable), Valor, Operacion.TipoOperacion.RESTA, fila, columna).Ejecutar(ent)));
                    }
                    else if (TipoAsig.Equals(TipoAsignacion.AS_MULTIPLICACION))
                    {
                        ent.ReemplazarVariable(Variable, new Variable(ValorType, Variable, new Operacion(new Identificador(Variable), Valor, Operacion.TipoOperacion.MULTIPLICACION, fila, columna).Ejecutar(ent)));
                    }
                    else if (TipoAsig.Equals(TipoAsignacion.AS_DIVISION))
                    {
                        ent.ReemplazarVariable(Variable, new Variable(ValorType, Variable, new Operacion(new Identificador(Variable), Valor, Operacion.TipoOperacion.DIVISION, fila, columna).Ejecutar(ent)));
                    }
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[ASIGNACION]", "No se puede realizar una 'Asignación y Operación' a valores que no sean numéricos.", fila, columna);
                }
                break;
        }
    }
}