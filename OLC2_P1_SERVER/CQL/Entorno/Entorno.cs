using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OLC2_P1_SERVER.FCL.Entorno
{
    public class Entorno
    {

        private Hashtable tabla;
        private Entorno anterior;

        public Entorno(Entorno anterior)
        {
            this.tabla = new Hashtable();
            this.anterior = anterior;
        }

        public void Agregar(String id, Simbolo simbolo)
        {

        }


    }
}