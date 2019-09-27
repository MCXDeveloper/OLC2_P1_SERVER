using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Procedimiento : InstruccionBD
{
    public string NombreProcedimiento { get; set; }
    public string InstruccionesEnString { get; set; }
    public List<Parametro> ListaRetornos { get; set; }
    public List<Parametro> ListaParametros { get; set; }
    public List<Instruccion> ListaInstrucciones { get; set; }

    public Procedimiento(string nombre_procedimiento, List<Parametro> lista_params, List<Parametro> lista_returns, List<Instruccion> lista_instrucciones, string ins_en_texto)
    {
        ListaRetornos = lista_returns;
        ListaParametros = lista_params;
        InstruccionesEnString = ins_en_texto;
        ListaInstrucciones = lista_instrucciones;
        NombreProcedimiento = nombre_procedimiento;
    }

    public string CrearChison(int numTabs)
    {
        List<string> ChisonParametros = new List<string>();

        /* OPEN CHISON */
        string chison = new string('\t', numTabs + 1) + "<" + Environment.NewLine;

        /* CHISON CQL-TYPE */
        chison += new string('\t', numTabs + 2) + "\"CQL-TYPE\" = \"PROCEDURE\", " + Environment.NewLine;

        /* CHISON NAME */
        chison += new string('\t', numTabs + 2) + "\"NAME\" = \""+ NombreProcedimiento +"\", " + Environment.NewLine;

        /* CHISON PARAMETERS */
        chison += new string('\t', numTabs + 2) + "\"PARAMETERS\" = [" + Environment.NewLine;

        /* CHISON DE PARAMETROS DE ENTRADA */
        foreach (Parametro p in ListaParametros)
        {
            p.TipoParametro = "IN";
            ChisonParametros.Add(p.CrearChison(numTabs + 2));
        }

        /* CHISON DE PARAMETROS DE SALIDA */
        foreach (Parametro p in ListaRetornos)
        {
            p.TipoParametro = "OUT";
            ChisonParametros.Add(p.CrearChison(numTabs + 2));
        }

        chison += string.Join(", ", ChisonParametros);
        chison += new string('\t', numTabs + 2) + "], " + Environment.NewLine;

        /* CHISON INSTR */
        chison += new string('\t', numTabs + 2) + "\"INSTR\" = $ " + InstruccionesEnString + " $" + Environment.NewLine;

        /* CLOSE CHISON */
        chison += new string('\t', numTabs + 1) + ">" + Environment.NewLine;

        return chison;
    }

    public string CrearPaqueteLUP(string user)
    {
        return "[+PROCEDURES]"+ NombreProcedimiento +"[-PROCEDURES]";
    }
}