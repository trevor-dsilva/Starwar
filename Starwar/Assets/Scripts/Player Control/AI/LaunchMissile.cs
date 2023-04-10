using UnityEngine;

public class LaunchMissile : SteeringMovement
{
    public GameObject Target;
    public float Angle;
    public bool Ready;
    public float Interval;

    private float lastLaunchTime = 0;
    public override Steering GetSteering(SteeringAgent agent)
    {
        Steering ret = base.GetSteering(agent);
        if (!Ready && lastLaunchTime + Interval < Time.time)
        {
            Ready = true;
        }
        Vector3 direction = Target.transform.position - agent.transform.position;
        float angle = Vector3.Angle(direction, agent.transform.forward);
        if (Ready && angle <= Angle)
        {
            if (agent.GetComponent<MissileLauncherManager>().LockOn(Target))
            {
                agent.GetComponent<MissileLauncherManager>().Fire();
                lastLaunchTime = Time.time;
                Ready = false;
            }
        }
        return ret;
    }
}
