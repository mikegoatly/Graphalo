using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Graphalo.Test.Unit
{
    public partial class DirectedGraphTests
    {
        public class RemovingEdges
        {
            private readonly DirectedGraph<string> sut;
            private readonly Edge<string> edge1;
            private readonly Edge<string> edge2;
            private readonly Edge<string> edge3;

            public RemovingEdges()
            {
                this.sut = CreateTestGraph();
                this.edge1 = new Edge<string>("A", "B");
                this.edge2 = new Edge<string>("B", "C");
                this.edge3 = new Edge<string>("C", "D");
                this.sut.AddEdges(new[] { this.edge1, this.edge2, this.edge3 });
            }

            [Fact]
            public void ShouldReduceEdgeCount()
            {
                this.sut.RemoveEdge(this.edge2);

                this.sut.AllEdges.Should().BeEquivalentTo(this.edge1, this.edge3);
            }

            [Fact]
            public void ShouldReturnTrueIfEdgeRemoved()
            {
                this.sut.RemoveEdge(this.edge2).Should().BeTrue();
            }

            [Fact]
            public void ShouldReturnFalseIfEdgeNotInGraph()
            {
                this.sut.RemoveEdge(new Edge<string>("D", "B")).Should().BeFalse();
            }

            [Fact]
            public void ShouldReturnFalseIfEdgeReferencesVertexNotInGraph()
            {
                this.sut.RemoveEdge(new Edge<string>("D", "F")).Should().BeFalse();
                this.sut.RemoveEdge(new Edge<string>("F", "B")).Should().BeFalse();
            }

            [Fact]
            public void ShouldReduceDegreeCountOfAffectedVertices()
            {
                this.sut.OutDegree("B").Should().Be(1);
                this.sut.InDegree("C").Should().Be(1);

                this.sut.RemoveEdge(this.edge2);

                this.sut.OutDegree("B").Should().Be(0);
                this.sut.InDegree("C").Should().Be(0);
            }

            [Fact]
            public void ShouldRaiseEdgeRemovedEvent()
            {
                var removedEdges = new List<Edge<string>>();
                this.sut.EdgeRemoved += (s, e) => removedEdges.Add(e.Edge);
                this.sut.RemoveEdge(this.edge2);

                removedEdges.Should().BeEquivalentTo(this.edge2);
            }

            [Fact]
            public void RemovingEdgesWithPredicate_ShouldRemoveAffectedEdges()
            {
                this.sut.RemoveEdgeWhere(e => e.From[0] > 'A').Should().BeEquivalentTo(this.edge2, this.edge3);

                this.sut.EdgeCount.Should().Be(1);

                this.sut.AllEdges.Should().BeEquivalentTo(this.edge1);
            }
        }
    }
}
