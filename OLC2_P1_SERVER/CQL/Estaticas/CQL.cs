using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class CQL
{
    public static RaizBD RootBD { get; set; }
    public static bool WhereFlag { get; set; }
    public static Table TablaEnUso { get; set; }
    public static DataRow TuplaEnUso { get; set; }
    public static string BaseDatosEnUso { get; set; }
    public static string UsuarioLogueado { get; set; }
    
    public static bool ExisteBaseDeDatosEnUso()
    {
        return !BaseDatosEnUso.Equals(String.Empty);
    }

    public static bool ExisteTablaEnBD(string nombre_tabla)
    {
        return RootBD.GetDatabase(BaseDatosEnUso).ExisteTabla(nombre_tabla);
    }

    public static Table ObtenerTabla(string nombre_tabla)
    {
        return RootBD.GetDatabase(BaseDatosEnUso).ObtenerTabla(nombre_tabla);
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
}