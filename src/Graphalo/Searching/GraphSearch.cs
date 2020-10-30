namespace Graphalo.Searching
{
    public static class GraphSearch
    {
        public static IGraphSearch<TVertex, TEdge> DepthFirst<TVertex, TEdge>(IDirectedGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            return new DepthFirstSearch<TVertex, TEdge>(graph);
        }
    }
}
