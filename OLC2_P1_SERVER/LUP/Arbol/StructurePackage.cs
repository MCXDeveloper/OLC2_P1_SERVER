using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class StructurePackage : LUP_Instruccion
{
    public string Usuario { get; set; }

    public StructurePackage(string user)
    {
        Usuario = user;
    }

    public object Ejecutar()
    {
        return CQL.RootBD.CrearPaqueteLUP(Usuario);
    }
}