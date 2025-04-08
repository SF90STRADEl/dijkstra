using System;
using GraphLibrary;

namespace GraphConsoleApp
{
    class Program
    {
        static GrafoDirigidoPonderado grafo;

        static void Main()
        {
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("  SISTEMA DE GRAFOS DIRIGIDOS AVANZADO");
            Console.WriteLine("═══════════════════════════════════════");

            InicializarGrafo();

            bool salir = false;
            while (!salir)
            {
                MostrarMenuPrincipal();
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        GestionarNodos();
                        break;
                    case "2":
                        GestionarAristas();
                        break;
                    case "3":
                        EjecutarAlgoritmos();
                        break;
                    case "4":
                        VisualizarGrafo();
                        break;
                    case "5":
                        salir = ConfirmarSalida();
                        break;
                    default:
                        Console.WriteLine("Opción inválida. Intente nuevamente.");
                        break;
                }
            }
        }

        static void InicializarGrafo()
        {
            Console.Write("\nIngrese el número de nodos para el grafo: ");
            int nodos = int.Parse(Console.ReadLine());
            grafo = new GrafoDirigidoPonderado(nodos);
            Console.WriteLine($"\nGrafo creado con {nodos} nodos.");
        }

        static void MostrarMenuPrincipal()
        {
            Console.WriteLine("\n════════════ MENÚ PRINCIPAL ════════════");
            Console.WriteLine("1. Gestión de Nodos");
            Console.WriteLine("2. Gestión de Aristas");
            Console.WriteLine("3. Ejecutar Algoritmos");
            Console.WriteLine("4. Visualización del Grafo");
            Console.WriteLine("5. Salir");
            Console.Write("Seleccione una opción: ");
        }

        static void GestionarNodos()
        {
            Console.WriteLine("\n════════════ GESTIÓN DE NODOS ════════════");
            Console.WriteLine("1. Renombrar nodo");
            Console.WriteLine("2. Volver al menú principal");
            Console.Write("Seleccione: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Nodo a renombrar: ");
                    int nodo = int.Parse(Console.ReadLine());
                    Console.Write("Nuevo nombre: ");
                    string nombre = Console.ReadLine();
                    grafo.RenombrarNodo(nodo, nombre);
                    break;
            }
        }

        static void GestionarAristas()
        {
            Console.WriteLine("\n════════════ GESTIÓN DE ARISTAS ════════════");
            Console.WriteLine("1. Agregar arista");
            Console.WriteLine("2. Eliminar arista");
            Console.WriteLine("3. Volver al menú principal");
            Console.Write("Seleccione: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Nodo origen: ");
                    int origen = int.Parse(Console.ReadLine());
                    Console.Write("Nodo destino: ");
                    int destino = int.Parse(Console.ReadLine());
                    Console.Write("Peso: ");
                    int peso = int.Parse(Console.ReadLine());
                    grafo.AgregarArista(origen, destino, peso);
                    break;
                case "2":
                    Console.Write("Nodo origen: ");
                    int origenDel = int.Parse(Console.ReadLine());
                    Console.Write("Nodo destino: ");
                    int destinoDel = int.Parse(Console.ReadLine());
                    grafo.EliminarArista(origenDel, destinoDel);
                    break;
            }
        }

        static void EjecutarAlgoritmos()
        {
            Console.WriteLine("\n════════════ ALGORITMOS ════════════");
            Console.WriteLine("1. Dijkstra (Caminos mínimos)");
            Console.WriteLine("2. Encontrar todos los caminos");
            Console.WriteLine("3. Volver al menú principal");
            Console.Write("Seleccione: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Nodo origen: ");
                    int origen = int.Parse(Console.ReadLine());
                    grafo.Dijkstra(origen);
                    break;
                case "2":
                    Console.Write("Nodo inicio: ");
                    int inicio = int.Parse(Console.ReadLine());
                    Console.Write("Nodo fin: ");
                    int fin = int.Parse(Console.ReadLine());
                    grafo.MostrarTodosLosCaminos(inicio, fin);
                    break;
            }
        }

        static void VisualizarGrafo()
        {
            Console.WriteLine("\n════════════ VISUALIZACIÓN ════════════");
            grafo.MostrarMatrizAdyacencia();
        }

        static bool ConfirmarSalida()
        {
            Console.Write("\n¿Está seguro que desea salir? (s/n): ");
            return Console.ReadLine().Trim().ToLower() == "s";
        }
    }
}
