using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Ship : MonoBehaviour
{
    public enum Belong
    {
        Red, Blue
    }

    public bool IsSpotted;
    // After a ship disappear from field of view, it will still be spotted for x seconds.
    // Then it goes unspotted.
    public float SpotTime = 3.0f;
    public Belong ShipBelong;

    private float spotTimeCountDown = 0;
    private void Start()
    {
        RegisterShip(this);
    }
    public void Spotted()
    {
        IsSpotted = true;
        spotTimeCountDown = SpotTime;
    }
    public void Unspotted()
    {
        IsSpotted = false;
    }

    private void FixedUpdate()
    {
        if (IsSpotted && spotTimeCountDown <= 0)
        { Unspotted(); }
        else
        { spotTimeCountDown -= Time.fixedDeltaTime; }
    }

    public static List<Ship> BlueShips = new List<Ship>();
    public static List<Ship> RedShips = new List<Ship>();

    public static List<Ship> Ships(Belong shipBelong)
    {
        switch (shipBelong)
        {
            case Belong.Red:
                return RedShips;
            case Belong.Blue:
                return BlueShips;
            default:
                return null;
        }
    }

    public static void RegisterShip(Ship ship)
    {
        Ships(ship.ShipBelong).Add(ship);
    }
    public static void UnregisterShip(Ship ship)
    {
        Ships(ship.ShipBelong).Remove(ship);
        Debug.Log(Ships(ship.ShipBelong).Count);
    }
}

public class ExistSpottedEnemy : BehaviorNode
{
    public Ship.Belong shipBelong;
    public ExistSpottedEnemy(Ship.Belong shipBelong)
    {
        this.shipBelong = shipBelong;
    }

    public override BehaviorNodeState Evaluate()
    {
        switch (shipBelong)
        {
            case Ship.Belong.Red:
                foreach (Ship ship in Ship.BlueShips)
                {
                    if (ship.IsSpotted)
                    {
                        return BehaviorNodeState.SUCCESS;
                    }
                }
                return BehaviorNodeState.FAILURE;
            case Ship.Belong.Blue:
                foreach (Ship ship in Ship.RedShips)
                {
                    if (ship.IsSpotted)
                    {
                        return BehaviorNodeState.SUCCESS;
                    }
                }
                return BehaviorNodeState.FAILURE;
            default:
                return BehaviorNodeState.NONE;
        }
    }
}

public class SpotEnemyInFOV : BehaviorNode
{
    public Transform self;
    public float viewRange;
    public Ship.Belong shipBelong;
    // Will cast a ray toward enemy ship, ray should be hit obstacles and enemy ship, but not ally ship.
    public LayerMask viewMask;
    public SpotEnemyInFOV(Transform transform, float viewRange, Ship.Belong shipBelong, LayerMask viewMask)
    {
        this.self = transform;
        this.viewRange = viewRange;
        this.shipBelong = shipBelong;
        this.viewMask = viewMask;
    }

    public override BehaviorNodeState Evaluate()
    {
        Ship.Belong enenmyBelong = Ship.Belong.Blue;
        if (shipBelong == Ship.Belong.Blue)
        {
            enenmyBelong = Ship.Belong.Red;
        }
        foreach (Ship ship in Ship.Ships(enenmyBelong))
        {
            if (ship.IsSpotted)
            {
                continue;
            }
            Vector3 direction = ship.transform.position - self.position;
            if (Physics.Raycast(self.position, direction, out RaycastHit hitInfo, viewRange, viewMask))
            {
                if (hitInfo.collider.gameObject == ship.gameObject)
                {
                    ship.Spotted();
                }
            }
            Debug.DrawRay(self.position, direction * viewRange, Color.red, 1);
        }
        return BehaviorNodeState.SUCCESS;
    }
}
