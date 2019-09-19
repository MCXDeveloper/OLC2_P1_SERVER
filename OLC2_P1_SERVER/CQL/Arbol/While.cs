using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class While : Instruccion
{
    private readonly Expresion condicion;
    private readonly List<Instruccion> lista_instrucciones;

    public While(Expresion condicion, List<Instruccion> lista_instrucciones)
    {
        this.condicion = condicion;
        this.lista_instrucciones = lista_instrucciones;
    }

    public object Ejecutar(Entorno ent)
    {
        init_while:
        while ((bool)condicion.Ejecutar(ent))
        {
            Entorno local = new Entorno(ent);

            foreach (Instruccion ins in lista_instrucciones)
            {
                object result = ins.Ejecutar(local);

                if (result is Return)
                {
                    return ((Return)result).Ejecutar(local);
                }
                else if (result is Break)
                {
                    return result;
                }
                else if (result is Continue)
                {
                    goto init_while;
                }

            }

        }

        return new Nulo();
    }
}