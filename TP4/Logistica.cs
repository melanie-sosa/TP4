using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Trabajo_Practico_4
{
    class Logistica
    {
        public int codseguim { get; set; } = 10000;

        //ver si es necesario el diccionario
        Dictionary<int, int> ClienteyCodSeg = new Dictionary<int, int>();

        List<string> lista_txt = new List<string>();

        //Extraer del .txt los datos y guardarlos en una lista
        public void DatosCoddeSeg()
        {
            using (StreamReader sr = new StreamReader(@"CodigosdeSeguimiento.txt"))
            {
                string line;
                string[] data;

                while ((line = sr.ReadLine()) != null)
                {
                    data = line.Split(';');
                    int codigdodeseguim = Convert.ToInt32(data[0]);
                    int cliente = Convert.ToInt32(data[1]);
                    ClienteyCodSeg.Add(codigdodeseguim, cliente);

                    lista_txt.Add(line);
                }
                sr.Close();

                codseguim = ClienteyCodSeg.Keys.Max();
            }

        }

        //Genera y devuelve un nuevo código en base al último generado y lo muestra en pantalla.
        public int GeneraryMostrarMostrarCS(int nrocliente)
        {

            codseguim = codseguim + 1;

            ClienteyCodSeg.Add(codseguim, nrocliente);

            Console.WriteLine($"Se registró su servicio. Código de seguimiento: {codseguim}");



            return codseguim;
        }

        //ULTIMO EN EJECUTARSE - Genera el file en base a lo que tiene en la lista.
        public void GenerarFile(int cliente, int codseguim, double precio)
        {
            using (var sw = new StreamWriter(@"CodigosdeSeguimiento.txt"))
            {
                foreach (var item in lista_txt)
                {
                    sw.WriteLine(item);
                }

                sw.WriteLine(codseguim + ";" + cliente + ";" + "Recibido" + ";" + precio);

                sw.Close();
            }

        }


        public static int ValidarCodigoIngresado()
        {
            bool valido = false;
            int codseg;
            const int MinLenght = 10000;
            const int MaxLenght = 99999;
            var cliente = new Cliente();



            do
            {
                Console.WriteLine("Ingrese el código de seguimiento (5 dígitos, sin guiones ni espacios)");
                string codigo = Console.ReadLine();



                if (!int.TryParse(codigo, out codseg))
                {
                    Console.WriteLine("\nNo ha ingresado un código de seguimiento válido (5 dígitos, sin guiones ni espacios)" +
                    "\nPresione una tecla para continuar.\n");
                    Console.ReadKey();
                    Console.Clear();
                }



                else if (MinLenght > codseg || codseg > MaxLenght)
                {
                    Console.WriteLine($"\nEl código de seguimiento debe contener 5 dígitos." +
                    "\nPresione una tecla para continuar.\n");
                    Console.ReadKey();
                    Console.Clear();
                }



                else
                {
                    using (StreamReader lector = new StreamReader(@"CodigosdeSeguimiento.txt"))
                    {
                        string line;

                        while ((line = lector.ReadLine()) != null)
                        {
                            string combined = (string.Format(codigo, ",", cliente.nrocliente));
                            if (line.Contains(combined))
                            {
                                valido = true;
                            }
                        }

                        lector.Close();
                    }

                    if (!valido)
                        Console.WriteLine("No tenemos ningún servicio registrado con ese número, vuelva a intentarlo");
                }



            } while (valido == false);

            return codseg;

        }
    }
}
