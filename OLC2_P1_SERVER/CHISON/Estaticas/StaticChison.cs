using OLC2_P1_SERVER.CHISON.Arbol;
using OLC2_P1_SERVER.CHISON.Manejadores;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace OLC2_P1_SERVER.CHISON.Estaticas
{
    public class StaticChison
    {
        public static bool HasImports { get; set; }
        public static string CadenaEntrada { get; set; }
        public static List<string> CadenaSalida { get; set; }
        public static List<CHI_Error> PilaErrores { get; set; }
        public static string[] CadenaEntradaSeparada { get; set; }
        public static List<KeyValuePair<string, List<CHI_Atributo>>> ObjetosRecopilados { get; set; }

        public static void InitializeStaticEnvironment()
        {
            HasImports = false;
            CadenaEntrada = string.Empty;
            CadenaSalida = new List<string>();
            PilaErrores = new List<CHI_Error>();
            ObjetosRecopilados = new List<KeyValuePair<string, List<CHI_Atributo>>>();
        }

        public static void AddError(string tipo, string ubicacion, string descripcion, int fila, int columna)
        {
            PilaErrores.Add(new CHI_Error(tipo, ubicacion, descripcion, fila, columna));
        }

        public static void EstablecerYSepararCadenaEntrada(string entrada)
        {
            CadenaEntrada = entrada;
            string[] separadores = { "${", "}$" };
            CadenaEntradaSeparada = CadenaEntrada.Split(separadores, StringSplitOptions.None).Select(x => x.Trim()).ToArray();
        }
    }
}