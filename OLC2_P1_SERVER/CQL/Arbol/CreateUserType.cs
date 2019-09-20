
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CreateUserType : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public string Identificador { get; set; }
    public bool ValidacionExiste { get; set; }
    public List<AtributoUT> ListaAtributos { get; set; }

    public CreateUserType(bool validacionExiste, string identificador, List<AtributoUT> lista_atributos, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        Identificador = identificador;
        ListaAtributos = lista_atributos;
        ValidacionExiste = validacionExiste;
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Procedo a verificar si existe alguna base de datos en uso, de lo contrario, se reporta el error.
        if (CQL.ExisteBaseDeDatosEnUso())
        {
            // 2. Procedo a validar que el UserType que desea crear no exista uno con el mismo nombre en la base de datos.
            if (!CQL.ExisteUserTypeEnBD(Identificador))
            {
                CQL.RegistrarUserTypeEnBD(new UserType(Identificador, ListaAtributos));
            }
            else
            {
                if (!ValidacionExiste)
                {
                    CQL.AddLUPError("Semántico", "[CREATE_USER_TYPE]", "Error.  Una UserType con el mismo nombre ya se encuentra en la base de datos.  (BD: " + CQL.BaseDatosEnUso + " | UserType: " + Identificador + ").", fila, columna);
                }
            }
        }
        else
        {
            CQL.AddLUPError("Semántico", "[CREATE_USER_TYPE]", "Error.  No se puede crear un UserType si no se ha especificado la base de datos a utilizar.", fila, columna);
        }

        return new Nulo();
    }
}