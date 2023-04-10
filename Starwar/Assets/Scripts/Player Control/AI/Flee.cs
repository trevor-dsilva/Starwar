using UnityEngine;
public class Flee : SteeringMovement
{
    public GameObject target;
    public float MaximumAngle;
    public override Steering GetSteering(SteeringAgent agent)
    {
        Steering ret = base.GetSteering(agent);
        Vector3 fleeDirection = agent.transform.position - target.transform.position;
        float angle = Vector3.Angle(fleeDirection, agent.transform.forward);

        if (angle <= MaximumAngle)
        {
            ret.ForwardLinear = 1;
        }

        return ret;
    }
}
