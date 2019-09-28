using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class UserType : InstruccionBD
{
    public string Identificador { get; set; }
    public List<AtributoUT> ListaAtributos { get; set; }
    
    public UserType(string identificador, List<AtributoUT> lista_atributos)
    {
        Identificador = identificador;
        ListaAtributos = lista_atributos;
    }

    public string CrearChison(int numTabs)
    {
        List<string> ChisonAtributos = new List<string>();

        string chison = new string('\t', numTabs + 1) + "<" + Environment.NewLine;
        chison += new string('\t', numTabs + 2) + "\"CQL-TYPE\" = \"OBJECT\", " + Environment.NewLine;
        chison += new string('\t', numTabs + 2) + "\"NAME\" = \"" + Identificador + "\", " + Environment.NewLine;
        chison += new string('\t', numTabs + 2) + "\"ATTRS\" = [" + Environment.NewLine;

        foreach (AtributoUT aout in ListaAtributos)
        {
            ChisonAtributos.Add(aout.CrearChison(numTabs + 2));
        }

        chison += string.Join(", ", ChisonAtributos);
        chison += new string('\t', numTabs + 2) + "]" + Environment.NewLine;
        chison += new string('\t', numTabs + 1) + ">" + Environment.NewLine;

        return chison;
    }

    public string CrearPaqueteLUP(string user)
    {
        string response = "[+TYPEX]";
        response += "[+NAME]"+ Identificador +"[-NAME]";
        foreach (AtributoUT aut in ListaAtributos)
        {
            response += "[+ATTRIBUTES]" + aut.Identificador + "[-ATTRIBUTES]";
        }
        response += "[-TYPEX]";
        return response;
    }

    public override string ToString()
    {
        string response = "{ UserType : "+ Identificador +" | Lista_de_atributos : ";

        foreach (AtributoUT aut in ListaAtributos)
        {
            response += aut.ToString();
            response += ListaAtributos.Last().Equals(aut) ? "" : " , ";
        }

        return response + " }";
    }
}