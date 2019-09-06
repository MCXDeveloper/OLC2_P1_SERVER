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
    
    public string CrearChison()
    {
        throw new NotImplementedException();
    }

    public string CrearPaqueteLUP()
    {
        throw new NotImplementedException();
    }
}