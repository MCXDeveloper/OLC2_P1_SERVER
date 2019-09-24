using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Procedimiento : InstruccionBD
{
    public string NombreProcedimiento { get; set; }
    public List<Parametro> ListaRetornos { get; set; }
    public List<Parametro> ListaParametros { get; set; }
    public List<Instruccion> ListaInstrucciones { get; set; }

    public Procedimiento(string nombre_procedimiento, List<Parametro> lista_params, List<Parametro> lista_returns, List<Instruccion> lista_instrucciones)
    {
        ListaRetornos = lista_returns;
        ListaParametros = lista_params;
        ListaInstrucciones = lista_instrucciones;
        NombreProcedimiento = nombre_procedimiento;
    }

    public string CrearChison()
    {
        throw new NotImplementedException();
    }

    public string CrearPaqueteLUP(string user)
    {
        return "[+PROCEDURES]"+ NombreProcedimiento +"[-PROCEDURES]";
    }
}