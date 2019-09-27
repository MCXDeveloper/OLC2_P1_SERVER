using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class RaizBD : InstruccionBD
{
    public Dictionary<string, Database> ListaDatabase { get; set; }

    public RaizBD()
    {
        ListaDatabase = new Dictionary<string, Database>(StringComparer.InvariantCultureIgnoreCase);
    }

    #region FUNCIONES_DE_BD

    public Database GetDatabase(string nombre_bd)
    {
        return ListaDatabase[nombre_bd];
    }

    public bool ExistsDatabase(string NombreBD)
    {
        return ListaDatabase.ContainsKey(NombreBD);
    }
    
    public void InsertDatabase(string NombreBD, Database BD)
    {
        ListaDatabase.Add(NombreBD, BD);
    }

    public void DeleteDatabase(string NombreBD)
    {
        ListaDatabase.Remove(NombreBD);
    }

    #endregion

    public string CrearChison(int numTabs)
    {
        #region CHISON_DE_BASES_DE_DATOS

        List<string> ChisonBDS = new List<string>();
        string chisonDB = new string('\t', numTabs + 1) + "\"DATABASES\" = [" + Environment.NewLine;

        foreach (KeyValuePair<string, Database> entryDB in ListaDatabase)
        {
            ChisonBDS.Add(entryDB.Value.CrearChison(numTabs + 1));
        }

        chisonDB += string.Join(", ", ChisonBDS);
        chisonDB += new string('\t', numTabs + 1) + "], ";

        #endregion

        #region CHISON_DE_USUARIOS

        List<string> ChisonUsuarios = new List<string>();
        string chisonUS = new string('\t', numTabs + 1) + "\"USERS\" = [" + Environment.NewLine;

        foreach (Usuario ux in CQL.ListaUsuariosDisponibles)
        {
            ChisonUsuarios.Add(ux.CrearChison(numTabs + 1));
        }

        chisonUS += string.Join(", ", ChisonUsuarios);
        chisonUS += new string('\t', numTabs + 1) + "]" + Environment.NewLine;

        #endregion

        if (ChisonBDS.Count > 0 || ChisonUsuarios.Count > 0)
        {
            return "$<" + Environment.NewLine + chisonDB + Environment.NewLine + chisonUS + ">$";
        }
        else
        {
            return string.Empty;
        }
    }

    public string CrearPaqueteLUP(string user)
    {
        string response = "[+DATABASES]";

        // Obtengo todas las bases de datos donde el user brindado en el parámetro sea el creador.
        foreach (KeyValuePair<string, Database> entry in ListaDatabase)
        {
            if (user.Equals("admin", StringComparison.InvariantCultureIgnoreCase))
            {
                response += entry.Value.CrearPaqueteLUP(user);
            }
            else if (entry.Value.UsuarioCreador.Equals(user, StringComparison.InvariantCultureIgnoreCase))
            {
                response += entry.Value.CrearPaqueteLUP(user);
            }
        }

        response += "[-DATABASES]";

        return response;
    }
}