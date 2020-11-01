namespace Graphalo.Traversal
{
    public interface ITraversalAlgorithm<TVertex, TEdge> where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Attempts to traverse from <paramref name="sourceVertex"/> to <paramref name="targetVertex"/> in the given graph.
        /// </summary>
        /// <param name="graph">
        /// The graph the vertices exist in.
        /// </param>
        /// <param name="sourceVertex">
        /// The vertex to start the traversal from.
        /// </param>
        /// <param name="targetVertex">
        /// The vertex to attempt to traverse to.
        /// </param>
        /// <returns>
        /// The <see cref="TraversalResult{TVertex}"/>. For a successful traversal, <see cref="TraversalResult{TVertex}.Success"/>
        /// will be <c>true</c> and <see cref="TraversalResult{TVertex}.Results"/> will be populated with the vertices traversed from
        /// source to target. When traversal isn't possible, <see cref="TraversalResult{TVertex}.Success"/> will be <c>false</c>.
        /// </returns>
        TraversalResult<TVertex> Traverse(IDirectedGraph<TVertex, TEdge> graph, TVertex sourceVertex, TVertex targetVertex);
    }
}