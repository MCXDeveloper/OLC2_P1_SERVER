using OLC2_P1_SERVER.CHISON.Abstracto;
using OLC2_P1_SERVER.CHISON.Estaticas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OLC2_P1_SERVER.CHISON.Arbol
{
    public class CHI_Usuario : CHI_Instruccion
    {
        public string User { get; set; }
        public string Pass { get; set; }
        public object ObjetoPermisos { get; set; } // List<CHI_Permiso> o Importar 

        public CHI_Usuario(string user, string pass, object permisos)
        {
            User = user;
            Pass = pass;
            ObjetoPermisos = permisos;
        }

        public object Ejecutar()
        {
            StaticChison.CadenaSalida.Add("CREATE USER " + User + " WITH PASSWORD " + Pass + "; ");

            if (ObjetoPermisos is CHI_Importar)
            {
                CHI_Importar ci = (CHI_Importar)ObjetoPermisos;
                ci.Ejecutar();
            }
            else
            {
                StaticChison.CadenaSalida.AddRange(ObtenerTextoPermisos((List<CHI_Permiso>)ObjetoPermisos));
            }

            return null;
        }

        private List<string> ObtenerTextoPermisos(List<CHI_Permiso> listPerm)
        {
            List<string> resp = new List<string>();

            foreach (CHI_Permiso p in listPerm)
            {
                resp.Add("GRANT " + User + (string)p.Ejecutar() + ";");
            }

            return resp;
        }

    }
}