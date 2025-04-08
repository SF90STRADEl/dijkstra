namespace GraphLibrary
{
    public class Edge
    {
        public Node Destino { get; }
        public int Peso { get; }

        public Edge(Node destino, int peso)
        {
            Destino = destino;
            Peso = peso;
        }
    }
}
