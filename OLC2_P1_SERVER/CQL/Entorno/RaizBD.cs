using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class RaizBD : InstruccionBD
{
    public Dictionary<string, Database> ListaDatabase { get; set; }

    public RaizBD()
    {
        ListaDatabase = new Dictionary<string, Database>();
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

    public bool HasPermissions(string NombreUsuario, string NombreBD)
    {
        return ListaDatabase[NombreBD].ListaUsuariosConPermisos.Contains(NombreUsuario) || ListaDatabase[NombreBD].UsuarioCreador.Equals(NombreUsuario);
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

    public string CrearPaqueteLUP()
    {
        // TODO | RaizBD | Escribir función de CrearPaqueteLup.
        throw new NotImplementedException();
    }
}