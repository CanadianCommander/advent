import * as fs from "fs";

const fileRaw = fs.readFileSync("./src/2022/1-1/input.txt", "utf-8");

let calElf = fileRaw.trim().split("\n\n");

const top3 = calElf
  .map((calStr) =>
    calStr
      .split("\n")
      .map((numStr) => parseInt(numStr, 10))
      .reduce((total, foodCal) => total + foodCal, 0)
  )
  .sort((elfCal0, elfCal1) => elfCal1 - elfCal0)
  .slice(0, 3)
  .reduce((acc, elfCal) => acc + elfCal, 0);

console.log("The Answer is!", top3);
