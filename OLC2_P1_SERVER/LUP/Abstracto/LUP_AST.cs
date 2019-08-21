using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OLC2_P1_SERVER.LUP.Abstracto
{
    public class LUP_AST
    {

        private readonly LinkedList<LUP_Instruccion> instrucciones;

        public LUP_AST(LinkedList<LUP_Instruccion> instrucciones)
        {
            this.instrucciones = instrucciones;
        }

    }
}