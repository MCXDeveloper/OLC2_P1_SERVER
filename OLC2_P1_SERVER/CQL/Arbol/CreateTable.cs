
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class CreateTable : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public bool IsExists { get; set; }
    public string NombreTabla { get; set; }
    public List<Columna> ListaColumnas { get; set; }
    public List<string> ListaLlavesPrimarias { get; set; }

    public CreateTable(bool isExists, string nombre_tabla, List<Columna> lista_columnas, int fila, int columna)
    {
        this.fila = fila;
        IsExists = isExists;
        this.columna = columna;
        NombreTabla = nombre_tabla;
        ListaLlavesPrimarias = null;
        ListaColumnas = lista_columnas;
    }

    public CreateTable(bool isExists, string nombre_tabla, List<Columna> lista_columnas, List<string> lista_llaves_primarias, int fila, int columna)
    {
        this.fila = fila;
        IsExists = isExists;
        this.columna = columna;
        NombreTabla = nombre_tabla;
        ListaColumnas = lista_columnas;
        ListaLlavesPrimarias = lista_llaves_primarias;
    }

    public object Ejecutar(Entorno ent)
    {
        // +----------------------------------------------------------------------------------------------+
        // |                                             Nota                                             |
        // +----------------------------------------------------------------------------------------------+
        // | Para realizar de forma correcta la creación de tablas, se deben seguir los siguientes pasos: |
        // | 1. Se debe validar que exista una base de datos en uso actual para poder crear la tabla.     |
        // | 2. Se debe validar que la tabla que se desea crear no exista en la base de datos.            |
        // | 3. Se debe validar que las columnas cumplan con la siguientes restricciones:                 |
        // |    a. Se debe validar que no existan columnas con nombres repetidos.                         |
        // |    b. Se debe validar si existe lista de llaves primarias compuestas.  Si sí, se hacen las   |
        // |    siguientes validaciones:                                                                  |
        // |    i. Verificar que ninguno de los campos tenga la cláusula PK.                              |
        // |    ii. Verificar que los campos dentro de la lista existan en la declaración.                |
        // |    iii. Verificar que si uno de los campos es de tipo COUNTER, todos los demás deben ser de  |
        // |    tipo COUNTER.                                                                             |
        // |    iv. Verificar que si ningún campo de la lista es tipo COUNTER, entonces se debe validar   |
        // |    que todos sean de tipo primitivo.                                                         |
        // |    c. Si no existe la lista de llaves primarias compuestas, se debe validar lo siguiente:    |
        // |    i. Validar que exista 0 o 1 campo con la cláusula PK.  Si no existe un campo con PK hacer |
        // |    validación del punto (ii) de lo contrario, realizar validación en el punto (iii).         |
        // |    ii. Validar que todos los campos definidos sean de tipo distinto de COUNTER.              |
        // |    iii. Validar que la columna PK (si existe) sea de tipo primitivo.                         |
        // +----------------------------------------------------------------------------------------------+
        
        // 1. Procedo a verificar si existe alguna base de datos en uso, de lo contrario, se reporta el error.
        if (CQL.ExisteBaseDeDatosEnUso())
        {
            // 2. Procedo a validar que la tabla que desea crear no exista una con el mismo nombre en la base de datos.
            if (!CQL.ExisteTablaEnBD(NombreTabla))
            {
                // 3. Se realiza de forma exhaustiva la validación de columnas posteriormente a realizar la creación de la tabla.
                if (ValidateColumns())
                {
                    CQL.RegistrarTabla(BuildTable());
                }
            }
            else
            {
                if (!IsExists)
                {
                    string mensaje = "Error.  Una tabla con el mismo nombre ya se encuentra en la base de datos.  (BD: " + CQL.BaseDatosEnUso + " | Tabla: " + NombreTabla + ").";
                    CQL.AddLUPError("Semántico", "[CREATE_TABLE]", mensaje, fila, columna);
                    if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'TableAlreadyExists' no capturada.  " + mensaje); }
                    return new TableAlreadyExists(mensaje);
                }
            }
        }
        else
        {
            string mensaje = "Error.  No se puede crear una tabla si no se ha especificado la base de datos a utilizar.";
            CQL.AddLUPError("Semántico", "[CREATE_TABLE]", mensaje, fila, columna);
            if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'UseBDException' no capturada.  " + mensaje); }
            return new UseBDException(mensaje);
        }
        
        return new Nulo();
    }

    private bool ValidateColumns()
    {
        // 1. Valido que en la lista de columnas no existan valores con nombre repetidos.
        if(!ExistRepeatedColumns())
        {
            // 2. Valido si viene la declaración de llaves primarias compuestas.
            if (!(ListaLlavesPrimarias is null))
            {
                // 3. Verifico que ninguno de los campos declarados contenga la cláusula PK.  Si alguno la trae se debe mostrar error.
                if (!ExistPKField())
                {
                    // 4. Verifico que los campos dentro de la lista existan en la declaración de campos.
                    if (ValidateExistenceOfColumns())
                    {
                        // 5. Verifico que si uno de los campos es COUNTER entonces todos los demás deben de ser COUNTER.  De lo contrario, se valida
                        // que todos los campos sean de tipo primitivo.  Si no cumple con una de esas dos opciones, se muestra error.
                        if (ExistAtLeastOneCounterField())
                        {
                            // 6. Si existe por lo menos un campo que es de tipo de dato COUNTER, se debe validar que TODOS los campos en la lista de 
                            // llaves primarias compuestas sean de tipo COUNTER.
                            if (ValidateAllCounterValues())
                            {
                                return true;
                            }
                            else
                            {
                                CQL.AddLUPError("Semántico", "[CREATE_TABLE]", "Error.  Se indicó un campo de tipo COUNTER como PK.  Eso requiere que todos los campos sean de tipo COUNTER.", fila, columna);
                            }
                        }
                        // 5. Si no existe un campo de tipo COUNTER en la lista de llaves primarias compuestas, procedo a validar que todos los campos
                        // listados sean de tipo primitivo.
                        else if (ValidatePrimitiveTypes())
                        {
                            return true;
                        }
                        else
                        {
                            CQL.AddLUPError("Semántico", "[CREATE_TABLE]", "Error.  Se indicó un campo como PK que no es de tipo primitivo ni de tipo counter.", fila, columna);
                        }
                    }
                    else
                    {
                        CQL.AddLUPError("Semántico", "[CREATE_TABLE]", "Error.  Se indicó un campo en la lista de llaves primarias compuestas que no existe en la declaración.", fila, columna);
                    }
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[CREATE_TABLE]", "Error.  Al estar definida la cláusula de llaves primarias compuestas no puede venir un campo que sea PK.", fila, columna);
                }
            }
            else
            {
                // 3. Valido que solo exista 1 campo con PK o ninguno.
                int validate_pk = ValidateZeroOrOnePK();

                if (validate_pk.Equals(1))
                {
                    // 4. Valido que la columna PK sea de tipo primitivo.
                    if(ValidatePrimitivePK())
                    {
                        return true;
                    }
                    else
                    {
                        CQL.AddLUPError("Semántico", "[CREATE_TABLE]", "Error.  La columna definida como PRIMARY KEY no es de tipo primitivo.", fila, columna);
                    }
                }
                else if (validate_pk.Equals(0))
                {
                    if (!ValidateIfCounterFieldExists())
                    {
                        return true;
                    }
                    else
                    {
                        CQL.AddLUPError("Semántico", "[CREATE_TABLE]", "Error.  No se puede definir campos de tipo COUNTER si no se especifíca la cláusula PRIMARY KEY.", fila, columna);
                    }
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[CREATE_TABLE]", "Error.  Únicamente debe existir una o ninguna columna de tipo PRIMARY KEY.", fila, columna);
                }
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[CREATE_TABLE]", "Error.  No se puede crear una tabla con nombres de columna repetidos.", fila, columna);
        }

        return false;
    }
    
    private bool ExistRepeatedColumns()
    {
        return ListaColumnas.GroupBy(x => x.NombreColumna).Where(item => item.Count() > 1).Count() > 1;
    }

    private bool ExistPKField()
    {
        return ListaColumnas.Any(x => x.IsPK.Equals(true));
    }

    private bool ValidateExistenceOfColumns()
    {
        foreach (string field in ListaLlavesPrimarias)
        {
            if (!ListaColumnas.Any(x => x.NombreColumna.Equals(field)))
            {
                return false;
            }
        }

        return true;
    }

    private bool ExistAtLeastOneCounterField()
    {
        foreach (string field in ListaLlavesPrimarias)
        {
            if(ListaColumnas.Find(x => x.NombreColumna.Equals(field)).TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.COUNTER))
            {
                return true;
            }
        }

        return false;
    }

    private bool ValidateAllCounterValues()
    {
        foreach (string field in ListaLlavesPrimarias)
        {
            if (!ListaColumnas.Find(x => x.NombreColumna.Equals(field)).TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.COUNTER))
            {
                return false;
            }
        }

        return true;
    }

    private bool ValidatePrimitiveTypes()
    {
        foreach (string field in ListaLlavesPrimarias)
        {
            Columna found = ListaColumnas.Find(x => x.NombreColumna.Equals(field));

            if (!(
                found.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.INT) ||
                found.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.DOUBLE) ||
                found.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.STRING) ||
                found.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.BOOLEAN) ||
                found.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.DATE) ||
                found.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.TIME)
            ))
            {
                return false;
            }
        }

        return true;
    }

    private int ValidateZeroOrOnePK()
    {
        int response = 0;

        var nuevaLista = ListaColumnas.GroupBy(x => x.IsPK).Select(group => new
        {
            PKey = group.Key,
            Count = group.Count()
        });

        foreach (var list in nuevaLista)
        {
            if (list.PKey.Equals(true))
            {
                response = (list.Count == 1) ? 1 : -1;
                break;
            }
        }

        return response;
    }

    private bool ValidatePrimitivePK()
    {
        Columna found = ListaColumnas.Find(x => x.IsPK.Equals(true));

        if (!(
            found.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.INT) ||
            found.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.DOUBLE) ||
            found.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.STRING) ||
            found.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.BOOLEAN) ||
            found.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.DATE) ||
            found.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.TIME) ||
            found.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.COUNTER)
        ))
        {
            return false;
        }

        return true;
    }

    private bool ValidateIfCounterFieldExists()
    {
        return ListaColumnas.Any(x => x.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.COUNTER));
    }
    
    private Table BuildTable()
    {
        Table tabla = new Table(NombreTabla);

        foreach (Columna col in ListaColumnas)
        {
            tabla.AddColumn(col);
        }

        tabla.SetPrimaryKeys(BuildPKsArray());

        return tabla;
    }

    private Columna[] BuildPKsArray()
    {
        List<Columna> pkList = new List<Columna>();

        foreach (Columna col in ListaColumnas)
        {
            if (ListaLlavesPrimarias is null)
            {
                if (col.IsPK)
                {
                    pkList.Add(col);
                }
            }
            else
            {
                if (ListaLlavesPrimarias.Contains(col.NombreColumna))
                {
                    pkList.Add(col);
                }
            }
        }
        
        return pkList.ToArray();
    }

}