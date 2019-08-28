using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Error
{

    public static List<TError> ListaErrores;

    public Error()
    {
        ListaErrores = new List<TError>();
    }

    public static void AgregarError(string tipo, string ubicacion, string descripcion, int fila, int columna)
    {
        ListaErrores.Add(new TError(tipo, ubicacion, descripcion, fila, columna));
    }

}