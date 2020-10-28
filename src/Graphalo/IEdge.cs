namespace Graphalo
{
    /// <summary>
    /// The interface implemented by an edge in a graph.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    public interface IEdge<TVertex>
    {
        /// <summary>
        /// Gets or sets the vertex the edge comes from.
        /// </summary>
        /// <value>The from vertex.</value>
        TVertex From
        {
            get;
        }

        /// <summary>
        /// Gets the vertex the edge goes to.
        /// </summary>
        /// <value>The to vertex.</value>
        TVertex To
        {
            get;
        }
    }
}
