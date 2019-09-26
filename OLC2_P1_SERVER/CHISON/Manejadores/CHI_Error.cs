using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OLC2_P1_SERVER.CHISON.Manejadores
{
    public class CHI_Error
    {
        public int Fila { get; set; }
        public int Columna { get; set; }
        public string TipoError { get; set; }
        public string UbicacionError { get; set; }
        public string DescripcionError { get; set; }

        public CHI_Error(string tipo, string ubicacion, string descripcion, int fila, int columna)
        {
            TipoError = tipo;
            UbicacionError = ubicacion;
            DescripcionError = descripcion;
            Fila = fila;
            Columna = columna;
        }
    }
}