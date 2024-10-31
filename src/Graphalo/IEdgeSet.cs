using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Graphalo
{
    public interface IEdgeSet<TVertex, TEdge> 
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Gets the edges in the graph.
        /// </summary>
        /// <value>The edges in the graph.</value>
        IEnumerable<TEdge> AllEdges { get; }

        /// <summary>
        /// Gets the number of edges in the graph.
        /// </summary>
        int EdgeCount { get; }

        /// <summary>
        /// Gets a value indicating whether there are edges in this instance.
        /// </summary>
        /// <value><c>true</c> if this instance has edges; otherwise, <c>false</c>.</value>
        bool HasEdges { get; }

        /// <summary>
        /// Occurs when an edge is added to the graph.
        /// </summary>
        event EventHandler<EdgeEventArgs<TEdge>> EdgeAdded;

        /// <summary>
        /// Occurs when an edge is removed from the graph.
        /// </summary>
        event EventHandler<EdgeEventArgs<TEdge>> EdgeRemoved;

        /// <summary>
        /// Adds an edge to the graph. The referenced vertices will automatically
        /// be added to the graph if they don't already exist in it.
        /// </summary>
        /// <param name="edge">
        /// The edge to add to the graph.
        /// </param>
        void AddEdge(TEdge edge);

        /// <summary>
        /// Adds the given edges to the graph. Any referenced vertices will automatically
        /// be added to the graph if they don't already exist in it.
        /// </summary>
        /// <param name="edges">
        /// The edge to add to the graph.
        /// </param>
        void AddEdges(IEnumerable<TEdge> edges);

        /// <summary>
        /// Clears the in-edges and out-edges of <paramref name="vertex"/>
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        void ClearEdges(TVertex vertex);

        /// <summary>
        /// Clears the in-edges of <paramref name="vertex"/>
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        void ClearInEdges(TVertex vertex);

        /// <summary>
        /// Clears the out edges for the given vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        void ClearOutEdges(TVertex vertex);

        /// <summary>
        /// Determines whether this instance contains the specified edge.
        /// </summary>
        /// <param name="edge">The edge to check for.</param>
        /// <returns>
        /// <c>true</c> if this instance contains the specified edge; otherwise, <c>false</c>.
        /// </returns>
        bool ContainsEdge(TEdge edge);

        /// <summary>
        /// Gets the edges going in or out of the given vertex.
        /// </summary>
        /// <param name="vertex">The vertex to get the edges for.</param>
        /// <returns>The edges that start or end at the given vertex.</returns>
        IEnumerable<TEdge> Edges(TVertex vertex);

        /// <summary>
        /// Determines whether the given vertex has edges going in to it.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>
        /// <c>true</c> if the given vertex has edges going in to it; otherwise, <c>false</c>.
        /// </returns>
        bool HasInEdges(TVertex vertex);

        /// <summary>
        /// Determines whether the given vertex has edges coming out from it.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>
        /// <c>true</c> if the given vertex has edges coming out from it; otherwise, <c>false</c>.
        /// </returns>
        bool HasOutEdges(TVertex vertex);

        /// <summary>
        /// Gets the degree of <paramref name="vertex"/>, i.e.
        /// the sum of the out-degree and in-degree of <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>
        /// The sum of <see cref="OutDegree"/> and <see cref="InDegree"/> of <paramref name="vertex"/>
        /// </returns>
        int Degree(TVertex vertex);

        /// <summary>
        /// Gets the number of in-edges of <paramref name="vertex"/>
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>
        /// The number of in-edges pointing towards <paramref name="vertex"/>
        /// </returns>
        int InDegree(TVertex vertex);

        /// <summary>
        /// Gets the edges going in to the given vertex.
        /// </summary>
        /// <param name="vertex">The vertex to get the in edges for.</param>
        /// <returns>The edges that end at the given vertex.</returns>
        IEnumerable<TEdge> InEdges(TVertex vertex);

        /// <summary>
        /// Gets the count of out-edges of <paramref name="vertex"/>
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>
        /// The count of out-edges of <paramref name="vertex"/>
        /// </returns>
        int OutDegree(TVertex vertex);

        /// <summary>
        /// Gets the edges coming out of the given vertex.
        /// </summary>
        /// <param name="vertex">The vertex to get the out edges for.</param>
        /// <returns>The edges that start at the given vertex.</returns>
        IEnumerable<TEdge> OutEdges(TVertex vertex);

        /// <summary>
        /// Removes the edge from the graph.
        /// </summary>
        /// <param name="edge">The edge to remove.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="edge"/> was successfully removed; otherwise <c>false</c>.
        /// </returns>
        bool RemoveEdge(TEdge edge);

        /// <summary>
        /// Removes the given edges from the graph.
        /// </summary>
        /// <param name="edges">The edges to remove.</param>
        void RemoveEdgeRange(IEnumerable<TEdge> edges);

        /// <summary>
        /// Removes edges where the given predicate is true.
        /// </summary>
        /// <param name="predicate">The predicate to test.</param>
        /// <returns>The removed edges.</returns>
        IReadOnlyList<TEdge> RemoveEdgeWhere(Func<TEdge, bool> predicate);

        /// <summary>
        /// Removes edges where the given predicate is true.
        /// </summary>
        /// <param name="predicate">
        /// The predicate to test. The second parameter is the graph itself which can often allow for the use
        /// of static functions as predicates.
        /// </param>
        /// <returns>The removed edges.</returns>
        IReadOnlyList<TEdge> RemoveEdgeWhere(Func<TEdge, DirectedGraph<TVertex, TEdge>, bool> predicate);

        /// <summary>
        /// Tries to get the edges between the given vertices.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <c>false</c> to only consider edges from <paramref name="source"/> to <paramref name="target"/></param>
        /// <param name="edges">Returns the matched edges.</param>
        /// <returns><c>true</c> if the edges could be retrieved, otherwise <c>false</c></returns>
        bool TryGetEdges(TVertex source, TVertex target, [NotNullWhen(true)] out IEnumerable<TEdge>? edges);
    }
}