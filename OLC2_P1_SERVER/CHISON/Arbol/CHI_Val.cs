using OLC2_P1_SERVER.CHISON.Abstracto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OLC2_P1_SERVER.CHISON.Arbol
{
    public class CHI_Val : CHI_Instruccion
    {
        public object Clave { get; set; }
        public object Valor { get; set; }

        public CHI_Val(object key, object val)
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
            else if (Valor is List<CHI_Val>)
            {
                string aux = "{ ";

                List<CHI_Val> listita = (List<CHI_Val>)Valor;

                foreach (CHI_Val v in listita)
                {
                    aux += v.Clave.ToString() + " : " + v.Ejecutar() + (v.Equals(listita.Last()) ? "" : ", ");
                }

                aux += " }";

                return aux;
            }
            else
            {
                return Valor.ToString();
            }
        }
    }
}