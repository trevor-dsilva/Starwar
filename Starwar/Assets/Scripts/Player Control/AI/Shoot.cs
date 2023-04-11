using UnityEngine;

public class Shoot : SteeringMovement
{
    public GameObject Target;
    public float Angle;
    public MachineGun machineGun;
    public SoundController soundController;
    public override Steering GetSteering(SteeringAgent agent)
    {
        Steering ret = base.GetSteering(agent);
        Vector3 direction = Target.transform.position - agent.transform.position;
        float angle = Vector3.Angle(direction, agent.transform.forward);
        if (angle <= Angle)
        {
            machineGun.Fire();
        }
        return ret;
    }
}
