using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShips.Demo
{
    public class Ship
    {
        public event EventHandler ShipSunk;

        public string Name => Type.ToString();

        public int Length => (int)Type;

        public ShipType Type { get; }

        private List<GridPoint> Points { get; set; }

        public Ship(ShipType type)
        {
            Type = type;
        }
        
        public void SetPoints(IEnumerable<GridPoint> points)
        {
            Points = points.ToList();
            Points.ForEach(p => p.PointHit += PointHit);
        }

        private void PointHit(object sender, EventArgs e)
        {
            if (Points.All(p => p.HasBeenHit))
                ShipSunk?.Invoke(this, new EventArgs());
        }
    }

    public class GridPoint
    {
        public event EventHandler PointHit;

        public int X { get; }

        public int Y { get; }

        public bool HasBeenHit { get; private set; }
        
        public GridPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Hit()
        {
            HasBeenHit = true;

            PointHit?.Invoke(this, new EventArgs());
        }
    }
}
