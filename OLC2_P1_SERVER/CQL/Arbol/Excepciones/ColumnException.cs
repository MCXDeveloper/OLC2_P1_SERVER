using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ColumnException : Exception
{
    public static string Mensaje { get; set; }

    public ColumnException(string message) : base(GetCompleteMessage(message)) { }

    private static string GetCompleteMessage(string msj)
    {
        return "Ocurrió una excepción del tipo 'ColumnException'. " + msj;
    }
}