using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class TableAlreadyExists : Exception
{
    public static string Mensaje { get; set; }

    public TableAlreadyExists(string message) : base(GetCompleteMessage(message)) { }

    private static string GetCompleteMessage(string msj)
    {
        return "Ocurrió una excepción del tipo 'TableAlreadyExists'. " + msj;
    }
}