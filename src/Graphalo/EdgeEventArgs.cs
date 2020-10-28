using System;

namespace Graphalo
{
    /// <summary>
    /// An event involving an edge.
    /// </summary>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public sealed class EdgeEventArgs<TEdge> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeEventArgs&lt;TEdge&gt;"/> class.
        /// </summary>
        /// <param name="edge">The edge associated to the event.</param>
        public EdgeEventArgs(TEdge edge)
        {
            this.Edge = edge;
        }

        /// <summary>
        /// Gets the edge associated to the event.
        /// </summary>
        /// <value>The associated edge.</value>
        public TEdge Edge
        {
            get;
            private set;
        }
    }
}
