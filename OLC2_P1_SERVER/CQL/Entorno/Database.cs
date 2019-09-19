using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Database : InstruccionBD
{
    public string NombreBD { get; set; }
    public string UsuarioCreador { get; set; }
    public List<Table> ListaTablas { get; set; }
    public List<UserType> ListaUserTypes { get; set; }
    public List<string> ListaUsuariosConPermisos { get; set; }
    public List<Procedimiento> ListaProcedimientos { get; set; }

    public Database(string nombre_bd)
    {
        NombreBD = nombre_bd;
        ListaTablas = new List<Table>();
        UsuarioCreador = CQL.UsuarioLogueado;
        ListaUserTypes = new List<UserType>();
        ListaUsuariosConPermisos = new List<string>();
        ListaProcedimientos = new List<Procedimiento>();
    }

    #region FUNCIONES_DE_TABLAS

    public bool ExisteTabla(string nombre_tabla)
    {
        return ListaTablas.Any(tab => tab.NombreTabla.Equals(nombre_tabla));
    }

    public void RegistrarTabla(Table tabla)
    {
        ListaTablas.Add(tabla);
    }

    public Table ObtenerTabla(string nombre_tabla)
    {
        return ListaTablas.Find(x => x.NombreTabla.Equals(nombre_tabla));
    }

    public void EliminarTabla(string nombre_tabla)
    {
        ListaTablas.RemoveAll(x => x.NombreTabla.Equals(nombre_tabla));
    }

    public void TruncarTabla(string nombre_tabla)
    {
        ObtenerTabla(nombre_tabla).Tabla.Rows.Clear();
    }

    #endregion

    #region FUNCIONES_DE_USERTYPES

    public string UserTypeListToString()
    {
        string resp = "[";

        foreach (UserType ut in ListaUserTypes)
        {
            resp += ut.ToString();
            resp += ListaUserTypes.Last().Equals(ut) ? "" : ",";
        }

        return resp + "]";
    }

    public object ObtenerUserType(string nombre_user_type)
    {
        return ListaUserTypes.Find(x => x.Identificador.Equals(nombre_user_type));
    }

    public bool ExisteUserType(string nombre_user_type)
    {
        return ListaUserTypes.Any(ut => ut.Identificador.Equals(nombre_user_type));
    }

    public void RegistrarUserType(UserType item)
    {
        ListaUserTypes.Add(item);
    }

    #endregion

    public string CrearChison()
    {
        throw new NotImplementedException();
    }

    public string CrearPaqueteLUP()
    {
        throw new NotImplementedException();
    }
}