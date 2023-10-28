using System;
using System.Linq;
using System.Text.RegularExpressions;


var lines = File.ReadAllLines("./input.txt");
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
    var tailPos = (0, 0);
    visitMap.Add(tailPos, true);

    var moveCount = 0;
    foreach (var move in moves)
    {
      for (var i = 0; i < move.Distance; i++)
      {
        var newHeadPos = MovePos(headPos, move);
        tailPos = MoveTail(tailPos, newHeadPos, headPos);

        Console.WriteLine($"=== MOVE {++moveCount} ===");
        Console.WriteLine(newHeadPos.Sub(tailPos).Length());
        Console.WriteLine(headPos);
        Console.WriteLine(newHeadPos);
        Console.WriteLine(tailPos);

        headPos = newHeadPos;
        if (!visitMap.ContainsKey(tailPos))
        {
          Console.WriteLine($"Just visited {tailPos}");
          visitMap.Add(tailPos, true);
        }
      }
    }

    return visitMap.Aggregate(0, (acc, entries) => entries.Value ? acc + 1 : acc);
  }

  public static (int, int) MoveTail((int, int) tailPos, (int, int) headPos, (int, int) lastHeadPos)
  {
    var distanceVecHead = headPos.Sub(tailPos);

    if (distanceVecHead.Length() >= 1.9d)
    {
      // move 
      return lastHeadPos;
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
