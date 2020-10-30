using System.Collections.Generic;

namespace Graphalo.Searching
{
    public interface IGraphSearch<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        IEnumerable<TVertex> Execute();
    }
}
