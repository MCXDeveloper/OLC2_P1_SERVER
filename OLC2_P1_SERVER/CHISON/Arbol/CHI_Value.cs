using OLC2_P1_SERVER.CHISON.Abstracto;
using OLC2_P1_SERVER.CHISON.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OLC2_P1_SERVER.CHISON.Arbol
{
    public class CHI_Value : CHI_Instruccion
    {
        public List<CHI_Val> ListaValores { get; set; }
        public List<object> ListaColumnas { get; set; }

        public CHI_Value(List<CHI_Val> lista_valores)
        {
            ListaValores = lista_valores;
        }

        public object Ejecutar()
        {
            List<string> lista_campos = new List<string>();
            List<string> lista_valores = new List<string>();

            List<CHI_Columna> target = ListaColumnas.ConvertAll(x => (CHI_Columna)x);
            ListaValores = ListaValores.OrderBy(x => target.IndexOf(target.Find(y => y.NombreColumna.Equals(x.Clave.ToString().Replace("\"", ""))))).ToList();

            foreach (CHI_Val val in ListaValores)
            {
                val.ListaColumnas = ListaColumnas;

                // valResponse debería de retornar un string[] con dos posiciones:
                // En la posición [0]: nombre del campo
                // En la posición [1]: valor
                // Si el campo fuese un COUNTER, devuelve un null.

                object valResponse = val.Ejecutar();

                if (valResponse != null)
                {
                    string[] valValues = (string[])valResponse;
                    lista_campos.Add(valValues[0]);
                    lista_valores.Add(valValues[1]);
                }
            }

            return " ( " + string.Join(", ", lista_campos) + " ) VALUES ( " + string.Join(", ", lista_valores) + " ); ";
        }
    }
}