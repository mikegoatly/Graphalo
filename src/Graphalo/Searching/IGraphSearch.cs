using System.Collections.Generic;

namespace Graphalo.Searching
{
    public interface IGraphSearch<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Searches across all the vertices in the graph.
        /// </summary>
        /// <param name="graph">
        /// The graph to search.
        /// </param>
        /// <returns>
        /// An ordered set of vertices matched by the search.
        /// </returns>
        IEnumerable<TVertex> Execute(IDirectedGraph<TVertex, TEdge> graph);

        /// <summary>
        /// Searches across all the vertices in the graph. Only vertices reachable from the given start 
        /// vertex will be considered.
        /// </summary>
        /// <param name="graph">
        /// The graph to search.
        /// </param>
        /// <param name="startVertex">
        /// The vertex to start searching from.
        /// </param>
        /// <returns>
        /// An ordered set of vertices matched by the search.
        /// </returns>
        IEnumerable<TVertex> Execute(IDirectedGraph<TVertex, TEdge> graph, TVertex startVertex);
    }
}
