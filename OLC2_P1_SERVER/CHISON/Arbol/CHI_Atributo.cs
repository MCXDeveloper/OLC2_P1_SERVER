using OLC2_P1_SERVER.CHISON.Abstracto;
using OLC2_P1_SERVER.CHISON.Manejadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OLC2_P1_SERVER.CHISON.Arbol
{
    public class CHI_Atributo : CHI_Instruccion
    {
        public string NombreAtributo { get; set; }
        public CHIDataType TipoDatoAtributo { get; set; }

        public CHI_Atributo(string nombre_atributo, CHIDataType tipo_dato_atributo)
        {
            NombreAtributo = nombre_atributo;
            TipoDatoAtributo = tipo_dato_atributo;
        }

        public object Ejecutar()
        {
            return NombreAtributo + " " + TipoDatoAtributo.ToString();
        }
    }
}