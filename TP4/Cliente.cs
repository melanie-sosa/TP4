using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Trabajo_Practico_4
{
    class Cliente
    {

        public int nrocliente { get; set; }

        private bool Corporativo = false;
        public bool corporativo
        {
            get
            {
                return Corporativo;
            }
            set
            {
                Corporativo = value;
            }
        }

        public void Validacion()
        {
            //Validación del tipo de dato númerico. Se almacena si es int
            string input;
            bool valido = false;

            const int MinLenght = 10000;
            const int MaxLenght = 99999;
            do
            {
                Console.WriteLine("Ingrese su número de cliente corporativo (5 dígitos, sin guiones ni espacios)");
                input = Console.ReadLine();

                if (!int.TryParse(input, out int Nrocliente))
                {
                    Console.WriteLine("\nNo ha ingresado un número de cliente corporativo válido (5 dígitos, sin guiones ni espacios)" +
                        "\nPresione una tecla para continuar.\n");
                    Console.ReadKey();
                    Console.Clear();
                }
                else if (MinLenght > Nrocliente || Nrocliente > MaxLenght)
                {
                    Console.WriteLine($"\nEl número de cliente corporativo debe ser positivo y contener 5 dígitos. " +
                        "\nPresione una tecla para continuar.\n");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    valido = true;
                    nrocliente = Convert.ToInt32(input);
                }


            } while (valido == false);

            //Validación en archivo del ingreso en el paso anterior
            using (StreamReader sr = new StreamReader(@"ClientesCorporativos.txt"))
            {
                string line;
                string record = nrocliente.ToString();
                Corporativo = false;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains(record))
                    {
                        Corporativo = true;
                    }
                }

                sr.Close();
            }
        }

        public void DatosClientes()
        {
            //Verifico si el file existe, en caso de no existir se crea.

        }



    }
}
