using System;
using System.Collections.Generic;

namespace BattleShips.Demo.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var ships = new List<Ship>();
            foreach (ShipType type in Enum.GetValues(typeof(ShipType)))
            {
                ships.Add(new Ship(type));
            }

            foreach (var ship in ships)
            {
                System.Console.WriteLine($"{ship.Name}: {ship.Length}");
            }

            System.Console.ReadLine();
        }
    }
}
