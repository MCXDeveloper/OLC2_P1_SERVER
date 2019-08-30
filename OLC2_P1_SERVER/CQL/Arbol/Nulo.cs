﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OLC2_P1_SERVER.CQL.Arbol
{
    public class Nulo : Instruccion, Expresion
    {
        public object Ejecutar(Entorno ent)
        {
            return new Nulo();
        }

        public Entorno.Tipo GetTipo(Entorno ent)
        {
            return Entorno.Tipo.NULO;
        }
    }
}