using System;
using System.Collections.Generic;

namespace Graphalo
{
    public interface IVertexSet<TVertex, TEdge> where TEdge : IEdge<TVertex>
    {
        IEnumerable<VertexInfo<TVertex, TEdge>> AllVertices { get; }
        bool HasVertices { get; }
        IEqualityComparer<TVertex> VertexComparer { get; }
        int VertexCount { get; }

        event EventHandler<VertexEventArgs<TVertex>> VertexAdded;
        event EventHandler<VertexEventArgs<TVertex>> VertexRemoved;

        bool AddVertex(TVertex vertex);
        bool ContainsVertex(TVertex vertex);
        int Degree(TVertex vertex);
        bool RemoveVertex(TVertex vertex);
        void RemoveVertexRange(IEnumerable<TVertex> vertices);
        IReadOnlyList<TVertex> RemoveVertexWhere(Func<TVertex, bool> predicate);
    }
}