using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class Select : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public string NombreTabla { get; set; }
    public bool IsFuncionAgregacion { get; set; }
    public Expresion ExpresionWhere { get; set; }
    public Expresion ExpresionLimit { get; set; }
    public List<Expresion> ListaCampos { get; set; }
    public List<Order> ListaOrdenamiento { get; set; }
    
    public Select(List<Expresion> lista_campos, string nombre_tabla, Expresion valor_where, Expresion valor_limit, List<Order> lista_ordenamiento, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        ListaCampos = lista_campos;
        NombreTabla = nombre_tabla;
        IsFuncionAgregacion = false;
        ExpresionWhere = valor_where;
        ExpresionLimit = valor_limit;
        ListaOrdenamiento = lista_ordenamiento;
    }

    public object Ejecutar(Entorno ent)
    {
        // +-----------------------------------------------------------------------------------------------------------------------------------------------------+
        // |                                                                       Nota                                                                          |
        // +-----------------------------------------------------------------------------------------------------------------------------------------------------+
        // | Para realizar de forma correcta el select en una tabla, se deben seguir los siguientes pasos:                                                       |
        // | 1. Se debe validar que exista una base de datos en uso.                                                                                             |
        // | 2. Se debe validar que la tabla exista en la base de datos.                                                                                         |
        // | 3. Si la lista de campos proporcionada es distinto de null, se realiza lo siguiente:                                                                |
        // |    a. Se debe validar que todas las expresiones dentro de la lista de campos sean de tipo primitivo o de acceso a columna (identificador . ACCESO), |
        // |    de acceso a collection (identificador [ EXPRESION ]), de columna-tabla (identificador).                                                          |
        // | 4. Para las expresiones anteriores se debe validar lo siguiente:                                                                                    |
        // |                                                                                                                                                     |
        // |    EXPRESION ACCESO-COLUMNA                                                                                                                         |
        // |    a. Se debe validar que la columna exista en la tabla.                                                                                            |
        // |    c. Se debe validar que la columna corresponda al tipo de dato de un objeto.                                                                      |
        // |    b. Se debe validar, de forma recursiva, que cada elemento en lista de acceso, exista en el objeto.                                               |
        // |                                                                                                                                                     |
        // |    EXPRESION ACCESO-COLLECTION                                                                                                                      |
        // |    a. Se debe validar que la columna exista en la tabla.                                                                                            |
        // |    b. Se debe validar que la columna corresponda al tipo de dato de un collection.                                                                  |
        // |    c. Se debe validar que la posición proporcionada exista en el collection.                                                                        |
        // |                                                                                                                                                     |
        // |    EXPRESION COLUMNA-TABLA                                                                                                                          |
        // |    a. Se debe validar que la columna exista en la tabla.                                                                                            |
        // |                                                                                                                                                     |
        // |    EXPRESION                                                                                                                                        |
        // |    a. Se debe de obtener el valor y repetirlo la cantidad de filas que contenga la tabla ya filtrada (WHERE).                                       |
        // +-----------------------------------------------------------------------------------------------------------------------------------------------------+

        // 1. Primero valido que exista una base de datos en uso.
        if (CQL.ExisteBaseDeDatosEnUso())
        {
            // 2. Valido que la tabla exista en la base de datos.
            if (CQL.ExisteTablaEnBD(NombreTabla))
            {
                // 3. Para ejecutar de forma correcta la consulta, se debe de realizar en el siguiente orden:
                //    3.1 FROM
                //    3.2 WHERE
                //    3.3 SELECT
                //    3.4 ORDER BY
                //    3.5 LIMIT

                // 3.1  FROM - Esa tabla obtenida se mantiene de forma estática para acceder a ella en la siguiente validaciones.
                CQL.TablaEnUso = CQL.ObtenerTabla(NombreTabla);

                // 3.2  WHERE - En una tabla temporal almaceno el resultado de ejecutar el where (si hubiese, de lo contrario se sigue utilizando la tabla original).
                CQL.WhereFlag = true;
                CQL.TuplaEnUso = null;
                Table tablaTemp = (!(ExpresionWhere is null) ? EjecutarWhere(CQL.TablaEnUso.Tabla, ent) : CQL.TablaEnUso);
                CQL.WhereFlag = false;
                CQL.TuplaEnUso = null;

                if (!(tablaTemp is null))
                {
                    // 3.3 SELECT - En una nueva tabla temporal almaceno el resultado de seleccionar solo ciertas columnas (si hubiese, de lo contrario se muestran todas).
                    CQL.SelectFlag = true;
                    CQL.TuplaEnUso = null;
                    Table tablaTemp2 = !(ListaCampos is null) ? EjecutarSeleccion(tablaTemp, ent) : tablaTemp;
                    CQL.SelectFlag = false;
                    CQL.TuplaEnUso = null;

                    if (!(tablaTemp2 is null))
                    {
                        // 3.4 ORDER BY - En una nueva tabla temporal almaceno el resultado de ordernar (si hubiese, de lo contrario, se deja la misma) la tabla resultante del select y el where.
                        Table tablaTemp3 = !(ListaOrdenamiento is null) ? EjecutarOrderBy(tablaTemp2) : tablaTemp2;

                        // 3.5 LIMIT - En una nueva tabla temporal almaceno el resultado de obtener solo la cantidad de filas especificada por la expresión (si hubiese).
                        Table tablaTemp4 = !(ExpresionLimit is null) ? EjecutarLimit(tablaTemp3, ent) : tablaTemp3;

                        // Agrego a la pila de respuestas el resultado del select.
                        if (!IsFuncionAgregacion)
                        {
                            CQL.AddLUPData(GetSelectInAscii(tablaTemp4));
                        }

                        return tablaTemp4;
                    }
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[SELECT]", "Error.  La tabla especificada '" + NombreTabla + "' no existe en la base de datos actual.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[SELECT]", "Error.  No se puede seleccionar de una tabla si no se ha especificado la base de datos a utilizar.", fila, columna);
        }

        return new Nulo();
    }
    
    private bool ValidateFieldTypesOfSelect()
    {
        foreach (Expresion exp in ListaCampos)
        {
            if (exp is AccesoColumna)
            {
                if (!Validate_AccesoColumna((AccesoColumna)exp))
                {
                    return false;
                }
            }
            else if (exp is AccesoCollection)
            {
                if (!Validate_AccesoCollection((AccesoCollection)exp))
                {
                    return false;
                }
            }
            else if (exp is ColumnaTabla)
            {
                if (!Validate_AccesoColumnaTabla((ColumnaTabla)exp))
                {
                    return false;
                }
            }
            /*else
            {
                if (!(exp is Primitivo))
                {
                    return false;
                }
            }*/
        }

        return true;
    }

    private bool Validate_AccesoColumnaTabla(ColumnaTabla exp)
    {
        // 1. Valido que la columna exista en la tabla.
        if (CQL.ExisteColumnaEnTabla(NombreTabla, exp.NombreColumna))
        {
            return true;
        }
        else
        {
            CQL.AddLUPError("Semántico", "[SELECT]", "Error.  La columna '" + exp.NombreColumna + "' especificada en la instrucción SELECT no existe en la tabla.", fila, columna);
        }

        return false;
    }

    private bool Validate_AccesoCollection(AccesoCollection exp)
    {
        // 1. Valido que la columna exista en la tabla.
        if (CQL.ExisteColumnaEnTabla(NombreTabla, exp.NombreColumna))
        {
            // 2. Valido que el tipo de dato de la columna sea de tipo MAP/SET/LIST.
            if (CQL.ValidarTipoDatoColumna(NombreTabla, exp.NombreColumna, new TipoDato.Tipo[] { TipoDato.Tipo.MAP, TipoDato.Tipo.SET, TipoDato.Tipo.LIST }))
            {
                return true;
            }
            else
            {
                CQL.AddLUPError("Semántico", "[SELECT]", "Error.  La columna '" + exp.NombreColumna + "' debe ser de tipo Objeto (UserType) para intentar acceder a sus propiedades.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[SELECT]", "Error.  La columna '" + exp.NombreColumna + "' especificada en la instrucción SELECT no existe en la tabla.", fila, columna);
        }
        
        return false;
    }

    private bool Validate_AccesoColumna(AccesoColumna exp)
    {
        // 1. Valido que la columna exista en la tabla.
        if (CQL.ExisteColumnaEnTabla(NombreTabla, exp.NombreColumna))
        {
            // 2. Valido que el tipo de dato de la columna sea de tipo Objeto.
            if (CQL.ValidarTipoDatoColumna(NombreTabla, exp.NombreColumna, new TipoDato.Tipo[] { TipoDato.Tipo.OBJECT }))
            {
                // TODO | SELECT | No se si validar de forma recursiva el elemento al que se desea acceder porque no se si eso ya lo hago en otra clase.
                return true;
            }
            else
            {
                CQL.AddLUPError("Semántico", "[SELECT]", "Error.  La columna '" + exp.NombreColumna + "' debe ser de tipo Objeto (UserType) para intentar acceder a sus propiedades.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[SELECT]", "Error.  La columna '"+ exp.NombreColumna +"' especificada en la instrucción SELECT no existe en la tabla.", fila, columna);
        }

        return false;
    }
    
    private Table EjecutarWhere(DataTable tablaOriginal, Entorno ent)
    {
        // La instrucción WHERE se llevará a cabo si y solo sí fue definida.  Para ello se itera sobre cada tupla de la tabla para poder evaluar
        // la expresión del WHERE.  En el caso de que no exista una expresión WHERE, se almacena el valor de la tupla normal en la tablaResult.

        // 1. Creo una nueva tabla para almacenar el resultado del WHERE
        Table tablaResult = new Table(CQL.GenerateName(5));

        // 2. Agrego las columnas de la tabla del FROM a la tabla del resultado.
        AgregarColumnasATabla(tablaOriginal, tablaResult.Tabla);

        // 3. Por cada fila se pregunta si existe la expresión WHERE ya que de lo contrario solo se almacena la tupla.
        for (int i = 0; i < tablaOriginal.Rows.Count; i++)
        {
            CQL.TuplaEnUso = tablaOriginal.Rows[i];
            object whereResponse = ExpresionWhere.Ejecutar(ent);

            if (whereResponse is bool)
            {
                if ((bool)whereResponse)
                {
                    DataRow r = tablaResult.Tabla.NewRow();

                    foreach (DataColumn col in tablaOriginal.Columns)
                    {
                        r[col.ColumnName] = tablaOriginal.Rows[i][col.ColumnName];
                    }

                    tablaResult.Tabla.Rows.Add(r);
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[SELECT]", "Error.  La operación WHERE en la consulta a la tabla '" + NombreTabla + "' no retorna un valor booleano.", fila, columna);
                return null;
            }
        }
        
        return tablaResult;
    }

    private Table EjecutarSeleccion(Table temp, Entorno ent)
    {
        // La instrucción SELECT se llevará a cabo si y solo sí fueron definidos una lista de campos para seleccionarse.  Para ello se va a ejecutar expresión por expresión dentro de la lista de campos.
        // - Aquellas que retornen un valor primitivo se debe crear una nueva columna con el valor proporcionado y repetir sus valores para todas aquellas tuplas existentes dentro de la tabla.
        // - Aquellas que retornen un valor de tipo ColumnaTabla/AccesoColumna/AccesoCollection todas van a retornar su valor correspondiente en String para no tener complicaciones.

        // 1. Valido que los tipos de dato sean los permitidos dentro de un select, de lo contrario, se debe mostrar un error.
        if (ValidateFieldTypesOfSelect())
        {
            // 2. Creo una nueva tabla para almacenar el resultado del SELECT
            Table tablaResult = new Table(CQL.GenerateName(5));

            // 3. Creo un indice que me indica el nombre de la columna calculada.
            int calCol = 0;

            // 4. Agrego los DataRows vacios necesarios para la tabla de respuesta.
            AgregarFilasATabla(temp.Tabla, tablaResult.Tabla);

            foreach (Expresion exp in ListaCampos)
            {
                if (exp is ColumnaTabla)
                {
                    ColumnaTabla ct = (ColumnaTabla)exp;

                    // Verifico si ya existe en la tabla de respuesta la columna calculada.  Si no existe, se la agrego.
                    if (!tablaResult.Tabla.Columns.Contains(ct.NombreColumna))
                    {
                        tablaResult.Tabla.Columns.Add(ct.NombreColumna, temp.Tabla.Columns[ct.NombreColumna].DataType);
                    }

                    // Por cada fila en la tabla de respuesta se almacena el valor correspondiente a la posición de la tabla original.
                    for (int i = 0; i < tablaResult.Tabla.Rows.Count; i++)
                    {
                        tablaResult.Tabla.Rows[i][ct.NombreColumna] = temp.Tabla.Rows[i][ct.NombreColumna];
                    }
                }
                else if (exp is AccesoColumna)
                {
                    object access_response = ((AccesoColumna)exp).Ejecutar(ent);

                    if (!(access_response is Nulo))
                    {
                        tablaResult.Tabla.Merge((DataTable)access_response);
                    }
                }
                else if (exp is AccesoCollection)
                {
                    object access_response = ((AccesoCollection)exp).Ejecutar(ent);

                    if (!(access_response is Nulo))
                    {
                        tablaResult.Tabla.Merge((DataTable)access_response);
                    }
                }
                else
                {
                    // Genero el nombre de la columna calculada.
                    string nombre_columna = "Columna_" + calCol;

                    // Verifico si ya existe en la tabla de respuesta la columna calculada.  Si no existe, se la agrego.
                    if (!tablaResult.Tabla.Columns.Contains(nombre_columna))
                    {
                        tablaResult.Tabla.Columns.Add(nombre_columna);
                    }

                    // Por cada fila de la tabla original se almacena la tupla de forma estática por si el valor de una columna
                    // es utilizado por una expresión.  Al final, se ejecuta esa expresión y el valor devuelto es el almacenado
                    // en la tabla de respuesta.
                    for (int i = 0; i < temp.Tabla.Rows.Count; i++)
                    {
                        DataRow row = temp.Tabla.Rows[i];
                        CQL.TuplaEnUso = row;
                        tablaResult.Tabla.Rows[i][nombre_columna] = exp.Ejecutar(ent).ToString();
                    }
                    
                    calCol++;
                }
            }

            return tablaResult;
        }
        else
        {
            CQL.AddLUPError("Semántico", "[SELECT]", "Error.  Un tipo de dato no permitido existe en la sentencia SELECT.  Estos deben ser solo primitivos o los permitidos en base a las columnas.", fila, columna);
        }
        
        return null;
    }

    private Table EjecutarOrderBy(Table temp)
    {
        // 1. Armo el string que corresponde al ordenamiento proporcionado por el usuario.
        string ordenamiento = "";
        
        foreach (Order orden in ListaOrdenamiento)
        {
            ordenamiento += orden.NombreColumnaOrden + (orden.TipoDeOrden.Equals(String.Empty) ? " ASC" : " " + orden.TipoDeOrden.ToUpper());
            ordenamiento += !ListaOrdenamiento.Last().Equals(orden) ? ", " : "";
        }

        // 2. Construyo un DataTable (copia de temp.Tabla) para poder ordenar bien los valores.
        DataTable cleanDT = ConstruirDataTableLimpio(temp.Tabla);

        // 2. Convierto la tabla recibida por parametro a un DataView para poderlo ordenar.
        cleanDT.DefaultView.Sort = ordenamiento;
        cleanDT = cleanDT.DefaultView.ToTable();

        // 3. Una vez ordenado el DataView, procedo a crear una nueva Table y le adjunto a el atributo Tabla la conversión del DataView a DataTable nuevamente.
        Table tablaResult = new Table(CQL.GenerateName(5));
        tablaResult.Tabla = cleanDT;

        return tablaResult;
    }

    private DataTable ConstruirDataTableLimpio(DataTable dtSucio)
    {
        DataTable dt = new DataTable(dtSucio.TableName);

        foreach (DataColumn col in dtSucio.Columns)
        {
            dt.Columns.Add(col.ColumnName);
        }

        foreach (DataRow row in dtSucio.Rows)
        {
            DataRow dr = dt.NewRow();

            foreach (DataColumn col in dtSucio.Columns)
            {
                dr[col.ColumnName] = row[col.ColumnName];
            }

            dt.Rows.Add(dr);
        }

        return dt;
    }

    private Table EjecutarLimit(Table temp, Entorno ent)
    {
        object limite = ExpresionLimit.Ejecutar(ent);

        if (limite is int)
        {
            Table tablita = new Table(CQL.GenerateName(5));
            tablita.Tabla = temp.Tabla.AsEnumerable().Take((int)limite).CopyToDataTable();
            return tablita;
        }
        else
        {
            CQL.AddLUPError("Semántico", "[SELECT]", "Error.  El valor correspondiente a la cláusula LIMIT debe ser de tipo entero.", fila, columna);
        }

        return null;
    }

    private void AgregarFilasATabla(DataTable source, DataTable destiny)
    {
        for (int i = 0; i < source.Rows.Count; i++)
        {
            DataRow row = destiny.NewRow();
            destiny.Rows.Add(row);
        }
    }
    
    private void AgregarColumnasATabla(DataTable source, DataTable destiny)
    {
        foreach (DataColumn col in source.Columns)
        {
            destiny.Columns.Add(col.ColumnName, col.DataType);
        }
    }

    private string GetSelectInAscii(Table t)
    {
        DataTable dt = new DataTable();
        dt.Clear();

        // Defino las columnas del DataTable
        foreach (DataColumn col in t.Tabla.Columns)
        {
            dt.Columns.Add(col.ColumnName);
        }

        // Por cada fila en la tabla de la BD, creo una nueva fila y para cada columna le agrego su valor en string.
        foreach (DataRow row in t.Tabla.Rows)
        {
            DataRow r = dt.NewRow();

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                object rowVal = row.ItemArray[i];
                r[dt.Columns[i].ColumnName] = (rowVal.Equals("OLC2_P1_SERVER.CQL.Arbol.Nulo")) ? "NULL" : rowVal.ToString();
            }

            dt.Rows.Add(r);
        }

        // Una vez tenemos el DataTable completamente en String retornamos la tabla generada en ASCII.

        return AsciiTableGenerator.CreateAsciiTableFromDataTable(dt).ToString();
    }
}