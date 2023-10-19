import { group } from "console";
import * as fs from "fs";
import { it } from "node:test";

class Group {
  constructor(public sacks: Sack[]) {}
}
class Sack {
  constructor(public left: string, public right: string) {}
}

// =================================================
// =================================================

const fileRaw = fs.readFileSync("./src/2022/3/input.txt", "utf-8");

const sacksRaw = fileRaw.trim().split("\n");

const sacks = sacksRaw.map((sackRaw) => toSack(sackRaw));

const groups: Group[] = [];
let remainingSacks = sacks;
while (remainingSacks.length > 0) {
  groups.push(new Group(remainingSacks.slice(0, 3)));
  remainingSacks = remainingSacks.slice(3);
}

const totalScore = groups
  .map((group) => getParisAmongSacks(group.sacks))
  .map((pairs) =>
    pairs.reduce((score, item) => score + getScoreForItem(item), 0)
  )
  .reduce((total, groupScore) => total + groupScore, 0);

console.log("Answer is!", totalScore);

// =================================================
// =================================================

function toSack(sackRaw: string): Sack {
  return new Sack(
    sackRaw.slice(0, sackRaw.length / 2),
    sackRaw.slice(sackRaw.length / 2)
  );
}

function getParisAmongSacks(sack: Sack[]): string[] {
  const occurrences = new Map<string, number>();

  sack
    .map((sack) => getPairsFromSack(sack))
    .forEach((pairSet) => {
      Array.from(pairSet.values()).map((item) => {
        occurrences.set(
          item,
          (occurrences.has(item) ? occurrences.get(item) : 0) + 1
        );
      });
    });

  return Array.from(occurrences.entries())
    .filter((occurrence) => occurrence[1] > 2)
    .map((occurrence) => occurrence[0]);
}

// well not really anymore :P
function getPairsFromSack(sack: Sack): Set<string> {
  return new Set(Array.from(sack.right).concat(Array.from(sack.left)));
}

function getScoreForItem(item: string) {
  const codePoint = item.charCodeAt(0);
  // yes this only works with acsii
  if (codePoint >= 97 && codePoint < 123) {
    return codePoint - 96;
  } else if (codePoint >= 65 && codePoint < 91) {
    return codePoint - 38;
  } else {
    throw new Error(
      `Could not detect character code point correctly for char [${item}]`
    );
  }
}
