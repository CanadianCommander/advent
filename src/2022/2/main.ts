import * as fs from "fs";

enum HandType {
  Rock,
  Paper,
  Scissors,
}

abstract class Hand {
  abstract beets(other: Hand): boolean;
  draw(other: Hand): boolean {
    return this.type == other.type;
  }

  abstract get type(): HandType;
  abstract get handVal(): number;
}

class Rock extends Hand {
  beets(other: Hand): boolean {
    return other.type == HandType.Scissors;
  }

  get type(): HandType {
    return HandType.Rock;
  }

  get handVal(): number {
    return 1;
  }
}

class Paper extends Hand {
  beets(other: Hand): boolean {
    return other.type == HandType.Rock;
  }

  get type(): HandType {
    return HandType.Paper;
  }

  get handVal(): number {
    return 2;
  }
}

class Scissors extends Hand {
  beets(other: Hand): boolean {
    return other.type == HandType.Paper;
  }

  get type(): HandType {
    return HandType.Scissors;
  }

  get handVal(): number {
    return 3;
  }
}

const scoreHandMap = {
  X: 1,
  Y: 2,
  Z: 3,
};

const scoreOutcombMap = {
  Lose: 0,
  Draw: 3,
  Win: 6,
};

// ======================================================
// ======================================================

const fileRaw = fs.readFileSync("./src/2022/2/input.txt", "utf-8");

const roundsRaw = fileRaw.trim().split("\n");

const rounds = roundsRaw.map((roundRaw) => ({
  adversaryHand: mapAdversaryHand(roundRaw[0]),
  myHand: mapMyHand(roundRaw[2], mapAdversaryHand(roundRaw[0])),
}));

const totalScore = rounds.reduce(
  (score, round) => score + calcOutcomeScore(round.myHand, round.adversaryHand),
  0
);

console.log(totalScore);

// ======================================================
// ======================================================

function handFactory(handType: HandType): Hand {
  switch (handType) {
    case HandType.Rock:
      return new Rock();
    case HandType.Paper:
      return new Paper();
    case HandType.Scissors:
      return new Scissors();
  }
}

function mapAdversaryHand(hand: string): Hand {
  switch (hand) {
    case "A":
      return new Rock();
    case "B":
      return new Paper();
    case "C":
      return new Scissors();
  }
}

function mapMyHand(hand: string, otherHand: Hand): Hand {
  switch (hand) {
    case "X":
      return new Rock();
    case "Y":
      return new Paper();
    case "Z":
      return new Scissors();
  }
}

function calcOutcomeScore(myHand: Hand, adversaryHand: Hand): number {
  const outcomeVal = myHand.beets(adversaryHand)
    ? scoreOutcombMap.Win
    : myHand.draw(adversaryHand)
    ? scoreOutcombMap.Draw
    : scoreOutcombMap.Lose;

  return outcomeVal + myHand.handVal;
}
