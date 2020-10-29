using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace Graphalo.Test.Unit
{
    public partial class DirectedGraphTests
    {
        public class AddingEdges
        {
            private DirectedGraph<string> sut;

            public AddingEdges()
            {
                this.sut = CreateTestGraph();
            }

            [Fact]
            public void ShouldIncreaseEdgeCount()
            {
                this.sut.AddEdge(new Edge<string>("A", "B"));
                this.sut.EdgeCount.Should().Be(1);
                this.sut.AddEdge(new Edge<string>("B", "C"));
                this.sut.EdgeCount.Should().Be(2);
            }

            [Fact]
            public void ShouldMakeEdgesAvailableFromEdgesProperty()
            {
                var aToB = new Edge<string>("A", "B");
                var bToC = new Edge<string>("B", "C");
                var bToA = new Edge<string>("B", "A");

                this.sut.AddEdges(new[] { aToB, bToC, bToA });

                this.sut.AllEdges.Should().BeEquivalentTo(aToB, bToC, bToA);
            }

            [Fact]
            public void AddingDuplicateEdge_ShouldIncreaseEdgeCount()
            {
                this.sut.AddEdge(new Edge<string>("A", "B"));
                this.sut.EdgeCount.Should().Be(1);
                this.sut.AddEdge(new Edge<string>("A", "B"));
                this.sut.EdgeCount.Should().Be(2);

                this.sut.HasInEdges("B").Should().BeTrue();
                this.sut.HasOutEdges("A").Should().BeTrue();
            }

            [Fact]
            public void ShouldRaiseEdgeAddedEvent()
            {
                var addedEdges = new List<Edge<string>>();
                this.sut.EdgeAdded += (s, e) => addedEdges.Add(e.Edge);

                var edge = new Edge<string>("A", "B");
                this.sut.AddEdge(edge);

                addedEdges.Should().BeEquivalentTo(edge);
            }

            [Fact]
            public void ShouldAutomaticallyAddMissingVertices()
            {
                this.sut.AddVertex("A");
                this.sut.AddEdge(new Edge<string>("A", "B"));
                this.sut.AllVertices
                    .OrderByDescending(v => v.InDegree)
                    .Select(v => v.Vertex)
                    .Should().BeEquivalentTo(new[] { "B", "A" }, o => o.WithStrictOrdering());
            }

            [Fact]
            public void ShouldIncreaseDegreeCountOfVertices()
            {
                this.sut.AddEdge(new Edge<string>("A", "B"));
                this.sut.AddEdge(new Edge<string>("B", "C"));
                this.sut.AddEdge(new Edge<string>("B", "D"));

                this.VerifyDegrees("A", 1, 0, 1);
                this.VerifyDegrees("B", 3, 1, 2);
                this.VerifyDegrees("C", 1, 1, 0);
                this.VerifyDegrees("D", 1, 1, 0);
            }

            [Fact]
            public void ShouldBeAbleToRetrieveAddedEdgesBetweenVertices()
            {
                var aToB = new Edge<string>("A", "B");
                var bToC = new Edge<string>("B", "C");
                var bToA = new Edge<string>("B", "A");
                var aToB2 = new Edge<string>("A", "B");
                var dToA = new Edge<string>("D", "A");
                var eToA = new Edge<string>("E", "A");
                this.sut.AddEdges(new[] { aToB, bToC, bToA, aToB2, dToA, eToA });

                this.sut.TryGetEdges("A", "B", out var edges).Should().BeTrue();
                edges.Should().BeEquivalentTo(aToB, aToB2);

                this.sut.TryGetEdges("B", "A", out edges).Should().BeTrue();
                edges.Should().BeEquivalentTo(bToA);

                this.sut.TryGetEdges("A", "C", out _).Should().BeFalse();
            }

            protected void VerifyDegrees(string vertex, int totalDegree, int inDegree, int outDegree)
            {
                this.sut.Degree(vertex).Should().Be(totalDegree, $"Vertex {vertex} should have {totalDegree} degree");
                this.sut.InDegree(vertex).Should().Be(inDegree, $"Vertex {vertex} should have {inDegree} in degree");
                this.sut.OutDegree(vertex).Should().Be(outDegree, $"Vertex {vertex} should have {outDegree} out degree");
            }
        }
    }
}
