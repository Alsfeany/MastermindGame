using System;
using System.Collections.Generic;

/// <summary>
/// Entry point for the Mastermind console game.
/// Accepts CLI args for custom code (-c) and max attempts (-t).
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        string secretCode = "";
        int maxAttempts = 10; // default attempts

        // Parse CLI args
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-c" && i + 1 < args.Length)
            {
                secretCode = args[i + 1];
            }
            else if (args[i] == "-t" && i + 1 < args.Length)
            {
                if (!int.TryParse(args[i + 1], out maxAttempts) || maxAttempts <= 0)
                {
                    Console.WriteLine("Invalid number of attempts. Using default (10).");
                    maxAttempts = 10;
                }
            }
        }

        // Validate user code or fallback to random gen
        if (!IsValidCode(secretCode))
        {
            secretCode = GenerateRandomCode(); // fallback
        }

        // Start the game logic
        Game game = new Game(secretCode, maxAttempts);
        game.Start();
    }

    /// <summary>
    /// Checks if a code is valid (4 unique digits from 0-8)
    /// </summary>
    static bool IsValidCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length != 4)
            return false;

        HashSet<char> digits = new HashSet<char>();
        foreach (char c in code)
        {
            if (c < '0' || c > '8' || !digits.Add(c))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Generates a random valid secret code from digits 0–8
    /// </summary>
    static string GenerateRandomCode()
    {
        Random rand = new Random();
        List<char> pool = new List<char>("012345678");
        string code = "";

        for (int i = 0; i < 4; i++)
        {
            int index = rand.Next(pool.Count);
            code += pool[index];
            pool.RemoveAt(index);
        }

        return code;
    }
}

/// <summary>
/// Manages the gameplay loop, guess logic, validation, feedback, etc.
/// </summary>
class Game
{
    private readonly string secretCode;
    private readonly int maxAttempts;

    /// <summary>
    /// Game constructor with a fixed code and number of attempts
    /// </summary>
    public Game(string code, int attempts)
    {
        secretCode = code;
        maxAttempts = attempts;
    }

    /// <summary>
    /// Main game loop — handles rounds, inputs, and result feedback
    /// </summary>
    public void Start()
    {
        Console.WriteLine("Can you break the code? Enter a valid guess.");

        int validAttempts = 0;
        HashSet<string> previousGuesses = new HashSet<string>();

        while (validAttempts < maxAttempts)
        {
            Console.WriteLine($"Round {validAttempts}");
            Console.Write(">");

            string? guess = Console.ReadLine();

            if (guess == null)
            {
                Console.WriteLine("\n[EOF detected. Exiting...]");
                return;
            }

            if (!IsValidGuess(guess))
            {
                Console.WriteLine("Wrong input! Try again w/ 4 unique digits.");
                continue;
            }

            if (previousGuesses.Contains(guess))
            {
                Console.WriteLine("You already tried this! Pick something else.");
                continue;
            }

            previousGuesses.Add(guess);
            validAttempts++;

            if (guess == secretCode)
            {
                Console.WriteLine("Congratz! You did it!");
                return;
            }

            var (wellPlaced, misplaced) = CompareGuess(guess, secretCode);
            Console.WriteLine($"Well placed pieces: {wellPlaced}");
            Console.WriteLine($"Misplaced pieces: {misplaced}");

            int trialsLeft = maxAttempts - validAttempts;
            if (trialsLeft > 0)
                Console.WriteLine($"Trials left: {trialsLeft}");
        }

        Console.WriteLine("You've used all attempts. Better luck next time!");
        Console.WriteLine($"The code was: {secretCode}");
    }

    /// <summary>
    /// Checks if a guess is valid: 4 unique digits, 0–8
    /// </summary>
    private bool IsValidGuess(string guess)
    {
        if (guess.Length != 4) return false;

        HashSet<char> digits = new HashSet<char>();
        foreach (char c in guess)
        {
            if (c < '0' || c > '8' || !digits.Add(c))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Compares guess vs. the secret code and returns (wellPlaced, misplaced)
    /// </summary>
    private (int wellPlaced, int misplaced) CompareGuess(string guess, string code)
    {
        int wellPlaced = 0;
        int misplaced = 0;

        for (int i = 0; i < 4; i++)
        {
            if (guess[i] == code[i])
                wellPlaced++;
        }

        foreach (char c in guess)
        {
            if (code.Contains(c))
                misplaced++;
        }

        misplaced -= wellPlaced; // Remove the already counted ones
        return (wellPlaced, misplaced);
    }
}
