﻿using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;


var inputRaw = File.ReadAllText("./input.txt");
var parseRegex = new Regex(@"^Monkey(.(?!Monkey))+", RegexOptions.Multiline | RegexOptions.Singleline);
var parseMonkey = new Regex(@"^Monkey (\d+).*\n.*Starting items: ([\d\s,]+)\n.* Operation: (.*)\n.* Test: (.*)\n.*If true: (.*)\n.*If false: (.*)", RegexOptions.Multiline);


List<Monkey> monkeys = new List<Monkey>();
foreach (var monkeyRaw in parseRegex.Matches(inputRaw))
{
  if (monkeyRaw is Match monkeyRawMatch)
  {
    var monkeyMatch = parseMonkey.Match(monkeyRawMatch.Value);
    monkeys.Add(new Monkey(
      id: monkeyMatch.Groups[1].Value,
      items: monkeyMatch.Groups[2].Value.Split(",").Select(int.Parse).ToList(),
      operation: MonkeyHelper.BuildMonkeyOp(monkeyMatch.Groups[3].Value),
      monkeyTest: MonkeyHelper.BuildMonkeyTest(monkeyMatch.Groups[4].Value, monkeyMatch.Groups[5].Value, monkeyMatch.Groups[6].Value)
    ));
  }
  else
  {
    throw new Exception("AWWWWW SHEEEEEET" + monkeyRaw.GetType());
  }
}

for (var i = 0; i < 20; i++)
{
  foreach (var mky in monkeys)
  {
    while (mky.Items.Count() > 0)
    {
      var currItem = mky.Items[0];
      mky.Items.RemoveAt(0);

      currItem = mky.Operation(mky, currItem);
      mky.MonkeyTest(mky, monkeys, currItem);
    }
  }
}

var topTwoMonkeys = (
  from m in monkeys
  orderby m.InspectCount descending
  select m
).Take(2).ToList();

Console.WriteLine($"Top two monkeys are {topTwoMonkeys[0].Id}, {topTwoMonkeys[1].Id}.");
Console.WriteLine($"Monkey business score is {topTwoMonkeys[0].InspectCount * topTwoMonkeys[1].InspectCount}");


public delegate int MonkeyOp(Monkey monkey, int currItem);
public delegate void MonkeyTest(Monkey monkey, List<Monkey> monkeys, int currItem);

public class Monkey
{
  public string Id;
  public List<int> Items;
  public MonkeyOp Operation;
  public MonkeyTest MonkeyTest;
  public int InspectCount;


  public Monkey(string id, List<int> items, MonkeyOp operation, MonkeyTest monkeyTest, int inspectCount = 0)
  {
    this.Id = id;
    this.Items = items;
    this.Operation = operation;
    this.MonkeyTest = monkeyTest;
    this.InspectCount = inspectCount;
  }
}

public class MonkeyHelper
{
  public static MonkeyOp BuildMonkeyOp(string monkeyOpString)
  {
    var monkeyOpRegex = new Regex(@"new = (\w+)\s(\+|\*)\s(\w+)");
    var opMatch = monkeyOpRegex.Match(monkeyOpString);

    return (Monkey monkey, int currItem) =>
    {
      monkey.InspectCount++;

      var arg1 = (opMatch.Groups[1].Value.Equals("old") ? currItem : int.Parse(opMatch.Groups[1].Value));
      var arg2 = (opMatch.Groups[3].Value.Equals("old") ? currItem : int.Parse(opMatch.Groups[3].Value));

      return opMatch.Groups[2].Value switch
      {
        "+" => (arg1 + arg2) / 3,
        "*" => (arg1 * arg2) / 3,
        _ => throw new Exception($"Don't understand operation! {opMatch.Groups[3].Value}"),
      };
    };
  }


  public static MonkeyTest BuildMonkeyTest(string monkeyTestString, string trueOp1, string falseOp2)
  {
    var testRegex = new Regex(@"divisible by (\d+)");
    var throwRegex = new Regex(@"throw to monkey (\d+)");


    var testMatch = testRegex.Match(monkeyTestString);
    var throwMatchTrue = throwRegex.Match(trueOp1);
    var throwMatchFalse = throwRegex.Match(falseOp2);

    return (Monkey monkey, List<Monkey> monkeys, int currItem) =>
    {
      if (currItem % int.Parse(testMatch.Groups[1].Value) == 0)
      {
        var targetMonkey = monkeys.Find((mky) => mky.Id.Equals(throwMatchTrue.Groups[1].Value));

        if (targetMonkey != null)
        {
          targetMonkey.Items.Add(currItem);
        }
        else
        {
          throw new Exception("MONKEY BE NULL 1");
        }
      }
      else
      {
        var targetMonkey = monkeys.Find((mky) => mky.Id.Equals(throwMatchFalse.Groups[1].Value));

        if (targetMonkey != null)
        {
          targetMonkey.Items.Add(currItem);
        }
        else
        {
          throw new Exception("MONKEY BE NULL 2");
        }
      }
    };
  }
}
