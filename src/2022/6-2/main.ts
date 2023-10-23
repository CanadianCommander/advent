import * as fs from "fs";

const fileRaw = fs.readFileSync("./src/2022/6-1/input.txt", "utf-8");

const dataStream = Array.from(fileRaw.trim());
let slidingWindow = [];

for (const [idx, char] of dataStream.entries()) {
  if (slidingWindow.length < 14) {
    slidingWindow.push(char);
  } else {
    slidingWindow = slidingWindow.slice(1);
    slidingWindow.push(char);
  }

  if (
    slidingWindow.length == 14 &&
    slidingWindow.length == Array.from(new Set(slidingWindow).values()).length
  ) {
    console.log(
      `First message packet found! [${slidingWindow}] idx ${idx + 1}`
    );
    process.exit(1);
  }
}
