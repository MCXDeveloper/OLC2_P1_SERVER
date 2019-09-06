using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class DeclaracionProcedimiento : Instruccion
{
    public string NombreProcedimiento { get; set; }
    public List<Parametro> ListaRetornos { get; set; }
    public List<Parametro> ListaParametros { get; set; }
    public List<Instruccion> ListaInstrucciones { get; set; }

    public DeclaracionProcedimiento(string nombre_procedimiento, List<Parametro> lista_parametros, List<Parametro> lista_retornos, List<Instruccion> lista_instrucciones)
    {
        ListaRetornos = lista_retornos;
        ListaParametros = lista_parametros;
        ListaInstrucciones = lista_instrucciones;
        NombreProcedimiento = nombre_procedimiento;
    }

    public object Ejecutar(Entorno ent)
    {
        // TODO | DeclaracionProcedimiento | Realizar accion de almacenamiento cuando se declara un procedimiento.
        throw new NotImplementedException();
    }
}