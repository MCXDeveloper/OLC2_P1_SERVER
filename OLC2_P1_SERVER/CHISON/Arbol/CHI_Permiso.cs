using OLC2_P1_SERVER.CHISON.Abstracto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OLC2_P1_SERVER.CHISON.Arbol
{
    public class CHI_Permiso : CHI_Instruccion
    {
        public string NombreBD { get; set; }

        public CHI_Permiso(string nombre_bd)
        {
            NombreBD = nombre_bd;
        }

        public object Ejecutar()
        {
            return " ON " + NombreBD;
        }
    }
}