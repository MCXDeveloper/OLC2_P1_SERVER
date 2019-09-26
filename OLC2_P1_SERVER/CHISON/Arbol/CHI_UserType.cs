using OLC2_P1_SERVER.CHISON.Abstracto;
using OLC2_P1_SERVER.CHISON.Estaticas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OLC2_P1_SERVER.CHISON.Arbol
{
    public class CHI_UserType : CHI_Instruccion
    {
        public string NombreUserType { get; set; }
        public object ObjetoAtributos { get; set; } // List<CHI_Atributo> o Importar

        public CHI_UserType(string nombre_type, object atributos)
        {
            NombreUserType = nombre_type;
            ObjetoAtributos = atributos;
        }

        public object Ejecutar()
        {
            if (ObjetoAtributos is CHI_Importar)
            {
                CHI_Importar ci = (CHI_Importar)ObjetoAtributos;
                ci.Ejecutar();
            }
            else
            {
                StaticChison.CadenaSalida.Add("CREATE TYPE " + NombreUserType + " ( ");
                StaticChison.CadenaSalida.AddRange(ObtenerTextoAtributos((List<CHI_Atributo>)ObjetoAtributos));
                StaticChison.CadenaSalida.Add(");" + Environment.NewLine);
            }

            return null;
        }

        private List<string> ObtenerTextoAtributos(List<CHI_Atributo> listAtrs)
        {
            List<string> resp = new List<string>();

            foreach (CHI_Atributo atr in listAtrs)
            {
                resp.Add("\t" + (string)atr.Ejecutar() + (atr.Equals(listAtrs.Last()) ? "" : ","));
            }

            return resp;
        }
    }
}