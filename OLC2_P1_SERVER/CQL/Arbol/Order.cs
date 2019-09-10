using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Order
{
    public string TipoDeOrden { get; set; }
    public string NombreColumnaOrden { get; set; }
    
    public Order(string nombre_columna_orden, string tipo_orden)
    {
        TipoDeOrden = tipo_orden;
        NombreColumnaOrden = nombre_columna_orden;
    }
}