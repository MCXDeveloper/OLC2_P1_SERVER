using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class LlamadaProcedimiento : Instruccion, Expresion
{
    private readonly int fila;
    private readonly int columna;
    public string NombreProcedimiento { get; set; }
    public List<Expresion> ListaExpresiones { get; set; }

    public LlamadaProcedimiento(string nombre_procedimiento, List<Expresion> lista_expresiones, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        ListaExpresiones = lista_expresiones;
        NombreProcedimiento = nombre_procedimiento;
    }

    public TipoDato GetTipo(Entorno ent)
    {
        object valor = Ejecutar(ent);
        return new TipoDato(TipoDato.Tipo.NULO);
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Valido que exista una base de datos en uso para poder obtener el procedimiento.
        if (CQL.ExisteBaseDeDatosEnUso())
        {
            // 2. Genero el id unico que generaría la llamada a función recibida y verifico si existe en la lista de procedimientos
            //    de la base de datos.
            string key = GenerarIdentificadorProcedimiento(ent);

            if (CQL.ExisteProcedimientoEnBD(key))
            {
                Procedimiento procsito = CQL.ObtenerProcedimientoDeBD(key);

                // 3. Valido que la cantidad de parametros concuerde con la cantidad de expresiones recibidas.
                if (procsito.ListaParametros.Count.Equals(ListaExpresiones.Count))
                {
                    // 4. Valido que los tipos de datos de las expresiones recibidas concuerden con los tipos de dato que recibe el procedimiento.
                    if (ValidarTiposDeValoresConTiposDeProcedimiento(procsito.ListaParametros, ent))
                    {
                        // 5. Procedo a ejecutar el procedimiento.
                        return EjecutarProcedimiento(procsito, ent);
                    }
                    else
                    {
                        CQL.AddLUPError("Semántico", "[LLAMADA_PROCEDIMIENTO]", "Error.  Los tipos de datos de los valores proporcionados no concuerda con los definidos en el procedimiento.", fila, columna);
                    }
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[LLAMADA_PROCEDIMIENTO]", "Error.  La cantidad de valores proporcionados no concuerda con la cantidad de parámetros del procedimiento.", fila, columna);
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[LLAMADA_PROCEDIMIENTO]", "Error.  No se puede hacer una llamada a un procedimiento que no existe en la base de datos.", fila, columna);
            }
        }
        else
        {
            string mensaje = "Error.  No se puede llamar un procedimiento si no se ha especificado la base de datos a utilizar.";
            CQL.AddLUPError("Semántico", "[LLAMADA_PROCEDIMIENTO]", mensaje, fila, columna);
            if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'UseBDException' no capturada.  " + mensaje); }
            return new UseBDException(mensaje);
        }

        return new Nulo();
    }

    private bool ValidarTiposDeValoresConTiposDeProcedimiento(List<Parametro> listaParams, Entorno ent)
    {
        for (int i = 0; i < listaParams.Count; i++)
        {
            object expVal = ListaExpresiones[i].Ejecutar(ent);
            TipoDato paramType = listaParams[i].TipoDatoParametro;

            switch (paramType.GetRealTipo())
            {
                case TipoDato.Tipo.STRING:
                    if (!(expVal is string || expVal is Nulo)) { return false;  }
                    break;
                case TipoDato.Tipo.MAP:
                    if (!(expVal is Map || expVal is Nulo)) { return false; }
                    break;
                case TipoDato.Tipo.SET:
                    if (!(expVal is XSet || expVal is Nulo)) { return false; }
                    break;
                case TipoDato.Tipo.LIST:
                    if (!(expVal is XList || expVal is Nulo)) { return false; }
                    break;
                case TipoDato.Tipo.DATE:
                    if (!(expVal is Date || expVal is Nulo)) { return false; }
                    break;
                case TipoDato.Tipo.TIME:
                    if (!(expVal is Time || expVal is Nulo)) { return false; }
                    break;
                case TipoDato.Tipo.OBJECT:
                    if (!((expVal is Objeto && paramType.GetElemento().Equals(((Objeto)expVal).TipoDatoObjeto.GetElemento())) || expVal is Nulo)) { return false; }
                    break;
                case TipoDato.Tipo.INT:
                    if (!(expVal is int)) { return false; }
                    break;
                case TipoDato.Tipo.DOUBLE:
                    if (!(expVal is double)) { return false; }
                    break;
                case TipoDato.Tipo.BOOLEAN:
                    if (!(expVal is bool)) { return false; }
                    break;
            }
        }

        return true;
    }

    private object ValidarTiposDeRetornosConTiposDeProcedimiento(List<Parametro> listaRetornos, object returnExec, Entorno local)
    {
        if (listaRetornos.Count > 0)
        {
            List<object> ListaValoresRetorno = new List<object>();

            if (returnExec is List<object>)
            {
                ListaValoresRetorno.AddRange((List<object>)returnExec);
                goto init_validation;
            }
            else if (returnExec is object)
            {
                ListaValoresRetorno.Add(returnExec);
                goto init_validation;
            }
            else
            {
                goto exit;
            }

            init_validation:
            for (int i = 0; i < listaRetornos.Count; i++)
            {
                object expVal = ListaValoresRetorno[i];
                TipoDato paramType = listaRetornos[i].TipoDatoParametro;

                switch (paramType.GetRealTipo())
                {
                    case TipoDato.Tipo.STRING:
                        if (!(expVal is string || expVal is Nulo)) { return false; }
                        break;
                    case TipoDato.Tipo.MAP:
                        if (!(expVal is Map || expVal is Nulo)) { return false; }
                        break;
                    case TipoDato.Tipo.SET:
                        if (!(expVal is XSet || expVal is Nulo)) { return false; }
                        break;
                    case TipoDato.Tipo.LIST:
                        if (!(expVal is XList || expVal is Nulo)) { return false; }
                        break;
                    case TipoDato.Tipo.DATE:
                        if (!(expVal is Date || expVal is Nulo)) { return false; }
                        break;
                    case TipoDato.Tipo.TIME:
                        if (!(expVal is Time || expVal is Nulo)) { return false; }
                        break;
                    case TipoDato.Tipo.OBJECT:
                        if (!((expVal is Objeto && paramType.GetElemento().Equals(((Objeto)expVal).TipoDatoObjeto.GetElemento())) || expVal is Nulo)) { return false; }
                        break;
                    case TipoDato.Tipo.INT:
                        if (!(expVal is int)) { return false; }
                        break;
                    case TipoDato.Tipo.DOUBLE:
                        if (!(expVal is double)) { return false; }
                        break;
                    case TipoDato.Tipo.BOOLEAN:
                        if (!(expVal is bool)) { return false; }
                        break;
                    case TipoDato.Tipo.CURSOR:
                        if (!(expVal is Table)) { return false; }
                        break;
                }
            }

            return true;
        }

        exit:
        return new Nulo();
    }

    public string GenerarIdentificadorProcedimiento(Entorno ent)
    {
        string id = "_" + NombreProcedimiento + "(";

        foreach (Expresion exp in ListaExpresiones)
        {
            object resultado = exp.Ejecutar(ent);

            if (resultado is int)
            {
                id += "_" + TipoDato.Tipo.INT;
            }
            else if (resultado is double)
            {
                id += "_" + TipoDato.Tipo.DOUBLE;
            }
            else if (resultado is string)
            {
                id += "_" + TipoDato.Tipo.STRING;
            }
            else if (resultado is bool)
            {
                id += "_" + TipoDato.Tipo.BOOLEAN;
            }
            else if (resultado is Date)
            {
                id += "_" + TipoDato.Tipo.DATE;
            }
            else if (resultado is Time)
            {
                id += "_" + TipoDato.Tipo.TIME;
            }
            else if (resultado is Map)
            {
                id += "_" + TipoDato.Tipo.MAP;
            }
            else if (resultado is XList)
            {
                id += "_" + TipoDato.Tipo.LIST;
            }
            else if (resultado is XSet)
            {
                id += "_" + TipoDato.Tipo.SET;
            }
            else if (resultado is Objeto)
            {
                id += "_" + TipoDato.Tipo.OBJECT + "_" + (string)((Objeto)resultado).TipoDatoObjeto.GetElemento();
            }
        }

        id += ")";

        return id;
    }

    private object EjecutarProcedimiento(Procedimiento proc, Entorno ent)
    {
        // 1. Creo un nuevo entorno para ejecutar el procedimiento.
        Entorno local = new Entorno(AST.global);

        // 2. Agrego los valores recibidos en la lista de expresiones y se lo asigno a las variables definidas en los parametros.
        List<Parametro> listParams = proc.ListaParametros;

        for (int i = 0; i < listParams.Count; i++)
        {
            string nombreVariable = listParams[i].NombreParametro;
            TipoDato tipoVariable = listParams[i].TipoDatoParametro;
            object valorVariable = ListaExpresiones[i].Ejecutar(ent);

            local.Agregar(nombreVariable, new Variable(tipoVariable, nombreVariable, valorVariable));
        }

        // 3. Ejecuto todas las instrucciones del procedimiento.
        foreach (Instruccion ins in proc.ListaInstrucciones)
        {
            object result = ins.Ejecutar(local);

            if (result is Return)
            {
                // 4. Verifico que la cantidad de valores devueltos concuerde con los establecidos en la definición del procedimiento.
                Return ret = ((Return)result);

                if (ret.CantidadRetornos().Equals(proc.ListaRetornos.Count))
                {
                    // 5. Verifico que los tipos de dato de los retornos concuerde con los establecidos en la definición del procedimiento.
                    object returnExec = ret.Ejecutar(local);
                    object RetornoTypesResponse = ValidarTiposDeRetornosConTiposDeProcedimiento(proc.ListaRetornos, returnExec, local);

                    if (RetornoTypesResponse is bool)
                    {
                        if ((bool)RetornoTypesResponse)
                        {
                            return returnExec;
                        }
                        else
                        {
                            CQL.AddLUPError("Semántico", "[LLAMADA_PROCEDIMIENTO]", "Error.  Los tipos del retorno no concuerdan con los definidos en el procedimiento.", fila, columna);
                        }
                    }
                    else
                    {
                        CQL.AddLUPError("Semántico", "[LLAMADA_PROCEDIMIENTO]", "Error.  Los tipos del retorno no concuerdan con los definidos en el procedimiento.", fila, columna);
                    }
                }
                else
                {
                    string mensaje = "Error.  El número de retornos no concuerda con el definido en el procedimiento.";
                    CQL.AddLUPError("Semántico", "[LLAMADA_PROCEDIMIENTO]", mensaje, fila, columna);
                    if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'NumberReturnsException' no capturada.  " + mensaje); }
                    return new NumberReturnsException(mensaje);
                }
            }
        }

        return new Nulo();
    }
}