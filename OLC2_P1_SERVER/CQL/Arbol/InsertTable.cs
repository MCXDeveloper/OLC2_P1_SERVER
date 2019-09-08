using OLC2_P1_SERVER.CQL.Arbol;
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
        if (!CQL.BaseDatosEnUso.Equals(String.Empty))
        {
            // 2. Procedo a verificar que la tabla exista en la base de datos.
            if (CQL.RootBD.GetDatabase(CQL.BaseDatosEnUso).ExisteTabla(NombreTabla))
            {
                Table tablita = CQL.RootBD.GetDatabase(CQL.BaseDatosEnUso).ObtenerTabla(NombreTabla);

                // 3. Procedo a validar el tipo de insert que se desea realizar.  Si ListaCampos es igual a null
                // significa que viene una inserción normal, de lo contrario, será una inserción especial.
                if (ListaCampos is null)
                {
                    // 4. Procedo a realizar una validación exhaustiva para la inserción normal.
                    if (ValidateNormalInsert(tablita, ent))
                    {
                        // 5. Procedo a realizar la inserción de los valores.
                        tablita.AddRow(GetEvaluatedValues(ent));
                    }
                }
                else
                {
                    // 4. Procedo a realizar una validación exhaustiva para la inserción especial.
                    if (ValidateSpecialInsert(tablita, ent))
                    {
                        // 5. Procedo a realizar la inserción de los valores.
                        tablita.AddRow(ListaCampos, GetEvaluatedValues(ent));
                    }
                }
            }
            else
            {
                Error.AgregarError("Semántico", "[INSERT_TABLE]", "Error.  La tabla especificada '" + NombreTabla + "' no existe en la base de datos actual.", fila, columna);
            }
        }
        else
        {
            Error.AgregarError("Semántico", "[INSERT_TABLE]", "Error.  No se puede insertar en una tabla si no se ha especificado la base de datos a utilizar.", fila, columna);
        }

        return new Nulo();
    }

    private bool ValidateNormalInsert(Table tablita, Entorno ent)
    {
        // 1. Valido que la cantidad de valores proporcionados concuerde con la cantidad de columnas de la tabla.
        if (tablita.GetColumnCountWithoutCounterColumns().Equals(ListaValores.Count))
        {
            // 2. Valido que cada tipo de dato del valor corresponda con el tipo de dato de la columna.
            if (ValidateValueTypesWithColumnTypes(tablita, ent))
            {
                return true;
            }
            else
            {
                Error.AgregarError("Semántico", "[INSERT_TABLE]", "Error.  Los tipos de dato de los valores no concuerdan con los definidos en las columnas.", fila, columna);
            }
        }
        else
        {
            Error.AgregarError("Semántico", "[INSERT_TABLE]", "Error.  La cantidad de valores proporcionados no concuerda con la cantidad de columnas de la tabla.", fila, columna);
        }

        return false;
    }

    private bool ValidateSpecialInsert(Table tablita, Entorno ent)
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
                        Error.AgregarError("Semántico", "[INSERT_TABLE]", "Error.  Los tipos de dato de los valores no concuerdan con los definidos en las columnas.", fila, columna);
                    }
                }
                else
                {
                    Error.AgregarError("Semántico", "[INSERT_TABLE]", "Error.  No se puede insertar un valor en una columna de tipo COUNTER.", fila, columna);
                }
            }
            else
            {
                Error.AgregarError("Semántico", "[INSERT_TABLE]", "Error.  Alguno de los campos proporcionados en la lista de campos no existe.", fila, columna);
            }
        }
        else
        {
            Error.AgregarError("Semántico", "[INSERT_TABLE]", "Error.  La cantidad de valores proporcionados no concuerda con la cantidad de columnas.", fila, columna);
        }

        return false;
    }

    private bool ValidateValueTypesWithColumnTypes(Table tablita, Entorno ent)
    {
        for (int i = 0; i < tablita.Tabla.Columns.Count; i++)
        {
            if (!((Columna)tablita.Tabla.Columns[i]).TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.COUNTER))
            {
                if (!((Columna)tablita.Tabla.Columns[i]).TipoDatoColumna.GetRealTipo().Equals(ListaValores[i].GetTipo(ent).GetRealTipo()))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool ValidateValueTypeWithListFieldTypes(Table tablita, Entorno ent)
    {
        for (int i = 0; i < ListaCampos.Count; i++)
        {
            if(!tablita.GetColumn(ListaCampos[i]).TipoDatoColumna.GetRealTipo().Equals(ListaValores[i].GetTipo(ent).GetRealTipo()))
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

        foreach (Expresion exp in ListaValores)
        {
            list.Add(exp.Ejecutar(ent));
        }

        return list;
    }
}