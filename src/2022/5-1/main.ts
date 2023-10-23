import * as fs from "fs";

// yes, the class turned out to be redundant
class StackController {
  private _stacks: string[][];

  constructor(stackCount: number) {
    this._stacks = [];
    for (let col = 0; col < stackCount; col++) {
      this._stacks.push([]);
    }
  }

  public pushStack(item: string, stack: number): void {
    this._stacks[stack].push(item);
  }

  public popStack(stack: number): string {
    if (this._stacks[stack].length == 0) {
      throw new Error(`Stack ${stack} has bottomed out!`);
    }
    return this._stacks[stack].pop();
  }

  public toString(): string {
    return this._stacks.map((stack) => stack[stack.length - 1]).join("");
  }
}

// ====================================

const fileRaw = fs.readFileSync("./src/2022/5-1/input.txt", "utf-8");

const stacksInstruct = fileRaw.trim().split("\n\n");

const stacksRaw = stacksInstruct[0];
const instructLinesRaw = stacksInstruct[1].split("\n");

const stackController = parseStack(stacksRaw);
performOperations(instructLinesRaw, stackController);

console.log(stackController);
console.log("Stack Top Is, ", stackController.toString());

// ====================================

function performOperations(
  instructLinesRaw: string[],
  stackController: StackController
): void {
  for (const instruction of instructLinesRaw) {
    const movement = parseInstruction(instruction);
    let tmpQueue = [];

    for (let i = 0; i < movement[0]; i++) {
      tmpQueue.push(stackController.popStack(movement[1]));
    }

    // see, it's a queue :P
    tmpQueue = tmpQueue.reverse();

    for (let i = 0; i < movement[0]; i++) {
      stackController.pushStack(tmpQueue.pop(), movement[2]);
    }
  }
}

/**
 * parse an instruction
 * @param instruction - instruction to parse
 * @returns - array of numbers, meaning.
 *  0 - number to move
 *  1 - from stack
 *  2 - to stack
 */
function parseInstruction(instruction: string): number[] {
  const match = instruction.match(/^move\s(\d+)\sfrom\s(\d+)\sto\s(\d+)/);
  return [parseInt(match[1]), parseInt(match[2]) - 1, parseInt(match[3]) - 1];
}

function parseStack(stackRaw: string): StackController {
  const stackSplit = stackRaw.split("\n");

  const stackSlices = stackSplit
    .slice(0, stackSplit.length - 1)
    .map((stkSlice) => " " + stkSlice)
    .reverse();

  const countStacks = Array.from(
    stackSplit[stackSplit.length - 1].matchAll(/\d+\s*/g)
  ).length;
  const stackController = new StackController(countStacks);

  for (const stackSlice of stackSlices) {
    for (let col = 0; col < countStacks; col++) {
      if (stackSlice[col * 4 + 2] != " ") {
        stackController.pushStack(stackSlice[col * 4 + 2], col);
      }
    }
  }

  return stackController;
}
