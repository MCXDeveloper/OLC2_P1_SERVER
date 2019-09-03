using OLC2_P1_SERVER.CQL.Arbol;
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
        do
        {
            Entorno local = new Entorno(ent);
            
            foreach(Instruccion ins in ListaInstrucciones)
            {
                ins.Ejecutar(local);
            }

        } while ((bool)Condicion.Ejecutar(ent));

        return new Nulo();
    }
}