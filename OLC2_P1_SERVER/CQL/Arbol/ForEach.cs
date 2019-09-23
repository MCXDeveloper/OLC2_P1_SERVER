using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class ForEach : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public string VariableCursor { get; set; }
    public List<Parametro> ListaCampos { get; set; }
    public List<Instruccion> ListaInstrucciones { get; set; }

    public ForEach(List<Parametro> lista_campos, string variable_cursor, List<Instruccion> lista_instrucciones, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        ListaCampos = lista_campos;
        VariableCursor = variable_cursor;
        ListaInstrucciones = lista_instrucciones;
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Valido que la variable del cursor exista en el entorno.
        object simbolo = ent.ObtenerVariable(VariableCursor);

        if (!(simbolo is Nulo))
        {            
            Variable sim = (Variable)simbolo;

            // 2. Valido que el simbolo obtenido sea de tipo CURSOR.
            if (sim.Tipo.GetRealTipo().Equals(TipoDato.Tipo.CURSOR))
            {
                // 3. Valido que el cusor se encuentre abierto, de lo contrario se arroja error.
                if (sim.Tipo.GetElemento().Equals(TipoAccionCursor.OPEN))
                {
                    // 4. Valido que la lista de campos existan en la variable del cursor y que los tipos sean correctos.
                    Table tab = (Table)sim.Valor;

                    if (ValidarNombreDeCamposYTipos(tab))
                    {
                        foreach (DataRow row in tab.Tabla.Rows)
                        {
                            Entorno local = new Entorno(ent);

                            foreach (Parametro param in ListaCampos)
                            {
                                local.Agregar(param.NombreParametro, new Variable(param.TipoDatoParametro, param.NombreParametro, row[param.NombreParametro.Replace("@", "")]));
                            }

                            foreach (Instruccion ins in ListaInstrucciones)
                            {
                                if (ins is Return || ins is Break || ins is Continue)
                                {
                                    return ins;
                                }
                                else
                                {
                                    ins.Ejecutar(local);
                                }
                            }

                        }
                    }
                    else
                    {
                        CQL.AddLUPError("Semántico", "[FOR_EACH]", "Error. Los nombres de campos y los tipos no concuerdan con los contenidos en el cursor.", fila, columna);
                    }
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[FOR_EACH]", "Error. No se puede ejecutar el FOR EACH ya que el CURSOR se encuentra cerrado.", fila, columna);
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[FOR_EACH]", "Error. La variable proporcionada no es de tipo CURSOR.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[FOR_EACH]", "Error. La variable proporcionada como contenedor del cursor (" + VariableCursor + "), no existe en el entorno.", fila, columna);
        }

        return new Nulo();
    }

    private bool ValidarNombreDeCamposYTipos(Table tablaCursor)
    {
        DataTable dt = tablaCursor.Tabla;

        foreach (Parametro param in ListaCampos)
        {
            if (dt.Columns.Contains(param.NombreParametro))
            {
                if (!(
                    (dt.Columns[param.NombreParametro].DataType.Equals(typeof(int)) && param.TipoDatoParametro.GetRealTipo().Equals(TipoDato.Tipo.INT)) ||
                    (dt.Columns[param.NombreParametro].DataType.Equals(typeof(double)) && param.TipoDatoParametro.GetRealTipo().Equals(TipoDato.Tipo.DOUBLE)) ||
                    (dt.Columns[param.NombreParametro].DataType.Equals(typeof(string)) && param.TipoDatoParametro.GetRealTipo().Equals(TipoDato.Tipo.STRING)) ||
                    (dt.Columns[param.NombreParametro].DataType.Equals(typeof(bool)) && param.TipoDatoParametro.GetRealTipo().Equals(TipoDato.Tipo.BOOLEAN)) ||
                    (dt.Columns[param.NombreParametro].DataType.Equals(typeof(DateTime)) && (param.TipoDatoParametro.GetRealTipo().Equals(TipoDato.Tipo.DATE) || param.TipoDatoParametro.GetRealTipo().Equals(TipoDato.Tipo.TIME))) ||
                    (dt.Columns[param.NombreParametro].DataType.Equals(typeof(object)) && (param.TipoDatoParametro.GetRealTipo().Equals(TipoDato.Tipo.MAP) || param.TipoDatoParametro.GetRealTipo().Equals(TipoDato.Tipo.SET) || param.TipoDatoParametro.GetRealTipo().Equals(TipoDato.Tipo.LIST)))
                ))
                {
                    return false;
                }
            }
        }

        return true;
    }
}