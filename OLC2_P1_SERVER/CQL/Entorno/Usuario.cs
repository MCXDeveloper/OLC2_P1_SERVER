using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Usuario : InstruccionBD
{
    public string NombreUsuario { get; set; }
    public string PasswordUsuario { get; set; }
    public List<string> ListaDeBaseDeDatos { get; set; }

    public Usuario(string user, string pass)
    {
        NombreUsuario = user;
        PasswordUsuario = pass;
        ListaDeBaseDeDatos = new List<string>();
    }

    public string CrearChison(int numTabs)
    {
        List<string> ChisonDBS = new List<string>();
        string chison = new string('\t', numTabs + 1) + "<" + Environment.NewLine;

        chison += new string('\t', numTabs + 2) + "\"NAME\" = \""+ NombreUsuario +"\", " + Environment.NewLine;
        chison += new string('\t', numTabs + 2) + "\"PASSWORD\" = \"" + PasswordUsuario + "\", " + Environment.NewLine;
        chison += new string('\t', numTabs + 2) + "\"PERMISSIONS\" = [" + Environment.NewLine;

        /* CHISON DE BASES DE DATOS A LOS QUE EL USUARIO TIENE PERMISOS */
        foreach (string db in ListaDeBaseDeDatos)
        {
            ChisonDBS.Add(new string('\t', numTabs + 3) + "< \"NAME\" = \""+ db +"\" >" + Environment.NewLine);
        }

        chison += string.Join(", ", ChisonDBS);
        chison += new string('\t', numTabs + 2) + "]" + Environment.NewLine;
        chison += new string('\t', numTabs + 1) + ">" + Environment.NewLine;

        return chison;
    }

    public string CrearPaqueteLUP(string user)
    {
        throw new NotImplementedException();
    }
}