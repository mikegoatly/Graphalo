using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Graphalo.Searching
{
    public static class SearchState
    {
        public static SearchState<TVertex, TEdge> Create<TVertex, TEdge>(
            IDirectedGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            return new SearchState<TVertex, TEdge>(graph);
        }
    }

    public class SearchState<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private readonly HashSet<TVertex> unmarkedVertices;
        private readonly HashSet<TVertex> permanentMarks;
        private readonly HashSet<TVertex> temporaryMarks;

        public SearchState(IDirectedGraph<TVertex, TEdge> graph)
        {
            this.unmarkedVertices = graph.AllVertices.Select(v => v.Vertex).ToHashSet(graph.VertexComparer);
            this.permanentMarks = new HashSet<TVertex>(graph.VertexComparer);
            this.temporaryMarks = new HashSet<TVertex>(graph.VertexComparer);
        }

        public void SetTemporaryMark(TVertex vertex)
        {
            this.temporaryMarks.Add(vertex);
        }

        public void SetPermanentMark(TVertex vertex)
        {
            this.permanentMarks.Add(vertex);
            this.unmarkedVertices.Remove(vertex);
        }

        public bool TryGetVertexWithoutPermanentMark([NotNullWhen(true)]out TVertex vertex)
        {
            if (unmarkedVertices.Count == 0)
            {
                vertex = default!;
                return false;
            }

            vertex = unmarkedVertices.First()!;
            return true;
        }

        public void RemoveTemporaryMark(TVertex vertex)
        {
            if (!this.temporaryMarks.Remove(vertex))
            {
                throw new GraphaloException(Resources.UnsupportedSearchKind);
            }
        }

        public bool HasPermanentMark(TVertex vertex)
        {
            return this.permanentMarks.Contains(vertex);
        }

        public bool HasTemporaryMark(TVertex vertex)
        {
            return this.temporaryMarks.Contains(vertex);
        }
    }
}
