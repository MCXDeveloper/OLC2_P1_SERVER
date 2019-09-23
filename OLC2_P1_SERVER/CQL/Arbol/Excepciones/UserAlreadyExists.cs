using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class UserAlreadyExists : Exception
{
    public static string Mensaje { get; set; }

    public UserAlreadyExists(string message) : base(GetCompleteMessage(message)) { }

    private static string GetCompleteMessage(string msj)
    {
        return "Ocurrió una excepción del tipo 'UserAlreadyExists'. " + msj;
    }
}