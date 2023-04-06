using UnityEngine;
public class Arrive : SteeringMovement
{
    public GameObject Target;
    public float MaximumAngle, StopDistance;
    public override Steering GetSteering(SteeringAgent agent)
    {
        Steering ret = base.GetSteering(agent);
        Vector3 targetDirection = Target.transform.position - agent.transform.position;
        float angle = Vector3.Angle(targetDirection, agent.transform.forward);

        float distance = Vector3.Distance(agent.transform.position, Target.transform.position);

        if (angle <= MaximumAngle)
        {
            if (distance > StopDistance)
            {
                ret.ForwardLinear = 1;
            }
        }

        return ret;
    }
}
