using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OLC2_P1_SERVER.LUP.Abstracto;

namespace OLC2_P1_SERVER.LUP.Arbol
{
    public class LogoutPackage : LUP_Instruccion
    {

        private readonly string user;

        public LogoutPackage(string user)
        {
            this.user = user;
        }

        public object ejecutar()
        {
            throw new NotImplementedException();
        }

    }
}