using OLC2_P1_SERVER.CHISON.Abstracto;
using OLC2_P1_SERVER.CHISON.Constantes;
using OLC2_P1_SERVER.CHISON.Estaticas;
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
        public bool IsCounterFlag { get; set; }
        public string NombreClaveActual { get; set; }
        public string NombreObjetoActual { get; set; }
        public List<object> ListaColumnas { get; set; }

        public CHI_Val(object key, object val)
        {
            Clave = key;
            Valor = val;
        }

        public object Ejecutar()
        {
            if (ListaColumnas != null)
            {
                object ElementoColumna = (CHI_Columna)ListaColumnas.Find(x => ((CHI_Columna)x).NombreColumna.Equals(Clave.ToString().Replace("\"", "")));
                NombreClaveActual = ElementoColumna != null ? ((CHI_Columna)ElementoColumna).NombreColumna : NombreClaveActual;
                NombreObjetoActual = ElementoColumna != null ? ((CHI_Columna)ElementoColumna).TipoDatoColumna.OtroTipo: NombreObjetoActual;
                IsCounterFlag = ((CHI_Columna)ElementoColumna).TipoDatoColumna.Tipo.Equals(CHITipoDato.COUNTER);
            }

            if (!IsCounterFlag)
            {
                if (Valor is List<object>)
                {
                    return new string[] { NombreClaveActual, "[" + string.Join(", ", (List<object>)Valor) + "]" };
                }
                else if (Valor is List<CHI_Val>)
                {
                    bool IsMapFlag = NombreObjetoActual.StartsWith("Map<", StringComparison.InvariantCultureIgnoreCase) ? true : false;

                    string aux = "{ ";
                    List<CHI_Val> listita = (List<CHI_Val>)Valor;

                    if (!string.IsNullOrEmpty(NombreObjetoActual) && !IsMapFlag)
                    {
                        KeyValuePair<string, List<CHI_Atributo>> kvp = StaticChison.ObjetosRecopilados.FirstOrDefault(x => x.Key.Equals(NombreObjetoActual));
                        List<CHI_Atributo> target = kvp.Value;
                        listita = listita.OrderBy(x => target.IndexOf(target.Find(y => y.NombreAtributo.Equals(x.Clave.ToString().Replace("\"", ""))))).ToList();
                    }

                    foreach (CHI_Val v in listita)
                    {
                        if (IsMapFlag)
                        {
                            aux += v.Clave.ToString() + " : " + ((string[])v.Ejecutar())[1] + (v.Equals(listita.Last()) ? "" : ", ");
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(NombreObjetoActual))
                            {
                                KeyValuePair<string, List<CHI_Atributo>> kvp = StaticChison.ObjetosRecopilados.FirstOrDefault(x => x.Key.Equals(NombreObjetoActual));
                                CHI_Atributo catr = kvp.Value.Find(x => x.NombreAtributo.Equals(v.Clave.ToString().Replace("\"", ""), StringComparison.InvariantCultureIgnoreCase));
                                v.NombreObjetoActual = catr.TipoDatoAtributo.OtroTipo;
                                aux += ((string[])v.Ejecutar())[1] + (v.Equals(listita.Last()) ? "" : ", ");
                            }
                            else
                            {
                                v.NombreObjetoActual = string.Empty;
                                aux += ((string[])v.Ejecutar())[1] + (v.Equals(listita.Last()) ? "" : ", ");
                            }
                        }
                    }

                    aux += " } " + (IsMapFlag ? "" : "as " + NombreObjetoActual);

                    return new string[] { NombreClaveActual, aux };
                }
                else
                {
                    return new string[] { NombreClaveActual, Valor is null ? "null" : Valor.ToString() };
                }
            }

            return null;
        }
    }
}