using UnityEngine;
public class Arrive : SteeringMovement
{
    public GameObject Target;
    public float MaximumAngle, StopDistance, SlowDistance, SlowZoneSpeedLimit;
    public override Steering GetSteering(SteeringAgent agent)
    {
        Steering ret = base.GetSteering(agent);
        Vector3 targetDirection = Target.transform.position - agent.transform.position;
        float angle = Vector3.Angle(targetDirection, agent.transform.forward);

        float distance = Vector3.Distance(agent.transform.position, Target.transform.position);

        if (angle <= MaximumAngle)
        {
            if (distance < SlowDistance)
            {
                if (distance < StopDistance)
                {
                    return ret;
                }
                // Speed Limit
                Vector3 selfVelocity = agent.GetComponent<Rigidbody>().velocity;
                Vector3 velocityTowardTarget = Vector3.Project(selfVelocity, targetDirection);
                float speedTowardTarget = velocityTowardTarget.magnitude * Mathf.Sign(Vector3.Dot(selfVelocity.normalized, targetDirection.normalized));
                if (speedTowardTarget < SlowZoneSpeedLimit)
                {
                    ret.ForwardLinear = 1;
                }
                return ret;
            }
            ret.ForwardLinear = 1;
        }

        return ret;
    }
}
