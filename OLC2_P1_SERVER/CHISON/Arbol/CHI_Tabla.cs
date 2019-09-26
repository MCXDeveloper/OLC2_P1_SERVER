using OLC2_P1_SERVER.CHISON.Abstracto;
using OLC2_P1_SERVER.CHISON.Estaticas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OLC2_P1_SERVER.CHISON.Arbol
{
    public class CHI_Tabla : CHI_Instruccion
    {
        public string NombreTabla { get; set; }
        public object ObjetoValores { get; set; } //List<CHI_Value> o Importar
        public object ObjetoColumnas { get; set; } //List<CHI_Columna> o Importar

        public CHI_Tabla(string nombre_tabla, object columnas, object valores)
        {
            ObjetoValores = valores;
            ObjetoColumnas = columnas;
            NombreTabla = nombre_tabla;
        }

        public object Ejecutar()
        {
            if (ObjetoColumnas is CHI_Importar)
            {
                CHI_Importar ci = (CHI_Importar)ObjetoColumnas;
                ci.Ejecutar();
            }
            else
            {
                StaticChison.CadenaSalida.Add("CREATE TABLE " + NombreTabla + " (");
                StaticChison.CadenaSalida.AddRange(ObtenerTextoColumnas((List<object>)ObjetoColumnas));
                StaticChison.CadenaSalida.Add(");" + Environment.NewLine);
            }

            if (ObjetoValores is CHI_Importar)
            {
                CHI_Importar ci = (CHI_Importar)ObjetoValores;
                ci.Ejecutar();
            }
            else
            {
                StaticChison.CadenaSalida.AddRange(ObtenerTextoValores((List<object>)ObjetoValores));
            }

            return null;
        }

        private List<string> ObtenerTextoColumnas(List<object> listCols)
        {
            List<string> listAuxCol = new List<string>();
            List<string> colPKS = new List<string>();

            foreach (object item in listCols)
            {
                CHI_Columna col = (CHI_Columna)item;

                if (col.IsPK)
                {
                    colPKS.Add(col.NombreColumna);
                }

                listAuxCol.Add("\t" + (string)col.Ejecutar() + (col.Equals(listCols.Last()) && colPKS.Count.Equals(0) ? "" : ","));
            }

            if (colPKS.Count > 0)
            {
                listAuxCol.Add("\t" + "PRIMARY KEY ( " + string.Join(", ", colPKS) + " )");
            }

            return listAuxCol;
        }

        private List<string> ObtenerTextoValores(List<object> listValores)
        {
            List<string> listAuxVal = new List<string>();

            foreach (object item in listValores)
            {
                CHI_Value ciValue = new CHI_Value((List<CHI_Val>)item);
                listAuxVal.Add("INSERT INTO " + NombreTabla + (string)ciValue.Ejecutar() + Environment.NewLine);
            }

            return listAuxVal;
        }
    }
}