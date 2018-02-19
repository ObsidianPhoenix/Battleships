namespace BattleShips.Demo
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Zero length ships are not valid.")]
    public enum ShipType
    {
        Destroyer = 2,
        Submarine = 3,
        Cruiser = 3, //This works, but only because we need cast from name to value. If we need to go from value to name, enum could cause issues.
        Battleship = 4,
        Carrier = 5
    }
}