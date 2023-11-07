using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;

var lines = File.ReadAllLines("./input.txt");

var lineLst = lines.Where((line) => !line.Trim().Equals("")).ToList();

PairParser.ParsePairs(lineLst);

public class PairParser
{

  public static List<(Pair, Pair)> ParsePairs(List<string> lines)
  {
    for (var i = 0; i < lines.Count(); i += 2)
    {
      ParsePair(lines[i]);
      ParsePair(lines[i + 1]);
    }

    return null;
  }

  public static Pair ParsePair(string line)
  {
    var rgx = new Regex(@"^\[(.*)\]$");

    var mtch = rgx.Match(line);

    var pair = new Pair();
    Console.WriteLine("=====");
    Console.WriteLine(mtch.Groups[1].Value);
    Console.WriteLine(">>>");
    foreach (var val in SplitBracketAware(mtch.Groups[1].Value))
    {
      Console.WriteLine(val);
    }
    Console.WriteLine("=====");

    return null;
  }

  public static List<string> SplitBracketAware(string str)
  {
    var splits = new List<string>();

    int lastSplitIdx = 0;
    int bracketDepth = 0;
    for (var i = 0; i < str.Count(); i++)
    {
      switch (str[i])
      {
        case '[':
          bracketDepth++;
          break;
        case ']':
          bracketDepth--;
          break;
        case ',':
          if (bracketDepth == 0)
          {
            splits.Add(str.Substring(lastSplitIdx, i - lastSplitIdx));
            lastSplitIdx = i + 1;
          }
          break;
      }
    }

    splits.Add(str.Substring(lastSplitIdx, str.Count() - lastSplitIdx));
    return splits;
  }
}



public class Pair
{
  List<Pair> Pairs = new List<Pair>();
  int Value;

  bool IsValue => this.Pairs.Count() == 0;
  bool IsArray => this.Pairs.Count() > 0;
}
