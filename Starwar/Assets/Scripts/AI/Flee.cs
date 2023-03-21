using UnityEngine;
public class Flee : SteeringMovement
{
    public GameObject target;
    public float MinimumAngle;
    public override Steering GetSteering(SteeringAgent agent)
    {
        Steering ret = base.GetSteering(agent);
        Vector3 targetDirection = target.transform.position - transform.position;
        float angle = Vector3.Angle(targetDirection, transform.forward);

        if (angle <= MinimumAngle)
        {
            ret.ForwardLinear = 1;
        }

        return ret;
    }
}
