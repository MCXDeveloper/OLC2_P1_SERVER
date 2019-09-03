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

    public int GetDay()
    {
        return GetParsedDate().Day;
    }

    public int GetMonth()
    {
        return GetParsedDate().Month;
    }

    public int GetYear()
    {
        return GetParsedDate().Year;
    }
}