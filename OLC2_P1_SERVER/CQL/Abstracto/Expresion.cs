using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface Expresion
{
    TipoDato GetTipo(Entorno ent);
    object Ejecutar(Entorno ent);
}
