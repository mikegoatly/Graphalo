using FluentAssertions;
using FluentAssertions.Execution;
using Graphalo.Searching;
using System;
using Xunit;

namespace Graphalo.Test.Unit.Searching
{
    public class DepthFirstSearchTests
    {
        [Fact]
        public void WithEmptyGraph_ShouldReturnEmptyResults()
        {
            var graph = new DirectedGraph<string>();
            var search = GraphSearch.DepthFirst(graph);
            search.Execute().Should().BeEmpty();
        }

        [Fact]
        public void WithSingleVertex_ShouldReturnSingleVertex()
        {
            var graph = new DirectedGraph<string>();
            graph.AddVertex("A");

            ExecuteTest(graph, "A");
        }

        [Fact]
        public void WithTwoDisconnectedVertices_ShouldReturnVerticesInDiscoveredOrder()
        {
            var graph = new DirectedGraph<string>();
            graph.AddVertex("A");
            graph.AddVertex("B");

            ExecuteTest(graph, "A", "B");
        }

        [Fact]
        public void WithTwoConnectedVertices_ShouldReturnDeepestVertexFirst()
        {
            var graph = new DirectedGraph<string>();
            graph.AddEdge(new Edge<string>("A", "B"));

            ExecuteTest(graph, "B", "A");
        }

        [Fact]
        public void WithMultipleVertices_ShouldReturnDeepestVertexFirst()
        {
            //    F  G
            //   /    \
            // A-B-C-D-E
            //   \---/

            var graph = new DirectedGraph<string>();
            graph.AddEdge(new Edge<string>("A", "B"));
            graph.AddEdge(new Edge<string>("B", "C"));
            graph.AddEdge(new Edge<string>("C", "D"));
            graph.AddEdge(new Edge<string>("B", "D"));
            graph.AddEdge(new Edge<string>("D", "E"));
            graph.AddEdge(new Edge<string>("B", "F"));
            graph.AddEdge(new Edge<string>("G", "E"));

            ExecuteTest(graph, "E", "D", "C", "F", "B", "A", "G");
        }

        [Fact]
        public void WithCyclicGraph_ShouldThrowException()
        {
            var graph = new DirectedGraph<string>();
            graph.AddEdge(new Edge<string>("A", "B"));
            graph.AddEdge(new Edge<string>("B", "C"));
            graph.AddEdge(new Edge<string>("C", "A"));

            Assert.Throws<CyclicGraphsNotSupportedException>(() => ExecuteTest(graph, Array.Empty<string>()));
        }

        private void ExecuteTest(DirectedGraph<string> graph, params string[] expectedOrder)
        {
            var search = GraphSearch.DepthFirst(graph);
            search.Execute().Should().BeEquivalentTo(
                expectedOrder,
                o => o.WithStrictOrdering());
        }
    }
}
