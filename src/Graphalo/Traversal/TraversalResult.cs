using System.Collections.Generic;
using System.Linq;

namespace Graphalo.Traversal
{
    /// <summary>
    /// The result of a <see cref="ITraversalAlgorithm{TVertex, TEdge}.Traverse(IDirectedGraph{TVertex, TEdge}, TVertex, TVertex)"/> invocation.
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    public class TraversalResult<TVertex>
    {
        private TraversalResult(bool success, IEnumerable<TVertex> results)
        {
            this.Success = success;
            this.Results = results;
        }

        /// <summary>
        /// Gets an instance of <see cref="TraversalResult{TVertex}"/> that represents a failure to traverse from the source to target 
        /// vertex in a graph.
        /// </summary>
        public static TraversalResult<TVertex> Failure { get; } = new TraversalResult<TVertex>(false, Enumerable.Empty<TVertex>());

        /// <summary>
        /// Gets a value indicating whether the traversal was successful. When the target vertex could be reached from the source vertex,
        /// this will be <c>true</c>, otherwise <c>false</c>.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Gets the vertices that were traversed, starting from the source vertex and ending on the target vertex. For unsuccessful traversals
        /// (when <see cref="Success"/> is <c>false</c>) this will be empty.
        /// </summary>
        public IEnumerable<TVertex> Results { get; }

        /// <summary>
        /// Creates a successful (complete) traversal of a graph.
        /// </summary>
        /// <param name="results">
        /// The vertices that were traversed, starting from the source vertex and ending on the target vertex.
        /// </param>
        /// <returns>
        /// The constructed <see cref="TraversalResult{TVertex}"/> instance.
        /// </returns>
        public static TraversalResult<TVertex> Complete(IEnumerable<TVertex> results)
        {
            return new TraversalResult<TVertex>(true, results);
        }
    }
}
