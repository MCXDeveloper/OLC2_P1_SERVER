using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Parametro : InstruccionBD
{
    public string TipoParametro { get; set; } // Sirve para crear el CHISON.  Su valores deben ser IN o OUT.
    public string NombreParametro { get; set; }
    public TipoDato TipoDatoParametro { get; set; }
    
    public Parametro(TipoDato tipo_dato_parametro, string nombre_parametro)
    {
        NombreParametro = nombre_parametro;
        TipoDatoParametro = tipo_dato_parametro;
    }

    public string CrearPaqueteLUP(string user)
    {
        throw new NotImplementedException();
    }

    public string CrearChison(int numTabs)
    {
        string chison = new string('\t', numTabs + 1) + "<" + Environment.NewLine;
        chison += new string('\t', numTabs + 2) + "\"NAME\" = \"" + NombreParametro + "\", " + Environment.NewLine;
        chison += new string('\t', numTabs + 2) + "\"TYPE\" = \"" + TipoDatoExtensions.ToFriendlyString(TipoDatoParametro) + "\", " + Environment.NewLine;
        chison += new string('\t', numTabs + 2) + "\"AS\" = " + TipoParametro + Environment.NewLine;
        chison += new string('\t', numTabs + 1) + ">" + Environment.NewLine;
        return chison;
    }
}