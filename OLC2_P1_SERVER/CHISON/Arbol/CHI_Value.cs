using OLC2_P1_SERVER.CHISON.Abstracto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OLC2_P1_SERVER.CHISON.Arbol
{
    public class CHI_Value : CHI_Instruccion
    {
        public List<CHI_Val> ListaValores { get; set; }

        public CHI_Value(List<CHI_Val> lista_valores)
        {
            ListaValores = lista_valores;
        }

        public object Ejecutar()
        {
            List<string> lista_campos = new List<string>();
            List<string> lista_valores = new List<string>();

            foreach (CHI_Val val in ListaValores)
            {
                lista_campos.Add(val.Clave.ToString().Replace("\"", ""));
                lista_valores.Add(val.Ejecutar().ToString());
            }

            return " ( " + string.Join(", ", lista_campos) + " ) VALUES ( " + string.Join(", ", lista_valores) + " ); ";
        }
    }
}