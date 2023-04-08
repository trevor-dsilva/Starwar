using UnityEngine;
public class Seek : SteeringMovement
{
    public GameObject Target;
    public float MaximumAngle, SpeedLimit = float.MaxValue;
    public override Steering GetSteering(SteeringAgent agent)
    {
        Steering ret = base.GetSteering(agent);
        Vector3 targetDirection = Target.transform.position - agent.transform.position;
        float angle = Vector3.Angle(targetDirection, agent.transform.forward);

        if (angle <= MaximumAngle)
        {
            if (SpeedLimit == float.MaxValue)
            {
                ret.ForwardLinear = 1;
            }
            else
            {
                // Speed Limit
                Vector3 selfVelocity = agent.GetComponent<Rigidbody>().velocity;
                Vector3 velocityTowardTarget = Vector3.Project(selfVelocity, targetDirection);
                float speedTowardTarget = velocityTowardTarget.magnitude * Mathf.Sign(Vector3.Dot(selfVelocity.normalized, targetDirection.normalized));
                if (speedTowardTarget < SpeedLimit)
                {
                    ret.ForwardLinear = 1;
                }
            }
        }

        return ret;
    }
}
