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
        public class RemovingVertices
        {
            private readonly DirectedGraph<string> sut;
            private readonly Edge<string> edge1;
            private readonly Edge<string> edge2;
            private readonly Edge<string> edge3;

            public RemovingVertices()
            {
                this.sut = CreateTestGraph();
                this.edge1 = new Edge<string>("A", "B");
                this.edge2 = new Edge<string>("B", "C");
                this.edge3 = new Edge<string>("C", "D");
                this.sut.AddEdges(new[] { this.edge1, this.edge2, this.edge3 });
            }

            [Fact]
            public void ShouldReturnTrueWhenVertexWasRemoved()
            {
                this.sut.RemoveVertex("B").Should().BeTrue();
            }

            [Fact]
            public void ShouldRaiseVertexRemovedEvent()
            {
                var removedVertices = new List<string>();
                this.sut.VertexRemoved += (s, e) => removedVertices.Add(e.Vertex);

                this.sut.RemoveVertex("B").Should().BeTrue();

                removedVertices.Should().BeEquivalentTo("B");
            }

            [Fact]
            public void ShouldReturnFalseWhenVertexWasNotInGraph()
            {
                this.sut.RemoveVertex("Z").Should().BeFalse();
            }

            [Fact]
            public void ShouldRemoveAssociatedEdges()
            {
                this.sut.EdgeCount.Should().Be(3);

                this.sut.RemoveVertex("B");

                this.sut.EdgeCount.Should().Be(1);
            }

            [Fact]
            public void ShouldReduceVertexCount()
            {
                this.sut.VertexCount.Should().Be(4);

                this.sut.RemoveVertex("B");

                this.sut.VertexCount.Should().Be(3);
            }

            [Fact]
            public void RemoveWhere_ShouldRemoveMatchingVertices()
            {
                this.sut.RemoveVertexWhere(v => v[0] > 'B').Should().BeEquivalentTo("C", "D");

                this.sut.VertexCount.Should().Be(2);
            }
        }
    }
}
