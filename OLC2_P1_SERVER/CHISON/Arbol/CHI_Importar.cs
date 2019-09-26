using OLC2_P1_SERVER.CHISON.Abstracto;
using OLC2_P1_SERVER.CHISON.Estaticas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace OLC2_P1_SERVER.CHISON.Arbol
{
    public class CHI_Importar : CHI_Instruccion
    {
        private readonly int fila;
        private readonly int columna;
        public int PosicionInicio { get; set; }
        public string NombreArchivo { get; set; }
        public int CantidadCaracteres { get; set; }

        public CHI_Importar(string nombre_archivo, int posicion_inicio, int cantidad_caracteres, int fila, int columna)
        {
            this.fila = fila;
            this.columna = columna;
            NombreArchivo = nombre_archivo;
            PosicionInicio = posicion_inicio;
            CantidadCaracteres = cantidad_caracteres;
        }

        public object Ejecutar()
        {
            // 1. Actualizo el valor de la bandera que indica que hubo por lo menos un importar.
            StaticChison.HasImports = true;

            // 2. Armo el path del servidor con el nombre del archivo proporcionado.
            string fileName = NombreArchivo + ".chison";
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/ChisonFilesContainer/" + fileName);

            // 3. Verifico que el archivo exista en el folder de ChisonFilesContainer.
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                string contenido = string.Join("\n", lines);

                // 4. En la variable StaticChison.CadenaEntradaSeparada se encuentra separado en posiciones la cadena de
                // entrada con el fin de separar las importaciones y reemplazar el nombre de archivo por el contenido del
                // mismo.

                int indice = Array.IndexOf(StaticChison.CadenaEntradaSeparada, fileName);
                StaticChison.CadenaEntradaSeparada[indice] = contenido;

                return true;
            }
            else
            {
                StaticChison.AddError("Semántico", "[CHI_IMPORTAR]", "Error.  El archivo '"+ NombreArchivo +".chison' no existe en el folder.", fila, columna);
            }

            return false;
        }
    }
}