using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

public class Time
{
    public string Tiempo { get; set; }

    public Time(string tiempo)
    {
        Tiempo = tiempo;
    }

    public TimeSpan GetParsedTime()
    {
        return TimeSpan.ParseExact(Tiempo, @"hh\:mm\:ss", CultureInfo.InvariantCulture);
    }
}