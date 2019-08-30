using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Asignacion;

public class AsignacionObjeto : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    private readonly string variable;
    private readonly Expresion valor;
    private readonly TipoAsignacion tipo_asig;
    private readonly List<string> lista_acceso;

    public AsignacionObjeto(TipoAsignacion tipo_asig, string variable, List<string> lista_acceso, Expresion valor, int fila, int columna)
    {
        this.fila = fila;
        this.valor = valor;
        this.columna = columna;
        this.variable = variable;
        this.tipo_asig = tipo_asig;
        this.lista_acceso = lista_acceso;
    }

    public object Ejecutar(Entorno ent)
    {
        throw new NotImplementedException();
    }
}