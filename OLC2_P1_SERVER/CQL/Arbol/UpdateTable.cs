using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using static Asignacion;

public class UpdateTable : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public string NombreTabla { get; set; }
    public Expresion ExpresionWhere { get; set; }
    public List<AsignacionColumna> ListaAsignaciones { get; set; }

    public UpdateTable(string nombre_tabla, List<AsignacionColumna> lista_asignaciones, int fila, int columna)
    {
        this.fila = fila;
        ExpresionWhere = null;
        this.columna = columna;
        NombreTabla = nombre_tabla;
        ListaAsignaciones = lista_asignaciones;
    }

    public UpdateTable(string nombre_tabla, List<AsignacionColumna> lista_asignaciones, Expresion expresion_where, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        NombreTabla = nombre_tabla;
        ExpresionWhere = expresion_where;
        ListaAsignaciones = lista_asignaciones;
    }

    public object Ejecutar(Entorno ent)
    {
        // +----------------------------------------------------------------------------------------------------------------------------------+
        // |                                                               Nota                                                               |
        // +----------------------------------------------------------------------------------------------------------------------------------+
        // | Para ejecutar de forma correcta la instrucción UPDATE para una tabla se deben de tomar en cuenta las siguientes consideraciones: |
        // | 1. Se debe validar que exista una base de datos en uso.                                                                          |
        // | 2. Se debe validar que la tabla proporcionada en la instrucción exista en la base de datos.                                      |
        // | 3. Se debe validar que cada columna proporcionada en la lista de asignaciones exista en la tabla.                                |
        // | 4. Se debe validar que no se esté deseando actualizar una columna que sea de tipo COUNTER.                                       |
        // | 5. Se debe validar que el tipo de la columna a actualizar sea del mismo tipo que la expresión que está recibiendo.               |
        // |                                                                                                                                  |
        // | Una vez validado todo lo anterior, cada tupla dentro de la tabla se debe de colocar de forma estática para poder acceder a sus   |
        // | valores en las diferentes clases.                                                                                                |
        // +----------------------------------------------------------------------------------------------------------------------------------+

        if (CQL.ExisteBaseDeDatosEnUso())
        {
            if (CQL.ExisteTablaEnBD(NombreTabla))
            {
                object vec = ValidarExistenciaDeColumnasYQueNoSeanCounter();

                if (vec is bool)
                {
                    if ((bool)vec)
                    {
                        CQL.WhereFlag = true;
                        CQL.TablaEnUso = null;
                        CQL.TuplaEnUso = null;

                        if (ValidarTipoColumnaConTipoValor(ent))
                        {
                            if (!CQL.BatchFlag)
                            {
                                RealizarActualizacionDeValores(ent);
                            }
                        }
                        else
                        {
                            // Valido si la instrucción se esta validando desde un BATCH
                            if (!CQL.BatchFlag)
                            {
                                string mensaje = "Error.  Los tipos de dato de los valores no concuerdan con los definidos en las columnas.";
                                CQL.AddLUPError("Semántico", "[UPDATE_TABLE]", mensaje, fila, columna);
                                if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'ValuesException' no capturada.  " + mensaje); }
                                return new ValuesException(mensaje);
                            }
                            else
                            {
                                CQL.BatchErrorCounter++;
                            }
                        }

                        CQL.WhereFlag = false;
                        CQL.TablaEnUso = null;
                        CQL.TuplaEnUso = null;
                    }
                }
                else
                {
                    // Valido si la instrucción se esta validando desde un BATCH
                    if (!CQL.BatchFlag)
                    {
                        return vec;
                    }
                    else
                    {
                        CQL.BatchErrorCounter++;
                    }
                }
            }
            else
            {
                // Valido si la instrucción se esta validando desde un BATCH
                if (!CQL.BatchFlag)
                {
                    string mensaje = "Error.  La tabla especificada '" + NombreTabla + "' no existe en la base de datos actual.";
                    CQL.AddLUPError("Semántico", "[UPDATE_TABLE]", mensaje, fila, columna);
                    if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'TableDontExists' no capturada.  " + mensaje); }
                    return new TableDontExists(mensaje);
                }
                else
                {
                    CQL.BatchErrorCounter++;
                }
            }
        }
        else
        {
            // Valido si la instrucción se esta validando desde un BATCH
            if (!CQL.BatchFlag)
            {
                string mensaje = "Error.  No se puede actualizar los valores en una tabla si no se ha especificado la base de datos a utilizar.";
                CQL.AddLUPError("Semántico", "[UPDATE_TABLE]", mensaje, fila, columna);
                if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'UseBDException' no capturada.  " + mensaje); }
                return new UseBDException(mensaje);
            }
            else
            {
                CQL.BatchErrorCounter++;
            }
        }

        return new Nulo();
    }

    public object ValidarExistenciaDeColumnasYQueNoSeanCounter()
    {
        foreach (AsignacionColumna asig in ListaAsignaciones)
        {
            if (!CQL.ExisteColumnaEnTabla(NombreTabla, asig.NombreColumna))
            {
                string mensaje = "Error.  La columna '" + asig.NombreColumna + "' especificada en la instrucción UPDATE no existe en la tabla.";
                CQL.AddLUPError("Semántico", "[UPDATE_TABLE]", mensaje, fila, columna);
                if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'ColumnException' no capturada.  " + mensaje); }
                return new ColumnException(mensaje);
            }
            else
            {
                if (CQL.ValidarTipoDatoColumna(NombreTabla, asig.NombreColumna, new TipoDato.Tipo[] { TipoDato.Tipo.COUNTER }))
                {
                    string mensaje = "Error.  No se puede actualizar un valor en una columna de tipo COUNTER.";
                    CQL.AddLUPError("Semántico", "[UPDATE_TABLE]", mensaje, fila, columna);
                    if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'CounterTypeException' no capturada.  " + mensaje); }
                    return new CounterTypeException(mensaje);
                }
            }
        }

        return true;
    }

    public bool ValidarTipoColumnaConTipoValor(Entorno ent)
    {
        DataTable tablaOriginal = CQL.ObtenerTabla(NombreTabla).Tabla;

        for (int i = 0; i < tablaOriginal.Rows.Count; i++)
        {
            CQL.TuplaEnUso = tablaOriginal.Rows[i];

            foreach (AsignacionColumna ac in ListaAsignaciones)
            {
                TipoDato.Tipo valueType = ac.ValorColumna.GetTipo(ent).GetRealTipo();

                // Verifico que AsignacionColumna no sea del tipo 'identificador[expresion]' ya que requiere una validación diferente.
                if (ac.ValorPosicionObjeto is Nulo)
                {
                    if (!valueType.Equals(TipoDato.Tipo.NULO))
                    {
                        if (!(
                            (valueType.Equals(TipoDato.Tipo.INT) && tablaOriginal.Columns[ac.NombreColumna].DataType.Equals(typeof(int))) ||
                            (valueType.Equals(TipoDato.Tipo.DOUBLE) && tablaOriginal.Columns[ac.NombreColumna].DataType.Equals(typeof(double))) ||
                            (valueType.Equals(TipoDato.Tipo.BOOLEAN) && tablaOriginal.Columns[ac.NombreColumna].DataType.Equals(typeof(bool))) ||
                            (valueType.Equals(TipoDato.Tipo.STRING) && tablaOriginal.Columns[ac.NombreColumna].DataType.Equals(typeof(string))) ||
                            ((valueType.Equals(TipoDato.Tipo.DATE) || valueType.Equals(TipoDato.Tipo.TIME)) && tablaOriginal.Columns[ac.NombreColumna].DataType.Equals(typeof(DateTime))) ||
                            ((valueType.Equals(TipoDato.Tipo.MAP) || valueType.Equals(TipoDato.Tipo.SET) || valueType.Equals(TipoDato.Tipo.LIST) || valueType.Equals(TipoDato.Tipo.OBJECT)) && tablaOriginal.Columns[ac.NombreColumna].DataType.Equals(typeof(object)))
                        ))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    bool objValidation = VerificarTipoDatoConObjetoCollection(tablaOriginal, ac, valueType, tablaOriginal.Rows[i], ent);
                    if (!objValidation)
                        return false;
                }
            }
        }

        return true;
    }

    private void RealizarActualizacionDeValores(Entorno ent)
    {
        // 1. Agrego a la variable estática 'TablaEnUso' la tabla que se desea actualizar.
        CQL.TablaEnUso = CQL.ObtenerTabla(NombreTabla);
        DataTable tablaOriginal = CQL.TablaEnUso.Tabla;

        // 2. Realizo un recorrido por cada tupla de la tabla para colocar su valor de forma estática para que puedan
        //    acceder las demás clases donde se necesite el valor (si existiese la expresión WHERE).  Si la expresión
        //    WHERE no estuviera definida, se actualizan todos los valores de la tabla.

        if (ExpresionWhere is null)
        {
            for (int i = 0; i < tablaOriginal.Rows.Count; i++)
            {
                CQL.TuplaEnUso = tablaOriginal.Rows[i];

                foreach (AsignacionColumna ac in ListaAsignaciones)
                {
                    if (ac.ValorPosicionObjeto is Nulo)
                    {
                        object valorcito = ObtenerValorFinal(ac, tablaOriginal.Rows[i][ac.NombreColumna], ac.ValorColumna.Ejecutar(ent), ent);
                        tablaOriginal.Rows[i][ac.NombreColumna] = valorcito;
                    }
                    else
                    {
                        object clave = ac.ValorPosicionObjeto.Ejecutar(ent);
                        
                        TipoDato TipoDatoCol = ((Columna)tablaOriginal.Columns[ac.NombreColumna]).TipoDatoColumna;
                        TipoDato.Tipo TipoCol = TipoDatoCol.GetRealTipo();

                        if (TipoCol.Equals(TipoDato.Tipo.MAP))
                        {
                            Map mapita = (Map)tablaOriginal.Rows[i][ac.NombreColumna];
                            object oldValue = mapita.Get(clave);
                            object valorcito = ObtenerValorFinal(ac, oldValue, ac.ValorColumna.Ejecutar(ent), ent);
                            mapita.Set(clave, valorcito);
                        }
                        else if (TipoCol.Equals(TipoDato.Tipo.OBJECT))
                        {
                            Objeto obj = (Objeto)tablaOriginal.Rows[i][ac.NombreColumna];
                            AtributoObjeto atrobj = obj.ListaAtributosObjeto.Find(x => x.Nombre.Equals(clave));
                            object oldValue = atrobj.Valor;
                            object valorcito = ObtenerValorFinal(ac, oldValue, ac.ValorColumna.Ejecutar(ent), ent);
                            atrobj.Valor = valorcito;
                        }
                        else if (TipoCol.Equals(TipoDato.Tipo.LIST))
                        {
                            XList lx = (XList)tablaOriginal.Rows[i][ac.NombreColumna];
                            object oldValue = lx.Get((int)clave);
                            object valorcito = ObtenerValorFinal(ac, oldValue, ac.ValorColumna.Ejecutar(ent), ent);
                            lx.Set((int)clave, valorcito);
                        }
                        else if (TipoCol.Equals(TipoDato.Tipo.SET))
                        {
                            XSet lx = (XSet)tablaOriginal.Rows[i][ac.NombreColumna];
                            object oldValue = lx.Get((int)clave);
                            object valorcito = ObtenerValorFinal(ac, oldValue, ac.ValorColumna.Ejecutar(ent), ent);
                            lx.Set((int)clave, valorcito);
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < tablaOriginal.Rows.Count; i++)
            {
                CQL.TuplaEnUso = tablaOriginal.Rows[i];
                object whereResponse = ExpresionWhere.Ejecutar(ent);

                if (whereResponse is bool)
                {
                    if ((bool)whereResponse)
                    {
                        foreach (AsignacionColumna ac in ListaAsignaciones)
                        {
                            if (ac.ValorPosicionObjeto is Nulo)
                            {
                                object valorcito = ObtenerValorFinal(ac, tablaOriginal.Rows[i][ac.NombreColumna], ac.ValorColumna.Ejecutar(ent), ent);
                                tablaOriginal.Rows[i][ac.NombreColumna] = valorcito;
                            }
                            else
                            {
                                object clave = ac.ValorPosicionObjeto.Ejecutar(ent);

                                TipoDato TipoDatoCol = ((Columna)tablaOriginal.Columns[ac.NombreColumna]).TipoDatoColumna;
                                TipoDato.Tipo TipoCol = TipoDatoCol.GetRealTipo();

                                if (TipoCol.Equals(TipoDato.Tipo.MAP))
                                {
                                    Map mapita = (Map)tablaOriginal.Rows[i][ac.NombreColumna];
                                    object oldValue = mapita.Get(clave);
                                    object valorcito = ObtenerValorFinal(ac, oldValue, ac.ValorColumna.Ejecutar(ent), ent);
                                    mapita.Set(clave, valorcito);
                                }
                                else if (TipoCol.Equals(TipoDato.Tipo.OBJECT))
                                {
                                    Objeto obj = (Objeto)tablaOriginal.Rows[i][ac.NombreColumna];
                                    AtributoObjeto atrobj = obj.ListaAtributosObjeto.Find(x => x.Nombre.Equals(clave));
                                    object oldValue = atrobj.Valor;
                                    object valorcito = ObtenerValorFinal(ac, oldValue, ac.ValorColumna.Ejecutar(ent), ent);
                                    atrobj.Valor = valorcito;
                                }
                                else if (TipoCol.Equals(TipoDato.Tipo.LIST))
                                {
                                    XList lx = (XList)tablaOriginal.Rows[i][ac.NombreColumna];
                                    object oldValue = lx.Get((int)clave);
                                    object valorcito = ObtenerValorFinal(ac, oldValue, ac.ValorColumna.Ejecutar(ent), ent);
                                    lx.Set((int)clave, valorcito);
                                }
                                else if (TipoCol.Equals(TipoDato.Tipo.SET))
                                {
                                    XSet lx = (XSet)tablaOriginal.Rows[i][ac.NombreColumna];
                                    object oldValue = lx.Get((int)clave);
                                    object valorcito = ObtenerValorFinal(ac, oldValue, ac.ValorColumna.Ejecutar(ent), ent);
                                    lx.Set((int)clave, valorcito);
                                }
                            }
                        }
                    }
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[UPDATE_TABLE]", "Error.  La operación WHERE en la consulta a la tabla '" + NombreTabla + "' no retorna un valor booleano.", fila, columna);
                    break;
                }
            }
        }
    }

    private object ObtenerValorFinal(AsignacionColumna ac, object rowVal, object newVal, Entorno ent)
    {
        switch (ac.TipoAsignacionColumna)
        {
            case TipoAsignacion.AS_NORMAL:
                return newVal;
            default:

                if (VerificarSiAmbosValoresSonNumericos(rowVal, newVal))
                {
                    Operacion op = new Operacion(new Primitivo(rowVal), new Primitivo(newVal), Operacion.GetTipoOperacion(ObtenerSimboloDeOperacion(ac.TipoAsignacionColumna)), fila, columna);
                    return op.Ejecutar(ent);
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[UPDATE_TABLE]", "No se puede realizar una Asignación y Operación a valores que no son numéricos.", fila, columna);
                }
                break;
        }

        return new Nulo();
    }

    private string ObtenerSimboloDeOperacion(TipoAsignacion tipoAsignacionColumna)
    {
        switch (tipoAsignacionColumna)
        {
            case TipoAsignacion.AS_SUMA:
                return "+";
            case TipoAsignacion.AS_RESTA:
                return "-";
            case TipoAsignacion.AS_MULTIPLICACION:
                return "*";
            default:
                return "/";
        }
    }

    private bool VerificarSiAmbosValoresSonNumericos(object filaVal, object nuevoVal)
    {
        return (filaVal is int || filaVal is double) && (nuevoVal is int || nuevoVal is double);
    }

    private bool VerificarTipoDatoConObjetoCollection(DataTable tablaOriginal, AsignacionColumna ac, TipoDato.Tipo valueType, DataRow rowsi, Entorno ent)
    {
        // 1. Verifico que la columna si sea de tipo Objeto/Collection para poder acceder a una posición.
        TipoDato TipoDatoCol = ((Columna)tablaOriginal.Columns[ac.NombreColumna]).TipoDatoColumna;
        TipoDato.Tipo TipoCol = TipoDatoCol.GetRealTipo();
        TipoDato.Tipo[] permitidos = { TipoDato.Tipo.OBJECT, TipoDato.Tipo.MAP, TipoDato.Tipo.SET, TipoDato.Tipo.LIST };

        if (permitidos.Contains(TipoCol))
        {
            // 2. Verifico, si la columna es Map ú Objeto, que el atributo exista.  De lo contrario solo se valida que la expresión sea de tipo
            // entero, si no, se arroja error.  Si la expresión de la posición es de tipo entero pero no existe, también se debe de arrojar error.

            if (TipoCol.Equals(TipoDato.Tipo.MAP))
            {
                Map mapita = (Map)rowsi[ac.NombreColumna];

                if (mapita.Contains(ac.ValorPosicionObjeto.Ejecutar(ent)))
                {
                    // 3. Verifico que el tipo de dato del valor correspondiente a esa clave concuerde con el que se le quiere actualizar.
                    if (!mapita.TipoDatoValor.GetRealTipo().Equals(valueType))
                    {
                        return false;
                    }
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[UPDATE_TABLE]", "Error. La clave proporcionada para actualizar su valor no existe en la collection.", fila, columna);
                    return false;
                }
            }
            else if (TipoCol.Equals(TipoDato.Tipo.OBJECT))
            {
                Objeto obj = (Objeto)rowsi[ac.NombreColumna];
                object valPos = ac.ValorPosicionObjeto.Ejecutar(ent);

                if (obj.ListaAtributosObjeto.Any(x => x.Nombre.Equals(valPos)))
                {
                    AtributoObjeto atrobj = obj.ListaAtributosObjeto.Find(x => x.Nombre.Equals(valPos));

                    // 3. Verifico que el tipo de dato del valor correspondiente a esa clave concuerde con el que se le quiere actualizar.
                    if (!atrobj.Tipo.GetRealTipo().Equals(valueType))
                    {
                        return false;
                    }
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[UPDATE_TABLE]", "Error. La clave proporcionada para actualizar su valor no existe en el objeto.", fila, columna);
                    return false;
                }
            }
            else
            {
                // Si el tipo de la columna no es ni Map ni Objeto, entonces signfica que es o List o Set.  Para ello se agrega la validación adicional
                // de que el valor brindado como posición sea de tipo entero.
                object valPos = ac.ValorPosicionObjeto.Ejecutar(ent);

                if (valPos is int)
                {
                    if (TipoCol.Equals(TipoDato.Tipo.LIST))
                    {
                        XList lx = (XList)rowsi[ac.NombreColumna];

                        if (!lx.TipoDatoList.GetRealTipo().Equals(valueType))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        XSet lx = (XSet)rowsi[ac.NombreColumna];

                        if (!lx.TipoDatoSet.GetRealTipo().Equals(valueType))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[UPDATE_TABLE]", "Error. La clave proporcionada para actualizar su valor debe ser de tipo entero para las colecciones List/Set.", fila, columna);
                    return false;
                }
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[UPDATE_TABLE]", "Error. No se puede actualizar el valor en la posición indicada ya que el elemento no es de tipo Objeto/Collection.", fila, columna);
            return false;
        }

        return true;
    }
}