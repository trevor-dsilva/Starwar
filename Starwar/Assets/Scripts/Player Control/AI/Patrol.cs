using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Patrol : SteeringMovement
{
    public List<Transform> Waypoints;
    [SerializeField]
    private GameObject target;
    public GameObject Target
    {
        get { return target; }
        set
        {
            target = value;
            if (value == null) { return; }
            lookAtTarget.Target = value.gameObject;
            seek.Target = value.gameObject;
        }
    }
    
    public float SeekAngle, StopDistance, SpeedLimit;

    private Seek seek;
    private LookAtTarget lookAtTarget;
    private PID pid;

    private void Start()
    {
        pid = GetComponent<PID>();
        seek = new Seek() { MaximumAngle = SeekAngle, SpeedLimit = SpeedLimit };
        lookAtTarget = new LookAtTarget()
        {
            Kp = pid.Kp,
            Ki = pid.Ki,
            Kd = pid.Kd,
        };
        Target = target;
    }

    public override Steering GetSteering(SteeringAgent agent)
    {
        Steering ret = base.GetSteering(agent);
        if (Target == null) { return ret; }
        ret.Add(lookAtTarget.GetSteering(agent));
        ret.Add(seek.GetSteering(agent));
        return ret;
    }
}

public class IsPatrolTargetNull : BehaviorNode
{
    public Patrol patrol;
    public IsPatrolTargetNull(Patrol patrol)
    {
        this.patrol = patrol;
    }

    public override BehaviorNodeState Evaluate()
    {
        if (patrol.Target == null)
        {
            return BehaviorNodeState.SUCCESS;
        }
        else { return BehaviorNodeState.FAILURE; }
    }
}

public class SetPatrolNewTarget : BehaviorNode
{

    public Patrol patrol;
    public SetPatrolNewTarget(Patrol patrol)
    {
        this.patrol = patrol;
    }

    public override BehaviorNodeState Evaluate()
    {
        Transform closestTarget = null;
        float distance = float.MaxValue;

        foreach (Transform t in patrol.Waypoints)
        {
            float tempDistance = Vector3.Distance(t.position, patrol.transform.position);
            if (tempDistance < distance)
            {
                closestTarget = t;
                distance = tempDistance;
            }
        }
        patrol.Target = closestTarget.gameObject;

        return BehaviorNodeState.SUCCESS;
    }
}

public class IsAtPatrolTarget : BehaviorNode
{
    public Patrol patrol;
    public IsAtPatrolTarget(Patrol patrol)
    {
        this.patrol = patrol;
    }

    public override BehaviorNodeState Evaluate()
    {
        float distance = Vector3.Distance(patrol.transform.position, patrol.Target.transform.position);
        if (distance <= patrol.StopDistance)
        {
            return BehaviorNodeState.SUCCESS;
        }
        else { return BehaviorNodeState.FAILURE; }
    }
}

public class SetPatrolNextTarget : BehaviorNode
{
    public Patrol patrol;
    public SetPatrolNextTarget(Patrol patrol)
    {
        this.patrol = patrol;
    }

    public override BehaviorNodeState Evaluate()
    {
        Debug.Log("Test");
        int index = patrol.Waypoints.IndexOf(patrol.Target.transform);
        index++;
        if (index >= patrol.Waypoints.Count) { index = 0; }
        patrol.Target = patrol.Waypoints[index].gameObject;
        return BehaviorNodeState.SUCCESS;
    }
}