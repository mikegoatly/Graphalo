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

Reference: [Wikipedia](https://en.wikipedia.org/wiki/Depth-first_search)

Returns the deepest vertices first, working back to the root(s). Does not support cyclic graphs.

``` csharp
var graph = new DirectedGraph<string>();

// Search across the entire graph (in the case of multiple disconnected 
// graphs being contained in the same structure)
foreach (var vertex in graph.Search(SearchKind.DepthFirst))
{
	Console.WriteLine(vertex);
}

// Or search from a specific starting vertex - only the connected vertices will be returned
foreach (var vertex in graph.Search(SearchKind.DepthFirst, "A"))
{
	Console.WriteLine(vertex);
}
```
