using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class BatchException : Exception
{
    public static string Mensaje { get; set; }

    public BatchException(string message) : base(GetCompleteMessage(message)) { }

    private static string GetCompleteMessage(string msj)
    {
        return "Ocurrió una excepción del tipo 'BatchException'. " + msj;
    }
}