using System;

namespace Graphalo
{
    /// <summary>
    /// Event arguments relating to a vertex.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    public sealed class VertexEventArgs<TVertex> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexEventArgs&lt;TVertex&gt;"/> class.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        public VertexEventArgs(TVertex vertex)
        {
            this.Vertex = vertex;
        }

        /// <summary>
        /// Gets the vertex.
        /// </summary>
        /// <value>The vertex.</value>
        public TVertex Vertex
        {
            get;
            private set;
        }
    }
}
