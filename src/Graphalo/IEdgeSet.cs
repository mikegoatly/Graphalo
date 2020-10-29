using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Graphalo
{
    public interface IEdgeSet<TVertex, TEdge> 
        where TEdge : IEdge<TVertex>
    {
        IEnumerable<TEdge> AllEdges { get; }
        int EdgeCount { get; }
        bool HasEdges { get; }

        event EventHandler<EdgeEventArgs<TEdge>> EdgeAdded;
        event EventHandler<EdgeEventArgs<TEdge>> EdgeRemoved;

        void AddEdge(TEdge edge);
        void AddEdges(IEnumerable<TEdge> edges);
        void ClearEdges(TVertex vertex);
        void ClearInEdges(TVertex vertex);
        void ClearOutEdges(TVertex vertex);
        bool ContainsEdge(TEdge edge);
        IEnumerable<TEdge> Edges(TVertex vertex);
        bool HasInEdges(TVertex vertex);
        bool HasOutEdges(TVertex vertex);
        int InDegree(TVertex vertex);
        IEnumerable<TEdge> InEdges(TVertex vertex);
        int OutDegree(TVertex vertex);
        IEnumerable<TEdge> OutEdges(TVertex vertex);
        bool RemoveEdge(TEdge edge);
        void RemoveEdgeRange(IEnumerable<TEdge> edges);
        IReadOnlyList<TEdge> RemoveEdgeWhere(Func<TEdge, bool> predicate);
        bool TryGetEdges(TVertex source, TVertex target, [NotNullWhen(true)] out IEnumerable<TEdge>? edges);
    }
}