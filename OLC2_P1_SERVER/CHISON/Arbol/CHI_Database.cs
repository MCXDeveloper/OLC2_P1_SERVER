using OLC2_P1_SERVER.CHISON.Abstracto;
using OLC2_P1_SERVER.CHISON.Estaticas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OLC2_P1_SERVER.CHISON.Arbol
{
    public class CHI_Database : CHI_Instruccion
    {
        public string NombreBD { get; set; }
        public object ObjetoElementos { get; set; } // List<CHI_Instruccion> o Importar

        public CHI_Database(string nombre_bd, object elementos)
        {
            NombreBD = nombre_bd;
            ObjetoElementos = elementos;
        }

        public object Ejecutar()
        {
            if (ObjetoElementos is CHI_Importar)
            {
                CHI_Importar ci = (CHI_Importar)ObjetoElementos;
                ci.Ejecutar();
            }
            else
            {
                StaticChison.CadenaSalida.Add("CREATE DATABASE "+ NombreBD + ";" + Environment.NewLine);
                StaticChison.CadenaSalida.Add("USE " + NombreBD + ";" + Environment.NewLine);

                List<CHI_Instruccion> listaObjs = (List<CHI_Instruccion>)ObjetoElementos;
                
                foreach (CHI_Instruccion obj in listaObjs)
                {
                    obj.Ejecutar();
                }
            }

            return null;
        }
    }
}