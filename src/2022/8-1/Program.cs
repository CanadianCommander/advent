
var inputLines = File.ReadAllLines("./src/2022/8-1/input.txt");


// ==========================================

class InputParser
{
  public static List<Tree> ParseInputGrid(List<string> inputGrid)
  {
    var topLeft = new Tree(int.Parse(inputGrid[0][0].ToString()), null, null, null, null);
    Tree? last = topLeft;

    foreach (var line in inputGrid)
    {
      for (var idx = 1; idx < line.Length; idx++)
      {
        var newTree = new Tree(int.Parse(line[idx].ToString()), null, null, last, null);
        last.Right = newTree;
      }
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
