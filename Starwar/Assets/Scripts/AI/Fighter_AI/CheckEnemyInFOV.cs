using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BehaviorTree;

public class CheckEnemyInFOV : Node
{
    public Transform self;
    public float viewAngle, viewRange;
    public CheckEnemyInFOV(Transform transform, float viewAngle, float viewRange)
    {
        this.self = transform;
        this.viewAngle = viewAngle;
        this.viewRange = viewRange;
    }
}
