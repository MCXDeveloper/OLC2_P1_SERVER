using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

public class Commit : Instruccion
{
    private readonly int fila;
    private readonly int columna;

    public Commit(int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Genero toda la base de datos en memoria en formato CHISON.
        string DBMSChison = CQL.RootBD.CrearChison(0);

        // 2. Verifico si la cadena de CHISON devuelta tiene contenido.
        if (!string.IsNullOrEmpty(DBMSChison))
        {
            // 3. Escribo el archivo 'Principal.chison' con el contenido devuelto por la memoria.
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/ChisonFilesContainer/Principal.chison");
            File.WriteAllText(path, DBMSChison);

            CQL.AddLUPMessage(" *** Commit realizado exitosamente. *** ");
        }
        else
        {
            CQL.AddLUPMessage(" *** No se genero archivo de CHISON ya que no hay información en memoria. *** ");
        }

        return new Nulo();
    }
}