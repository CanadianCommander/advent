﻿
using System.Text.Json;

var inputLines = File.ReadAllLines("input.txt");


var start = DateTime.Now;

var bottomRight = InputParser.ParseInputGrid(inputLines);


Console.WriteLine($"{TreeVisibility.CountVisible(bottomRight.GetLeftMost().GetTopMost())} Trees are visible form the outside of the grid");

var end = DateTime.Now;

Console.WriteLine($"Time taken: {(end - start).TotalMilliseconds}ms");

// ==========================================

class TreeVisibility
{
  public static int CountVisible(Tree topLeft)
  {
    var currentTree = topLeft;
    var last = currentTree;
    var visCount = 0;

    while (currentTree != null)
    {
      while (currentTree != null)
      {
        if (currentTree.Visible())
        {
          visCount++;
        }

        last = currentTree;
        currentTree = currentTree.Right;
      }
      currentTree = last.GetLeftMost().Bottom;
    }

    return visCount;
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
  public bool Visible()
  {
    return !this.TallerThanLeft(this._height) ||
     !this.TallerThanRight(this._height) ||
     !this.TallerThanTop(this._height) ||
     !this.TallerThanBottom(this._height);
  }

  /// <summary>
  /// Is there a tree taller than the specified height to the left 
  /// </summary>
  /// <param name="height"></param>
  /// <returns></returns>
  public bool TallerThanLeft(int height)
  {
    if (this._left != null)
    {
      if (this._left.Height >= height)
      {
        return true;
      }
      else
      {
        return this._left.TallerThanLeft(height);
      }
    }
    return false;
  }

  public bool TallerThanRight(int height)
  {
    if (this._right != null)
    {
      if (this._right.Height >= height)
      {
        return true;
      }
      else
      {
        return this._right.TallerThanRight(height);
      }
    }
    return false;
  }

  public bool TallerThanTop(int height)
  {
    if (this._top != null)
    {
      if (this._top.Height >= height)
      {
        return true;
      }
      else
      {
        return this._top.TallerThanTop(height);
      }
    }
    return false;
  }

  public bool TallerThanBottom(int height)
  {
    if (this._bottom != null)
    {
      if (this._bottom.Height >= height)
      {
        return true;
      }
      else
      {
        return this._bottom.TallerThanBottom(height);
      }
    }
    return false;
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
