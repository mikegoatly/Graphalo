using System;

namespace Graphalo
{
    public interface IDirectedGraph<TVertex, TEdge> : IEdgeSet<TVertex, TEdge>, IVertexSet<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        event EventHandler Cleared;

        void Clear();
    }
}