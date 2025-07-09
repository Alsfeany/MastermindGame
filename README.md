#  my_mastermind – Console Game in C#

Hi! This is a simple Mastermind-style console game I built in C# for the Savvy Kickstarter Program 2025 – Gameplay Programming Track.

The goal is to guess a secret 4-digit code made of digits from 0 to 8. All digits are unique. After every guess, the game tells you how close you are by showing:
- ✅ How many digits are in the correct place ("well placed")
- 🔁 How many digits are correct but in the wrong spot ("misplaced")

You only get a limited number of guesses, so make them count!

---

## How It Works

- The code is either randomly generated (default) or passed using `-c`
- You can also set the number of attempts using `-t` (default is 10)
- Only **valid and unique** guesses reduce your trial count
- Invalid or duplicate guesses don’t cost you anything

Example:
```bash
dotnet run -- -c 1234 -t 5
