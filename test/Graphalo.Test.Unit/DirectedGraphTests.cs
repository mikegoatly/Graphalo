using FluentAssertions;

using System;
using System.Collections.Generic;

using Xunit;

namespace Graphalo.Test.Unit
{
    public partial class DirectedGraphTests
    {
        [Fact]
        public void EmptyGraph_HasNoVerticesOrEdges()
        {
            var graph = CreateTestGraph();
            VerifyEmptyGraph(graph);
        }

        private static void VerifyEmptyGraph(DirectedGraph<string> graph)
        {
            graph.EdgeCount.Should().Be(0);
            graph.VertexCount.Should().Be(0);
            graph.AllEdges.Should().BeEmpty();
            graph.AllVertices.Should().BeEmpty();
            graph.HasEdges.Should().BeFalse();
            graph.HasVertices.Should().BeFalse();
            graph.OutEdges("A").Should().BeEmpty();
            graph.InEdges("A").Should().BeEmpty();

            graph.TryGetEdges("A", "B", out var edges).Should().BeFalse();
            edges.Should().BeNull();
        }

        [Fact]
        public void ClearingGraph_ClearsAllData()
        {
            var graph = CreateTestGraph();
            graph.AddEdge(new Edge<string>("A", "B"));
            graph.AddEdge(new Edge<string>("B", "C"));
            graph.Clear();
            VerifyEmptyGraph(graph);
        }

        [Fact]
        public void ClearingGraph_RaisesGraphClearedEvent()
        {
            var eventFired = false;

            var graph = CreateTestGraph();
            graph.AddEdge(new Edge<string>("A", "B"));
            graph.AddEdge(new Edge<string>("B", "C"));

            graph.Cleared += (s, e) => eventFired = true;
            graph.Clear();
            eventFired.Should().BeTrue();
        }

        [Fact]
        public void ContainsVertex_ShouldReturnTrueIfVertexInGraph()
        {
            var graph = CreateTestGraph();
            graph.AddVertex("A");
            graph.ContainsVertex("A").Should().BeTrue();
        }

        [Fact]
        public void ContainsVertex_ShouldUseCustomComparer()
        {
            var graph = new DirectedGraph<string>(StringComparer.OrdinalIgnoreCase);
            graph.AddVertex("A");
            graph.ContainsVertex("a").Should().BeTrue();
        }

        [Fact]
        public void ContainsVertex_ShouldReturnFalseIfGraphDoesntContainVertex()
        {
            var graph = CreateTestGraph();
            graph.AddVertex("A");
            graph.ContainsVertex("B").Should().BeFalse();
        }

        [Fact]
        public void ClearingOutEdges_ClearsOutEdgesForVertex()
        {
            var graph = CreateTestGraph();
            graph.AddEdge(new Edge<string>("A", "B"));
            graph.AddEdge(new Edge<string>("B", "C"));
            graph.AddEdge(new Edge<string>("A", "C"));
            graph.OutEdges("A").Should().HaveCount(2);
            graph.ClearOutEdges("A");
            graph.OutEdges("A").Should().HaveCount(0);
            graph.EdgeCount.Should().Be(1);
        }

        [Fact]
        public void ClearingOutEdges_ShouldRaiseVertexRemoved()
        {
            var graph = CreateTestGraph();
            var aToB = new Edge<string>("A", "B");
            var bToC = new Edge<string>("B", "C");
            var aToC = new Edge<string>("A", "C");
            graph.AddEdge(aToB);
            graph.AddEdge(bToC);
            graph.AddEdge(aToC);
            var removedEdges = new List<Edge<string>>();
            graph.EdgeRemoved += (s, e) => removedEdges.Add(e.Edge);
            graph.ClearOutEdges("A");

            removedEdges.Should().BeEquivalentTo(aToB, aToC);
        }

        [Fact]
        public void ClearingInEdges_ClearsInEdgesForVertex()
        {
            var graph = CreateTestGraph();
            graph.AddEdge(new Edge<string>("A", "B"));
            graph.AddEdge(new Edge<string>("B", "C"));
            graph.AddEdge(new Edge<string>("A", "C"));
            graph.InEdges("C").Should().HaveCount(2);
            graph.ClearInEdges("C");
            graph.InEdges("C").Should().HaveCount(0);
            graph.EdgeCount.Should().Be(1);
        }

        [Fact]
        public void ClearingEdges_ClearsAllEdgesForVertex()
        {
            var graph = CreateTestGraph();
            graph.AddEdge(new Edge<string>("A", "B"));
            graph.AddEdge(new Edge<string>("B", "C"));
            graph.AddEdge(new Edge<string>("A", "C"));
            graph.Edges("B").Should().HaveCount(2);
            graph.ClearEdges("B");
            graph.Edges("B").Should().HaveCount(0);
            graph.EdgeCount.Should().Be(1);
        }

        [Fact]
        public void HasOutEdges_ShouldReturnFalseIfRelatedEdgesHaveBeenRemoved()
        {
            var graph = CreateTestGraph();
            graph.AddEdge(new Edge<string>("A", "B"));

            var removed = graph.RemoveVertexWhere(v => graph.HasOutEdges(v) == false);

            removed.Should().BeEquivalentTo(["B"]);

            removed = graph.RemoveVertexWhere(v => graph.HasOutEdges(v) == false);

            removed.Should().BeEquivalentTo(["A"]);
        }

        [Fact]
        public void TryGetVertexInfo_ShouldReturnTrueIfVertexExists()
        {
            var graph = CreateTestGraph();
            graph.AddVertex("A");
            graph.TryGetVertexInfo("A", out var vertexInfo).Should().BeTrue();
            vertexInfo.Vertex.Should().Be("A");
        }

        [Fact]
        public void TryGetVertexInfo_ShouldReturnFalseIfVertexDoesntExist()
        {
            var graph = CreateTestGraph();
            graph.AddVertex("B");
            graph.TryGetVertexInfo("A", out var vertexInfo).Should().BeFalse();
            vertexInfo.Should().BeEquivalentTo(default(VertexInfo<string, Edge<string>>));
        }

        private static DirectedGraph<string> CreateTestGraph() => new DirectedGraph<string>();
    }
}
