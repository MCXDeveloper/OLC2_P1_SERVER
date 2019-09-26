using OLC2_P1_SERVER.CHISON.Abstracto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OLC2_P1_SERVER.CHISON.Arbol
{
    public class CHI_Val : CHI_Instruccion
    {
        public string Clave { get; set; }
        public object Valor { get; set; }

        public CHI_Val(string key, object val)
        {
            Clave = key;
            Valor = val;
        }

        public object Ejecutar()
        {
            if (Valor is List<object>)
            {
                return "[" + string.Join(", ", (List<object>)Valor) + "]";
            }
            else
            {
                return Valor;
            }
        }
    }
}