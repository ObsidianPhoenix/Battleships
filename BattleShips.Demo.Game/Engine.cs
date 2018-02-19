using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BattleShips.Demo
{
    public class Engine
    {
        #region Private Fields

        private List<Cell> cells;
        private List<Ship> fleet;

        #endregion Private Fields

        #region Private Enums

        private enum PlacementAxis
        {
            Horizontal = 0,
            Vertical = 1
        }

        #endregion Private Enums

        #region Public Properties

        public IReadOnlyList<Cell> Cells => cells.AsReadOnly();

        public IReadOnlyList<Ship> Fleet => fleet.AsReadOnly();

        public bool IsGameOver { get; private set; }

        #endregion Public Properties

        #region Private Properties

        private static Random Rand => new Random(Guid.NewGuid().GetHashCode());

        private static Regex ValidCoordinates => new Regex(@"^[a-zA-Z]{1}|\d{1,2}$");

        private int ColumnCount { get; set; }

        private string InvalidCoordinatesErrorMessage => $"Invalid co-ordinates. Value must be between A1 and {(char)(ColumnCount + 64)}{RowCount}";

        private int RowCount { get; set; }

        #endregion Private Properties

        #region Public Methods

        public AttackResult Attack(string coordinates)
        {
            var referencePoints = ValidCoordinates.Matches(coordinates.Trim())
                .Cast<Match>()
                .Select(m => m.Value)
                .ToArray();

            if (referencePoints.Length != 2) throw new ArgumentException(InvalidCoordinatesErrorMessage, nameof(coordinates));

            //Can assume two array items, since we passed validation.
            //cells are 1 based, this should be fine..
            var x = char.ToUpper(referencePoints[0].ToCharArray()[0]) - 64;
            var y = int.Parse(referencePoints[1]);

            if (x < 1 || x > ColumnCount) throw new ArgumentException(InvalidCoordinatesErrorMessage, nameof(coordinates));
            if (y < 1 || y > RowCount) throw new ArgumentException(InvalidCoordinatesErrorMessage, nameof(coordinates));

            var cell = cells.Single(c => c.X == x && c.Y == y);
            var returnvalue = cell.Attack();

            //Check the status of all ships in the ships.
            if (fleet.All(s => s.Status == Ship.ShipStatus.Sunk)) IsGameOver = true;

            return returnvalue;
        }

        public void InitiateGame(int columns, int rows, IEnumerable<ShipType> ships)
        {
            if (columns < 10) throw new ArgumentException("Minimum number of columns is 10", nameof(columns));
            if (rows < 10) throw new ArgumentException("Minimum number of rows is 10", nameof(rows));

            RowCount = rows;
            ColumnCount = columns;

            GenerateBattlefield();
            PositionFleet(ships);
        }

        #endregion Public Methods

        #region Private Methods

        private void GenerateBattlefield()
        {
            cells = new List<Cell>();

            //TODO: Is there a more efficient way to build this list?
            //Configuring grid as 1-based means we can avoid subtracting values on each attack.
            for (var column = 1; column <= ColumnCount; column++)
            {
                for (var row = 1; row <= RowCount; row++)
                {
                    cells.Add(new Cell(column, row));
                }
            }
        }

        private void PlaceShip(Ship ship)
        {
            var isPlaced = false;
            do
            {
                var axis = (PlacementAxis)Rand.Next(0, 1);
                var startRow = Rand.Next(1, RowCount);
                var startCol = Rand.Next(1, ColumnCount);

                List<Cell> availableCells;
                if (axis == PlacementAxis.Vertical)
                    availableCells = this.cells.Where(c => c.X == startCol && c.Y >= startRow).ToList();
                else
                    availableCells = this.cells.Where(c => c.Y == startRow && c.X >= startCol).ToList();

                availableCells = availableCells.Take(ship.Length).ToList();

                if (availableCells.Any(c => c.IsOccupied) || availableCells.Count < ship.Length) continue;

                ship.SetPoints(availableCells);
                isPlaced = true;
            }
            while (!isPlaced);
        }

        private void PositionFleet(IEnumerable<ShipType> ships)
        {
            fleet = new List<Ship>();

            //First, Create the ships.
            foreach (var shipType in ships)
            {
                fleet.Add(new Ship(shipType));
            }

            //Now position each ship
            fleet.ForEach(PlaceShip);
        }

        #endregion Private Methods
    }
}