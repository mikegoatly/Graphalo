# Graphalo
A very simple graph data structure library.

> Currently under construction!

## Example usage:

``` csharp

var graph = new DirectedGraph<string>();

// You can add vertices directly...
graph.AddVertex("A");

// Alternatively, adding an edge will automatically create missing vertices
graph.AddEdge(new Edge<string>("B", "C"));

// Get all vertices without edges coming out of them
graph.AllVertices.Where(v => graph.HasOutEdges(v) == false);

```