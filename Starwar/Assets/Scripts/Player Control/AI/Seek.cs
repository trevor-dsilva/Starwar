using UnityEngine;
public class Seek : SteeringMovement
{
    public GameObject Target;
    public float MaximumAngle;
    public override Steering GetSteering(SteeringAgent agent)
    {
        Steering ret = base.GetSteering(agent);
        Vector3 targetDirection = Target.transform.position - transform.position;
        float angle = Vector3.Angle(targetDirection, transform.forward);

        if (angle <= MaximumAngle)
        {
            ret.ForwardLinear = 1;
        }

        return ret;
    }
}
