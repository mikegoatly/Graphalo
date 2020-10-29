namespace Graphalo
{
    public struct VertexInfo<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private readonly IDirectedGraph<TVertex, TEdge> graph;

        public VertexInfo(IDirectedGraph<TVertex, TEdge> graph, TVertex vertex)
        {
            this.graph = graph;
            this.Vertex = vertex;
        }

        public TVertex Vertex { get; }

        public bool HasInEdges => this.graph.HasInEdges(this.Vertex);

        public bool HasOutEdges => this.graph.HasOutEdges(this.Vertex);

        public int Degree => this.graph.Degree(this.Vertex);

        public int InDegree => this.graph.InDegree(this.Vertex);

        public int OutDegree => this.graph.OutDegree(this.Vertex);
    }
}
