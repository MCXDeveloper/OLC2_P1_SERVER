using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;

public class CQL
{
    public static RaizBD RootBD { get; set; }
    public static bool WhereFlag { get; set; }
    public static bool BatchFlag { get; set; }
    public static bool SelectFlag { get; set; }
    public static Table TablaEnUso { get; set; }
    public static bool TryCatchFlag { get; set; }
    public static DataRow TuplaEnUso { get; set; }
    public static string BaseDatosEnUso { get; set; }
    public static int BatchErrorCounter { get; set; }
    public static string UsuarioLogueado { get; set; }
    public static List<string> PilaRespuestas = new List<string>();
    public static List<Usuario> ListaUsuariosDisponibles { get; set; }

    public static void testing()
    {
        Debug.WriteLine(string.Join(Environment.NewLine, PilaRespuestas));
    }

    #region FUNCIONES_DE_LOGIN

    public static bool ExisteUsuarioLogueado()
    {
        return !string.IsNullOrEmpty(UsuarioLogueado);
    }

    public static bool ValidarLogin(string user, string pass)
    {
        return ListaUsuariosDisponibles.Any(x => (x.NombreUsuario.Equals(user) && x.PasswordUsuario.Equals(pass)));
    }

    #endregion

    #region FUNCIONES_DE_BASE_DE_DATOS

    public static bool ExisteBaseDeDatos(string nombre_bd)
    {
        return RootBD.ExistsDatabase(nombre_bd);
    }

    public static void RegistrarBaseDeDatos(string nombre_bd)
    {
        RootBD.InsertDatabase(nombre_bd, new Database(nombre_bd));
    }

    public static void EliminarBaseDeDatos(string nombre_bd)
    {
        RootBD.DeleteDatabase(nombre_bd);
    }

    public static bool ExisteBaseDeDatosEnUso()
    {
        return !string.IsNullOrEmpty(BaseDatosEnUso);
    }

    public static bool TienePermisosSobreBaseDeDatos(string nombre_usuario, string nombre_bd)
    {
        return RootBD.GetDatabase(nombre_bd).TieneUsuarioPermisosEnBD(nombre_usuario);
    }

    public static string ObtenerUsuarioCreador(string nombre_bd)
    {
        return RootBD.GetDatabase(nombre_bd).UsuarioCreador;
    }

    #endregion

    #region FUNCIONES_DE_USUARIO

    public static bool ExisteUsuarioEnPermisosBD(string nombre_bd, string nombre_usuario)
    {
        return RootBD.GetDatabase(nombre_bd).ExisteUsuarioEnPermisos(nombre_usuario);
    }

    public static bool ExisteUsuarioEnSistema(string nombre_usuario)
    {
        return ListaUsuariosDisponibles.Any(x => x.NombreUsuario.Equals(nombre_usuario, StringComparison.InvariantCultureIgnoreCase));
    }

    public static void RegistrarUsuarioEnBD(string nombre_usuario, string password)
    {
        Usuario user = RootBD.GetDatabase(BaseDatosEnUso).RegistrarUsuario(nombre_usuario, password);
        ListaUsuariosDisponibles.Add(user);
    }

    public static void RegistrarUsuarioEnPermisos(string nombre_bd, string nombre_usuario)
    {
        Usuario user = ListaUsuariosDisponibles.Find(x => x.NombreUsuario.Equals(nombre_usuario, StringComparison.InvariantCultureIgnoreCase));
        RootBD.GetDatabase(nombre_bd).RegistrarUsuarioAPermisos(user);
    }

    public static void EliminarUsuarioEnPermisos(string nombre_bd, string nombre_usuario)
    {
        Usuario user = ListaUsuariosDisponibles.Find(x => x.NombreUsuario.Equals(nombre_usuario, StringComparison.InvariantCultureIgnoreCase));
        RootBD.GetDatabase(nombre_bd).EliminarUsuarioDePermisos(user);
    }

    #endregion

    #region FUNCIONES_DE_TABLAS

    public static bool ExisteTablaEnBD(string nombre_tabla)
    {
        return RootBD.GetDatabase(BaseDatosEnUso).ExisteTabla(nombre_tabla);
    }

    public static Table ObtenerTabla(string nombre_tabla)
    {
        return RootBD.GetDatabase(BaseDatosEnUso).ObtenerTabla(nombre_tabla);
    }

    public static void RegistrarTabla(Table tabla)
    {
        RootBD.GetDatabase(BaseDatosEnUso).RegistrarTabla(tabla);
    }

    public static void TruncarTabla(string nombre_tabla)
    {
        RootBD.GetDatabase(BaseDatosEnUso).TruncarTabla(nombre_tabla);
    }

    public static void DropearTabla(string nombre_tabla)
    {
        RootBD.GetDatabase(BaseDatosEnUso).EliminarTabla(nombre_tabla);
    }

    public static bool VerificarSiColumnaEsPK(string nombre_tabla, string nombre_columna)
    {
        return RootBD.GetDatabase(BaseDatosEnUso).ObtenerTabla(nombre_tabla).IsPrimaryKeyColumn(nombre_columna);
    }

    public static void AgregarColumnaEnTabla(string nombre_tabla, Columna col)
    {
        RootBD.GetDatabase(BaseDatosEnUso).ObtenerTabla(nombre_tabla).AddColumn(col);
    }

    public static void EliminarColumnaDeTabla(string nombre_tabla, string nombre_columna)
    {
        RootBD.GetDatabase(BaseDatosEnUso).ObtenerTabla(nombre_tabla).DeleteColumn(nombre_columna);
    }

    public static bool ExisteColumnaEnTabla(string nombre_tabla, string nombre_columna)
    {
        return RootBD.GetDatabase(BaseDatosEnUso).ObtenerTabla(nombre_tabla).ExistsColumn(nombre_columna);
    }

    public static bool ValidarTipoDatoColumna(string nombre_tabla, string nombre_columna, TipoDato.Tipo[] tipos_a_validar)
    {
        return tipos_a_validar.Contains(RootBD.GetDatabase(BaseDatosEnUso).ObtenerTabla(nombre_tabla).GetColumn(nombre_columna).TipoDatoColumna.GetRealTipo());
    }

    public static Columna ObtenerColumnaDeTabla(string nombre_columna)
    {
        return CQL.TablaEnUso.GetColumn(nombre_columna);
    }

    public static void EliminarTodosLosRegistrosDeTabla(string nombre_tabla)
    {
        RootBD.GetDatabase(BaseDatosEnUso).EliminarTodosLosRegistros(nombre_tabla);
    }

    #endregion

    #region FUNCIONES_DE_USERTYPES

    public static string ObtenerListaUserTypesEnString()
    {
        return RootBD.GetDatabase(BaseDatosEnUso).UserTypeListToString();
    }

    public static object ObtenerUserType(string nombre_user_type)
    {
        return RootBD.GetDatabase(BaseDatosEnUso).ObtenerUserType(nombre_user_type);
    }

    public static bool ExisteUserTypeEnBD(string nombre_user_type)
    {
        return RootBD.GetDatabase(BaseDatosEnUso).ExisteUserType(nombre_user_type);
    }

    public static void RegistrarUserTypeEnBD(UserType item)
    {
        RootBD.GetDatabase(BaseDatosEnUso).RegistrarUserType(item);
    }

    #endregion

    #region FUNCIONES_DE_PROCEDIMIENTOS

    public static bool ExisteProcedimientoEnBD(string nombre_proc)
    {
        return RootBD.GetDatabase(BaseDatosEnUso).ExisteProcedimiento(nombre_proc);
    }

    public static void RegistrarProcedimientoEnBD(Procedimiento proc)
    {
        RootBD.GetDatabase(BaseDatosEnUso).RegistrarProcedimiento(proc);
    }

    public static Procedimiento ObtenerProcedimientoDeBD(string nombre_proc)
    {
        return RootBD.GetDatabase(BaseDatosEnUso).ObtenerProcedimiento(nombre_proc);
    }

    #endregion

    #region FUNCIONES_DE_LUP

    public static void AddLUPMessage(string message)
    {
        PilaRespuestas.Add("[+MESSAGE]>> " + message + "[-MESSAGE]" + Environment.NewLine);
    }

    public static void AddLUPData(string content)
    {
        PilaRespuestas.Add("[+DATA]" + content + "[-DATA]" + Environment.NewLine);
    }

    public static void AddLUPError(string type, string location, string description, int line, int column)
    {
        PilaRespuestas.Add("[+ERROR][+LINE]" + line + "[-LINE][+COLUMN]" + column + "[-COLUMN][+TYPE]" + type + "[-TYPE][+LOCATION]" + location + "[-LOCATION][+DESC]" + description + "[-DESC][-ERROR]" + Environment.NewLine);
    }

    #endregion

    #region FUNCIONES_ADICIONALES

    public static string GetCompleteResponse()
    {
        return String.Join(String.Empty, PilaRespuestas.ToArray());
    }

    public static bool CompararTiposDeObjeto(TipoDato t1, TipoDato t2)
    {
        return ((string)t1.GetElemento()).Equals((string)t2.GetElemento(), StringComparison.InvariantCultureIgnoreCase);
    }

    // Esta función se encarga de iniciar todos los elementos estáticos y limpiar la pila de respuestas.
    public static void AccionesIniciales()
    {
        RootBD = new RaizBD();
        PilaRespuestas.Clear();
        TablaEnUso = null;
        TuplaEnUso = null;
        BaseDatosEnUso = string.Empty;
        UsuarioLogueado = string.Empty;
        ListaUsuariosDisponibles = new List<Usuario>();
    }

    public static string TransformEntornoToTable(Entorno ent)
    {
        DataTable dt = new DataTable();
        dt.Clear();

        dt.Columns.Add("Tipo");
        dt.Columns.Add("Nombre");
        dt.Columns.Add("Valor");

        // 1. Convierto el entorno a un DataTable.
        for (Entorno e = ent; e != null; e = e.Anterior)
        {
            foreach (DictionaryEntry de in e.TablaVariables)
            {
                Variable v = (Variable)de.Value;
                DataRow rowsito = dt.NewRow();
                rowsito["Tipo"] = v.Tipo.GetRealTipo().ToString();
                rowsito["Nombre"] = v.Nombre;

                if (v.Valor is Date)
                {
                    rowsito["Valor"] = ((Date)v.Valor).Fecha;
                }
                else if (v.Valor is Time)
                {
                    rowsito["Valor"] = ((Time)v.Valor).Tiempo;
                }
                else
                {
                    rowsito["Valor"] = v.Valor.ToString();
                }

                dt.Rows.Add(rowsito);
            }
        }

        return AsciiTableGenerator.CreateAsciiTableFromDataTable(dt).ToString();
    }

    public static string GenerateName(int len)
    {
        Random r = new Random();
        string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
        string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
        string Name = "";
        Name += consonants[r.Next(consonants.Length)].ToUpper();
        Name += vowels[r.Next(vowels.Length)];
        int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
        while (b < len)
        {
            Name += consonants[r.Next(consonants.Length)];
            b++;
            Name += vowels[r.Next(vowels.Length)];
            b++;
        }

        return Name;
    }

    #endregion
}