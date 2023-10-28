using System;
using System.Linq;
using System.Text.RegularExpressions;


var lines = File.ReadAllLines("./input-test.txt");
var movements = new List<Movement>();

foreach (var line in lines)
{
  movements.Add(new Movement { Direction = line[0], Distance = int.Parse(line.Substring(1)) });
}


Console.WriteLine($"Unique cells visited, {Mover.ProcessMovements(movements)}");


public class Mover
{

  public static int ProcessMovements(List<Movement> moves)
  {
    var visitMap = new Dictionary<(int, int), bool>();
    var headPos = (0, 0);
    var knots = new List<(int, int)> {
      (0, 0),
      (0, 0),
      (0, 0),
      (0, 0),
      (0, 0),
      (0, 0),
      (0, 0),
      (0, 0),
      (0, 0),
    };

    visitMap.Add(knots[8], true);

    var moveCount = 0;
    foreach (var move in moves)
    {
      Console.WriteLine($"=== MOVE {++moveCount} {move} ===");

      for (var i = 0; i < move.Distance; i++)
      {
        var newHeadPos = MovePos(headPos, move);


        var oldKnot = knots[0];
        knots[0] = MoveTail(knots[0], newHeadPos, headPos);
        for (var k = 1; k < knots.Count(); k++)
        {
          var oldOldKnot = knots[k];
          knots[k] = MoveTail(knots[k], knots[k - 1], oldKnot);
          oldKnot = oldOldKnot;
        }

        headPos = newHeadPos;
        if (!visitMap.ContainsKey(knots[8]))
        {
          visitMap.Add(knots[8], true);
        }

        Console.WriteLine($"==== SUB {i} ====");
        Render(headPos, knots);
      }
    }

    return visitMap.Aggregate(0, (acc, entries) => entries.Value ? acc + 1 : acc);
  }

  public static void Render((int, int) head, List<(int, int)> knots)
  {
    var range = 15;
    for (var y = range; y >= -range; y--)
    {
      for (var x = -range; x <= range; x++)
      {
        var idx = knots.IndexOf((x, y));
        if (head.Equals((x, y)))
        {
          Console.Write("H");
        }
        else if (idx != -1)
        {
          Console.Write(idx + 1);
        }
        else
        {
          Console.Write(".");
        }
      }
      Console.Write("\n");
    }
  }

  public static (int, int) MoveTail((int, int) tailPos, (int, int) headPos, (int, int) lastHeadPos)
  {
    var distanceVecHead = headPos.Sub(tailPos);

    if (Math.Abs(distanceVecHead.Item1) > 1)
    {
      // move X 
      return tailPos.Add((distanceVecHead.Item1 / Math.Abs(distanceVecHead.Item1), 0));
    }
    else if (Math.Abs(distanceVecHead.Item2) > 1)
    {
      // move Y
      return tailPos.Add((0, (distanceVecHead.Item2 / Math.Abs(distanceVecHead.Item2))));
    }
    else
    {
      // no move
      return tailPos;
    }
  }

  public static (int, int) MovePos((int, int) currPos, Movement move)
  {
    var (currX, currY) = currPos;

    return move.Direction switch
    {
      'R' => (currX + 1, currY),
      'L' => (currX - 1, currY),
      'U' => (currX, currY + 1),
      'D' => (currX, currY - 1),
      _ => throw new Exception("O SHIT")
    };
  }
}

public static class VecExtension
{

  public static double Length(this (int, int) vec)
  {
    return Math.Sqrt(vec.Item1 * vec.Item1 + vec.Item2 * vec.Item2);
  }

  public static (int, int) Sub(this (int, int) vec1, (int, int) vec2)
  {
    return ((vec1.Item1 - vec2.Item1), (vec1.Item2 - vec2.Item2));
  }

  public static (int, int) Add(this (int, int) vec1, (int, int) vec2)
  {
    return ((vec2.Item1 + vec1.Item1), (vec2.Item2 + vec1.Item2));
  }
}


public struct Movement
{
  public char Direction;
  public int Distance;

  public override string ToString()
  {
    return $"{Direction} - {Distance}";
  }
}
