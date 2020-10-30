using System.Collections.Generic;
using System.Linq;

namespace Graphalo.Searching
{
    public class DepthFirstSearch<TVertex, TEdge> : IGraphSearch<TVertex, TEdge>
        where TEdge: IEdge<TVertex>
    {
        private readonly IDirectedGraph<TVertex, TEdge> graph;

        public DepthFirstSearch(IDirectedGraph<TVertex, TEdge> graph)
        {
            this.graph = graph;
        }

        public IEnumerable<TVertex> Execute()
        {
            var output = new Queue<TVertex>(this.graph.VertexCount);
            var state = SearchState.Create(this.graph);

            while (state.TryGetVertexWithoutPermanentMark(out var vertex))
            {
                this.Visit(this.graph, vertex, state, output);
            }

            return output;
        }

        private void Visit(
            IDirectedGraph<TVertex, TEdge> graph,
            TVertex vertex, 
            SearchState<TVertex, TEdge> state,
            Queue<TVertex> output)
        {
            if (state.HasPermanentMark(vertex))
            {
                return;
            }

            if (state.HasTemporaryMark(vertex))
            {
                throw new CyclicGraphsNotSupportedException();
            }

            state.SetTemporaryMark(vertex);

            foreach (var connectedVertex in graph.OutEdges(vertex).Select(e => e.To))
            {
                this.Visit(graph, connectedVertex, state, output);
            }

            state.RemoveTemporaryMark(vertex);
            state.SetPermanentMark(vertex);
            output.Enqueue(vertex);
        }
    }
}
