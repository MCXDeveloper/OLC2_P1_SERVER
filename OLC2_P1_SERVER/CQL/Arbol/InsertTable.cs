using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class InsertTable : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public string NombreTabla { get; set; }
    public List<string> ListaCampos { get; set; }
    public List<Expresion> ListaValores { get; set; }

    public InsertTable(string nombre_tabla, List<Expresion> lista_valores, int fila, int columna)
    {
        this.fila = fila;
        ListaCampos = null;
        this.columna = columna;
        NombreTabla = nombre_tabla;
        ListaValores = lista_valores;
    }

    public InsertTable(string nombre_tabla, List<string> lista_campos, List<Expresion> lista_valores, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        NombreTabla = nombre_tabla;
        ListaCampos = lista_campos;
        ListaValores = lista_valores;
    }

    public object Ejecutar(Entorno ent)
    {
        // +---------------------------------------------------------------------------------------------------------+
        // |                                                  Nota                                                   |
        // +---------------------------------------------------------------------------------------------------------+
        // | Para realizar de forma correcta la inserción en una tabla, se deben seguir los siguientes pasos:        |
        // | 1. Se debe validar que exista una base de datos en uso.                                                 |
        // | 2. Se debe validar que la tabla exista en la base de datos.                                             |
        // | 3. Si viene especificada la lista de campos a los cuales se le hará inserción, se realiza lo siguiente: |
        // |    a. Se debe validar que la cantidad de campos y de valores concuerden.                                |
        // |    b. Se debe validar que los campos listados existan en la tabla.                                      |
        // |    c. Se debe validar que no venga la inserción sobre un campo de tipo COUNTER.                         |
        // |    d. Se debe validar que el tipo de dato del valor corresponda al indicado en la tabla.                |
        // |    e. Si un campo no viene especificado en la lista, se debe insertar un null.                          |
        // | 4. Si NO viene especificada una lista de campos, se realiza lo siguiente:                               |
        // |    a. Se debe validar que la cantidad de valores proporcionados concuerde con la cantidad de columnas.  |
        // |    b. Se debe validar que el tipo de dato del valor corresponda al indicado en la tabla.                |
        // +---------------------------------------------------------------------------------------------------------+

        // 1. Procedo a verificar si existe alguna base de datos en uso, de lo contrario, se reporta el error.
        if (CQL.ExisteBaseDeDatosEnUso())
        {
            // 2. Procedo a verificar que la tabla exista en la base de datos.
            if (CQL.ExisteTablaEnBD(NombreTabla))
            {
                Table tablita = CQL.ObtenerTabla(NombreTabla);

                // 3. Procedo a validar el tipo de insert que se desea realizar.  Si ListaCampos es igual a null
                // significa que viene una inserción normal, de lo contrario, será una inserción especial.
                if (ListaCampos is null)
                {
                    // 4. Procedo a realizar una validación exhaustiva para la inserción normal.
                    object vni = ValidateNormalInsert(tablita, ent);

                    if (vni is bool)
                    {
                        if ((bool)vni)
                        {
                            // Valido si la instrucción se esta validando desde un BATCH
                            if (!CQL.BatchFlag)
                            {
                                // 5. Procedo a realizar la inserción de los valores.
                                tablita.AddRow(GetEvaluatedValues(ent));
                            }
                        }   
                    }
                    else
                    {
                        // Valido si la instrucción se esta validando desde un BATCH
                        if (!CQL.BatchFlag)
                        {
                            return vni;
                        }
                        else
                        {
                            CQL.BatchErrorCounter++;
                        }
                    }
                }
                else
                {
                    // 4. Procedo a realizar una validación exhaustiva para la inserción especial.
                    object vsi = ValidateSpecialInsert(tablita, ent);

                    if (vsi is bool)
                    {
                        if ((bool)vsi)
                        {
                            // Valido si la instrucción se esta validando desde un BATCH
                            if (!CQL.BatchFlag)
                            {
                                // 5. Procedo a realizar la inserción de los valores.
                                tablita.AddRow(ListaCampos, GetEvaluatedValues(ent));
                            }
                        }
                    }
                    else
                    {
                        // Valido si la instrucción se esta validando desde un BATCH
                        if (!CQL.BatchFlag)
                        {
                            return vsi;
                        }
                        else
                        {
                            CQL.BatchErrorCounter++;
                        }
                    }
                }
            }
            else
            {
                // Valido si la instrucción se esta validando desde un BATCH
                if (!CQL.BatchFlag)
                {
                    string mensaje = "Error.  La tabla especificada '" + NombreTabla + "' no existe en la base de datos actual.";
                    CQL.AddLUPError("Semántico", "[INSERT_TABLE]", mensaje, fila, columna);
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
                string mensaje = "Error.  No se puede insertar en una tabla si no se ha especificado la base de datos a utilizar.";
                CQL.AddLUPError("Semántico", "[INSERT_TABLE]", mensaje, fila, columna);
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

    private object ValidateNormalInsert(Table tablita, Entorno ent)
    {
        // 1. Valido que la cantidad de valores proporcionados concuerde con la cantidad de columnas de la tabla.
        if (tablita.GetColumnCountWithoutCounterColumns().Equals(ListaValores.Count))
        {
            // 2. Valido que cada tipo de dato del valor corresponda con el tipo de dato de la columna.
            object vct = ValidateValueTypesWithColumnTypes(tablita, ent);

            if (vct is bool)
            {
                if ((bool)vct)
                {
                    return true;
                }
            }
            else
            {
                return vct;
            }
        }
        else
        {
            string mensaje = "Error.  La cantidad de valores proporcionados no concuerda con la cantidad de columnas de la tabla.";
            CQL.AddLUPError("Semántico", "[INSERT_TABLE]", mensaje, fila, columna);
            if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'ValuesException' no capturada.  " + mensaje); }
            return new ValuesException(mensaje);
        }

        return false;
    }

    private object ValidateSpecialInsert(Table tablita, Entorno ent)
    {
        // 1. Valido que la cantidad de valores proporcionados concuerde con la cantidad de campos.
        if (ListaCampos.Count.Equals(ListaValores.Count))
        {
            // 2. Valido que los campos listados existan en la tabla.
            if (ValidateExistenceOfFields(tablita))
            {
                // 3. Valido que los campos listados no sean de tipo COUNTER.
                if (ValidateIfAFieldIsCounterType(tablita))
                {
                    // 4. Valido que cada tipo de dato del valor corresponda con el tipo de dato de la columna.
                    if (ValidateValueTypeWithListFieldTypes(tablita, ent))
                    {
                        return true;
                    }
                    else
                    {
                        string mensaje = "Error.  Los tipos de dato de los valores no concuerdan con los definidos en las columnas.";
                        CQL.AddLUPError("Semántico", "[INSERT_TABLE]", mensaje, fila, columna);
                        if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'ValuesException' no capturada.  " + mensaje); }
                        return new ValuesException(mensaje);
                    }
                }
                else
                {
                    string mensaje = "Error.  No se puede insertar un valor en una columna de tipo COUNTER.";
                    CQL.AddLUPError("Semántico", "[INSERT_TABLE]", mensaje, fila, columna);
                    if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'CounterTypeException' no capturada.  " + mensaje); }
                    return new CounterTypeException(mensaje);
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[INSERT_TABLE]", "Error.  Alguno de los campos proporcionados en la lista de campos no existe.", fila, columna);
            }
        }
        else
        {
            string mensaje = "Error.  La cantidad de valores proporcionados no concuerda con la cantidad de columnas.";
            CQL.AddLUPError("Semántico", "[INSERT_TABLE]", mensaje, fila, columna);
            if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'ValuesException' no capturada.  " + mensaje); }
            return new ValuesException(mensaje);
        }

        return false;
    }

    private object ValidateValueTypesWithColumnTypes(Table tablita, Entorno ent)
    {
        for (int i = 0; i < tablita.Tabla.Columns.Count; i++)
        {
            TipoDato.Tipo colType = ((Columna)tablita.Tabla.Columns[i]).TipoDatoColumna.GetRealTipo();
            TipoDato.Tipo valType = ListaValores[i].GetTipo(ent).GetRealTipo();

            if (!colType.Equals(TipoDato.Tipo.COUNTER))
            {
                if (colType.Equals(TipoDato.Tipo.STRING) || colType.Equals(TipoDato.Tipo.DATE) || colType.Equals(TipoDato.Tipo.TIME) || colType.Equals(TipoDato.Tipo.MAP) || colType.Equals(TipoDato.Tipo.SET) || colType.Equals(TipoDato.Tipo.LIST) || colType.Equals(TipoDato.Tipo.OBJECT))
                {
                    if (!(valType.Equals(colType) || valType.Equals(TipoDato.Tipo.NULO)))
                    {
                        string mensaje = "Error.  Los tipos de dato de los valores no concuerdan con los definidos en las columnas.";
                        CQL.AddLUPError("Semántico", "[INSERT_TABLE]", mensaje, fila, columna);
                        if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'ValuesException' no capturada.  " + mensaje); }
                        return new ValuesException(mensaje);
                    }
                }
                else
                {
                    if (!valType.Equals(colType))
                    {
                        string mensaje = "Error.  Los tipos de dato de los valores no concuerdan con los definidos en las columnas.";
                        CQL.AddLUPError("Semántico", "[INSERT_TABLE]", mensaje, fila, columna);
                        if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'ValuesException' no capturada.  " + mensaje); }
                        return new ValuesException(mensaje);
                    }
                }
            }
            else
            {
                string mensaje = "Error.  No se puede insertar un valor en una columna de tipo COUNTER.";
                CQL.AddLUPError("Semántico", "[INSERT_TABLE]", mensaje, fila, columna);
                if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'CounterTypeException' no capturada.  " + mensaje); }
                return new CounterTypeException(mensaje);
            }
        }

        return true;
    }

    private bool ValidateValueTypeWithListFieldTypes(Table tablita, Entorno ent)
    {
        for (int i = 0; i < ListaCampos.Count; i++)
        {
            Expresion exp = ListaValores[i];
            TipoDato.Tipo colType = tablita.GetColumn(ListaCampos[i]).TipoDatoColumna.GetRealTipo();
            TipoDato.Tipo valType = TipoDato.Tipo.NULO;

            if (colType.Equals(TipoDato.Tipo.LIST) && exp is CollectionValue)
            {
                CollectionValue cv = (CollectionValue)exp;
                cv.IsList = true;
                valType = cv.GetTipo(ent).GetRealTipo();
            }
            else if (colType.Equals(TipoDato.Tipo.SET) && exp is CollectionValue)
            {
                CollectionValue cv = (CollectionValue)exp;
                cv.IsList = false;
                valType = cv.GetTipo(ent).GetRealTipo();
            }
            else
            {
                valType = exp.GetTipo(ent).GetRealTipo();
            }

            if (!(valType.Equals(colType) || valType.Equals(TipoDato.Tipo.NULO)))
            {
                return false;
            }
        }

        return true;
    }
    
    private bool ValidateExistenceOfFields(Table tablita)
    {
        foreach (string field in ListaCampos)
        {
            if (!tablita.ExistsColumn(field))
            {
                return false;
            }
        }

        return true;
    }

    private bool ValidateIfAFieldIsCounterType(Table tablita)
    {
        foreach (string col in ListaCampos)
        {
            if (tablita.GetColumn(col).TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.COUNTER))
            {
                return false;
            }
        }

        return true;
    }

    private List<object> GetEvaluatedValues(Entorno ent)
    {
        List<object> list = new List<object>();

        for (int i = 0; i < ListaValores.Count; i++)
        {
            Expresion exp = ListaValores[i];

            if (exp is CollectionValue)
            {
                CollectionValue cv = (CollectionValue)exp;

                // 1. Obtengo la columna de la tabla para verificar su tipo de dato.
                Columna col = CQL.ObtenerColumnaDeTabla(NombreTabla, ListaCampos[i]);

                // 2. Verifico el tipo de dato de la columna para establecer la bandera para diferenciar List de Set.
                if (col.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.LIST))
                {
                    cv.IsList = true;
                }
                else if (col.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.SET))
                {
                    cv.IsList = false;
                }

                list.Add(cv.Ejecutar(ent));
            }
            else
            {
                object val = exp.Ejecutar(ent);

                if (val is Date)
                {
                    list.Add(((Date)val).GetParsedDate());
                }
                else if (val is Time)
                {
                    list.Add(((Time)val).GetTimeInDateTime());
                }
                else if (val is CollectionValue)
                {


                }
                else
                {
                    list.Add(val);
                }
            }
        }

        return list;
    }

}