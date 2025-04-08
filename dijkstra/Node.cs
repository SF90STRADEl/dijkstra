namespace GraphLibrary
{
    public class Node
    {
        public int Id { get; }
        public string Nombre { get; set; }

        public Node(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }

        public override string ToString() => Nombre;
    }
}
