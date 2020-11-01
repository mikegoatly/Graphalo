namespace Graphalo.Traversal
{
    /// <summary>
    /// The various kinds of supported traversal algorithm.
    /// </summary>
    public enum TraversalKind
    {
        Unknown = 0,

        /// <summary>
        /// Traverse the graph using Djikstra's algorithm. 
        /// </summary>
        Dijkstra = 1
    }
}