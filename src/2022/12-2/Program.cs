using System.IO.Compression;
using System.Linq;

var lines = File.ReadAllLines("../12-1/input.txt");


var (start, tiles) = TileParser.ParseTiles(lines);

Console.WriteLine($"The shortest path from any 'a' is {PathFinder.FindMinPathFromAnyA(tiles)}");


public class PathFinder
{
  public static int FindMinPathFromAnyA(List<List<Tile>> tiles)
  {
    int minPath = int.MaxValue;
    Parallel.ForEach(tiles, (row) =>
    {
      foreach (var tile in row)
      {
        if (tile.Elevation == 0)
        {
          Console.WriteLine($"Checking length when starting at ({tile.Pos.Item1}/{tile.Pos.Item2})");
          var pathLen = FindMinPath(tile, tiles);

          lock (tiles)
          {
            if (pathLen < minPath)
            {
              minPath = pathLen;
            }
          }
        }
      }
    });

    return minPath;
  }

  public static int FindMinPath(Tile start, List<List<Tile>> tiles, int maxMoveDepth = 100000)
  {
    List<Tile> visited = new List<Tile>();
    List<Tile> nextTiles = new List<Tile>();
    Queue<Tile> inspectTiles = new Queue<Tile>();
    var moves = 1;
    inspectTiles.EnqueueAll(PossibleMoves(start, tiles));

    while (inspectTiles.Count() > 0)
    {
      var currTile = inspectTiles.Dequeue();
      visited.Add(currTile);

      if (currTile.End)
      {
        return moves;
      }

      nextTiles.Add(currTile);

      if (inspectTiles.Count() == 0)
      {
        inspectTiles.EnqueueAll(nextTiles.SelectMany((t) => PossibleMoves(t, tiles)));
        inspectTiles = new Queue<Tile>(inspectTiles.Except(visited));
        moves++;
        nextTiles.Clear();
      }

      if (moves > maxMoveDepth)
      {
        throw new OverflowException("Max move depth exceeded");
      }
    }

    return int.MaxValue;
  }


  public static List<Tile> PossibleMoves(Tile curr, List<List<Tile>> tiles)
  {
    List<Tile> moves = new List<Tile>();

    if (curr.Pos.Item1 > 0 && CanMoveTo(curr, tiles[curr.Pos.Item2][curr.Pos.Item1 - 1]))
    {
      moves.Add(tiles[curr.Pos.Item2][curr.Pos.Item1 - 1]);
    }

    if (curr.Pos.Item1 < tiles[0].Count() - 1 && CanMoveTo(curr, tiles[curr.Pos.Item2][curr.Pos.Item1 + 1]))
    {
      moves.Add(tiles[curr.Pos.Item2][curr.Pos.Item1 + 1]);
    }

    if (curr.Pos.Item2 > 0 && CanMoveTo(curr, tiles[curr.Pos.Item2 - 1][curr.Pos.Item1]))
    {
      moves.Add(tiles[curr.Pos.Item2 - 1][curr.Pos.Item1]);
    }

    if (curr.Pos.Item2 < tiles.Count() - 1 && CanMoveTo(curr, tiles[curr.Pos.Item2 + 1][curr.Pos.Item1]))
    {
      moves.Add(tiles[curr.Pos.Item2 + 1][curr.Pos.Item1]);
    }
    return moves;
  }

  public static bool CanMoveTo(Tile curr, Tile to)
  {
    return curr.Elevation >= to.Elevation - 1;
  }

}

public static class QueueExtensions
{
  public static void EnqueueAll<T>(this Queue<T> queue, IEnumerable<T> values)
  {
    foreach (var val in values)
    {
      queue.Enqueue(val);
    }
  }
}

public class TileParser
{
  /**
  Parse raw tile input. Return the starting tile. 
  **/
  public static (Tile, List<List<Tile>>) ParseTiles(string[] tilesString)
  {
    List<List<char>> tiles = tilesString.Select((str) => str.ToCharArray().ToList()).ToList();
    List<List<Tile>> tilesOut = new List<List<Tile>>(tiles.Count());

    Tile? start = null;
    for (var y = 0; y < tiles.Count(); y++)
    {
      tilesOut.Add(new List<Tile>(tiles[y].Count()));

      for (var x = 0; x < tiles[y].Count(); x++)
      {
        var tile = tiles[y][x] switch
        {
          'E' => new Tile((x, y), 25, false, true),
          'S' => new Tile((x, y), 0, true, false),
          char c => new Tile((x, y), (int)c - 97, false, false)
        };

        if (tile.Start)
        {
          start = tile;
        }

        tilesOut[y].Add(tile);
      }
    }

    if (start is null)
    {
      throw new Exception("O no! I didn't find the start!??!?!?!?");
    }
    return (start, tilesOut);
  }
}


public record Tile((int, int) Pos, int Elevation, bool Start, bool End)
{
}
