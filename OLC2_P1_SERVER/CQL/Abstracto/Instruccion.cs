using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OLC2_P1_SERVER.CQL.Entorno;
using OLC2_P1_SERVER.CQL.Abstracto;

public interface Instruccion
{
    object Ejecutar(Entorno ent, AST arbol);
}
