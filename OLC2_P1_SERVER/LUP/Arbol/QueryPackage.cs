using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OLC2_P1_SERVER.LUP.Abstracto;

namespace OLC2_P1_SERVER.LUP.Arbol
{
    public class QueryPackage : LUP_Instruccion
    {

        private readonly string user;
        private readonly string consulta;

        public QueryPackage(string user, string consulta)
        {
            this.user = user;
            this.consulta = consulta;
        }

        public object ejecutar()
        {
            throw new NotImplementedException();
        }
    }
}