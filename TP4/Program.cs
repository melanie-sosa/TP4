using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Trabajo_Practico_4
{
    class Program
    {
        static void Main(string[] args)
        {

            CultureInfo.CurrentCulture = new CultureInfo("es-AR", true);

            var cliente = new Cliente();
            var logistica = new Logistica();
            var servicio = new Servicios();
            var estadodeserv = new Estado_de_servicio();
            bool salir;

            //Generación de files con casos forzados *NECESARIO que ejecute al principio*
            cliente.DatosClientes();
            logistica.DatosCoddeSeg();
            servicio.LeerDestinos();
            Estado_de_servicio.SolicitudesDeServicio();

            //Validación del nro de cliente (consulta con clase cliente unicamente)
            do
            {
                cliente.corporativo = false;
                cliente.Validacion();

                if (cliente.corporativo == false)
                {
                    Console.WriteLine("\nEl número de cliente ingresado no forma parte de nuestra base de clientes corporativos. Intente nuevamente." +
                        "\nPresione una tecla para continuar\n");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("----------------------------");
                    Console.WriteLine($"| Bienvenido cliente {cliente.nrocliente} |");
                    Console.WriteLine("----------------------------");
                }
                Console.ReadLine();
                Console.Clear();


            } while (cliente.corporativo == false);


            do
            {
                salir = false;
                //Menu, disponible si pasa la validación de cliente
                Console.Clear();
                Console.WriteLine("\n--------------------- MENÚ PRINCIPAL -------------------- \n");
                Console.WriteLine("1.Solicitar un servicio de correspondencia o encomienda");
                Console.WriteLine("2.Consultar el estado de un servicio");
                Console.WriteLine("3.Consultar el estado de cuenta");
                Console.WriteLine("4.Finalizar");
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("Ingrese su opción");

                //Ingreso de opción
                var opcion = Console.ReadLine();


                //Switch de opciones con sus clases y métodos
                switch (opcion)
                {
                    case "1":
                        Console.Clear();
                        servicio.elegirTipoPaquete();
                        servicio.elegirTipoEntrega(cliente.nrocliente);

                        Console.ReadKey();
                        break;

                    case "2":
                        Console.Clear();
                        Estado_de_servicio.ConsultarEstadoServicio();
                        Console.ReadKey();
                        break;

                    case "3":
                        do
                        {
                            salir = false;
                            Console.Clear();
                            //muestra 2 opciones disponibles
                            Console.WriteLine("1.Ver historial de servicios");
                            Console.WriteLine("2.Consultar saldo de cuenta");
                            Console.WriteLine("3.Volver atrás");

                            opcion = Console.ReadLine();

                            switch (opcion)
                            {
                                case "1":
                                    Console.Clear();
                                    // genera file de historial de solicitudes
                                    estadodeserv.ConsultarHistorialCuenta(cliente.nrocliente);
                                    break;

                                case "2":
                                    // genera file de facturas
                                    Console.Clear();
                                    Facturacion.DatosFacturacion();
                                    estadodeserv.ConsultarSaldoCuenta(cliente.nrocliente);
                                    break;

                                case "3":
                                    //volver a menu principal
                                    salir = true;
                                    break;

                                default:
                                    Console.WriteLine("No ingresó una opción válida" +
                                        "\nPresione una tecla para continuar\n");
                                    break;
                            }

                        } while (!salir);

                        salir = false;

                        break;

                    case "4":
                        Console.Clear();
                        Console.WriteLine("El programa se cerrará\n");
                        salir = true;
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("No ingresó una opción válida");
                        break;
                }

            } while (!salir);

            Console.ReadLine();

        }
    }
}

