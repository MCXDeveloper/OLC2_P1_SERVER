using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Date
{
    public string Fecha { get; set; }

    public Date(string fecha)
    {
        Fecha = fecha;
    }

    public DateTime GetParsedDate()
    {
        return DateTime.Parse(Fecha);
    }
}