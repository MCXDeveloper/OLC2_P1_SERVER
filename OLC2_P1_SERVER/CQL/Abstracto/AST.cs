using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class AST : Instruccion
{
    public static Entorno global;
    public List<Instruccion> Instrucciones { get; set; }

    public AST(List<Instruccion> instrucciones)
    {
        Instrucciones = instrucciones;
    }

    public object Ejecutar(Entorno ent)
    {
        global = ent;

        foreach (Instruccion ins in Instrucciones)
        {
            if(ins is Declaracion)
            {
                Declaracion d = (Declaracion)ins;
                d.Ejecutar(ent);
            }
        }
        
        return new Nulo();
    }
}