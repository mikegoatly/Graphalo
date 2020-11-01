using Graphalo.Searching;
using Graphalo.Traversal;
using System;
using System.Collections.Generic;

namespace Graphalo
{
    /// <summary>
    /// A graph whose edges support a direction.
    /// </summary>
    /// <typeparam name="TVertex">
    /// The type of data to store in vertices.
    /// </typeparam>
    /// <typeparam name="TEdge">
    /// The type of edge used by the graph.
    /// </typeparam>
    public interface IDirectedGraph<TVertex, TEdge> : IEdgeSet<TVertex, TEdge>, IVertexSet<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Occurs when the graph is cleared. Individual events for <see cref="IVertexSet{TVertex, TEdge}.VertexRemoved"/> 
        /// and <see cref="IEdgeSet{TVertex, TEdge}.EdgeRemoved"/> will not be called.
        /// </summary>
        event EventHandler Cleared;

        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Clear();

        /// <summary>
        /// Performs a search on the graph.
        /// </summary>
        /// <param name="searchKind">The kind of search to perform.</param>
        /// <returns>
        /// The vertices that were matched by the search.
        /// </returns>
        IEnumerable<TVertex> Search(SearchKind searchKind);

        /// <summary>
        /// Attempts to traverse the graph
        /// </summary>
        /// <param name="traversalKind">
        /// The traversal algorithm to use.
        /// </param>
        /// <param name="source">
        /// The vertex to start from.
        /// </param>
        /// <param name="target">
        /// The vertex to attempt to reach.
        /// </param>
        /// <returns>
        /// The <see cref="TraversalResult{TVertex}"/> containing the traversal results, including whether or not traversal was successful.
        /// </returns>
        TraversalResult<TVertex> Traverse(TraversalKind traversalKind, TVertex source, TVertex target);
    }
}