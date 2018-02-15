using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShips.Demo
{
    public enum ShipType
    {
        Destroyer = 2,
        Submarine = 3,
        Cruiser = 3, //This works, but only because we need cast from name to value. If we need to go from value to name, enum could cause issues.
        Battleship = 4,
        Carrier = 5
    }
}
