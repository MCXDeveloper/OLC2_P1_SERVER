
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Return : Instruccion
{
    public Expresion Valor { get; set; }
    
    public Return()
    {
        Valor = new Nulo();
    }

    public Return(Expresion valor)
    {
        Valor = valor;
    }

    public object Ejecutar(Entorno ent)
    {
        return (Valor is Nulo) ? Valor : Valor.Ejecutar(ent);
    }
}