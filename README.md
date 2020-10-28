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

```