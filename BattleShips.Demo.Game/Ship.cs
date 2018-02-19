using System.Collections.Generic;
using System.Linq;

namespace BattleShips.Demo
{
    public class Ship
    {
        #region Internal Constructors

        internal Ship(ShipType type)
        {
            Type = type;
        }

        #endregion Internal Constructors

        #region Public Enums

        public enum ShipStatus
        {
            Afloat = 0,
            Sunk
        }

        #endregion Public Enums

        #region Public Properties

        public int Length => (int)Type;

        public string Name => Type.ToString();

        public ShipStatus Status { get; set; }

        public ShipType Type { get; }

        #endregion Public Properties

        #region Private Properties

        private List<Cell> Cells { get; set; }

        #endregion Private Properties

        #region Internal Methods

        internal void Hit()
        {
            if (Cells.All(p => p.Status == Cell.CellStatus.Hit))
            {
                Status = ShipStatus.Sunk;
            }
        }

        internal void SetPoints(IEnumerable<Cell> points)
        {
            Cells = points.ToList();
            Cells.ForEach(p => p.SetShip(this));
        }

        #endregion Internal Methods
    }
}