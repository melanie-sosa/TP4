using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Trabajo_Practico_4
{
    class Facturacion
    {

        public static void DatosFacturacion()
        {
            //Verifica si el file existe, en caso de no existir se crea.
        }

        //devuelve facturación por cliente
        public static List<string> ConsultarFacturacionCliente(int cuit)
        {
            List<string> FacturasCliente = new List<string>();

            using (StreamReader lector = new StreamReader(@"Facturas.txt"))
            {
                string line;

                while ((line = lector.ReadLine()) != null)
                {
                    if (line.Contains(cuit.ToString()))
                    {
                        FacturasCliente.Add(line);
                    }
                }

                lector.Close();
            }

            return FacturasCliente;
        }
    }

}
