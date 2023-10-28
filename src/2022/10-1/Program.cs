using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

var instructionsRegex = new Regex(@"(\w+)\s*(-?\d+)?");
var instructionsRaw = File.ReadAllLines("./input.txt");

Queue<(string, string?)> instructions = new Queue<(string, string?)>(instructionsRaw.Select((str) =>
{
  var match = instructionsRegex.Match(str);
  return (match.Groups[1].Value, match.Groups[2]?.Value);
}).ToList());

var regX = 1;
var totalSignal = 0;

((string, string?), int)? activeInstr = null;
for (var clock = 1; clock <= 220; clock++)
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

  if ((clock - 20) % 40 == 0 || clock == 20)
  {
    Console.WriteLine($"Signal strength at clock cycle {clock} is {clock * regX}");
    totalSignal += clock * regX;
  }

  activeInstr = (activeInstr.Value.Item1, activeInstr.Value.Item2 - 1);
}

Console.WriteLine($"The value of register X is {regX}");
Console.WriteLine($"Total signal strength is {totalSignal}");
