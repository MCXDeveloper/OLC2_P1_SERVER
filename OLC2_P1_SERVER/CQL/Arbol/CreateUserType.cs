using OLC2_P1_SERVER.CQL.Arbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CreateUserType : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    private readonly string identificador;
    private readonly bool validacionExiste;
    private readonly List<AtributoUT> lista_atributos;
    
    public CreateUserType(bool validacionExiste, string identificador, List<AtributoUT> lista_atributos, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        this.identificador = identificador;
        this.lista_atributos = lista_atributos;
        this.validacionExiste = validacionExiste;
    }

    public object Ejecutar(Entorno ent)
    {
        // Se realiza una búsqueda para verificar si un UserType con el mismo nombre ya existe.
        object simbolo = ent.ObtenerUserType(identificador);

        // Si el UserType si existe...
        if(!(simbolo is Nulo))
        {
            // Unicamente se arrojará un error si la condición 'IF NOT EXISTS' no fue declarada.  Eso se verifica por medio de la bandera 'validacionExiste'.
            if(!validacionExiste)
            {
                Error.AgregarError("Semántico", "[CREATE_USER_TYPE]", "No se puede crear el UserType con el identificador: " + identificador + ".  Ya existe uno con el mismo nombre.", fila, columna);
            }
        }
        // De lo contrario, se procede a registrar el UserType en el entorno.
        else
        {
            ent.Agregar(identificador, new UserType(identificador, lista_atributos));
        }

        return new Nulo();
    }
}