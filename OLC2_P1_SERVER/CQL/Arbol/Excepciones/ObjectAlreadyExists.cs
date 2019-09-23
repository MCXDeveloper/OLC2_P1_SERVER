using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ObjectAlreadyExists : Exception
{
    public static string Mensaje { get; set; }

    public ObjectAlreadyExists(string message) : base(GetCompleteMessage(message)) { }

    private static string GetCompleteMessage(string msj)
    {
        return "Ocurrió una excepción del tipo 'ObjectAlreadyExists'. " + msj;
    }
}