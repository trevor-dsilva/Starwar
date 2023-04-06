using UnityEngine;
using BehaviorTree;
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
    [SerializeField]
    private bool missileReady;
    public bool MissileReady
    {
        get { return missileReady; }
        set
        {
            lauchMissile.Ready = value;
            missileReady = value;
        }
    }

    private Seek seek;
    private Flee flee;
    private PreAim preAim;
    private LookAtTarget lookAtTarget;
    private LookAway lookAway;
    private LaunchMissile lauchMissile;

    public MachineGun machineGun;
    public Vector3 Kp, Ki, Kd, PreviousError;
    public float SeekAngle, FleeAngle, MachineGunFireAngle, LeadFactor, MissileLaunchAngle;

    public float MaxTailgateDistance, MaxDogFightDistance, MinDogFightDistance;
    public State state = State.Tailgate;

    private void Start()
    {
        seek = new Seek() { MaximumAngle = SeekAngle };
        flee = new Flee() { MaximumAngle = FleeAngle };
        preAim = new PreAim()
        {
            FireAngle = MachineGunFireAngle,
            Kp = Kp,
            Ki = Ki,
            Kd = Kd,
            machineGun = machineGun
        };
        lookAtTarget = new LookAtTarget()
        {
            Kp = Kp,
            Ki = Ki,
            Kd = Kd
        };
        lookAway = new LookAway()
        {
            Kp = Kp,
            Ki = Ki,
            Kd = Kd
        };
        lauchMissile = new LaunchMissile()
        {
            Angle = MissileLaunchAngle
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
                Debug.Log(lauchMissile.Ready);
                ret.Add(lookAtTarget.GetSteering(agent));
                ret.Add(seek.GetSteering(agent));
                ret.Add(lauchMissile.GetSteering(agent));
                missileReady = lauchMissile.Ready;
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
                    MissileReady = true;
                }
                break;
            case State.Pull:
                if (distance > MaxTailgateDistance)
                {
                    state = State.Tailgate;
                    MissileReady = true;
                }
                break;
        }
    }

}

public class IsAssaultTargetNullOrDead : BehaviorNode
{
    public Assault assault;
    public IsAssaultTargetNullOrDead(Assault assault)
    {
        this.assault = assault;
    }

    public override BehaviorNodeState Evaluate()
    {
        if (assault.Target == null)
        {
            return BehaviorNodeState.SUCCESS;
        }
        else
        {
            if (!assault.Target.GetComponent<Health>().IsAlive)
            {
                return BehaviorNodeState.SUCCESS;
            }
            return BehaviorNodeState.FAILURE;
        }
    }
}

public class SetAssaultTarget : BehaviorNode
{
    public Assault assault;
    public Ship.Belong shipBelong;
    public SetAssaultTarget(Assault assault, Ship.Belong shipBelong)
    {
        this.assault = assault;
        this.shipBelong = shipBelong;
    }

    public override BehaviorNodeState Evaluate()
    {
        Ship targetShip = null;
        float distance = float.MaxValue;
        switch (shipBelong)
        {
            case Ship.Belong.Red:
                foreach (Ship ship in Ship.BlueShips)
                {
                    if (ship.IsSpotted)
                    {
                        float tempDistance = Vector3.Distance(assault.transform.position, ship.transform.position);
                        if (tempDistance < distance)
                        {
                            targetShip = ship;
                            distance = tempDistance;
                        }
                    }
                }
                break;
            case Ship.Belong.Blue:
                foreach (Ship ship in Ship.RedShips)
                {
                    if (ship.IsSpotted)
                    {
                        float tempDistance = Vector3.Distance(assault.transform.position, ship.transform.position);
                        if (tempDistance < distance)
                        {
                            targetShip = ship;
                            distance = tempDistance;
                        }
                    }
                }
                break;
        }
        assault.Target = targetShip.gameObject;
        return BehaviorNodeState.SUCCESS;
    }
}