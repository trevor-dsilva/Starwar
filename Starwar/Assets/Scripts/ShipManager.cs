using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class ShipManager
{
    public enum ShipBelong
    {
        Red, Blue
    }
    public static List<Ship> BlueShips = new List<Ship>();
    public static List<Ship> RedShips = new List<Ship>();

    public static List<Ship> Ships(ShipBelong shipBelong)
    {
        switch (shipBelong)
        {
            case ShipBelong.Red:
                return RedShips;
            case ShipBelong.Blue:
                return BlueShips;
            default:
                return null;
        }
    }

    public static void RegisterShip(Ship ship, ShipBelong shipBelong)
    {
        Ships(shipBelong).Add(ship);
    }
    public static void UnregisterShip(Ship ship, ShipBelong shipBelong)
    {
        Ships(shipBelong).Remove(ship);
    }
}
