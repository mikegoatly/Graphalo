using Graphalo.Searching;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Graphalo
{
    public class DirectedGraph<TVertex> : DirectedGraph<TVertex, Edge<TVertex>>
    {
        public DirectedGraph()
            : this(EqualityComparer<TVertex>.Default)
        {
        }

        public DirectedGraph(IEqualityComparer<TVertex> vertexComparer)
            : base(vertexComparer)
        {

        }
    }

    public class DirectedGraph<TVertex, TEdge> : IDirectedGraph<TVertex, TEdge> 
        where TEdge : IEdge<TVertex>
    {
        private readonly HashSet<TVertex> vertexLookup;

        /// <summary>
        /// The edges that come out of vertices.
        /// </summary>
        private readonly Dictionary<TVertex, IList<TEdge>> outEdges;

        /// <summary>
        /// The edges that go in to vertices.
        /// </summary>
        private readonly Dictionary<TVertex, IList<TEdge>> inEdges;

        private int edgeCount;

        public DirectedGraph()
            : this(EqualityComparer<TVertex>.Default)
        {

        }

        public DirectedGraph(IEqualityComparer<TVertex> vertexComparer)
        {
            this.VertexComparer = vertexComparer;
            this.vertexLookup = new HashSet<TVertex>(vertexComparer);
            this.outEdges = new Dictionary<TVertex, IList<TEdge>>(vertexComparer);
            this.inEdges = new Dictionary<TVertex, IList<TEdge>>(vertexComparer);
        }

        /// <inheritdoc />
        public IEnumerable<VertexInfo<TVertex, TEdge>> AllVertices => this.vertexLookup.Select(v => new VertexInfo<TVertex, TEdge>(this, v));

        /// <inheritdoc />
        public IEnumerable<TEdge> AllEdges => this.outEdges.SelectMany(i => i.Value);

        /// <inheritdoc />
        public int VertexCount => this.vertexLookup.Count;

        /// <inheritdoc />
        public int EdgeCount => this.edgeCount;

        
        public bool HasVertices => this.vertexLookup.Count > 0;

        /// <inheritdoc />
        public bool HasEdges => this.edgeCount > 0;

        /// <inheritdoc />
        public IEqualityComparer<TVertex> VertexComparer { get; }

        /// <inheritdoc />
        public event EventHandler<EdgeEventArgs<TEdge>>? EdgeAdded;

        /// <inheritdoc />
        public event EventHandler<EdgeEventArgs<TEdge>>? EdgeRemoved;

        /// <inheritdoc />
        public event EventHandler<VertexEventArgs<TVertex>>? VertexAdded;

        /// <inheritdoc />
        public event EventHandler<VertexEventArgs<TVertex>>? VertexRemoved;

        /// <inheritdoc />
        public event EventHandler? Cleared;

        /// <inheritdoc />
        public IEnumerable<TVertex> Search(SearchKind searchKind)
        {
            var searchImplementation = CreateSearchImplementation(searchKind);

            return searchImplementation.Execute();
        }

        /// <inheritdoc />
        public IEnumerable<TVertex> Search(SearchKind searchKind, TVertex startVertex)
        {
            var searchImplementation = CreateSearchImplementation(searchKind);

            return searchImplementation.Execute(startVertex);
        }

        private IGraphSearch<TVertex, TEdge> CreateSearchImplementation(SearchKind searchKind)
        {
            return searchKind switch
            {
                SearchKind.DepthFirst => new DepthFirstSearch<TVertex, TEdge>(this),
                _ => throw new ArgumentException(nameof(searchKind), string.Format(CultureInfo.InvariantCulture, Resources.UnsupportedSearchKind, searchKind))
            };
        }

        /// <inheritdoc />
        public void AddEdge(TEdge edge)
        {
            this.AddVertex(edge.From);
            this.AddVertex(edge.To);

            AddEdge(this.outEdges, edge.From, edge);
            AddEdge(this.inEdges, edge.To, edge);

            this.edgeCount++;

            this.EdgeAdded?.Invoke(this, new EdgeEventArgs<TEdge>(edge));
        }

        /// <inheritdoc />
        public void AddEdges(IEnumerable<TEdge> edges)
        {
            foreach (var edge in edges)
            {
                this.AddEdge(edge);
            }
        }

        /// <inheritdoc />
        public bool AddVertex(TVertex vertex)
        {
            var vertexAdded = this.vertexLookup.Add(vertex);
            if (vertexAdded)
            {
                this.VertexAdded?.Invoke(this, new VertexEventArgs<TVertex>(vertex));
            }

            return vertexAdded;
        }

        /// <inheritdoc />
        public bool TryGetEdges(TVertex source, TVertex target, [NotNullWhen(true)] out IEnumerable<TEdge>? edges)
        {
            if (!this.outEdges.TryGetValue(source, out var outEdgeList))
            {
                edges = null;
                return false;
            }

            if (!this.inEdges.TryGetValue(target, out var inEdgeList))
            {
                edges = null;
                return false;
            }

            var comparer = this.VertexComparer;
            if (outEdgeList.Count < inEdgeList.Count)
            {
                edges = outEdgeList.Where(e => comparer.Equals(e.To, target));
            }
            else
            {
                edges = inEdgeList.Where(e => comparer.Equals(e.From, source));
            }

            return edges.Any();
        }

        /// <inheritdoc />
        public bool RemoveVertex(TVertex vertex)
        {
            var removed = this.vertexLookup.Remove(vertex);
            if (removed)
            {
                this.ClearEdges(vertex);
                this.VertexRemoved?.Invoke(this, new VertexEventArgs<TVertex>(vertex));
            }

            return removed;
        }

        /// <inheritdoc />
        public void RemoveVertexRange(IEnumerable<TVertex> vertices)
        {
            foreach (var vertex in vertices)
            {
                this.RemoveVertex(vertex);
            }
        }

        /// <inheritdoc />
        public IReadOnlyList<TVertex> RemoveVertexWhere(Func<TVertex, bool> predicate)
        {
            var removeVertices = this.vertexLookup.Where(predicate).ToList();
            if (removeVertices.Count > 0)
            {
                this.RemoveVertexRange(removeVertices);
            }

            return removeVertices;
        }

        /// <inheritdoc />
        public void ClearEdges(TVertex vertex)
        {
            this.ClearInEdges(vertex);
            this.ClearOutEdges(vertex);
        }

        /// <inheritdoc />
        public void ClearOutEdges(TVertex vertex)
        {
            this.RemoveEdgesForVertex(vertex, this.outEdges, this.inEdges, false);
        }

        /// <inheritdoc />
        public void ClearInEdges(TVertex vertex)
        {
            this.RemoveEdgesForVertex(vertex, this.inEdges, this.outEdges, true);
        }

        /// <inheritdoc />
        public virtual void Clear()
        {
            this.inEdges.Clear();
            this.outEdges.Clear();
            this.vertexLookup.Clear();
            this.edgeCount = 0;

            this.Cleared?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        public int OutDegree(TVertex vertex)
        {
            if (this.outEdges.TryGetValue(vertex, out var edges))
            {
                return edges.Count;
            }

            return 0;
        }

        /// <inheritdoc />
        public int InDegree(TVertex vertex)
        {
            if (this.inEdges.TryGetValue(vertex, out var edges))
            {
                return edges.Count;
            }

            return 0;
        }

        /// <inheritdoc />
        public int Degree(TVertex vertex)
        {
            return this.OutDegree(vertex) + this.InDegree(vertex);
        }

        /// <inheritdoc />
        public bool ContainsEdge(TEdge edge)
        {
            return this.outEdges.ContainsKey(edge.From)
                && this.inEdges.ContainsKey(edge.To);
        }

        /// <inheritdoc />
        public bool HasInEdges(TVertex vertex)
        {
            return this.inEdges.ContainsKey(vertex);
        }

        /// <inheritdoc />
        public bool HasOutEdges(TVertex vertex)
        {
            return this.outEdges.ContainsKey(vertex);
        }

        /// <inheritdoc />
        public bool ContainsVertex(TVertex vertex)
        {
            return this.vertexLookup.Contains(vertex);
        }

        /// <inheritdoc />
        public bool RemoveEdge(TEdge edge)
        {
            if (!this.inEdges.TryGetValue(edge.To, out var inEdges))
            {
                return false;
            }

            if (!this.outEdges.TryGetValue(edge.From, out var outEdges))
            {
                return false;
            }

            if (inEdges.Remove(edge) && outEdges.Remove(edge))
            {
                this.edgeCount--;
                this.EdgeRemoved?.Invoke(this, new EdgeEventArgs<TEdge>(edge));
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public IEnumerable<TEdge> Edges(TVertex vertex)
        {
            return this.InEdges(vertex).Concat(this.OutEdges(vertex));
        }

        /// <inheritdoc />
        public IEnumerable<TEdge> InEdges(TVertex vertex)
        {
            if (this.inEdges.TryGetValue(vertex, out var edges))
            {
                return edges;
            }

            return Enumerable.Empty<TEdge>();
        }

        /// <inheritdoc />
        public IEnumerable<TEdge> OutEdges(TVertex vertex)
        {
            if (this.outEdges.TryGetValue(vertex, out var edges))
            {
                return edges;
            }

            return Enumerable.Empty<TEdge>();
        }

        /// <inheritdoc />
        public void RemoveEdgeRange(IEnumerable<TEdge> edges)
        {
            foreach (var edge in edges)
            {
                this.RemoveEdge(edge);
            }
        }

        /// <inheritdoc />
        public IReadOnlyList<TEdge> RemoveEdgeWhere(Func<TEdge, bool> predicate)
        {
            var removeEdges = this.AllEdges.Where(predicate).ToList();
            this.RemoveEdgeRange(removeEdges);
            return removeEdges;
        }

        private static void AddEdge(Dictionary<TVertex, IList<TEdge>> edgeDictionary, TVertex vertex, TEdge edge)
        {
            if (!edgeDictionary.TryGetValue(vertex, out var edges))
            {
                edges = new List<TEdge>();
                edgeDictionary.Add(vertex, edges);
            }

            edges.Add(edge);
        }

        /// <summary>
        /// Removes the edges for the given vertex, making sure that the edge is removed in the opposite direction as well.
        /// </summary>
        /// <param name="vertex">The vertex to remove.</param>
        /// <param name="edges1">The first lookup.</param>
        /// <param name="edges2">The second lookup.</param>
        /// <param name="reversed">if set to <c>true</c> the lookups will be reversed, i.e. edges2 contains the from vertices.</param>
        private void RemoveEdgesForVertex(
            TVertex vertex,
            Dictionary<TVertex, IList<TEdge>> edges1,
            Dictionary<TVertex, IList<TEdge>> edges2,
            bool reversed)
        {
            // Get the edges for the vertex from the first lookup - these need to be removed
            // for the linked vertex in the second lookup
            if (edges1.TryGetValue(vertex, out var removedEdges))
            {
                foreach (var edge in removedEdges)
                {
                    var reverseEdges = edges2[reversed ? edge.From : edge.To];
                    reverseEdges.Remove(edge);
                    this.edgeCount--;
                    this.EdgeRemoved?.Invoke(this, new EdgeEventArgs<TEdge>(edge));
                }

                edges1.Remove(vertex);
            }
        }
    }
}
