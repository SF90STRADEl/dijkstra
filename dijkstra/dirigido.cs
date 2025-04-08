using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphLibrary
{
    public class GrafoDirigidoPonderado
    {
        private List<List<Edge>> adj;
        public int NumNodos { get; }
        public List<Node> Nodos { get; }

        public GrafoDirigidoPonderado(int nodos)
        {
            NumNodos = nodos;
            adj = new List<List<Edge>>();
            Nodos = new List<Node>();

            for (int i = 0; i < nodos; i++)
            {
                adj.Add(new List<Edge>());
                Nodos.Add(new Node(i, $"Nodo {i}"));
            }
        }

        public void AgregarArista(int origen, int destino, int peso)
        {
            ValidarNodo(origen);
            ValidarNodo(destino);

            if (ExisteArista(origen, destino))
            {
                Console.WriteLine($"¡Advertencia! Ya existe una arista de {origen} a {destino}. Sobrescribiendo peso.");
                RemoverArista(origen, destino);
            }

            adj[origen].Add(new Edge(Nodos[destino], peso));
            Console.WriteLine($"Arista {origen} → {destino} con peso {peso} agregada correctamente.");
        }

        public void EliminarArista(int origen, int destino)
        {
            ValidarNodo(origen);
            ValidarNodo(destino);

            if (!ExisteArista(origen, destino))
            {
                Console.WriteLine("No existe la arista para eliminar.");
                return;
            }

            RemoverArista(origen, destino);
            Console.WriteLine($"Arista {origen} → {destino} eliminada correctamente.");
        }

        private void ValidarNodo(int nodo)
        {
            if (nodo < 0 || nodo >= NumNodos)
                throw new ArgumentException($"Nodo {nodo} no existe en el grafo");
        }

        private bool ExisteArista(int origen, int destino)
        {
            return adj[origen].Any(edge => edge.Destino.Id == destino);
        }

        private void RemoverArista(int origen, int destino)
        {
            adj[origen].RemoveAll(edge => edge.Destino.Id == destino);
        }

        public void RenombrarNodo(int nodo, string nuevoNombre)
        {
            ValidarNodo(nodo);
            Nodos[nodo].Nombre = nuevoNombre;
            Console.WriteLine($"Nodo {nodo} renombrado a '{nuevoNombre}'");
        }

        public void Dijkstra(int origen)
        {
            ValidarNodo(origen);

            int[] distancias = new int[NumNodos];
            int[] predecesores = new int[NumNodos];
            bool[] visitados = new bool[NumNodos];

            for (int i = 0; i < NumNodos; i++)
            {
                distancias[i] = int.MaxValue;
                predecesores[i] = -1;
            }
            distancias[origen] = 0;

            var comparer = Comparer<(int distancia, int nodo)>.Create((a, b) =>
            {
                int cmp = a.distancia.CompareTo(b.distancia);
                return cmp == 0 ? a.nodo.CompareTo(b.nodo) : cmp;
            });

            var pq = new SortedSet<(int distancia, int nodo)>(comparer);
            pq.Add((0, origen));

            while (pq.Count > 0)
            {
                var (distActual, nodoActual) = pq.Min;
                pq.Remove(pq.Min);
                visitados[nodoActual] = true;

                foreach (var edge in adj[nodoActual])
                {
                    int vecino = edge.Destino.Id;
                    if (visitados[vecino])
                        continue;

                    int nuevaDist = distActual + edge.Peso;
                    if (nuevaDist < distancias[vecino])
                    {
                        pq.Remove((distancias[vecino], vecino));
                        distancias[vecino] = nuevaDist;
                        predecesores[vecino] = nodoActual;
                        pq.Add((nuevaDist, vecino));
                    }
                }
            }

            MostrarResultados(origen, distancias, predecesores);
        }

        private void MostrarResultados(int origen, int[] distancias, int[] predecesores)
        {
            Console.WriteLine($"\n--- Resultados de Dijkstra desde {Nodos[origen].Nombre} ({origen}) ---");
            Console.WriteLine("Nodo\tNombre\t\tDistancia\tRuta");

            for (int i = 0; i < NumNodos; i++)
            {
                Console.Write($"{i}\t{Nodos[i].Nombre}\t");
                if (distancias[i] == int.MaxValue)
                    Console.Write("∞\t\tNo alcanzable");
                else
                {
                    Console.Write($"{distancias[i]}\t\t");
                    MostrarRutaRecursiva(predecesores, i, origen);
                }
                Console.WriteLine();
            }
        }

        private void MostrarRutaRecursiva(int[] predecesores, int nodo, int origen)
        {
            if (nodo == origen)
            {
                Console.Write(Nodos[origen].Nombre);
                return;
            }
            if (predecesores[nodo] == -1)
            {
                Console.Write("Sin ruta");
                return;
            }

            MostrarRutaRecursiva(predecesores, predecesores[nodo], origen);
            Console.Write($" → {Nodos[nodo].Nombre}");
        }

        public void MostrarMatrizAdyacencia()
        {
            Console.WriteLine("\n--- Matriz de Adyacencia ---");
            Console.Write("     ");
            for (int i = 0; i < NumNodos; i++)
                Console.Write($"{i,5}");
            Console.WriteLine();

            for (int i = 0; i < NumNodos; i++)
            {
                Console.Write($"{i,3}| ");
                for (int j = 0; j < NumNodos; j++)
                {
                    var edge = adj[i].FirstOrDefault(e => e.Destino.Id == j);
                    Console.Write(edge.Destino != null && edge.Destino.Id == j
                        ? $"{edge.Peso,5}"
                        : "    -");
                }
                Console.WriteLine();
            }
        }

        public void MostrarTodosLosCaminos(int inicio, int fin)
        {
            ValidarNodo(inicio);
            ValidarNodo(fin);

            Console.WriteLine($"\nTodos los caminos de {Nodos[inicio].Nombre} a {Nodos[fin].Nombre}:");
            List<int> caminoActual = new List<int>();
            HashSet<int> visitados = new HashSet<int>();

            EncontrarCaminosRecursivo(inicio, fin, caminoActual, visitados);
        }

        private void EncontrarCaminosRecursivo(int actual, int fin, List<int> caminoActual, HashSet<int> visitados)
        {
            caminoActual.Add(actual);
            visitados.Add(actual);

            if (actual == fin)
            {
                MostrarCamino(caminoActual);
            }
            else
            {
                foreach (var edge in adj[actual])
                {
                    int vecino = edge.Destino.Id;
                    if (!visitados.Contains(vecino))
                    {
                        EncontrarCaminosRecursivo(vecino, fin, caminoActual, visitados);
                    }
                }
            }

            caminoActual.RemoveAt(caminoActual.Count - 1);
            visitados.Remove(actual);
        }

        private void MostrarCamino(List<int> camino)
        {
            Console.Write("Camino: ");
            for (int i = 0; i < camino.Count; i++)
            {
                Console.Write(Nodos[camino[i]].Nombre);
                if (i < camino.Count - 1)
                    Console.Write(" → ");
            }

            int pesoTotal = CalcularPesoCamino(camino);
            Console.WriteLine($" (Peso total: {pesoTotal})");
        }

        private int CalcularPesoCamino(List<int> camino)
        {
            int pesoTotal = 0;
            for (int i = 0; i < camino.Count - 1; i++)
            {
                int origen = camino[i];
                int destino = camino[i + 1];
                int peso = adj[origen].First(e => e.Destino.Id == destino).Peso;
                pesoTotal += peso;
            }
            return pesoTotal;
        }
    }
}
