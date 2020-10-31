using FluentAssertions;
using FluentAssertions.Execution;
using Graphalo.Searching;
using System;
using System.Linq;
using Xunit;

namespace Graphalo.Test.Unit.Searching
{
    public class DepthFirstSearchTests
    {
        [Fact]
        public void WithEmptyGraph_ShouldReturnEmptyResults()
        {
            var graph = new DirectedGraph<string>();
            graph.Search(SearchKind.DepthFirst).Should().BeEmpty();
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
            var graph = GraphWithMultipleVertices();

            ExecuteTest(graph, "E", "D", "C", "F", "B", "Z", "G");
        }

        [Fact]
        public void WithVertexChain_OrderVerticesAddedShouldMakeNoDifferenceToResult()
        {
            var graph1 = new DirectedGraph<string>();
            graph1.AddEdge(new Edge<string>("A", "B"));
            graph1.AddEdge(new Edge<string>("B", "C"));
            graph1.AddEdge(new Edge<string>("C", "D"));

            var graph2 = new DirectedGraph<string>();
            graph2.AddEdge(new Edge<string>("B", "C"));
            graph2.AddEdge(new Edge<string>("A", "B"));
            graph2.AddEdge(new Edge<string>("C", "D"));

            var graph3 = new DirectedGraph<string>();
            graph3.AddEdge(new Edge<string>("C", "D"));
            graph3.AddEdge(new Edge<string>("B", "C"));
            graph3.AddEdge(new Edge<string>("A", "B"));

            var results1 = graph1.Search(SearchKind.DepthFirst);
            var results2 = graph2.Search(SearchKind.DepthFirst);
            var results3 = graph2.Search(SearchKind.DepthFirst);

            results1.Should().BeEquivalentTo(results2.ToList(), o => o.WithStrictOrdering());
            results2.Should().BeEquivalentTo(results3.ToList(), o => o.WithStrictOrdering());
        }

        [Fact]
        public void WithMultipleVertices_SearchingFromSpecificStartVertex_ShouldOnlyReturnReachableVertices()
        {
            var graph = GraphWithMultipleVertices();

            graph.Search(SearchKind.DepthFirst, "C")
                .Should().BeEquivalentTo(
                    new[] { "E", "D", "C" },
                    o => o.WithStrictOrdering());
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

        private static DirectedGraph<string> GraphWithMultipleVertices()
        {
            //    F  G
            //   /    \
            // Z-B-C-D-E
            //   \---/
            var graph = new DirectedGraph<string>();
            graph.AddEdge(new Edge<string>("B", "C"));
            graph.AddEdge(new Edge<string>("Z", "B"));
            graph.AddEdge(new Edge<string>("C", "D"));
            graph.AddEdge(new Edge<string>("B", "D"));
            graph.AddEdge(new Edge<string>("D", "E"));
            graph.AddEdge(new Edge<string>("B", "F"));
            graph.AddEdge(new Edge<string>("G", "E"));
            return graph;
        }

        private static void ExecuteTest(DirectedGraph<string> graph, params string[] expectedOrder)
        {
            graph.Search(SearchKind.DepthFirst)
                .Should().BeEquivalentTo(
                    expectedOrder,
                    o => o.WithStrictOrdering());
        }
    }
}
