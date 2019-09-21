using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Revoke : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public string NombreBD { get; set; }
    public string NombreUsuario { get; set; }

    public Revoke(string nombre_usuario, string nombre_bd, int fila, int columna)
    {
        this.fila = fila;
        NombreBD = nombre_bd;
        this.columna = columna;
        NombreUsuario = nombre_usuario;
    }

    public object Ejecutar(Entorno ent)
    {
        throw new NotImplementedException();
    }
}