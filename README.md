![Build and test](https://github.com/mikegoatly/Graphalo/workflows/Build%20and%20test/badge.svg)

# Graphalo
A *very* simple graph data structure library.

## Installation

``` powershell
Install-Package Graphalo
```

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

## Traversal

### Dijkstra's Algorithm

Reference: [Wikipedia](https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm)

Attempts to find the shortest path between two vertices, taking into account the weights of each edge.
Cycles in the graph are supported and will not cause infinite loops.

``` csharp
//   B-
//  /5 \2
// A    C
//  \2 /1
//   D-
var graph = new DirectedGraph<string>();
graph.AddEdge(new Edge<string>("A", "B", 5));
graph.AddEdge(new Edge<string>("B", "C", 2));
graph.AddEdge(new Edge<string>("A", "D", 2));
graph.AddEdge(new Edge<string>("D", "C", 1));

var traversalResult = graph.Traverse(TraversalKind.Dijkstra, "A", "C");

// traversalResult.Success will be true
foreach (var vertex in traversalResult.Results)
{
	Console.Write(vertex);
	Console.Write(" ");
}

// Output: A D C (The route via D is the cheapest)

// Attempt a tranversal to an unreachable node:
traversalResult = graph.Traverse(TraversalKind.Dijkstra, "B", "D");
// traversalResult.Success will be false
```
