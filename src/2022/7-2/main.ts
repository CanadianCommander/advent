import { dir } from "console";
import * as fs from "fs";
import path from "path";

enum CommandMode {
  None = "none",
  Cd = "$ cd",
  Ls = "$ ls",
}

enum NodeType {
  Dir,
  File,
}

interface Node {
  parent: Node;
  name: string;
  size: number;
  type: NodeType;
}

class Dir implements Node {
  constructor(
    private _parent: Node,
    private _name: string,
    private _children: Node[] = []
  ) {}

  public addChild(child: Node): void {
    this._children.push(child);
  }

  public findChild(name: string): Node {
    return this._children.find((child) => child.name === name);
  }

  get parent(): Node {
    return this._parent;
  }

  get name(): string {
    return this._name;
  }

  get size(): number {
    return this._children.reduce((total, child) => total + child.size, 0);
  }

  get type(): NodeType {
    return NodeType.Dir;
  }

  get children(): Node[] {
    return this._children;
  }
}

class File implements Node {
  constructor(
    private _parent: Node,
    private _name: string,
    private _size: number
  ) {}

  get parent(): Node {
    return this._parent;
  }

  get name(): string {
    return this._name;
  }
  get size(): number {
    return this._size;
  }

  get type(): NodeType {
    return NodeType.File;
  }
}

// ==================================================

const fileRaw = fs.readFileSync("./src/2022/7-1/input.txt", "utf-8");
const inputLines = fileRaw.split("\n");
const root = new Dir(null, null);

processCommands(inputLines, root);
console.log("FS Free ", 70000000 - root.size);
console.log("Need space ", 30000000 - (70000000 - root.size));
console.log(
  "Size of deletion target is ",
  findDeletionTarget(root, 30000000 - (70000000 - root.size)).size
);

// ==================================================

function findDeletionTarget(
  currNode: Node,
  minDeleteSize: number,
  smallestNode: Node = null
): Node {
  // assume min delete not negative lol

  if (
    currNode.size >= minDeleteSize &&
    (!smallestNode || currNode.size < smallestNode.size)
  ) {
    smallestNode = currNode;
  }

  if (currNode.type == NodeType.Dir) {
    smallestNode = (currNode as Dir).children
      .filter((child) => child.type === NodeType.Dir)
      .reduce(
        (smallNode, child) =>
          findDeletionTarget(child, minDeleteSize, smallNode),
        smallestNode
      );
  }

  return smallestNode;
}

function processCommands(inputLines: string[], root: Dir): Dir {
  let commandMode = CommandMode.None;
  let cwd = "/";
  let outputBuffer = [];
  let currNode: Dir = root;

  for (const line of inputLines) {
    if (line[0] == "$") {
      if (commandMode == CommandMode.Ls && outputBuffer.length > 0) {
        for (const output of outputBuffer) {
          currNode.addChild(fileLineToNode(output, currNode));
        }
        outputBuffer = [];
      }

      if (line.startsWith(CommandMode.Cd)) {
        commandMode = CommandMode.Cd;
        currNode = handleCd(line, currNode);
      } else if (line.startsWith(CommandMode.Ls)) {
        commandMode = CommandMode.Ls;
      } else {
        throw new Error(`Unknown command ${line}`);
      }
    } else if (commandMode == CommandMode.Ls) {
      outputBuffer.push(line);
    }
  }

  return root;
}

/**
 * handle the cd command
 * @param line - cd command line
 * @param currNode - current node
 * @returns - the new current node
 */
function handleCd(line: string, currNode: Dir): Dir {
  const match = line.match(/\$\scd\s(.*)$/);
  const cdPath = match[1];

  // depend on having seen the node via `ls` first.

  if (cdPath === "..") {
    if (currNode.parent.type === NodeType.Dir) {
      currNode = currNode.parent as Dir;
    } else {
      throw new Error(
        "Parent node is not type Dir! How can this be! Freak out!"
      );
    }
  } else if (cdPath === "/") {
    while (!!currNode.parent) {
      currNode = currNode.parent as Dir;
    }
  } else {
    // also assume simple single step paths.
    const child = currNode.findChild(cdPath);

    if (child.type !== NodeType.Dir) {
      throw new Error("Can only 'cd' in to Directories!");
    }
    currNode = child as Dir;
  }

  return currNode;
}

function fileLineToNode(fileLine: string, parent: Node): Node {
  if (fileLine.startsWith("dir")) {
    // directory
    return new Dir(parent, fileLine.match(/^dir\s(.*)$/)[1]);
  } else {
    // file
    const match = fileLine.match(/^(\d+)\s(.*)$/);
    return new File(parent, match[2], parseInt(match[1]));
  }
}
