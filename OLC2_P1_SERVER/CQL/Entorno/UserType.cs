﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class UserType : InstruccionBD
{
    public string Identificador { get; set; }
    public List<AtributoUT> ListaAtributos { get; set; }
    
    public UserType(string identificador, List<AtributoUT> lista_atributos)
    {
        Identificador = identificador;
        ListaAtributos = lista_atributos;
    }

    public string CrearChison()
    {
        // TODO | UserType | Escribir función de CrearChison.
        throw new NotImplementedException();
    }

    public string CrearPaqueteLUP()
    {
        // TODO | UserType | Escribir función de CrearPaqueteLup.
        throw new NotImplementedException();
    }
}