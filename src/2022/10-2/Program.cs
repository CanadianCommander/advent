using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

var instructionsRegex = new Regex(@"(\w+)\s*(-?\d+)?");
var instructionsRaw = File.ReadAllLines("../10-1/input.txt");

Queue<(string, string?)> instructions = new Queue<(string, string?)>(instructionsRaw.Select((str) =>
{
  var match = instructionsRegex.Match(str);
  return (match.Groups[1].Value, match.Groups[2]?.Value);
}).ToList());

var regX = 1;

// build screen 
char[][] screen = new char[6][];
for (var i = 0; i < 6; i++)
{
  screen[i] = new char[40];
}

((string, string?), int)? activeInstr = null;
for (var clock = 0; instructions.Count() > 0; clock++)
{
  if (activeInstr == null || activeInstr.Value.Item2 < 1)
  {

    if (activeInstr != null)
    {
      switch (activeInstr.Value.Item1.Item1)
      {
        case "addx":
          if (activeInstr.Value.Item1.Item2 != null)
          {
            regX += int.Parse(activeInstr.Value.Item1.Item2);
          }
          else
          {
            throw new Exception("O SHIT");
          }
          break;
      };
    }
    var instr = instructions.Dequeue();
    activeInstr = (instr, instr.Item1.Equals("addx") ? 2 : 1);
  }

  var y = (clock / 40) % 6;
  var x = clock % 40;

  if (x >= regX - 1 && x <= regX + 1)
  {
    screen[y][x] = '#';
  }
  else
  {
    screen[y][x] = '.';
  }

  activeInstr = (activeInstr.Value.Item1, activeInstr.Value.Item2 - 1);
}


Console.WriteLine("============ SCREEN O MATIC ==============");
foreach (var row in screen)
{
  Console.WriteLine(" " + String.Join("", row));
}
Console.WriteLine("==========================================");
