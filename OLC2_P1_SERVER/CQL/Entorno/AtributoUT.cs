using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static TipoDato;

public class AtributoUT : InstruccionBD
{
    public TipoDato Tipo { get; set; }
    public string Identificador { get; set; }

    public AtributoUT(TipoDato tipo, string identificador)
    {
        Tipo = tipo;
        Identificador = identificador;
    }

    public override string ToString()
    {
        return "{ Tipo_Dato : "+ Tipo.GetRealTipo().ToString() +" | Elemento : "+ Identificador +"}";
    }

    public string CrearPaqueteLUP(string user)
    {
        throw new NotImplementedException();
    }

    public string CrearChison(int numTabs)
    {
        string chison = new string('\t', numTabs + 1) + "<" + Environment.NewLine;
        chison += new string('\t', numTabs + 2) + "\"NAME\" = \"" + Identificador + "\", " + Environment.NewLine;
        chison += new string('\t', numTabs + 2) + "\"TYPE\" = \"" + TipoDatoExtensions.ToFriendlyString(Tipo) + "\"" + Environment.NewLine;
        chison += new string('\t', numTabs + 1) + ">" + Environment.NewLine;
        return chison;
    }
}