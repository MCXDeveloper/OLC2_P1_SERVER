using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OLC2_P1_SERVER.CHISON.Constantes;

namespace OLC2_P1_SERVER.CHISON.Manejadores
{
    public class CHIDataType
    {
        public string OtroTipo { get; set; }
        public CHITipoDato Tipo { get; set; }
        
        public CHIDataType(CHITipoDato tipo, string otro_tipo)
        {
            Tipo = tipo;
            OtroTipo = otro_tipo;
        }

        public override string ToString()
        {
            if (Tipo.Equals(CHITipoDato.INT))
            {
                return "int";
            }
            else if (Tipo.Equals(CHITipoDato.DOUBLE))
            {
                return "double";
            }
            else if (Tipo.Equals(CHITipoDato.STRING))
            {
                return "string";
            }
            else if (Tipo.Equals(CHITipoDato.BOOLEAN))
            {
                return "boolean";
            }
            else if (Tipo.Equals(CHITipoDato.DATE))
            {
                return "date";
            }
            else if (Tipo.Equals(CHITipoDato.TIME))
            {
                return "time";
            }
            else if (Tipo.Equals(CHITipoDato.COUNTER))
            {
                return "counter";
            }
            else if (Tipo.Equals(CHITipoDato.CURSOR))
            {
                return "cursor";
            }
            else
            {
                return OtroTipo;
            }
        }
    }
}