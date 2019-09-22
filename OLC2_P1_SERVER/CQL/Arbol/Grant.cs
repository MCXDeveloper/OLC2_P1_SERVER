using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Grant : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public string NombreBD { get; set; }
    public string NombreUsuario { get; set; }

    public Grant(string nombre_usuario, string nombre_bd, int fila, int columna)
    {
        this.fila = fila;
        NombreBD = nombre_bd;
        this.columna = columna;
        NombreUsuario = nombre_usuario;
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Verifico que exista la base de datos a la que se le desea dar permiso a un usuario.
        if (CQL.ExisteBaseDeDatos(NombreBD))
        {
            // 2. Verifico que el usuario exista en el sistema.
            if (CQL.ExisteUsuarioEnSistema(NombreUsuario))
            {
                // 3. Verifico que el usuario no se encuentre actualmente en la lista de usuarios con permisos de la base de datos.
                if (!CQL.ExisteUsuarioEnPermisosBD(NombreBD, NombreUsuario) && !CQL.ObtenerUsuarioCreador(NombreBD).Equals(NombreUsuario, StringComparison.InvariantCultureIgnoreCase))
                {
                    CQL.RegistrarUsuarioEnPermisos(NombreBD, NombreUsuario);
                    CQL.AddLUPMessage("Se dieron correctamente los permisos al usuario '" + NombreUsuario + "', sobre la base de datos '" + NombreBD + "'.");
                }
                else
                {
                    CQL.AddLUPError("Semántico", "[GRANT]", "Error. El usuario '" + NombreUsuario + "' ya cuenta con permisos sobre la base de datos '"+ NombreBD +"'.", fila, columna);
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[GRANT]", "Error. No se puede dar permisos al usuario '" + NombreUsuario + "' ya que no existe en el sistema.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[GRANT]", "Error. No se puede dar permisos a un usuario sobre una base de datos inexistente.", fila, columna);
        }
        
        return new Nulo();
    }
}