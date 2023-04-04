using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BehaviorTree;

public class SpotEnemyInFOV : Node
{
    public Transform self;
    public float viewRange;
    public ShipManager.ShipBelong shipBelong;
    // Will cast a ray toward enemy ship, ray should be hit obstacles and enemy ship, but not ally ship.
    public LayerMask viewMask;
    public SpotEnemyInFOV(Transform transform, float viewRange, ShipManager.ShipBelong shipBelong, LayerMask viewMask)
    {
        this.self = transform;
        this.viewRange = viewRange;
        this.shipBelong = shipBelong;
        this.viewMask = viewMask;
    }

    public override NodeState Evaluate()
    {
        foreach (Ship ship in ShipManager.Ships(shipBelong))
        {
            if (ship.IsSpotted) { continue; }
            Vector3 direction = ship.transform.position - self.position;
            if (Physics.Raycast(self.position, direction, out RaycastHit hitInfo, viewRange, viewMask))
            {
                if (hitInfo.collider.gameObject == ship.gameObject)
                {
                    ship.Spotted();
                }
            }
        }
        return NodeState.SUCCESS;
    }
}
