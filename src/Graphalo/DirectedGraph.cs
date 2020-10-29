using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        private HashSet<TVertex> vertexLookup;

        /// <summary>
        /// The edges that come out of vertices.
        /// </summary>
        private Dictionary<TVertex, IList<TEdge>> outEdges;

        /// <summary>
        /// The edges that go in to vertices.
        /// </summary>
        private Dictionary<TVertex, IList<TEdge>> inEdges;

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

        /// <summary>
        /// Gets the vertices in this graph.
        /// </summary>
        /// <value>The vertices in this graph.</value>
        public IEnumerable<VertexInfo<TVertex, TEdge>> AllVertices => this.vertexLookup.Select(v => new VertexInfo<TVertex, TEdge>(this, v));

        /// <summary>
        /// Gets the edges in the graph.
        /// </summary>
        /// <value>The edges in the graph.</value>
        public IEnumerable<TEdge> AllEdges => this.outEdges.SelectMany(i => i.Value);

        /// <summary>
        /// Gets the number of vertices in the graph.
        /// </summary
        public int VertexCount => this.vertexLookup.Count;

        /// <summary>
        /// Gets the number of edges in the graph.
        /// </summary>
        public int EdgeCount => this.edgeCount;

        /// <summary>
        /// Gets a value indicating whether there are vertices in this instance.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance contains vertices; otherwise, <c>false</c>.
        /// </value>
        public bool HasVertices => this.vertexLookup.Count > 0;

        /// <summary>
        /// Gets a value indicating whether there are edges in this instance.
        /// </summary>
        /// <value><c>true</c> if this instance has edges; otherwise, <c>false</c>.</value>
        public bool HasEdges => this.edgeCount > 0;

        /// <summary>
        /// Gets the comparer capable of comparing two vertices for equality.
        /// </summary>
        /// <value>The vertex comparer.</value>
        public IEqualityComparer<TVertex> VertexComparer { get; }

        /// <summary>
        /// Occurs when an edge is added to the graph.
        /// </summary>
        public event EventHandler<EdgeEventArgs<TEdge>> EdgeAdded;

        /// <summary>
        /// Occurs when an edge is removed from the graph.
        /// </summary>
        public event EventHandler<EdgeEventArgs<TEdge>> EdgeRemoved;

        /// <summary>
        /// Occurs when a vertex is added to the graph
        /// </summary>
        public event EventHandler<VertexEventArgs<TVertex>> VertexAdded;

        /// <summary>
        /// Occurs when a vertex is removed from the graph
        /// </summary>
        public event EventHandler<VertexEventArgs<TVertex>> VertexRemoved;

        /// <summary>
        /// Occurs when the graph is cleared. Individual events for VertexRemoved and EdgeRemoved will not be called.
        /// </summary>
        public event EventHandler Cleared;

        /// <summary>
        /// Adds an edge to the graph. The referenced vertices will automatically
        /// be added to the graph if they don't already exist in it.
        /// </summary>
        /// <param name="edge">
        /// The edge to add to the graph.
        /// </param>
        public void AddEdge(TEdge edge)
        {
            this.AddVertex(edge.From);
            this.AddVertex(edge.To);

            AddEdge(this.outEdges, edge.From, edge);
            AddEdge(this.inEdges, edge.To, edge);

            this.edgeCount++;

            this.EdgeAdded?.Invoke(this, new EdgeEventArgs<TEdge>(edge));
        }

        /// <summary>
        /// Adds the given edges to the graph. Any referenced vertices will automatically
        /// be added to the graph if they don't already exist in it.
        /// </summary>
        /// <param name="edges">
        /// The edge to add to the graph.
        /// </param>
        public void AddEdges(IEnumerable<TEdge> edges)
        {
            foreach (var edge in edges)
            {
                this.AddEdge(edge);
            }
        }

        /// <summary>
        /// Adds a vertex to the graph.
        /// </summary>
        /// <param name="vertex">The vertex to add.</param>
        /// <returns><c>true</c> if the vertex was added, <c>false</c> if it already existed in the graph.</returns>
        public bool AddVertex(TVertex vertex)
        {
            var vertexAdded = this.vertexLookup.Add(vertex);
            if (vertexAdded)
            {
                this.VertexAdded?.Invoke(this, new VertexEventArgs<TVertex>(vertex));
            }

            return vertexAdded;
        }

        /// <summary>
        /// Tries to get the edges between the given vertices.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <c>false</c> to only consider edges from <paramref name="source"/> to <paramref name="target"/></param>
        /// <param name="edges">Returns the matched edges.</param>
        /// <returns><c>true</c> if the edges could be retrieved, otherwise <c>false</c></returns>
        public bool TryGetEdges(TVertex source, TVertex target, [NotNullWhen(true)] out IEnumerable<TEdge>? edges)
        {
            if (!this.outEdges.TryGetValue(source, out IList<TEdge> outEdgeList))
            {
                edges = null;
                return false;
            }

            if (!this.inEdges.TryGetValue(target, out IList<TEdge> inEdgeList))
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

        /// <summary>
        /// Removes the vertex and any associated edges from the graph.
        /// </summary>
        /// <param name="vertex">The vertex to remove.</param>
        /// <returns>
        /// <c>true</c> if the vertex could be removed, otherwise <c>false</c>
        /// </returns>
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

        /// <summary>
        /// Removes the vertices and any associated edges from the graph.
        /// </summary>
        /// <param name="vertices">The vertices to remove.</param>
        public void RemoveVertexRange(IEnumerable<TVertex> vertices)
        {
            foreach (var vertex in vertices)
            {
                this.RemoveVertex(vertex);
            }
        }

        /// <summary>
        /// Removes vertices that match the given predicate.
        /// </summary>
        /// <param name="predicate">The predicate to match.</param>
        /// <returns>
        /// The number of removed vertices.
        /// </returns>
        public IReadOnlyList<TVertex> RemoveVertexWhere(Func<TVertex, bool> predicate)
        {
            var removeVertices = this.vertexLookup.Where(predicate).ToList();
            if (removeVertices.Count > 0)
            {
                this.RemoveVertexRange(removeVertices);
            }

            return removeVertices;
        }

        /// <summary>
        /// Clears the in-edges and out-edges of <paramref name="vertex"/>
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        public void ClearEdges(TVertex vertex)
        {
            this.ClearInEdges(vertex);
            this.ClearOutEdges(vertex);
        }

        /// <summary>
        /// Clears the out edges for the given vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        public void ClearOutEdges(TVertex vertex)
        {
            this.RemoveEdgesForVertex(vertex, this.outEdges, this.inEdges, false);
        }

        /// <summary>
        /// Clears the in-edges of <paramref name="vertex"/>
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        public void ClearInEdges(TVertex vertex)
        {
            this.RemoveEdgesForVertex(vertex, this.inEdges, this.outEdges, true);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public virtual void Clear()
        {
            this.inEdges.Clear();
            this.outEdges.Clear();
            this.vertexLookup.Clear();
            this.edgeCount = 0;

            this.Cleared?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets the count of out-edges of <paramref name="vertex"/>
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>
        /// The count of out-edges of <paramref name="vertex"/>
        /// </returns>
        public int OutDegree(TVertex vertex)
        {
            if (this.outEdges.TryGetValue(vertex, out IList<TEdge> edges))
            {
                return edges.Count;
            }

            return 0;
        }

        /// <summary>
        /// Gets the number of in-edges of <paramref name="vertex"/>
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>
        /// The number of in-edges pointing towards <paramref name="vertex"/>
        /// </returns>
        public int InDegree(TVertex vertex)
        {
            if (this.inEdges.TryGetValue(vertex, out IList<TEdge> edges))
            {
                return edges.Count;
            }

            return 0;
        }

        /// <summary>
        /// Gets the degree of <paramref name="vertex"/>, i.e.
        /// the sum of the out-degree and in-degree of <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>
        /// The sum of <see cref="OutDegree"/> and <see cref="InDegree"/> of <paramref name="vertex"/>
        /// </returns>
        public int Degree(TVertex vertex)
        {
            return this.OutDegree(vertex) + this.InDegree(vertex);
        }

        /// <summary>
        /// Determines whether this instance contains the specified edge.
        /// </summary>
        /// <param name="edge">The edge to check for.</param>
        /// <returns>
        /// <c>true</c> if this instance contains the specified edge; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsEdge(TEdge edge)
        {
            return this.outEdges.ContainsKey(edge.From)
                && this.inEdges.ContainsKey(edge.To);
        }

        /// <summary>
        /// Determines whether the given vertex has edges going in to it.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>
        /// <c>true</c> if the given vertex has edges going in to it; otherwise, <c>false</c>.
        /// </returns>
        public bool HasInEdges(TVertex vertex)
        {
            return this.inEdges.ContainsKey(vertex);
        }

        /// <summary>
        /// Determines whether the given vertex has edges coming out from it.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>
        /// <c>true</c> if the given vertex has edges coming out from it; otherwise, <c>false</c>.
        /// </returns>
        public bool HasOutEdges(TVertex vertex)
        {
            return this.outEdges.ContainsKey(vertex);
        }

        /// <summary>
        /// Determines whether this instance contains the given vertex.
        /// </summary>
        /// <param name="vertex">The vertex to check for.</param>
        /// <returns>
        /// <c>true</c> if this instance containst the vertex; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsVertex(TVertex vertex)
        {
            return this.vertexLookup.Contains(vertex);
        }

        /// <summary>
        /// Removes the edge from the graph.
        /// </summary>
        /// <param name="edge">The edge to remove.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="edge"/> was successfully removed; otherwise <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Gets the edges going in or out of the given vertex.
        /// </summary>
        /// <param name="vertex">The vertex to get the edges for.</param>
        /// <returns>The edges that start or end at the given vertex.</returns>
        public IEnumerable<TEdge> Edges(TVertex vertex)
        {
            return InEdges(vertex).Concat(OutEdges(vertex));
        }

        /// <summary>
        /// Gets the edges going in to the given vertex.
        /// </summary>
        /// <param name="vertex">The vertex to get the in edges for.</param>
        /// <returns>The edges that end at the given vertex.</returns>
        public IEnumerable<TEdge> InEdges(TVertex vertex)
        {
            if (this.inEdges.TryGetValue(vertex, out IList<TEdge> edges))
            {
                return edges;
            }

            return Enumerable.Empty<TEdge>();
        }

        /// <summary>
        /// Gets the edges coming out of the given vertex.
        /// </summary>
        /// <param name="vertex">The vertex to get the out edges for.</param>
        /// <returns>The edges that start at the given vertex.</returns>
        public IEnumerable<TEdge> OutEdges(TVertex vertex)
        {
            if (this.outEdges.TryGetValue(vertex, out IList<TEdge> edges))
            {
                return edges;
            }

            return Enumerable.Empty<TEdge>();
        }

        /// <summary>
        /// Removes the given edges from the graph.
        /// </summary>
        /// <param name="edges">The edges to remove.</param>
        public void RemoveEdgeRange(IEnumerable<TEdge> edges)
        {
            foreach (TEdge edge in edges)
            {
                this.RemoveEdge(edge);
            }
        }

        /// <summary>
        /// Removes edges where the given predicate is true.
        /// </summary>
        /// <param name="predicate">The predicate to test.</param>
        /// <returns>The removed edges.</returns>
        public IReadOnlyList<TEdge> RemoveEdgeWhere(Func<TEdge, bool> predicate)
        {
            var removeEdges = this.AllEdges.Where(predicate).ToList();
            this.RemoveEdgeRange(removeEdges);
            return removeEdges;
        }

        private static void AddEdge(Dictionary<TVertex, IList<TEdge>> edgeDictionary, TVertex vertex, TEdge edge)
        {
            if (!edgeDictionary.TryGetValue(vertex, out IList<TEdge> edges))
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
            if (edges1.TryGetValue(vertex, out IList<TEdge> removedEdges))
            {
                foreach (TEdge edge in removedEdges)
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
