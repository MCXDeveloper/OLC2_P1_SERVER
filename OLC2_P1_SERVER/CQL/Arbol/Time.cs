using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

public class Time
{
    private readonly string tiempo;

    public Time(string tiempo)
    {
        this.tiempo = tiempo;
    }

    public TimeSpan GetParsedTime()
    {
        return TimeSpan.ParseExact(tiempo, @"hh\:mm\:ss", CultureInfo.InvariantCulture);
    }
}