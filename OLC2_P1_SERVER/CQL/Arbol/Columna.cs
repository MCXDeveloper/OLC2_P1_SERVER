using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class Columna : DataColumn, InstruccionBD
{
    public bool IsPK { get; set; }
    public string NombreColumna { get; set; }
    public TipoDato TipoDatoColumna { get; set; }

    public Columna(bool is_pk, string nombre_columna, TipoDato tipo_dato)
    {
        IsPK = is_pk;
        TipoDatoColumna = tipo_dato;
        NombreColumna = nombre_columna;
    }

    public string CrearPaqueteLUP(string user)
    {
        throw new NotImplementedException();
    }

    public string CrearChison(int numTabs)
    {
        string chison = new string('\t', numTabs + 1) + "<" + Environment.NewLine;
        chison += new string('\t', numTabs + 2) + "\"NAME\" = \"" + NombreColumna +"\", " + Environment.NewLine;
        chison += new string('\t', numTabs + 2) + "\"TYPE\" = \"" + TipoDatoExtensions.ToFriendlyString(TipoDatoColumna) + "\", " + Environment.NewLine;
        chison += new string('\t', numTabs + 2) + "\"PK\" = " + IsPK.ToString() + Environment.NewLine;
        chison += new string('\t', numTabs + 1) + ">" + Environment.NewLine;
        return chison;
    }
}