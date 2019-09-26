using OLC2_P1_SERVER.CHISON.Abstracto;
using OLC2_P1_SERVER.CHISON.Constantes;
using OLC2_P1_SERVER.CHISON.Manejadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OLC2_P1_SERVER.CHISON.Arbol
{
    public class CHI_Parametro : CHI_Instruccion
    {
        public string NombreParametro { get; set; }
        public string TipoParametro { get; set; }
        public CHIDataType TipoDatoParametro { get; set; }
        
        public CHI_Parametro(string nombre_parametro, CHIDataType tipo_dato_parametro, string tipo_parametro)
        {
            TipoParametro = tipo_parametro;
            NombreParametro = nombre_parametro;
            TipoDatoParametro = tipo_dato_parametro;
        }

        public object Ejecutar()
        {
            return TipoDatoParametro.ToString() + " " + NombreParametro;
        }
    }
}