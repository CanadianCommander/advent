
using System.Text.Json;

var inputLines = File.ReadAllLines("../8-1/input.txt");


var start = DateTime.Now;

var bottomRight = InputParser.ParseInputGrid(inputLines);


Console.WriteLine($"{SceneryJudge.GetMaxSceneryScore(bottomRight.GetLeftMost().GetTopMost())} Is the max scenery score");

var end = DateTime.Now;

Console.WriteLine($"Time taken: {(end - start).TotalMilliseconds}ms");

// ==========================================

class SceneryJudge
{
  public static int GetMaxSceneryScore(Tree topLeft)
  {
    var currentTree = topLeft;
    var last = currentTree;
    var maxScore = 0;

    while (currentTree != null)
    {
      while (currentTree != null)
      {
        var score = currentTree.SceneryScore();
        if (score > maxScore)
        {
          maxScore = score;
        }

        last = currentTree;
        currentTree = currentTree.Right;
      }
      currentTree = last.GetLeftMost().Bottom;
    }

    return maxScore;
  }
}

class InputParser
{
  public static Tree ParseInputGrid(string[] inputGrid)
  {
    Tree? lastEnd = null;
    Tree? last = null;

    foreach (var line in inputGrid)
    {
      for (var idx = 0; idx < line.Length; idx++)
      {
        var newTree = new Tree(int.Parse(line[idx].ToString()), null, null, null, null);


        if (idx == 0 && last != null)
        {
          newTree.Top = last;
          last.Bottom = newTree;
        }
        else if (last != null)
        {
          var top = last.Top?.Right;

          newTree.Left = last;
          newTree.Top = top;
          last.Right = newTree;
          if (top != null)
          {
            top.Bottom = newTree;
          }
        }


        if (idx == line.Length - 1 && last != null)
        {
          lastEnd = newTree;
          last = last.GetLeftMost();
        }
        else
        {
          last = newTree;
        }
      }
    }

    if (lastEnd != null)
    {
      return lastEnd;
    }
    else
    {
      throw new Exception("OMG LAST IS NULL! HOW COULD THIS HAPPEN");
    }
  }
}

class Tree
{
  private readonly int _height;
  private Tree? _top;
  private Tree? _bottom;
  private Tree? _left;
  private Tree? _right;


  public Tree(int height, Tree? top, Tree? bottom, Tree? left, Tree? right)
  {
    this._height = height;
    this._top = top;
    this._bottom = bottom;
    this._left = left;
    this._right = right;
  }

  /// <summary>
  /// Is this Tree visible 
  /// </summary>
  public int SceneryScore()
  {
    return this.VisLeft(this.Height) *
      this.VisRight(this.Height) *
      this.VisTop(this.Height) *
      this.VisBottom(this.Height);
  }

  /// <summary>
  /// Is there a tree taller than the specified height to the left 
  /// </summary>
  /// <param name="height"></param>
  /// <returns></returns>
  public int VisLeft(int height, int sum = 0)
  {
    if (this._left == null)
    {
      return sum;
    }
    else if (this._left.Height >= height)
    {
      return sum + 1;
    }
    else
    {
      return this._left.VisLeft(height, sum + 1);
    }
  }

  public int VisRight(int height, int sum = 0)
  {
    if (this._right == null)
    {
      return sum;
    }
    else if (this._right.Height >= height)
    {
      return sum + 1;
    }
    else
    {
      return this._right.VisRight(height, sum + 1);
    }
  }

  public int VisTop(int height, int sum = 0)
  {
    if (this._top == null)
    {
      return sum;
    }
    else if (this._top.Height >= height)
    {
      return sum + 1;
    }
    else
    {
      return this._top.VisTop(height, sum + 1);
    }
  }

  public int VisBottom(int height, int sum = 0)
  {
    if (this._bottom == null)
    {
      return sum;
    }
    else if (this._bottom.Height >= height)
    {
      return sum + 1;
    }
    else
    {
      return this._bottom.VisBottom(height, sum + 1);
    }
  }

  /// <summary>
  /// <c>GetLeftMost</c> Gets the left most tree in that can be found from this tree 
  /// </summary>
  public Tree GetLeftMost()
  {
    if (this._left != null)
    {
      return this._left.GetLeftMost();
    }
    return this;
  }

  public Tree GetRightMost()
  {
    if (this._right != null)
    {
      return this._right.GetRightMost();
    }
    return this;
  }

  public Tree GetTopMost()
  {
    if (this._top != null)
    {
      return this._top.GetTopMost();
    }
    return this;
  }

  public Tree GetBottomMost()
  {
    if (this._bottom != null)
    {
      return this._bottom.GetBottomMost();
    }
    return this;
  }

  // ==========================
  // Getter / Setter
  // ==========================

  public int Height => this._height;
  public Tree? Top
  {
    get => this._top;
    set => this._top = value;
  }
  public Tree? Bottom
  {
    get => this._bottom;
    set => this._bottom = value;
  }
  public Tree? Left
  {
    get => this._left;
    set => this._left = value;
  }
  public Tree? Right
  {
    get => this._right;
    set => this._right = value;
  }
}
