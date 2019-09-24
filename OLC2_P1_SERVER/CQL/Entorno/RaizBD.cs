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

    public string CrearChison()
    {
        // TODO | RaizBD | Escribir función de CrearChison.
        throw new NotImplementedException();
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