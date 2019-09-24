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

    public string CrearChison()
    {
        // TODO | UserType | Escribir función de CrearChison.
        throw new NotImplementedException();
    }

    public string CrearPaqueteLUP(string user)
    {
        string response = "[+TYPE]";
        response += "[+NAME]"+ Identificador +"[-NAME]";
        foreach (AtributoUT aut in ListaAtributos)
        {
            response += "[+ATTRIBUTES]" + aut.Identificador + "[-ATTRIBUTES]";
        }
        response += "[-TYPE]";
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