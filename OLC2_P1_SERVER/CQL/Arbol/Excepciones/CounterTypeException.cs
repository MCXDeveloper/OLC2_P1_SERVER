using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CounterTypeException : Exception
{
    public static string Mensaje { get; set; }

    public CounterTypeException(string message) : base(GetCompleteMessage(message)) { }

    private static string GetCompleteMessage(string msj)
    {
        return "Ocurrió una excepción del tipo 'CounterTypeException'. " + msj;
    }
}