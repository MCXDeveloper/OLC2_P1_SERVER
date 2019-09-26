using OLC2_P1_SERVER.CHISON.Abstracto;
using OLC2_P1_SERVER.CHISON.Manejadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OLC2_P1_SERVER.CHISON.Arbol
{
    public class CHI_Columna : CHI_Instruccion
    {
        public bool IsPK { get; set; }
        public string NombreColumna { get; set; }
        public CHIDataType TipoDatoColumna { get; set; }

        public CHI_Columna(string nombre_columna, CHIDataType tipo_dato_columna, bool is_pk)
        {
            IsPK = is_pk;
            NombreColumna = nombre_columna;
            TipoDatoColumna = tipo_dato_columna;
        }

        public object Ejecutar()
        {
            return NombreColumna + " " + TipoDatoColumna.ToString();
        }
    }
}