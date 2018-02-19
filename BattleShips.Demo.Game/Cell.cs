using System;

namespace BattleShips.Demo
{
    public class Cell
    {
        #region Internal Constructors

        internal Cell(int x, int y)
        {
            X = x;
            Y = y;
            Status = CellStatus.Clear;
        }

        #endregion Internal Constructors

        #region Public Enums

        public enum CellStatus
        {
            Clear = 0,
            Hit,
            Miss
        }

        #endregion Public Enums

        #region Public Properties

        public bool IsOccupied => Ship != null;

        public CellStatus Status { get; private set; }

        public int X { get; }

        public int Y { get; }

        #endregion Public Properties

        #region Private Properties

        private Ship Ship { get; set; }

        #endregion Private Properties

        #region Internal Methods

        internal AttackResult Attack()
        {
            if (Status != CellStatus.Clear) throw new CellAlreadyAttackedException();

            if (Ship != null)
            {
                Status = CellStatus.Hit;
                Ship.Hit();
                return AttackResult.Hit;
            }
            else
            {
                Status = CellStatus.Miss;
                return AttackResult.Miss;
            }
        }

        internal void SetShip(Ship ship)
        {
            Ship = ship ?? throw new ArgumentNullException(nameof(ship));
        }

        #endregion Internal Methods
    }
}