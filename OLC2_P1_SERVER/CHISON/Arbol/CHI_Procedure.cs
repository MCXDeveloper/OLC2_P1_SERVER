using OLC2_P1_SERVER.CHISON.Abstracto;
using OLC2_P1_SERVER.CHISON.Estaticas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OLC2_P1_SERVER.CHISON.Arbol
{
    public class CHI_Procedure : CHI_Instruccion
    {
        public string NombreProc { get; set; }
        public object ObjetoParametros { get; set; } // List<CHI_Parametro> o Importar
        public string InstruccionesProc { get; set; }
        
        public CHI_Procedure(string nombre_proc, object parametros, string instrucciones_proc)
        {
            NombreProc = nombre_proc;
            ObjetoParametros = parametros;
            InstruccionesProc = instrucciones_proc;
        }

        public object Ejecutar()
        {
            if (ObjetoParametros is CHI_Importar)
            {
                CHI_Importar ci = (CHI_Importar)ObjetoParametros;
                ci.Ejecutar();
            }
            else
            {
                string procsito = "PROCEDURE " + NombreProc;
                List<string> inputParam = new List<string>();
                List<string> outputParam = new List<string>();
                
                foreach (CHI_Parametro param in (List<CHI_Parametro>)ObjetoParametros)
                {
                    if (param.TipoParametro.Equals("in", StringComparison.InvariantCultureIgnoreCase))
                    {
                        inputParam.Add((string)param.Ejecutar());
                    }
                    else
                    {
                        outputParam.Add((string)param.Ejecutar());
                    }
                }

                procsito += " ( " + string.Join(", ", inputParam) + " ), ( " + string.Join(", ", outputParam) + " ) {" + Environment.NewLine;
                procsito += InstruccionesProc + Environment.NewLine;
                procsito += "}" + Environment.NewLine;

                StaticChison.CadenaSalida.Add(procsito);
            }

            return null;
        }
    }
}