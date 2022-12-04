namespace AdventOfCode2022;

public sealed class Puzzle2 : Puzzle
{
    enum RpsChoice : int
    {
        Rock = 0,
        Paper = 1,
        Scissors = 2,
    }

    enum RpsResult : int
    {
        Loss = 0,
        Draw = 1,
        Win = 2,
    }

    // Represents a Rock, Paper, Scissors round vs an opponant
    record ElfRpsRound(RpsChoice Opponant, RpsChoice Mine)
    {
        public int GetScore()
        {
            int score = ElfRpsScorer.GetScore(this);
            score += ElfRpsScorer.GetScoreForChoice(Mine);
            return score;
        }
    }

    // Scoring rules
    static class ElfRpsScorer
    {
        public static readonly RpsResult[,] ScoreMatrixOpponantVsMine = new RpsResult[3 /* Opponant */, 3 /* Mine */]
        {
            // Read: When Opponant is (row of choice) and Mine is (column of choice)
            //     Rock           Paper        Scissors               
            { RpsResult.Draw, RpsResult.Win,  RpsResult.Loss },  // Rock
            { RpsResult.Loss, RpsResult.Draw, RpsResult.Win  },  // Paper
            { RpsResult.Win,  RpsResult.Loss, RpsResult.Draw },  // Scissors
        };

        public static readonly RpsChoice[,] OpponantChoiceToOutcome = new RpsChoice[3 /* Opponant Choice */, 3 /* Desired Outcome */ ]
        {
            // Read: When Opponant is (row of choice) and Desired Outcome is (column of choice)
            //     Loss                Draw                 Win          
            { RpsChoice.Scissors, RpsChoice.Rock,     RpsChoice.Paper },    // Rock
            { RpsChoice.Rock,     RpsChoice.Paper,    RpsChoice.Scissors }, // Paper
            { RpsChoice.Paper,    RpsChoice.Scissors, RpsChoice.Rock },     // Scissors
        };

        public static int GetScore(ElfRpsRound round)
        {
            const int CHOICE_TO_SCORE_SCALAR = 3; // Loss = 0 * 3, Draw = 1 * 3, Win = 2 * 3
            return (int)ScoreMatrixOpponantVsMine[(int)round.Opponant, (int)round.Mine] * CHOICE_TO_SCORE_SCALAR;
        }

        public static RpsChoice GetChoiceForOutcome(RpsChoice opponant, RpsResult outcome)
        {
            return OpponantChoiceToOutcome[(int)opponant, (int)outcome];
        }

        public static int GetScoreForChoice(RpsChoice choice)
        {
            return (int)choice + 1;
        }
    }

    (List<ElfRpsRound>, List<ElfRpsRound>) ParseInput()
    {
        const string INPUT_FILE = @".\Day2\Puzzle2Input.txt";
        List<ElfRpsRound> roundsV1 = new();
        List<ElfRpsRound> roundsV2 = new();

        var lines = File.ReadAllLines(INPUT_FILE);

        foreach (var line in lines)
        {
            RpsChoice oppChoice  = ChoiceFromChar(line[0]);
            RpsChoice myChoice   = ChoiceFromChar(line[2]);   // For when column 2 is a choice
            RpsResult desOutcome = ResultFromChar(line[2]); // For when column 2 is an outcome

            roundsV1.Add(new ElfRpsRound(oppChoice, myChoice));
            roundsV2.Add(new ElfRpsRound(oppChoice, 
                                         ElfRpsScorer.GetChoiceForOutcome(oppChoice, desOutcome)));
        }

        return (roundsV1, roundsV2);

        // Local functions
        // Parse a choice from the input file character
        RpsChoice ChoiceFromChar(char c)
        {
            // Using pattern matching syntax: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching
            return c switch
            {
                'A' or 'X' => RpsChoice.Rock,
                'B' or 'Y' => RpsChoice.Paper,
                'C' or 'Z' => RpsChoice.Scissors,
                _ => throw new InvalidDataException("Not a valid value.")
            }; ;
        }

        // Parse a result from the input file character
        RpsResult ResultFromChar(char c)
        {
            // Using pattern matching syntax: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching
            return c switch
            {
                'X' => RpsResult.Loss,
                'Y' => RpsResult.Draw,
                'Z' => RpsResult.Win,
                _ => throw new InvalidDataException("Not a valid value.")
            }; ;
        }
    }
    public override void Run()
    {
        (var roundsV1, var roundsV2) = ParseInput();

        int scoreV1 = roundsV1.Sum(r => r.GetScore());
        WriteLine($"Part 1\nTotal Score: {scoreV1}");

        int scoreV2 = roundsV2.Sum(r => r.GetScore());
        WriteLine($"Part 2\nTotal Score: {scoreV2}");
    }
}
