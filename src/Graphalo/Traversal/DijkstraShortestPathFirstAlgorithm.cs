using System.Collections.Generic;
using System.Linq;

namespace Graphalo.Traversal
{

    public class DijkstraShortestPathFirstAlgorithm<TVertex, TEdge> : ITraversalAlgorithm<TVertex, TEdge> where TEdge : IEdge<TVertex>
    {
        public static DijkstraShortestPathFirstAlgorithm<TVertex, TEdge> Instance { get; } = new DijkstraShortestPathFirstAlgorithm<TVertex, TEdge>();

        public TraversalResult<TVertex> Traverse(IDirectedGraph<TVertex, TEdge> graph, TVertex sourceVertex, TVertex targetVertex)
        {
            var comparer = graph.VertexComparer;
            var current = sourceVertex;
            var distance = new Dictionary<TVertex, double>(comparer)
            {
                { sourceVertex, 0D }
            };

            // Tracks the vertex that gave the shortest hop to another vertex (key: vertex, value: previous vertex)
            var previousBestHop = new Dictionary<TVertex, TVertex>(comparer);

            // The remaining vertices that haven't been considered as part of the traversal yet
            var remainingVertices = graph.AllVertices.Select(v => v.Vertex).ToHashSet();

            while (remainingVertices.Count > 0)
            {
                var next = GetNextVertex(distance, remainingVertices);
                if (next == null)
                {
                    return TraversalResult<TVertex>.Failure;
                }

                var (nextVertex, nextVertexDistance) = next.Value;

                if (comparer.Equals(nextVertex, targetVertex))
                {
                    return TraversalResult<TVertex>.Complete(BuildResultPath(comparer, sourceVertex, targetVertex, previousBestHop));
                }

                remainingVertices.Remove(nextVertex);

                var neighborEdges = graph.OutEdges(nextVertex)
                    .Where(v => remainingVertices.Contains(v.To));

                foreach (var neighborEdge in neighborEdges)
                {
                    var alt = nextVertexDistance + neighborEdge.Weight;
                    if (!distance.TryGetValue(neighborEdge.To, out var neighborDistance) || alt < neighborDistance)
                    {
                        distance[neighborEdge.To] = alt;
                        previousBestHop[neighborEdge.To] = nextVertex;
                    }
                }
            }

            // Shouldn't get here as we should either run out of edges to traverse, or we should reach the target
            return TraversalResult<TVertex>.Failure;
        }

        private static IEnumerable<TVertex> BuildResultPath(IEqualityComparer<TVertex> comparer, TVertex sourceVertex, TVertex targetVertex, Dictionary<TVertex, TVertex> previousBestHop)
        {
            var results = new Stack<TVertex>();
            results.Push(targetVertex);

            var curr = targetVertex;
            while (comparer.Equals(curr, sourceVertex) == false)
            {
                curr = previousBestHop[curr];
                results.Push(curr);
            }

            return results;
        }

        private static (TVertex nextVertex, double distance)? GetNextVertex(Dictionary<TVertex, double> distance, HashSet<TVertex> remainingVertices)
        {
            KeyValuePair<TVertex, double>? min = null;
            foreach (var vertexDistance in distance.Where(k => remainingVertices.Contains(k.Key)))
            {
                if (min == null || min.Value.Value > vertexDistance.Value)
                {
                    min = vertexDistance;
                }
            }

            if (min == null)
            {
                return null;
            }

            return (min.Value.Key, min.Value.Value);
        }
    }
}
