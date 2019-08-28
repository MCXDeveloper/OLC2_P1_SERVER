using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Date
{
    private readonly string fecha;

    public Date(string fecha)
    {
        this.fecha = fecha;
    }

    public DateTime GetParsedDate()
    {
        return DateTime.Parse(fecha);
    }
}