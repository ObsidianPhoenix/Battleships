using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShips.Demo
{
    public class Grid
    {
        private List<GridPoint> Points { get; set; }

        private List<Ship> Ships { get; set; }

        public void InitiateGrid()
        {
            GenerateGrid();
            PlaceShips();
        }

        private void PlaceShips()
        {
            //First, Create the ships.
            Ships = new List<Ship>();
            var i = 0;
            foreach (ShipType type in Enum.GetValues(typeof(ShipType)))
            {
                var ship = new Ship(type);
                Ships.Add(ship);

                //TODO: Find a place for the ships - Hardcoded for initial testing.
                var points = Points.Where(p => p.X == i && p.Y < ship.Length);
                ship.SetPoints(points);

                ship.ShipSunk += ShipSunk;
                i++;
            }
        }

        private void ShipSunk(object sender, EventArgs eventArgs)
        {
            //TODO: "You Sunk My ScrabbleShip"
        }

        private void GenerateGrid()
        {
            Points = new List<GridPoint>();

            //TODO: Is there a more efficient way to build this list?
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Points.Add(new GridPoint(x, y));
                }
            }
        }
    }
}
