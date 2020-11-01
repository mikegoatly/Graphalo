namespace Graphalo.Searching
{
    /// <summary>
    /// The various kinds of supported search algorithms.
    /// </summary>
    public enum SearchKind
    {
        Unknown = 0,

        /// <summary>
        /// Search the graph returning the deepest vertices in the graph first.
        /// </summary>
        DepthFirst = 1
    }
}
