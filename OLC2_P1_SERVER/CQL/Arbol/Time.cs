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

    public DateTime GetTimeInDateTime()
    {
        return DateTime.Parse(Tiempo);
    }

    public int GetHours()
    {
        return GetParsedTime().Hours;
    }

    public int GetMinutes()
    {
        return GetParsedTime().Minutes;
    }

    public int GetSeconds()
    {
        return GetParsedTime().Seconds;
    }
}