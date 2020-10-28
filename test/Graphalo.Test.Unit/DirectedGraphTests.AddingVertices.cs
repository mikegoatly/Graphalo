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
        public class AddingVertices
        {
            private DirectedGraph<string> sut;

            public AddingVertices()
            {
                this.sut = CreateTestGraph();
            }

            [Fact]
            public void ShouldIncreaseVertexCount()
            {
                this.sut.AddVertex("A");
                this.sut.VertexCount.Should().Be(1);
                this.sut.AddVertex("B");
                this.sut.VertexCount.Should().Be(2);
            }

            [Fact]
            public void ShouldReturnTrueIfVertexDidntAlreadyExist()
            {
                this.sut.AddVertex("A").Should().BeTrue();
                this.sut.AddVertex("a").Should().BeTrue();
            }

            [Fact]
            public void CustomComparer_ShouldBeUsedToCompareVerticesWhenAdding()
            {
                this.sut = new DirectedGraph<string>(StringComparer.OrdinalIgnoreCase);
                this.sut.AddVertex("A").Should().BeTrue();
                this.sut.AddVertex("a").Should().BeFalse();
            }

            [Fact]
            public void ShouldReturnFalseIfVertexAlreadyExisted()
            {
                this.sut.AddVertex("A").Should().BeTrue();
                this.sut.AddVertex("A").Should().BeFalse();
            }

            [Fact]
            public void ShouldDefaultToDegreeOfZero()
            {
                this.sut.AddVertex("A");
                this.sut.Degree("A").Should().Be(0);
                this.sut.InDegree("A").Should().Be(0);
                this.sut.OutDegree("A").Should().Be(0);
            }

            [Fact]
            public void ShouldNotAffectEdgeCount()
            {
                this.sut.AddVertex("A");
                this.sut.EdgeCount.Should().Be(0);
            }

            [Fact]
            public void ShouldRaiseVertexAddedEvent()
            {
                var addedVertices = new List<string>();
                this.sut.VertexAdded += (s, e) => addedVertices.Add(e.Vertex);

                this.sut.AddVertex("A");
                this.sut.AddVertex("B");

                addedVertices.Should().BeEquivalentTo("A", "B");
            }
        }
    }
}
