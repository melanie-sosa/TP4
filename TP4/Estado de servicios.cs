using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Trabajo_Practico_4
{
    class Estado_de_servicio
    {

        //verifico si el file existe, sino se crea
        public static void SolicitudesDeServicio()
        {

        }

        //devuelve historial de servicios por cliente
        public static List<string> ConsultarServiciosCliente(int cuit)
        {
            List<string> ServiciosCliente = new List<string>();

            using (StreamReader lector = new StreamReader(@"CodigosdeSeguimiento.txt"))
            {
                string line;

                while ((line = lector.ReadLine()) != null)
                {
                    if (line.Contains(cuit.ToString()))
                    {
                        ServiciosCliente.Add(line);
                    }
                }

                lector.Close();
            }

            return ServiciosCliente;
        }


        //en class

        //muestra historial de servicios
        public void ConsultarHistorialCuenta(int cuit)
        {
            Console.WriteLine($"Historial de servicios del cliente {cuit}:\n");

            foreach (var item in Estado_de_servicio.ConsultarServiciosCliente(cuit))
            {
                string[] linea = item.Split(';');
                string servicio_ = "Código de servicio: ";
                string monto_ = "\tMonto total: $";
                string estado_ = "\t\tEstado: ";

                servicio_ += string.Format("{0,5}", linea[0]);
                monto_ += string.Format("{0,5}", linea[3]);
                estado_ += string.Format("{0,-5}", linea[2]);
                double monto = double.Parse(linea[3]);

                Console.WriteLine($"{servicio_} {monto_} {estado_}");

                //Console.WriteLine($"Código servicio: {linea[0]} \t\t Monto total: ${linea[3]} \t\t Estado: {linea[2]}");
            }

            Console.ReadKey();
        }

        //muestra saldo deudor de la cuenta
        public void ConsultarSaldoCuenta(int cuit)
        {
            Console.WriteLine($"Servicios del cliente {cuit}:\n");
            double servicios_pagos = 0;
            double servicios_total = 0;
            List<string> servicios_facturados = Facturacion.ConsultarFacturacionCliente(cuit);
            List<string> codigos_facturados = new List<string>();

            foreach (var item in servicios_facturados)
            {
                string[] linea = item.Split(';');
                codigos_facturados.Add(linea[2]);

            }



            //arranca con lista de todos los servicios
            foreach (var item in Estado_de_servicio.ConsultarServiciosCliente(cuit))
            {
                string[] lineaservicio = item.Split(';');
                string servicio_ = "Código de servicio: ";
                string monto_ = "\tMonto total: $";
                string estado_ = "\t\tEstado: ";

                //escribe los que no estan facurados
                if (!codigos_facturados.Contains(lineaservicio[0]))
                {
                    servicio_ += string.Format("{0,5}", lineaservicio[0]);
                    monto_ += string.Format("{0,5}", lineaservicio[3]);
                    estado_ += string.Format("{0,-5}", "Pendiente Facturación");
                    double monto = double.Parse(lineaservicio[3]);

                    Console.WriteLine($"{servicio_} {monto_} {estado_}");
                }
                //escribe los facturados 
                else
                {
                    foreach (var item2 in servicios_facturados)
                    {
                        string[] lineafactura = item2.Split(';');

                        if (lineaservicio[0] == lineafactura[2])
                        {
                            //escribe los que estan pagos
                            if (lineafactura[3] == "si")
                            {
                                servicio_ += string.Format("{0,5}", lineafactura[2]);
                                monto_ += string.Format("{0,5}", lineafactura[4]);
                                estado_ += string.Format("{0,-5}", "Pago");
                                double monto = double.Parse(lineafactura[4]);

                                Console.WriteLine($"{servicio_} {monto_} {estado_}");
                                //Console.WriteLine($"{0,10}Código servicio: {lineafactura[2]} {5,10}Monto total: ${lineafactura[4]} {10,10}Estado: Pago");

                                servicios_pagos += monto;
                            }
                            else
                            {
                                servicio_ += string.Format("{0,5}", lineafactura[2]);
                                monto_ += string.Format("{0,5}", lineafactura[4]);
                                estado_ += string.Format("{0,-5}", "Facturado pendiente de pago");

                                Console.WriteLine($"{servicio_} {monto_} {estado_}");
                                //Console.WriteLine($"{0,10}Código servicio: {lineafactura[2]} {5,10}Monto total: ${lineafactura[4]} {10,10}Estado: Facturado pendiente de pago");
                            }

                        }

                    }

                }

                double monto2 = double.Parse(lineaservicio[3]);
                servicios_total += monto2;

            }

            Console.WriteLine($"\nSu saldo deudor es de ${Math.Round(servicios_total - servicios_pagos, 2)}" +
                        "\n------ENTER para volver al menú------\n");
            Console.ReadKey();
        }

        public static void ConsultarEstadoServicio()
        {
            string estado_servicio = "";
            int codigo = Logistica.ValidarCodigoIngresado();


            using (StreamReader sr = new StreamReader(@"CodigosdeSeguimiento.txt"))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains(codigo.ToString()))
                    {
                        string[] arraylinea = line.Split(';');
                        estado_servicio = arraylinea[2];
                    }
                }

                sr.Close();
            }

            Console.WriteLine();
            Console.WriteLine($"Estado del servicio {codigo}: {estado_servicio}" +
                $"\n------ENTER para volver al menú------\n");

        }

    }
}
