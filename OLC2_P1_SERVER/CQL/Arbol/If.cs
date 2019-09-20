
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class If : Instruccion
{
    private List<Instruccion> SubIfs;

    public If(Instruccion a)
    {
        this.SubIfs = new List<Instruccion>();
        this.SubIfs.Add(a);
    }

    public If(Instruccion a, List<Instruccion> b)
    {
        this.SubIfs = new List<Instruccion>();
        this.SubIfs.Add(a);
        this.SubIfs.AddRange(b);
    }

    public If(Instruccion a, List<Instruccion> b, Instruccion c)
    {
        this.SubIfs = new List<Instruccion>();
        this.SubIfs.Add(a);
        this.SubIfs.AddRange(b);
        this.SubIfs.Add(c);
    }

    public If(Instruccion a, Instruccion b)
    {
        this.SubIfs = new List<Instruccion>();
        this.SubIfs.Add(a);
        this.SubIfs.Add(b);
    }

    public object Ejecutar(Entorno ent)
    {
        foreach(Instruccion ins in SubIfs)
        {
            object response = ins.Ejecutar(ent);
            
            if(response is Return || response is Break || response is Continue)
            {
                return response;
            }
            else if (response is bool)
            {
                if((bool)response)
                {
                    break;
                }
            }
        }

        return new Nulo();
    }
}