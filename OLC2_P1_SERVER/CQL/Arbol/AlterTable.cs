﻿using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class AlterTable : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public string NombreTabla { get; set; }
    public List<string> ListaColumnasDrop { get; set; }
    public List<Columna> ListaColumnasAdd { get; set; }

    public AlterTable(string nombre_tabla, List<Columna> lista_columnas_add, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        ListaColumnasDrop = null;
        NombreTabla = nombre_tabla;
        ListaColumnasAdd = lista_columnas_add;
    }

    public AlterTable(string nombre_tabla, List<string> lista_columnas_drop, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        ListaColumnasAdd = null;
        NombreTabla = nombre_tabla;
        ListaColumnasDrop = lista_columnas_drop;
    }

    public object Ejecutar(Entorno ent)
    {
        // +------------------------------------------------------------------------------------------------+
        // |                                              Nota                                              |
        // +------------------------------------------------------------------------------------------------+
        // | Para realizar de forma correcta la alteración de tablas, se deben seguir los siguientes pasos: |
        // | 1. Se debe validar que exista una base de datos en uso actual para poder alterar la tabla.     |
        // | 2. Se debe validar que la tabla que se desea alterar exista en la base de datos.               |
        // | 3. Se debe validar que las columnas que se desean agregar                                      |
        // |    a. No exista una columna con el mismo nombre en la tabla.                                   |
        // |    b. Que la columna que se desea agregar no sea de tipo dato counter.                         |
        // | 4. Se debe validar que las columnas que se desean eliminar:                                    |
        // |    a. Existan en la tabla.                                                                     |
        // |    b. La columna que se desea eliminar no sea una llave primaria o parte de una llave primaria |
        // |    compuesta.                                                                                  |
        // +------------------------------------------------------------------------------------------------+

        // 1. Procedo a verificar si existe alguna base de datos en uso, de lo contrario, se reporta el error.
        if (!CQL.BaseDatosEnUso.Equals(String.Empty))
        {
            // 2. Procedo a verificar que la tabla que se desea alterar exista en la base de datos.
            if (CQL.RootBD.GetDatabase(CQL.BaseDatosEnUso).ExisteTabla(NombreTabla))
            {
                // 3. Procedo a validar si la acción que se desea realizar es un ADD o un DROP.
                if (ListaColumnasAdd is null)
                {
                    // 4. Procedo a realizar una validación exhaustiva para ver si se cumple o no el agregar columnas.
                    if (ValidateAddingColumns())
                    {
                        // 5. Si la validación de columnas es correcta, procedo a agregarlas.
                        AddColumns();
                    }
                    else
                    {
                        Error.AgregarError("Semántico", "[ALTER_TABLE]", "Error.  No se pueden agregar columnas que ya existen en la tabla ni columnas que sean de tipo COUNTER.", fila, columna);
                    }
                }
                else
                {
                    // 4. Procedo a realizar una validación exhaustiva para ver si se cumple o no el eliminar columnas.
                    if (ValidateDeletingColumns())
                    {
                        // 5. Si la validación de columnas es correcta, procedo a eliminarlas.
                        DeleteColumns();
                    }
                    else
                    {
                        Error.AgregarError("Semántico", "[ALTER_TABLE]", "Error.  No se pueden eliminar columnas que no existen dentro de la tabla.", fila, columna);
                    }
                }
            }
            else
            {
                Error.AgregarError("Semántico", "[ALTER_TABLE]", "Error.  La tabla especificada '"+ NombreTabla +"' no existe en la base de datos actual.", fila, columna);
            }
        }
        else
        {
            Error.AgregarError("Semántico", "[ALTER_TABLE]", "Error.  No se puede alterar una tabla si no se ha especificado la base de datos a utilizar.", fila, columna);
        }

        return new Nulo();
    }

    private bool ValidateAddingColumns()
    {
        // 1. Primero valido que no exista una columna con el mismo nombre dentro de la tabla.
        foreach (Columna col in ListaColumnasAdd)
        {
            if(CQL.RootBD.GetDatabase(CQL.BaseDatosEnUso).ObtenerTabla(NombreTabla).ExistsColumn(col.NombreColumna))
            {
                return false;
            }
            else
            {
                // 2. Valido que el tipo de dato de la columna no sea counter
                if (ValidateIfColumnIsCounter(col))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool ValidateDeletingColumns()
    {
        foreach (string col in ListaColumnasDrop)
        {
            // 1. Valido que las columnas que se desean eliminar existan en la tabla
            if (!CQL.RootBD.GetDatabase(CQL.BaseDatosEnUso).ObtenerTabla(NombreTabla).ExistsColumn(col))
            {
                return false;
            }
            else
            {
                // 2. Valido que la columna que se desea eliminar, no sea PK.
                if (ValidateIfColumnIsPK(col))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool ValidateIfColumnIsPK(string colName)
    {
        return CQL.RootBD.GetDatabase(CQL.BaseDatosEnUso).ObtenerTabla(NombreTabla).IsPrimaryKeyColumn(colName);
    }

    private bool ValidateIfColumnIsCounter(Columna col)
    {
        return col.TipoDatoColumna.GetRealTipo().Equals(TipoDato.Tipo.COUNTER);
    }

    private void AddColumns()
    {
        foreach (Columna col in ListaColumnasAdd)
        {
            CQL.RootBD.GetDatabase(CQL.BaseDatosEnUso).ObtenerTabla(NombreTabla).AddColumn(col);
        }
    }

    private void DeleteColumns()
    {
        foreach (string col in ListaColumnasDrop)
        {
            CQL.RootBD.GetDatabase(CQL.BaseDatosEnUso).ObtenerTabla(NombreTabla).DeleteColumn(col);
        }
    }

}