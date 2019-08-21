using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OLC2_P1_SERVER.LUP.Abstracto;

namespace OLC2_P1_SERVER.LUP.Arbol
{
    public class LoginPackage : LUP_Instruccion
    {

        private readonly string user;
        private readonly string pass;

        public LoginPackage(string user, string pass)
        {
            this.user = user;
            this.pass = pass;
        }

        public object ejecutar()
        {
            throw new NotImplementedException();
        }

    }
}