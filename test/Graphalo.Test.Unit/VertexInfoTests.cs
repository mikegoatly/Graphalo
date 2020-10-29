using FluentAssertions;
using Moq;
using Xunit;

namespace Graphalo.Test.Unit
{
    public class VertexInfoTests
    {
        private const string Vertex = "A";
        private readonly Mock<IDirectedGraph<string, Edge<string>>> graphMock;
        private readonly VertexInfo<string, Edge<string>> sut;

        public VertexInfoTests()
        {
            this.graphMock = new Mock<IDirectedGraph<string, Edge<string>>>();
            this.sut = new VertexInfo<string, Edge<string>>(
                this.graphMock.Object,
                Vertex);
        }

        [Fact]
        public void GettingVertex_ShouldReturnVertex()
        {
            this.sut.Vertex.Should().Be(Vertex);
        }

        [Fact]
        public void GettingDegree_ShouldReturnValueFromGraph()
        {
            this.graphMock.Setup(g => g.Degree(Vertex)).Returns(9);
            this.sut.Degree.Should().Be(9);
        }

        [Fact]
        public void GettingInDegree_ShouldReturnValueFromGraph()
        {
            this.graphMock.Setup(g => g.InDegree(Vertex)).Returns(9);
            this.sut.InDegree.Should().Be(9);
        }

        [Fact]
        public void GettingOutDegree_ShouldReturnValueFromGraph()
        {
            this.graphMock.Setup(g => g.OutDegree(Vertex)).Returns(9);
            this.sut.OutDegree.Should().Be(9);
        }

        [Fact]
        public void GettingHasInEdges_ShouldReturnValueFromGraph()
        {
            this.graphMock.Setup(g => g.HasInEdges(Vertex)).Returns(true);
            this.sut.HasInEdges.Should().Be(true);
        }

        [Fact]
        public void GettingHasOutEdges_ShouldReturnValueFromGraph()
        {
            this.graphMock.Setup(g => g.HasOutEdges(Vertex)).Returns(true);
            this.sut.HasOutEdges.Should().Be(true);
        }
    }
}
