using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FunctionAlreadyExists : Exception
{
    public static string Mensaje { get; set; }

    public FunctionAlreadyExists(string message) : base(GetCompleteMessage(message)) { }

    private static string GetCompleteMessage(string msj)
    {
        return "Ocurrió una excepción del tipo 'FunctionAlreadyExists'. " + msj;
    }
}