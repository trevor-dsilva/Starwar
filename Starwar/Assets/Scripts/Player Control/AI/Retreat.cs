using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BehaviorTree;

public class Retreat : SteeringMovement
{
    [SerializeField]
    private GameObject target;
    public GameObject Target
    {
        get { return target; }
        set
        {
            arrive.Target = value;
            lookAtTarget.Target = value;
            target = value;
        }
    }

    private Arrive arrive;
    private LookAtTarget lookAtTarget;
}
