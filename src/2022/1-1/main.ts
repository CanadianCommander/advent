import * as fs from "fs";

const fileRaw = fs.readFileSync("./src/2022/1-1/input.txt", "utf-8");

let calElf = fileRaw.trim().split("\n\n");

const max = calElf
  .map((calStr) =>
    calStr
      .split("\n")
      .map((numStr) => parseInt(numStr, 10))
      .reduce((total, foodCal) => total + foodCal, 0)
  )
  .reduce((max, elfCal) => Math.max(max, elfCal), 0);

console.log("The Answer is!", max);
