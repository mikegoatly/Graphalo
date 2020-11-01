using System.Collections.Generic;
using System.Linq;

namespace Graphalo.Searching
{
    public class DepthFirstSearch<TVertex, TEdge> : IGraphSearch<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        public static DepthFirstSearch<TVertex, TEdge> Instance { get; } = new DepthFirstSearch<TVertex, TEdge>();

        /// <inheritdoc />
        public IEnumerable<TVertex> Execute(IDirectedGraph<TVertex, TEdge> graph)
        {
            var output = new Queue<TVertex>(graph.VertexCount);
            var state = SearchState.Create(graph);

            while (state.TryGetVertexWithoutPermanentMark(out var vertex))
            {
                this.Visit(graph, vertex, state, output);
            }

            return output;
        }

        /// <inheritdoc />
        public IEnumerable<TVertex> Execute(IDirectedGraph<TVertex, TEdge> graph, TVertex startVertex)
        {
            var output = new Queue<TVertex>(graph.VertexCount);
            var state = SearchState.Create(graph);

            this.Visit(graph, startVertex, state, output);

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
