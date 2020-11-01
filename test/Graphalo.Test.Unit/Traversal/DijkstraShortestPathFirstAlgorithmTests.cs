using FluentAssertions;
using Graphalo.Traversal;
using Xunit;

namespace Graphalo.Test.Unit.Traversal
{
    public class DijkstraShortestPathFirstAlgorithmTests
    {
        [Fact]
        public void TraversingFromSameSourceAndTarget_ShouldReturnSingleVertex()
        {
            var graph = new DirectedGraph<string>();
            graph.AddVertex("A");

            this.RunSuccessfulTest(graph, "A", "A", "A");
        }

        [Fact]
        public void TraversingBetweenTwoDirectlyConnectedVertices_ShouldReturnVerticesInExpectedOrder()
        {
            var graph = new DirectedGraph<string>();
            graph.AddEdge(new Edge<string>("A", "B"));

            this.RunSuccessfulTest(graph, "A", "B", "A", "B");
        }

        [Fact]
        public void ForDiamondGraph_ShouldReturnShortestPath()
        {
            var graph = new DirectedGraph<string>();
            graph.AddEdge(new Edge<string>("Start", "B", 2D));
            graph.AddEdge(new Edge<string>("B", "C", 3D));
            graph.AddEdge(new Edge<string>("C", "End", 1D));
            graph.AddEdge(new Edge<string>("Start", "E", 1D));
            graph.AddEdge(new Edge<string>("E", "F", 2D));
            graph.AddEdge(new Edge<string>("F", "End", 1D));

            // Start -2> B -3> C -1> End == 6
            // Start -1> E -2> F -1> End == 4

            this.RunSuccessfulTest(graph, "Start", "End", "Start", "E", "F", "End");
        }

        [Fact]
        public void WithCycleAndNoReachablePath_ShouldReturnUnsuccessfulTraversal()
        {
            var graph = new DirectedGraph<string>();
            graph.AddEdge(new Edge<string>("A", "B", 2D));
            graph.AddEdge(new Edge<string>("B", "A", 3D));
            graph.AddVertex("C");

            this.RunUnSuccessfulTest(graph, "A", "C");
        }

        [Fact]
        public void WithCycleAndReachablePath_ShouldReturnReachablePath()
        {
            var graph = new DirectedGraph<string>();
            graph.AddEdge(new Edge<string>("A", "B"));
            graph.AddEdge(new Edge<string>("B", "A"));
            graph.AddEdge(new Edge<string>("B", "C"));
            graph.AddEdge(new Edge<string>("B", "D"));
            graph.AddEdge(new Edge<string>("D", "E"));

            this.RunSuccessfulTest(graph, "A", "E", "A", "B", "D", "E");
        }

        [Fact]
        public void WhenTargetVertexNotReachable_ShouldReturnUnsuccessfulTraversal()
        {
            var graph = new DirectedGraph<string>();
            graph.AddEdge(new Edge<string>("A", "B"));
            graph.AddVertex("C");

            this.RunUnSuccessfulTest(graph, "A", "C");
        }

        private void RunUnSuccessfulTest(DirectedGraph<string> graph, string source, string target)
        {
            var result = graph.Traverse(TraversalKind.Dijkstra, source, target);
            result.Success.Should().BeFalse();
        }

        private void RunSuccessfulTest(DirectedGraph<string> graph, string source, string target, params string[] expectedVertices)
        {
            var result = graph.Traverse(TraversalKind.Dijkstra, source, target);
            result.Success.Should().BeTrue();
            result.Results.Should().BeEquivalentTo(expectedVertices, o => o.WithStrictOrdering());
        }
    }
}
