using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class UserType
{
    private readonly string identificador;
    private readonly List<AtributoUT> lista_atributos;

    public UserType(string identificador, List<AtributoUT> lista_atributos)
    {
        this.identificador = identificador;
        this.lista_atributos = lista_atributos;
    }

    public List<AtributoUT> GetListaAtributos()
    {
        return lista_atributos;
    }
}