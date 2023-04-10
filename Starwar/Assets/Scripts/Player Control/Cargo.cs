using UnityEngine;
public class Cargo : MonoBehaviour
{
    public float Range;

    private Ship.Belong belong;

    private void Start()
    {
        belong = GetComponent<Ship>().ShipBelong;
    }
    private void FixedUpdate()
    {
        foreach (Ship ship in Ship.Ships(belong))
        {
            if (ship.IsCargoShip) { continue; }
            if (!ship.GetComponent<Health>().IsAlive) { continue; }
            float distance = Vector3.Distance(ship.transform.position, transform.position);
            if (distance > Range) { continue; }
            ship.Resupply();
        }
    }
}