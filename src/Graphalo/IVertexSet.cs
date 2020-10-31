using System;
using System.Collections.Generic;

namespace Graphalo
{
    public interface IVertexSet<TVertex, TEdge> where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Gets the vertices in this graph.
        /// </summary>
        /// <value>The vertices in this graph.</value>
        IEnumerable<VertexInfo<TVertex, TEdge>> AllVertices { get; }

        /// <summary>
        /// Gets a value indicating whether there are vertices in this instance.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance contains vertices; otherwise, <c>false</c>.
        /// </value>
        bool HasVertices { get; }

        /// <summary>
        /// Gets the comparer capable of comparing two vertices for equality.
        /// </summary>
        /// <value>The vertex comparer.</value>
        IEqualityComparer<TVertex> VertexComparer { get; }

        /// <summary>
        /// Gets the number of vertices in the graph.
        /// </summary
        int VertexCount { get; }

        /// <summary>
        /// Occurs when a vertex is added to the graph
        /// </summary>
        event EventHandler<VertexEventArgs<TVertex>> VertexAdded;

        /// <summary>
        /// Occurs when a vertex is removed from the graph
        /// </summary>
        event EventHandler<VertexEventArgs<TVertex>> VertexRemoved;

        /// <summary>
        /// Adds a vertex to the graph.
        /// </summary>
        /// <param name="vertex">The vertex to add.</param>
        /// <returns><c>true</c> if the vertex was added, <c>false</c> if it already existed in the graph.</returns>
        bool AddVertex(TVertex vertex);

        /// <summary>
        /// Determines whether this instance contains the given vertex.
        /// </summary>
        /// <param name="vertex">The vertex to check for.</param>
        /// <returns>
        /// <c>true</c> if this instance containst the vertex; otherwise, <c>false</c>.
        /// </returns>
        bool ContainsVertex(TVertex vertex);

        /// <summary>
        /// Removes the vertex and any associated edges from the graph.
        /// </summary>
        /// <param name="vertex">The vertex to remove.</param>
        /// <returns>
        /// <c>true</c> if the vertex could be removed, otherwise <c>false</c>
        /// </returns>
        bool RemoveVertex(TVertex vertex);

        /// <summary>
        /// Removes the vertices and any associated edges from the graph.
        /// </summary>
        /// <param name="vertices">The vertices to remove.</param>
        void RemoveVertexRange(IEnumerable<TVertex> vertices);

        /// <summary>
        /// Removes vertices that match the given predicate.
        /// </summary>
        /// <param name="predicate">The predicate to match.</param>
        /// <returns>
        /// The number of removed vertices.
        /// </returns>
        IReadOnlyList<TVertex> RemoveVertexWhere(Func<TVertex, bool> predicate);
    }
}