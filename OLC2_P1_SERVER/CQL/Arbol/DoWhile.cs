
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class DoWhile : Instruccion
{
    public Expresion Condicion { get; set; }
    public List<Instruccion> ListaInstrucciones { get; set; }

    public DoWhile(Expresion condicion, List<Instruccion> lista_instrucciones)
    {
        Condicion = condicion;
        ListaInstrucciones = lista_instrucciones;
    }

    public object Ejecutar(Entorno ent)
    {
        init_while:
        while ((bool)Condicion.Ejecutar(ent))
        {
            Entorno local = new Entorno(ent);

            foreach (Instruccion ins in ListaInstrucciones)
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