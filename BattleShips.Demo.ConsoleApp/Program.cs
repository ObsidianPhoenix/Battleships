using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShips.Demo.ConsoleApp
{
    internal class Program
    {
        #region Private Fields

        private static int gridSize = 10;

        #endregion Private Fields

        #region Private Methods

        private static void DisplayFleetStatus(Engine engine)
        {
            Console.WriteLine();
            foreach (var ship in engine.Fleet)
            {
                Console.Write($"{ship.Name} ({ship.Length}):");

                if (ship.Status == Ship.ShipStatus.Sunk) Console.ForegroundColor = ConsoleColor.Red;
                else Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine($" {ship.Status}");

                Console.ResetColor();
            }

            Console.WriteLine();
        }

        private static void DisplayGrid(Engine engine)
        {
            var rows = engine.Cells
                .GroupBy(c => c.Y)
                .OrderBy(g => g.Key) //Order the Rows.
                .Select(g => new
                {
                    Row = g.Key,
                    Cells = g.OrderBy(c => c.Y).ToList()
                });

            //TODO: There's an occasional flicker on the grid redraw. Rework the code to only redraw the square thats targetted.

            Console.Write("    |");
            for (int i = 1; i <= gridSize; i++)
            {
                Console.Write($" {(char)(i + 64)} |");
            }

            Console.WriteLine();

            foreach (var row in rows)
            {
                DrawRow(row.Row, row.Cells);
            }

            Console.WriteLine(new string('-', 4 * (gridSize + 1)));
        }

        private static void DrawRow(int rowNumber, IEnumerable<Cell> cells)
        {
            Console.WriteLine(new string('-', 4 * (gridSize + 1)));

            Console.Write($" {rowNumber:D2} |");

            foreach (var cell in cells)
            {
                switch (cell.Status)
                {
                    case Cell.CellStatus.Hit:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" x ");
                        Console.ResetColor();
                        Console.Write("|");
                        break;

                    case Cell.CellStatus.Miss:
                        Console.Write(" o |");
                        break;

                    default:
                        Console.Write("   |");
                        break;
                }
            }

            Console.WriteLine();
        }

        private static void Main(string[] args)
        {
            ConsoleKeyInfo key;
            do
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Welcome to Battleships Demo\n\nPlease select an option to begin");
                Console.WriteLine("\t1. Play Battleships");
                Console.WriteLine("\tQ. Quit");
                key = Console.ReadKey();

                if (key.Key == ConsoleKey.D1 || key.Key == ConsoleKey.NumPad1) PlayBattleShips();
            }
            while (key.Key != ConsoleKey.Q);
        }

        private static void PlayBattleShips()
        {
            Console.Clear();

            var engine = new Engine();
            var fleet = new[] { ShipType.Destroyer, ShipType.Destroyer, ShipType.Battleship };
            engine.InitiateGame(gridSize, gridSize, fleet);

            string errorMessage = null;
            AttackResult? lastResult = null;
            string lastInput = null;
            do
            {
                RefreshDisplay(engine);

                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n\n{errorMessage}");
                    Console.ResetColor();
                    errorMessage = null;
                }

                if (lastResult != null)
                {
                    Console.Write($"\n\n\t{lastInput.ToUpper()}: ");
                    if (lastResult == AttackResult.Hit) Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(lastResult.ToString());
                    Console.ResetColor();
                    lastResult = null;
                }

                Console.WriteLine("\n\nSelect the point to target");
                lastInput = Console.ReadLine().Trim();

                try
                {
                    lastResult = engine.Attack(lastInput);
                }
                catch (ArgumentException e) when (e.Message.StartsWith("Invalid co-ordinates."))
                {
                    errorMessage = $"\tInvalid selection: {lastInput.ToUpper()}. Please select a point between A1 and J10";
                }
                catch (CellAlreadyAttackedException)
                {
                    errorMessage = $"\t{lastInput.ToUpper()} has already been attacked.";
                }
            }
            while (!engine.IsGameOver);

            RefreshDisplay(engine);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\n\tYou sunk my ScrabbleShips!");
            Console.ResetColor();
            Console.WriteLine("\nPress any key to return to the main menu");
            Console.ReadKey();
        }

        private static void RefreshDisplay(Engine engine)
        {
            Console.CursorVisible = false;
            Console.Clear();
            DisplayGrid(engine);
            DisplayFleetStatus(engine);
            Console.CursorVisible = true;
        }

        #endregion Private Methods
    }
}