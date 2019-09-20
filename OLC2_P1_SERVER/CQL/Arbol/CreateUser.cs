using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CreateUser : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public string Password { get; set; }
    public string NombreUsuario { get; set; }
    
    public CreateUser(string nombre_usuario, string password, int fila, int columna)
    {
        this.fila = fila;
        Password = password;
        this.columna = columna;
        NombreUsuario = nombre_usuario;
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Verifico que exista una base de datos en uso.
        if (CQL.ExisteBaseDeDatosEnUso())
        {
            // 2. Verifico que no exista actualmente el usuario que se desea crear.
            if (!CQL.ExisteUsuarioEnSistema(NombreUsuario))
            {
                // 3. Registro el usuario tanto en la lista de usuarios de la base de datos asi como también en la lista estática de usuarios disponibles.
                // La siguiente función realiza ambas cosas.
                CQL.RegistrarUsuarioEnBD(NombreUsuario, Password);
            }
            else
            {
                CQL.AddLUPError("Semántico", "[CREATE_USER]", "Error. No se puede crear el usuario '"+ NombreUsuario +"'.  Existe uno con el mismo nombre actualmente en la base de datos.", fila, columna);
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[CREATE_USER]", "Error. No se puede crear un usuario si no existe una base de datos en uso.", fila, columna);
        }

        return new Nulo();
    }
}