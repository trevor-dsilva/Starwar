using UnityEngine;
public class Assault : SteeringMovement
{
    public enum State
    {
        Tailgate, DogFight, Pull
    }
    [SerializeField]
    private GameObject target;
    public GameObject Target
    {
        get { return target; }
        set
        {
            seek.Target = value;
            flee.target = value;
            preAim.target = value;
            lookAtTarget.Target = value;
            lookAway.target = value;
            lauchMissile.Target = value;
            target = value;
        }
    }

    private Seek seek;
    private Flee flee;
    private PreAim preAim;
    private LookAtTarget lookAtTarget;
    private LookAway lookAway;
    private LaunchMissile lauchMissile;
    private PID pid;
    public float SeekAngle, FleeAngle, MachineGunFireAngle, LeadFactor, MissileLaunchAngle, MissileLaunchInterval;

    public float MaxTailgateDistance, MaxDogFightDistance, MinDogFightDistance;
    public State state = State.DogFight;

    private void Start()
    {
        pid = GetComponent<PID>();
        seek = new Seek() { MaximumAngle = SeekAngle };
        flee = new Flee() { MaximumAngle = FleeAngle };
        preAim = new PreAim()
        {
            FireAngle = MachineGunFireAngle,
            Kp = pid.Kp,
            Ki = pid.Ki,
            Kd = pid.Kd,
            machineGunManager = GetComponent<MachineGunManager>(),
        };
        lookAtTarget = new LookAtTarget()
        {
            Kp = pid.Kp,
            Ki = pid.Ki,
            Kd = pid.Kd,
        };
        lookAway = new LookAway()
        {
            Kp = pid.Kp,
            Ki = pid.Ki,
            Kd = pid.Kd,
        };
        lauchMissile = new LaunchMissile()
        {
            Angle = MissileLaunchAngle,
            Interval = MissileLaunchInterval,
        };
        Target = target;
    }
    public override Steering GetSteering(SteeringAgent agent)
    {
        StateTransition();
        Steering ret = base.GetSteering(agent);
        if (Target == null) { return ret; }
        switch (state)
        {
            case State.Tailgate:
                ret.Add(lookAtTarget.GetSteering(agent));
                ret.Add(seek.GetSteering(agent));
                ret.Add(lauchMissile.GetSteering(agent));
                //missileReady = lauchMissile.Ready;
                break;
            case State.DogFight:
                ret.Add(seek.GetSteering(agent));
                ret.Add(preAim.GetSteering(agent));
                break;
            case State.Pull:
                ret.Add(flee.GetSteering(agent));
                ret.Add(lookAway.GetSteering(agent));
                break;
        }
        return ret;
    }

    public void StateTransition()
    {
        if (Target == null) { return; }
        float distance = Vector3.Distance(transform.position, Target.transform.position);
        switch (state)
        {
            case State.Tailgate:
                if (distance < MaxDogFightDistance)
                {
                    state = State.DogFight;
                }
                break;
            case State.DogFight:
                if (distance < MinDogFightDistance)
                {
                    state = State.Pull;
                }
                else if (distance > MaxDogFightDistance)
                {
                    state = State.Tailgate;
                    //MissileReady = true;
                }
                break;
            case State.Pull:
                if (distance > MaxTailgateDistance)
                {
                    state = State.Tailgate;
                    //MissileReady = true;
                }
                break;
        }
    }

}
