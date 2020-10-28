namespace Graphalo
{
    public class Edge<TVertex> : IEdge<TVertex>
    {
        public Edge(TVertex from, TVertex to)
        {
            From = from;
            To = to;
        }

        /// <inheritdoc />
        public TVertex From { get; }

        /// <inheritdoc />
        public TVertex To { get; }
    }
}
