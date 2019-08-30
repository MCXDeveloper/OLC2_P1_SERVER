using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class AST : Instruccion
{
    private List<Instruccion> instrucciones;

    public AST(List<Instruccion> instrucciones)
    {
        this.instrucciones = instrucciones;
    }

    public object Ejecutar(Entorno ent)
    {
        /*foreach (Instruccion ins in instrucciones)
        {
            if(ins is Declaracion)
            {

            }
        }*/

        // TODO Ejecutar todos los elementos del arbol

        Console.WriteLine("Aquí debería de ejecutar los elementos del árbol");

        return new Nulo();
    }
}