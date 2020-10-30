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
graph.AllVertices.Where(v => v.HasOutEdges == false);

// Order all vertices by their degree (count of in and out edges), largest first
graph.AllVertices.OrderByDescending(v => v.Degree);

```

## Searching

### Depth first search

``` csharp
var graph = new DirectedGraph<string>();
var search = GraphSearch.DepthFirst(graph); // Or new DepthFirstSearch<string, Edge<string>>(graph);

foreach (var vertex in search.Execute())
{
	Console.WriteLine(vertex);
}
```