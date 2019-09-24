using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Database : InstruccionBD
{
    public string NombreBD { get; set; }
    public string UsuarioCreador { get; set; }
    public List<Table> ListaTablas { get; set; }
    public List<Usuario> ListaUsuarios { get; set; }
    public List<UserType> ListaUserTypes { get; set; }
    public List<Usuario> ListaUsuariosConPermisos { get; set; }
    public List<Procedimiento> ListaProcedimientos { get; set; }

    public Database(string nombre_bd)
    {
        NombreBD = nombre_bd;
        ListaTablas = new List<Table>();
        ListaUsuarios = new List<Usuario>();
        UsuarioCreador = CQL.UsuarioLogueado;
        ListaUserTypes = new List<UserType>();
        ListaUsuariosConPermisos = new List<Usuario>();
        ListaProcedimientos = new List<Procedimiento>();
    }

    #region FUNCIONES_DE_BD

    public bool ExisteUsuarioEnPermisos(string nombre_usuario)
    {
        return ListaUsuariosConPermisos.Any(x => x.NombreUsuario.Equals(nombre_usuario, StringComparison.InvariantCultureIgnoreCase));
    }

    public bool TieneUsuarioPermisosEnBD(string nombre_usuario)
    {
        return ListaUsuariosConPermisos.Any(x => x.NombreUsuario.Equals(nombre_usuario, StringComparison.InvariantCultureIgnoreCase)) || UsuarioCreador.Equals(nombre_usuario, StringComparison.InvariantCultureIgnoreCase) || nombre_usuario.Equals("admin", StringComparison.InvariantCultureIgnoreCase);
    }
    
    #endregion

    #region FUNCIONES_DE_USUARIO

    public Usuario RegistrarUsuario(string nombre_usuario, string password)
    {
        Usuario user = new Usuario(nombre_usuario, password);
        ListaUsuarios.Add(user);
        return user;
    }

    public void RegistrarUsuarioAPermisos(Usuario user)
    {
        ListaUsuariosConPermisos.Add(user);
    }

    public void EliminarUsuarioDePermisos(Usuario user)
    {
        ListaUsuariosConPermisos.Remove(user);
    }

    #endregion

    #region FUNCIONES_DE_TABLAS

    public bool ExisteTabla(string nombre_tabla)
    {
        return ListaTablas.Any(tab => tab.NombreTabla.Equals(nombre_tabla, StringComparison.InvariantCultureIgnoreCase));
    }

    public void RegistrarTabla(Table tabla)
    {
        ListaTablas.Add(tabla);
    }

    public Table ObtenerTabla(string nombre_tabla)
    {
        return ListaTablas.Find(x => x.NombreTabla.Equals(nombre_tabla, StringComparison.InvariantCultureIgnoreCase));
    }

    public void EliminarTabla(string nombre_tabla)
    {
        ListaTablas.RemoveAll(x => x.NombreTabla.Equals(nombre_tabla, StringComparison.InvariantCultureIgnoreCase));
    }

    public void TruncarTabla(string nombre_tabla)
    {
        ObtenerTabla(nombre_tabla).Tabla.Clear();
    }

    public void EliminarTodosLosRegistros(string nombre_tabla)
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
        return ListaUserTypes.Find(x => x.Identificador.Equals(nombre_user_type, StringComparison.InvariantCultureIgnoreCase));
    }

    public bool ExisteUserType(string nombre_user_type)
    {
        return ListaUserTypes.Any(ut => ut.Identificador.Equals(nombre_user_type, StringComparison.InvariantCultureIgnoreCase));
    }

    public void RegistrarUserType(UserType item)
    {
        ListaUserTypes.Add(item);
    }

    #endregion

    #region FUNCIONES_DE_PROCEDIMIENTOS

    public bool ExisteProcedimiento(string nombre_proc)
    {
        return ListaProcedimientos.Any(x => x.NombreProcedimiento.Equals(nombre_proc, StringComparison.InvariantCultureIgnoreCase));
    }

    public void RegistrarProcedimiento(Procedimiento proc)
    {
        ListaProcedimientos.Add(proc);
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