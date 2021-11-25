using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace Trabajo_Practico_4
{
    class Servicios
    {
        public bool urgente { get; set; }
        public bool retiroPuerta { get; set; }
        public bool entregaPuerta { get; set; }
        public int alcanceEnvío { get; set; }
        public int alcanceEnvioInt { get; set; }
        public string direccionOrigen { get; set; }
        public string direccionDestino { get; set; }
        public int codigoPostalOrigen { get; set; }
        public int codigoPostalDestino { get; set; }
        public string tipoEntregaSeleccionada { get; set; }
        public string tipoPaqueteSeleccionado { get; set; }
        public string regionDeOrigenSeleccionada { get; set; }
        public string provinciaDeOrigenSeleccionada { get; set; }
        public string regionDeDestinoSeleccionada { get; set; }
        public string provinciaDeDestinoSeleccionada { get; set; }
        public string provinciaDestinoInternacional { get; set; }
        public string destEnvio { get; set; }///
        public double precioFinal { get; set; }
        public string nomapeDestinatario { get; set; }
        public string infoDomicilio { get; set; }

        List<string> Provincias = new List<string>();

        Dictionary<int, string> tipoEntrega = new Dictionary<int, string>()
        {
            [1] = "Nacional",
            [2] = "Internacional"
        };

        Dictionary<int, string> tipoPaquete = new Dictionary<int, string>()
        {
            [1] = "Sobres hasta 500 gramos",
            [2] = "Bultos hasta 10 kilogramos",
            [3] = "Bultos hasta 20 kilogramos",
            [4] = "Bultos hasta 30 kilogramos"
        };


        public void LeerDestinos()
        {
            using (StreamReader lector = new StreamReader(@"Destinos.txt"))
            {
                string line;

                while ((line = lector.ReadLine()) != null)
                {
                    Provincias.Add(line);
                }

                lector.Close();
            }

        }

        //Selecciona el usuario el peso del paquete a enviar

        int opcionPaquete;

        public void elegirTipoPaquete()
        {
            bool flag = false;
            string paquete;
            provinciaDeDestinoSeleccionada = "";
            provinciaDestinoInternacional = "";

            do
            {
                Console.WriteLine("\nPaso 1 - Seleccione el número del tipo de paquete a entregar y presione ENTER\n");

                foreach (KeyValuePair<int, string> opcion in tipoPaquete)
                {
                    Console.WriteLine($"{opcion.Key} - {opcion.Value}\n");
                }

                paquete = Console.ReadLine();

                //Valido la data ingresada por el usuario
                if (string.IsNullOrWhiteSpace(paquete))
                    Console.WriteLine("\nPor favor no ingrese valores vacíos\n");

                else if (!int.TryParse(paquete, out opcionPaquete))
                    Console.WriteLine("\nPor favor ingrese un valor numérico\n");

                else if (opcionPaquete != 1 && opcionPaquete != 2 && opcionPaquete != 3 && opcionPaquete != 4)
                    Console.WriteLine("\nPor favor ingrese una de las opciones\n");

                else
                    flag = true;

            } while (flag == false);

            tipoPaqueteSeleccionado = tipoPaquete[opcionPaquete];

            Console.WriteLine($"\nEligió: {tipoPaqueteSeleccionado}\n");
            Console.WriteLine("------Enter para continuar------");
            Console.ReadKey();

            Console.Clear();
        }

        //Selecciona el usuario si desea una entrega nacional o internacional

        int opcionEntrega;
        int opcionProvincia;

        public void elegirTipoEntrega(int nrocliente)
        {
            bool flag = false;
            string entrega;

            var precio_ = new Precios();
            precio_.DatosTarifas();

            do
            {
                Console.WriteLine("Paso 2 - Seleccione el número del tipo de entrega a realizar y presione ENTER");
                Console.WriteLine();

                foreach (KeyValuePair<int, string> opcion in tipoEntrega)
                {
                    Console.WriteLine($"{opcion.Key} - {opcion.Value}\n");
                }

                entrega = Console.ReadLine();

                //Valido la data ingresada por el usuario
                if (string.IsNullOrWhiteSpace(entrega))
                    Console.WriteLine("\nPor favor, no ingrese valores vacíos\n");

                else if (!int.TryParse(entrega, out opcionEntrega))
                    Console.WriteLine("\nPor favor ingrese un valor numérico\n");

                else if (opcionEntrega != 1 && opcionEntrega != 2)
                    Console.WriteLine("\nPor favor ingrese una de las opciones\n");

                else
                    flag = true;

            } while (flag == false);

            tipoEntregaSeleccionada = tipoEntrega[opcionEntrega];

            //Devuelvo la información seleccionada para el tipo de entrega
            Console.WriteLine($"\nEligió: {tipoEntregaSeleccionada}\n");
            Console.WriteLine("------Enter para continuar------");
            Console.ReadKey();

            Console.Clear();

            bool flagC = false;
            string provinciaDeOrigen;

            Console.WriteLine("Paso 3 - Seleccione la provincia de ORIGEN y presione ENTER\n");

            do
            {
                foreach (var item in Provincias)
                {
                    string[] linea = item.Split(';');
                    int num = int.Parse(linea[0]);

                    if (num < 25)
                        Console.WriteLine($"{linea[0]} - {linea[1]}");
                }

                provinciaDeOrigen = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(provinciaDeOrigen))
                    Console.WriteLine("\nPor favor, no ingrese valores vacíos\n");

                else if (!int.TryParse(provinciaDeOrigen, out opcionProvincia))
                    Console.WriteLine("\nPor favor ingrese un valor numérico\n");

                else if (opcionProvincia <= 0 || opcionProvincia > 24)
                    Console.WriteLine("\nPor favor ingrese una de las opciones\n");

                else
                    flagC = true;

            } while (flagC == false);

            foreach (var item in Provincias)
            {
                string[] linea = item.Split(';');
                int num = int.Parse(linea[0]);

                if (num == opcionProvincia)
                {
                    provinciaDeOrigenSeleccionada = linea[1];
                    regionDeOrigenSeleccionada = linea[2];
                }

            }

            Console.WriteLine($"\nEligió {provinciaDeOrigenSeleccionada} como ORIGEN\n");
            Console.WriteLine("------Enter para continuar------");
            Console.ReadKey();

            Console.Clear();

            if (tipoEntregaSeleccionada == "Nacional")
            {
                bool flagF = false;
                string provinciaDeDestino;

                Console.WriteLine("Paso 4 - Seleccione la provincia/estado de DESTINO y presione ENTER");
                Console.WriteLine();
                do
                {
                    foreach (var item in Provincias)
                    {
                        string[] linea = item.Split(';');
                        int num = int.Parse(linea[0]);

                        if (num < 25)
                            Console.WriteLine($"{linea[0]} - {linea[1]}");
                    }

                    provinciaDeDestino = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(provinciaDeDestino))
                        Console.WriteLine("\nPor favor, no ingrese valores vacíos\n");

                    else if (!int.TryParse(provinciaDeDestino, out opcionProvincia))
                        Console.WriteLine("\nPor favor ingrese un valor numérico\n");

                    else if (opcionProvincia <= 0 || opcionProvincia > 24)
                        Console.WriteLine("\nPor favor ingrese una de las opciones\n");

                    else
                        flagF = true;

                } while (flagF == false);

                foreach (var item in Provincias)
                {
                    string[] linea = item.Split(';');
                    int num = int.Parse(linea[0]);

                    if (num == opcionProvincia)
                    {
                        provinciaDeDestinoSeleccionada = linea[1];
                        regionDeDestinoSeleccionada = linea[2];
                    }

                }

                Console.WriteLine($"\nEligió {provinciaDeDestinoSeleccionada} como DESTINO\n");
                Console.WriteLine("------Enter para continuar------");
                Console.ReadKey();

                Console.Clear();
            }

            else
            {
                bool flagG = false;

                Console.WriteLine("Paso 4 - Seleccione el DESTINO y presione ENTER");

                do
                {
                    int opcion = 0;

                    foreach (var item in Provincias)
                    {
                        string[] linea = item.Split(';');
                        int num = int.Parse(linea[0]);

                        if (num > 24)
                        {
                            opcion = opcion + 1;
                            Console.WriteLine($"{opcion} - {linea[1]}");
                        }
                    }

                    provinciaDestinoInternacional = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(provinciaDestinoInternacional))
                        Console.WriteLine("\nPor favor, no ingrese valores vacíos\n");

                    else if (!int.TryParse(provinciaDestinoInternacional, out opcionProvincia))
                        Console.WriteLine("\nPor favor ingrese un valor numérico\n");

                    else if (opcionProvincia <= 0 || opcionProvincia > 10)
                        Console.WriteLine("\nPor favor ingrese una de las opciones\n");

                    else
                        flagG = true;

                } while (flagG == false);

                foreach (var item in Provincias)
                {
                    string[] linea = item.Split(';');
                    int num = int.Parse(linea[0]);

                    if (num == opcionProvincia + 24)
                    {
                        provinciaDestinoInternacional = linea[1];
                        regionDeDestinoSeleccionada = linea[2];
                    }
                }

                Console.WriteLine($"\nEligió {provinciaDestinoInternacional} como DESTINO\n");
                Console.WriteLine("------Enter para continuar------");
                Console.ReadKey();

                Console.Clear();
            }

            bool flagA = false;
            bool flagB = false;
            string codigoPostalIngresado;
            int codigoPostalValidadoOrigen = 0;
            int codigoPostalValidadoDestino = 0;

            do
            {
                Console.WriteLine("Paso 5.1 - Ingrese el Código Postal de origen (SOLO NUMEROS) y presione ENTER");

                codigoPostalIngresado = Console.ReadLine();
                Console.WriteLine();

                if (string.IsNullOrWhiteSpace(codigoPostalIngresado))
                    Console.WriteLine("\nPor favor, no ingrese valores vacíos\n");

                else if (!int.TryParse(codigoPostalIngresado, out codigoPostalValidadoOrigen))
                    Console.WriteLine("\nPor favor ingrese un Código Postal valido\n");

                else if (codigoPostalValidadoOrigen <= 0)
                    Console.WriteLine("\nPor favor ingrese un Código Postal valido\n");

                else
                    flagA = true;

            } while (flagA == false);

            codigoPostalOrigen = codigoPostalValidadoOrigen;

            do
            {
                Console.WriteLine();
                Console.WriteLine("Paso 5.2 - Ingrese el Código Postal de destino (SOLO NUMEROS) y presione ENTER");

                codigoPostalIngresado = Console.ReadLine();
                Console.WriteLine();

                if (string.IsNullOrWhiteSpace(codigoPostalIngresado))
                    Console.WriteLine("\nPor favor, no ingrese valores vacíos\n");

                else if (!int.TryParse(codigoPostalIngresado, out codigoPostalValidadoDestino))
                    Console.WriteLine("\nPor favor ingrese un Código Postal valido.\n");

                else if (codigoPostalValidadoDestino <= 0)
                    Console.WriteLine("\nPor favor ingrese un Código Postal valido.\n");

                else
                    flagB = true;

            } while (flagB == false);

            Console.WriteLine("------Enter para continuar------");
            Console.ReadKey();

            Console.Clear();

            codigoPostalDestino = codigoPostalValidadoDestino;

            string direccionDeOrigen;
            bool flagD = false;

            do
            {

                Console.WriteLine("Paso 6.1 - Ingrese la dirección de ORIGEN y presione ENTER");

                direccionDeOrigen = Console.ReadLine();
                Console.WriteLine();

                if (string.IsNullOrWhiteSpace(direccionDeOrigen))
                    Console.WriteLine("\nPor favor, no ingrese valores vacíos\n");
                else if (hasSpecialChar2(direccionDeOrigen))
                    Console.WriteLine("La dirección no debe contener símbolos");
                else if (hasSpecialChar3(direccionDeOrigen))
                {
                    Console.WriteLine("La dirección no debe contener símbolos");
                }
                else
                {
                    flagD = true;
                }
            } while (flagD == false);


            direccionOrigen = direccionDeOrigen;

            string direccionDeDestino;
            bool flagE = false;

            do
            {
                Console.WriteLine();
                Console.WriteLine("Paso 6.2 - Ingrese la dirección de DESTINO y presione ENTER");

                direccionDeDestino = Console.ReadLine();
                Console.WriteLine();

                if (string.IsNullOrWhiteSpace(direccionDeDestino))
                {
                    Console.WriteLine("\nPor favor, no ingrese valores vacíos\n");
                }
                else if (hasSpecialChar2(direccionDeDestino))
                {
                    Console.WriteLine("La dirección no debe contener símbolos");
                }
                else if (hasSpecialChar3(direccionDeDestino))
                {
                    Console.WriteLine("La dirección no debe contener símbolos");
                }
                {
                    flagE = true;
                }
            } while (flagE == false);

            Console.WriteLine("------Enter para continuar------");
            Console.ReadKey();

            Console.Clear();


            direccionDestino = direccionDeDestino;

            //calculo alcance del envío comparando regiones (alcance: 1=local / 2=provincial / 3=regional / 4=nacional )

            if (tipoEntregaSeleccionada == "Nacional")
            {

                if (regionDeDestinoSeleccionada == regionDeOrigenSeleccionada)
                {
                    if (provinciaDeDestinoSeleccionada == provinciaDeOrigenSeleccionada)
                    {
                        if (codigoPostalDestino == codigoPostalOrigen)
                        {
                            alcanceEnvío = 1;
                            destEnvio = "Local";
                        }

                        else
                        {
                            alcanceEnvío = 2;
                            destEnvio = "Provincial";
                        }

                    }
                    else
                    {
                        alcanceEnvío = 3;
                        destEnvio = "Regional";
                    }
                }
                else
                {
                    alcanceEnvío = 4;
                    destEnvio = "Inter-Regional";
                }
            }
            else
            {
                if (regionDeOrigenSeleccionada == "Metropolitana")
                {
                    if (provinciaDeOrigenSeleccionada == "CABA")
                    {
                        alcanceEnvío = 1;
                        destEnvio = "Local - Provincial hasta CABA";
                    }
                    else
                    {
                        alcanceEnvío = 2;
                        destEnvio = "Regional hasta CABA";
                    }
                }
                else
                {
                    alcanceEnvío = 4;
                    destEnvio = "Inter-Regional hasta CABA";
                }
            }

            if (regionDeDestinoSeleccionada == "Limitrofe")
                alcanceEnvioInt = 1;
            else if (regionDeDestinoSeleccionada == "AmercaLatina")
                alcanceEnvioInt = 2;
            else if (regionDeDestinoSeleccionada == "AmericaNorte")
                alcanceEnvioInt = 3;
            else if (regionDeDestinoSeleccionada == "Europa")
                alcanceEnvioInt = 4;
            else if (regionDeDestinoSeleccionada == "Asia")
                alcanceEnvioInt = 5;



            Console.WriteLine("Paso 7 - Ingrese información (piso, departamento, timbre, entre calles) y presione ENTER");

            string infoIngresada = Console.ReadLine();
            Console.WriteLine();

            infoDomicilio = infoIngresada;
            Console.Write("------Enter para continuar------");
            Console.ReadKey();
            Console.Clear();




            bool flag_ = false;
            string datosIngresados;
            do
            {

                Console.WriteLine("Paso 8 - Ingrese el nombre y apellido del destinatario y presione ENTER");
                Console.WriteLine();
                datosIngresados = Console.ReadLine();


                if (string.IsNullOrWhiteSpace(datosIngresados))
                    Console.Write("\nPor favor, no ingrese valores vacíos\n");
                else if (flag = hasSpecialChar(datosIngresados))
                    Console.Write("\nEl nombre y apellido no puede contener números ni tampoco símbolos\n");
                else
                {
                    flag_ = true;
                }

            } while (flag_ == false);


            Console.Write("\n------Enter para continuar------\n");
            Console.ReadKey();

            Console.Clear();

            nomapeDestinatario = datosIngresados;

            bool flag1 = false;
            do
            {
                Console.WriteLine($"Paso 9 - ¿Desea hacer el envío con entrega a domicilio? Su costo adicional es de ${precio_.DiccionarioPrecios["Entrega Puerta"]}. Si(S) / No(N)");
                var tecla = Console.ReadKey(intercept: true);

                Console.WriteLine();

                if (tecla.Key != ConsoleKey.S && tecla.Key != ConsoleKey.N)
                    Console.WriteLine("Ingrese S/N");

                if (tecla.Key == ConsoleKey.S)
                {
                    Console.WriteLine("El envío será realizado al domicilio");
                    entregaPuerta = true;
                    flag1 = true;
                    Console.WriteLine();
                }
                else if (tecla.Key == ConsoleKey.N)
                {
                    Console.WriteLine("El envío será realizado a la sucursal de DESTINO más cercana");
                    entregaPuerta = false;
                    flag1 = true;
                    Console.WriteLine();
                }


            } while (!flag1);

            Console.WriteLine("------Enter para continuar------");
            Console.ReadKey();

            Console.Clear();

            bool flag2 = false;
            do
            {

                Console.WriteLine($"Paso 10 - ¿Desea hacer el despacho desde su domicilio? El valor adicional es de ${precio_.DiccionarioPrecios["Retiro Puerta"]}. Si(S) / No(N)");
                var tecla = Console.ReadKey(intercept: true);
                Console.WriteLine();

                if (tecla.Key != ConsoleKey.S && tecla.Key != ConsoleKey.N)
                    Console.WriteLine("Ingrese S/N");

                if (tecla.Key == ConsoleKey.S)
                {
                    Console.WriteLine("El despacho será realizado desde su domicilio");
                    retiroPuerta = true;
                    flag2 = true;
                    Console.WriteLine();
                }
                else if (tecla.Key == ConsoleKey.N)
                {
                    Console.WriteLine("El despacho será realizado desde la sucursal");
                    retiroPuerta = false;
                    flag2 = true;
                    Console.WriteLine();
                }


            } while (!flag2);

            Console.WriteLine("------Enter para continuar------");
            Console.ReadKey();

            Console.Clear();

            bool flag3 = false;
            do
            {

                Console.WriteLine($"Paso 11 - ¿Desea que el envío sea urgente? El adicional es de un 15% sobre el valor del envío. \nTope máximo para envíos nacionales de ${precio_.DiccionarioPrecios["Tope Pais"]}\n" +
                    $"Tope máximo para envíos internacionales de ${precio_.DiccionarioPrecios["Tope Internacional"]}\n" +
                    $"Si(S) / No(N)");
                var tecla = Console.ReadKey(intercept: true);
                Console.WriteLine();

                if (tecla.Key != ConsoleKey.S && tecla.Key != ConsoleKey.N)
                    Console.WriteLine("Ingrese S/N");

                if (tecla.Key == ConsoleKey.S)
                {
                    Console.WriteLine("El envío será realizado de forma urgente");
                    urgente = true;
                    flag3 = true;
                    Console.WriteLine();
                }
                else if (tecla.Key == ConsoleKey.N)
                {
                    Console.WriteLine("El envío será realizado de forma normal");
                    urgente = false;
                    flag3 = true;
                    Console.WriteLine();
                }

            } while (!flag3);

            Console.WriteLine("------Enter para continuar------");
            Console.ReadKey();

            Console.Clear();

            //CALCULAR PRECIO
            var precio = new Precios();
            var logistica = new Logistica();

            if (tipoEntregaSeleccionada == "Nacional")
                precioFinal = precio.CalcularPrecioServicio(tipoPaqueteSeleccionado, alcanceEnvío, entregaPuerta, retiroPuerta, urgente);
            else
                precioFinal = precio.CalcularPrecioServicio(tipoPaqueteSeleccionado, alcanceEnvío, entregaPuerta, retiroPuerta, alcanceEnvioInt, urgente);

            Console.WriteLine($"Valor del envío: ${Math.Round(precioFinal, 2)} (IVA incluido)");



            flag = false;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Paso 12 - ¿Desea confirmar? Si(S) / No(N)");
                var tecla = Console.ReadKey(intercept: true);

                if (tecla.Key != ConsoleKey.S && tecla.Key != ConsoleKey.N)
                    Console.WriteLine("Ingrese Si(S) / No(N)");

                if (tecla.Key == ConsoleKey.S)
                {
                    Console.WriteLine("\nEl envío fue confirmado exitosamente" +
                        "\n------Enter para continuar------");
                    Console.ReadKey();

                    logistica.DatosCoddeSeg();

                    //MOSTRAR DETALLE
                    mostrarDetalle();
                    //Generar código de seguimiento y grabar servicio
                    logistica.GenerarFile(nrocliente, logistica.GeneraryMostrarMostrarCS(nrocliente), precioFinal);

                    flag = true;
                    Console.WriteLine("------Enter para volver al menú principal------");
                }
                else if (tecla.Key == ConsoleKey.N)
                {
                    Console.WriteLine("\n¿Esta seguro que quiere cancelarlo? Si(S) / No(N)");

                    var tecla2 = Console.ReadKey(intercept: true);

                    if (tecla2.Key != ConsoleKey.S && tecla2.Key != ConsoleKey.N)
                        Console.WriteLine("\nIngrese Si(S) / No(N)");

                    if (tecla2.Key == ConsoleKey.N)
                        flag = false;

                    if (tecla2.Key == ConsoleKey.S)
                    {
                        Console.WriteLine("\nEl envío fue cancelado");
                        Console.WriteLine("------Enter para volver al menú------");
                        //VOLVER AL MENU PRINCIPAL
                        flag = true;
                    }

                }

            } while (!flag);

        }

        public void mostrarDetalle()
        {
            Console.Clear();
            var logistica = new Logistica();

            Console.WriteLine($"\nResumen servicio: ");
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("Datos generales del servicio: ");
            Console.WriteLine($"Se enviará un {tipoPaqueteSeleccionado} ");
            Console.WriteLine($"El servicio a realizar será de alcance: {tipoEntregaSeleccionada} - {destEnvio}");
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("Datos de origen: ");
            Console.WriteLine($"Provincia: {provinciaDeOrigenSeleccionada}");
            Console.WriteLine($"Dirección: {direccionOrigen}");
            Console.WriteLine($"Codigo postal: {codigoPostalOrigen}");
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("Datos de destino: ");
            Console.WriteLine($"Destinatario: {nomapeDestinatario}");
            Console.WriteLine($"Provincia: {provinciaDeDestinoSeleccionada}{provinciaDestinoInternacional}");
            Console.WriteLine($"Dirección: {direccionDestino}");
            Console.WriteLine($"Piso / Departamento / Referencia: {infoDomicilio}");
            Console.WriteLine($"Codigo postal: {codigoPostalDestino}");
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("Servicios adicionales: ");

            if (urgente == false && entregaPuerta == false && retiroPuerta == false)
                Console.WriteLine("No se solicitó ningún servicio adicional");

            else
            {
                int numero = 0;

                if (urgente == true)
                {
                    numero++;
                    Console.WriteLine($" {numero}. El servicio será realizado con caracter urgente");
                }
                if (entregaPuerta == true)
                {
                    numero++;
                    Console.WriteLine($" {numero}. El servicio se entregará en puerta");
                }
                if (retiroPuerta == true)
                {
                    numero++;
                    Console.WriteLine($" {numero}. El servicio se retirará en puerta");
                }
            }

            Console.WriteLine("-------------------------------------------------------" +
                "\n------Enter para continuar------\n");
            Console.ReadKey();
        }


        //METOD PARA VALIDAR SIMBOLOS
        public static bool hasSpecialChar2(string input)
        {
            string specialChar = @"|¡!#$%&/\()`^=¿?»«@£§€{}.,;:[]+-~`'´°<>_*";

            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }
            return false;
        }

        //METODO PARA VALIDAR SIMBOLOS  Y NUMEROS
        public static bool hasSpecialChar(string input)
        {
            string specialChar = @"|¡!#$%&/\()`^=¿?»«@£§€{}.,;:[]+-~`´'°<>*_0123456789";

            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }
            return false;
        }

        public static bool hasSpecialChar3(string input)
        {
            string specialChar = "\"";

            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }
            return false;
        }

    }
}

