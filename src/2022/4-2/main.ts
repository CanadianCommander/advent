import * as fs from "fs";

class Range {
  constructor(public start: number, public end: number) {}

  /**
   * Dose this range fully contain the other range
   * @param other - other range, returns true if this range fully contains it.
   */
  public contains(other: Range): boolean {
    return this.start <= other.start && this.end >= other.end;
  }

  /**
   * Does this range overlap the other specified range.
   * @param other
   */
  public overlaps(other: Range): boolean {
    return (
      (this.start <= other.start && this.end >= other.start) ||
      (this.end <= other.end && this.start >= other.end)
    );
  }
}

// =================================================

const fileRaw = fs.readFileSync("./src/2022/4-1/input.txt", "utf-8");

const pairsRaw = fileRaw.trim().split("\n");

const pairs = pairsRaw.map((pairRaw) => {
  const ranges: Range[] = [];
  const rangesRaw = pairRaw.split(",");

  return rangesRaw.map((rangeRaw) => {
    const match = rangeRaw.match(/^(\d+)-(\d+)$/);
    const start = parseInt(match[1]);
    const end = parseInt(match[2]);

    return new Range(start, end);
  });
});

let totalOverlaps = 0;
for (const pair of pairs) {
  if (pair[0].overlaps(pair[1]) || pair[1].overlaps(pair[0])) {
    totalOverlaps++;
  }
}

console.log(`${totalOverlaps} Pairs contain partial overlaps!`);
