import * as fs from "fs";

class Sack {
  constructor(public left: string, public right: string) {}
}

// =================================================
// =================================================

const fileRaw = fs.readFileSync("./src/2022/3/input.txt", "utf-8");

const sacksRaw = fileRaw.trim().split("\n");

const sacks = sacksRaw.map((sackRaw) => toSack(sackRaw));

const totalScore = sacks
  .map((sack) => getPairsFromSack(sack))
  .map((pairs) =>
    Array.from(pairs.values()).reduce(
      (score, item) => score + getScoreForItem(item),
      0
    )
  )
  .reduce((total, sackScore) => total + sackScore, 0);

console.log("Answer is!", totalScore);

// =================================================
// =================================================

function toSack(sackRaw: string): Sack {
  return new Sack(
    sackRaw.slice(0, sackRaw.length / 2),
    sackRaw.slice(sackRaw.length / 2)
  );
}

function getPairsFromSack(sack: Sack): Set<string> {
  const rightArray = Array.from(sack.right);

  return new Set(
    Array.from(sack.left).filter((item) => rightArray.includes(item))
  );
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
