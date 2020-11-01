namespace Graphalo
{
    public class Edge<TVertex> : IEdge<TVertex>
    {
        public Edge(TVertex from, TVertex to, double weight = 1D)
        {
            From = from;
            To = to;
            this.Weight = weight;
        }

        /// <inheritdoc />
        public TVertex From { get; }

        /// <inheritdoc />
        public TVertex To { get; }
        
        /// <inheritdoc />
        public double Weight { get; }
    }
}
