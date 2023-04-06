using UnityEngine;

public class LaunchMissile : SteeringMovement
{
    public GameObject Target;
    public float Angle;
    public bool Ready;
    public override Steering GetSteering(SteeringAgent agent)
    {
        Steering ret = base.GetSteering(agent);
        Vector3 direction = Target.transform.position - agent.transform.position;
        float angle = Vector3.Angle(direction, agent.transform.forward);
        if (Ready && angle <= Angle)
        {
            if (agent.GetComponent<MissileLauncherManager>().LockOn(Target))
            {
                agent.GetComponent<MissileLauncherManager>().Fire();
                Ready = false;
            }
        }
        return ret;
    }
}
