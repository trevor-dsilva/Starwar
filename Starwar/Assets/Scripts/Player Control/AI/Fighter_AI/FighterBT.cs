using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;

public class FighterBT : BehaviorTree.BehaviorTree
{
    public enum State
    {
        Patrol,
        Assault,
        Retreat,
        Dead
    }
    public State state;
    public float viewRange, criticalHealthPercentage;
    // Will cast a ray toward enemy ship, ray should be hit obstacles and enemy ship, but not ally ship.
    public LayerMask viewMask;
    public BehaviorNode _root;
    protected override BehaviorNode SetupTree()
    {
        BehaviorNode root =
            new Selector(new List<BehaviorNode>()
            {
                // If alive
                // Spot enemy
                new Sequence(new List<BehaviorNode>
                {
                    new Message("1"),
                    new IsAlive(GetComponent<Health>()),
                    new Message("2"),
                    new Message("3"),
                    new Selector(new List<BehaviorNode>
                    {
                        // If state is Patrol
                        new Sequence(new List<BehaviorNode>()
                        {
                            new Message("4"),
                            new IsPatrolState(this),
                            new Message("5"),
                            new Selector(new List<BehaviorNode>()
                            {
                                // If low health or no ammo
                                // And exist alive cargo ship of team
                                // Set state as Retreat
                                new Sequence(new List<BehaviorNode>()
                                {
                                    new Message("6"),
                                    new Selector(new List<BehaviorNode>()
                                    {
                                        new IsNotHealthy(GetComponent<Health>(), criticalHealthPercentage),
                                        new IsAmmunitionEmpty(GetComponent<MachineGunManager>()),
                                    }),
                                    new Message("7"),
                                    new ExistAliveCargoShip(GetComponent<Ship>().ShipBelong),
                                    new Message("8"),
                                    new SetStateRetreat(this, GetComponent<SteeringAgent>()),
                                    new Message("9"),
                                }),
                                // Else If there is spotted enemy
                                // Set State of steering agent to Assault
                                new Sequence(new List<BehaviorNode>()
                                {
                                    new Message("10"),
                                    new ExistSpottedEnemy(GetComponent<Ship>().ShipBelong),
                                    new Message("11"),
                                    new SetStateAssault(this, GetComponent<SteeringAgent>()),
                                    new Message("12"),
                                }),
                                // Else if patrol target is null
                                // Set new Patrol target.
                                new Sequence(new List<BehaviorNode>()
                                {
                                    new Message("13"),
                                    new IsPatrolTargetNull(GetComponent<Patrol>()),
                                    new Message("14"),
                                    new SetPatrolNewTarget(GetComponent<Patrol>()),
                                    new Message("15"),
                                }),
                                // Else if we are currently at Patrol target
                                // Set to next Patrol target.
                                new Sequence(new List<BehaviorNode>()
                                {
                                    new Message("16"),
                                    new IsAtPatrolTarget(GetComponent<Patrol>()),
                                    new Message("17"),
                                    new SetPatrolNextTarget(GetComponent<Patrol>()),
                                    new Message("18"),
                                }),
                                // Else Return running 
                                // No enenmy & on my way to the next waypoint
                                new Running()
                            })
                        }),
                        // Else if state is Assault
                        new Sequence(new List<BehaviorNode>()
                        {
                            new Message("19"),
                            new IsAssaultState(this),
                            new Message("20"),
                            new Selector(new List<BehaviorNode>()
                            {
                                // If low health or no ammo
                                // And exist alive cargo ship of team
                                // Set state as Retreat
                                new Sequence(new List<BehaviorNode>()
                                {
                                    new Message("21"),
                                    new Selector(new List<BehaviorNode>()
                                    {
                                        new IsNotHealthy(GetComponent<Health>(), criticalHealthPercentage),
                                        new IsAmmunitionEmpty(GetComponent<MachineGunManager>())
                                    }),
                                    new Message("22"),
                                    new ExistAliveCargoShip(GetComponent<Ship>().ShipBelong),
                                    new Message("23"),
                                    new SetStateRetreat(this, GetComponent<SteeringAgent>()),
                                    new Message("24"),
                                }),
                                // Else If Assault Target is null or Dead
                                new Sequence(new List<BehaviorNode>()
                                {
                                    new Message("25"),
                                    new IsAssaultTargetNullOrDead(GetComponent<Assault>()),
                                    new Message("26"),
                                    new Selector(new List<BehaviorNode>()
                                    {
                                        // If exist spottd enemy
                                        // Set Assault target
                                        new Sequence(new List<BehaviorNode>()
                                        {
                                            new Message("27"),
                                            new ExistSpottedEnemy(GetComponent<Ship>().ShipBelong),
                                            new Message("28"),
                                            new SetAssaultTarget(GetComponent<Assault>(), GetComponent<Ship>().ShipBelong),
                                            new Message("29"),
                                        }),
                                        // Else set state as Patrol
                                        new SetStatePatrol(this, GetComponent<SteeringAgent>()),
                                    }),
                                }),
                                // Else keep running
                                // Means is currently fighting 
                                new Running()
                            }),
                        }),
                        // Else if state is Retreat
                        new Sequence(new List<BehaviorNode>()
                        {
                            new Message("30"),
                            new IsRetreatState(this),
                            new Message("31"),
                            new Selector(new List<BehaviorNode>()
                            {
                                // If Retreat target is null or dead
                                new Sequence(new List<BehaviorNode>()
                                {
                                    new Message("32"),
                                    new IsRetreatTargetNullOrDead(GetComponent<Retreat>()),
                                    new Message("33"),
                                    new Selector(new List<BehaviorNode>()
                                    {
                                        // If there exist an alive cargo ship in the team
                                        // Set it as Retreat target.
                                        new SetRetreatTarget(GetComponent<Retreat>(), GetComponent<Ship>().ShipBelong),
                                        // Else If exist spotted enemy
                                        // Set state as Assault
                                        new Sequence(new List<BehaviorNode>()
                                        {
                                            new ExistSpottedEnemy(GetComponent<Ship>().ShipBelong),
                                            new SetStateAssault(this, GetComponent<SteeringAgent>())
                                        }),
                                        // Else 
                                        // Set state as Patrol
                                        new SetStatePatrol(this, GetComponent<SteeringAgent>())
                                    }),
                                    new Message("34"),
                                }),
                                // Else if Ammunition is full & Health is full
                                new Sequence(new List<BehaviorNode>()
                                {
                                    new Message("35"),
                                    new IsAmmunitionFull(GetComponent<MachineGunManager>()),
                                    new Message("36"),
                                    new IsFullHealth(GetComponent<Health>()),
                                    new Message("37"),
                                    new Selector(new List<BehaviorNode>()
                                    {
                                        // If there is spotted enemy
                                        // Set state as Assault
                                        new Sequence(new List<BehaviorNode>()
                                        {
                                            new Message("38"),
                                            new ExistSpottedEnemy(GetComponent<Ship>().ShipBelong),
                                            new Message("39"),
                                            new SetStateAssault(this, GetComponent<SteeringAgent>()),
                                            new Message("40"),
                                        }),
                                        // Else set state as Patrol
                                        new SetStatePatrol(this, GetComponent<SteeringAgent>())
                                    }),
                                    new Message("41"),
                                }),
                                // Else 
                                // Still re supplying, stay close of cargo ship
                                new Running()
                            })
                        })
                    }),
                }),
                // Else if state is Dead
                new IsDeadState(this),
                // Else set state as Dead
                new SetStateDead(this, GetComponent<SteeringAgent>())
            }) ;

        return root;
    }
}

public class IsAssaultState : BehaviorNode
{
    public FighterBT tree;
    public IsAssaultState(FighterBT tree)
    {
        this.tree = tree;
    }
    public override BehaviorNodeState Evaluate()
    {
        if (tree.state == FighterBT.State.Assault)
        { return BehaviorNodeState.SUCCESS; }
        else
        { return BehaviorNodeState.FAILURE; }
    }
}

public class SetStateAssault : BehaviorNode
{
    public FighterBT tree;
    public SteeringAgent agent;
    public SetStateAssault(FighterBT tree, SteeringAgent steeringAgent)
    {
        this.tree = tree;
        agent = steeringAgent;
    }
    public override BehaviorNodeState Evaluate()
    {
        tree.state = FighterBT.State.Assault;
        agent.Assault();
        return BehaviorNodeState.SUCCESS;
    }
}

public class IsPatrolState : BehaviorNode
{
    public FighterBT tree;
    public IsPatrolState(FighterBT tree)
    {
        this.tree = tree;
    }
    public override BehaviorNodeState Evaluate()
    {
        if (tree.state == FighterBT.State.Patrol)
        { return BehaviorNodeState.SUCCESS; }
        else
        { return BehaviorNodeState.FAILURE; }
    }
}

public class SetStatePatrol : BehaviorNode
{
    public FighterBT tree;
    public SteeringAgent agent;
    public SetStatePatrol(FighterBT tree, SteeringAgent steeringAgent)
    {
        this.tree = tree;
        agent = steeringAgent;
    }
    public override BehaviorNodeState Evaluate()
    {
        tree.state = FighterBT.State.Patrol;
        agent.Patrol();
        return BehaviorNodeState.SUCCESS;
    }
}

public class IsDeadState : BehaviorNode
{
    public FighterBT tree;
    public IsDeadState(FighterBT tree)
    {
        this.tree = tree;
    }
    public override BehaviorNodeState Evaluate()
    {
        if (tree.state == FighterBT.State.Dead)
        { return BehaviorNodeState.SUCCESS; }
        else
        { return BehaviorNodeState.FAILURE; }
    }
}

public class SetStateDead : BehaviorNode
{
    public FighterBT tree;
    public SteeringAgent agent;
    public SetStateDead(FighterBT tree, SteeringAgent steeringAgent)
    {
        this.tree = tree;
        agent = steeringAgent;
    }
    public override BehaviorNodeState Evaluate()
    {
        tree.state = FighterBT.State.Dead;
        agent.Dead();
        return BehaviorNodeState.SUCCESS;
    }
}

public class IsRetreatState : BehaviorNode
{
    public FighterBT tree;
    public IsRetreatState(FighterBT tree)
    {
        this.tree = tree;
    }
    public override BehaviorNodeState Evaluate()
    {
        if (tree.state == FighterBT.State.Retreat)
        { return BehaviorNodeState.SUCCESS; }
        else
        { return BehaviorNodeState.FAILURE; }
    }
}

public class SetStateRetreat : BehaviorNode
{
    public FighterBT tree;
    public SteeringAgent agent;
    public SetStateRetreat(FighterBT tree, SteeringAgent steeringAgent)
    {
        this.tree = tree;
        agent = steeringAgent;
    }
    public override BehaviorNodeState Evaluate()
    {
        tree.state = FighterBT.State.Retreat;
        agent.Retreat();
        return BehaviorNodeState.SUCCESS;
    }
}

/// <summary>
/// If there is an alive cargo ship in the team, set it as retreat target and return success, otherwise return failure.
/// </summary>
public class SetRetreatTarget : BehaviorNode
{
    public Retreat retreat;
    public Ship.Belong belong;
    public SetRetreatTarget(Retreat retreat, Ship.Belong belong)
    {
        this.retreat = retreat;
        this.belong = belong;
    }
    public override BehaviorNodeState Evaluate()
    {
        foreach (Ship ship in Ship.Ships(belong))
        {
            if (ship.IsCargoShip && ship.GetComponent<Health>().IsAlive)
            {
                retreat.Target = ship.gameObject;
                return BehaviorNodeState.SUCCESS;
            }
        }
        return BehaviorNodeState.FAILURE;
    }
}

/// <summary>
/// If Retreat's target is null or not alive, return success, otherwise return failure
/// </summary>
public class IsRetreatTargetNullOrDead : BehaviorNode
{
    public Retreat retreat;
    public IsRetreatTargetNullOrDead(Retreat retreat)
    {
        this.retreat = retreat;
    }
    public override BehaviorNodeState Evaluate()
    {
        if (retreat.Target == null)
        {
            return BehaviorNodeState.SUCCESS;
        }
        else
        {
            if (!retreat.Target.GetComponent<Health>().IsAlive)
            {
                return BehaviorNodeState.SUCCESS;
            }
            return BehaviorNodeState.FAILURE;
        }
    }
}

/// <summary>
/// If exist an alive cargo ship in the team, return success, otherwise return failure.
/// </summary>
public class ExistAliveCargoShip : BehaviorNode
{
    public Ship.Belong belong;
    public ExistAliveCargoShip(Ship.Belong belong)
    {
        this.belong = belong;
    }
    public override BehaviorNodeState Evaluate()
    {
        foreach (Ship ship in Ship.Ships(belong))
        {
            if (ship.IsCargoShip)
            {
                if (ship.GetComponent<Health>().IsAlive)
                {
                    return BehaviorNodeState.SUCCESS;
                }
            }
        }
        return BehaviorNodeState.FAILURE;
    }
}

/// <summary>
/// If the ship's health is below critical percentage, return success, otherwise failure.
/// </summary>
public class IsNotHealthy : BehaviorNode
{
    public Health myHealth;
    public float criticalPercentage;
    public IsNotHealthy(Health health, float criticalPercentage)
    {
        myHealth = health;
        this.criticalPercentage = criticalPercentage;
    }
    public override BehaviorNodeState Evaluate()
    {
        float percentage = myHealth.CurrentHealth / myHealth.MaxHealth;
        if (percentage > criticalPercentage)
        {
            return BehaviorNodeState.FAILURE;
        }
        else
        {
            return BehaviorNodeState.SUCCESS;
        }
    }
}

/// <summary>
/// If this ship is alive, return success, otherwise failure.
/// </summary>
public class IsAlive : BehaviorNode
{
    public Health myHealth;
    public IsAlive(Health health)
    { myHealth = health; }
    public override BehaviorNodeState Evaluate()
    {
        return myHealth.IsAlive ? BehaviorNodeState.SUCCESS : BehaviorNodeState.FAILURE;
    }
}

/// <summary>
/// If the health of ship is at Max, return success, otherwise failure
/// </summary>
public class IsFullHealth : BehaviorNode
{
    public Health myHealth;
    public IsFullHealth(Health health)
    {
        myHealth = health;
    }
    public override BehaviorNodeState Evaluate()
    {
        return myHealth.CurrentHealth >= myHealth.MaxHealth ? BehaviorNodeState.SUCCESS : BehaviorNodeState.FAILURE;
    }
}


/// <summary>
/// If there is a ship belong to the other team that is alive & spotted, return success, otherwise failure.
/// </summary>
public class ExistSpottedEnemy : BehaviorNode
{
    public Ship.Belong shipBelong;
    public ExistSpottedEnemy(Ship.Belong shipBelong)
    {
        this.shipBelong = shipBelong;
    }
    public override BehaviorNodeState Evaluate()
    {
        switch (shipBelong)
        {
            case Ship.Belong.Red:
                foreach (Ship ship in Ship.BlueShips)
                {
                    if (ship.IsSpotted)
                    {
                        return BehaviorNodeState.SUCCESS;
                    }
                }
                return BehaviorNodeState.FAILURE;
            case Ship.Belong.Blue:
                foreach (Ship ship in Ship.RedShips)
                {
                    if (ship.IsSpotted)
                    {
                        return BehaviorNodeState.SUCCESS;
                    }
                }
                return BehaviorNodeState.FAILURE;
            default:
                return BehaviorNodeState.NONE;
        }
    }
}

/// <summary>
/// If Assault's target is null or not alive, return success, otherwise return failure
/// </summary>
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

/// <summary>
/// Set assault target as the closest enemy ship.
/// Always return success.
/// </summary>
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

/// <summary>
/// If the ammunition count of machine gun manager is equal or below 0, return success, otherwise return failure.
/// </summary>
public class IsAmmunitionEmpty : BehaviorNode
{
    public MachineGunManager machineGunManager;
    public IsAmmunitionEmpty(MachineGunManager machineGunManager)
    {
        this.machineGunManager = machineGunManager;
    }
    public override BehaviorNodeState Evaluate()
    {
        return machineGunManager.Ammunition <= 0 ? BehaviorNodeState.SUCCESS : BehaviorNodeState.FAILURE;
    }
}

/// <summary>
/// If the ammunition count of machine gun manager is equal or above Max, return success, otherwise return failure.
/// </summary>
public class IsAmmunitionFull : BehaviorNode
{
    public MachineGunManager machineGunManager;
    public IsAmmunitionFull(MachineGunManager machineGunManager)
    {
        this.machineGunManager = machineGunManager;
    }
    public override BehaviorNodeState Evaluate()
    {
        return machineGunManager.Ammunition >= machineGunManager.MaxAmmunition ? BehaviorNodeState.SUCCESS : BehaviorNodeState.FAILURE;
    }
}

public class AvoidObstaclesNode : BehaviorNode
{
    private AIAvoidance aiAvoidance;
    private SteeringAgent agent;

    public AvoidObstaclesNode(AIAvoidance aiAvoidance, SteeringAgent agent)
    {
        this.aiAvoidance = aiAvoidance;
        this.agent = agent;
    }

    public override BehaviorNodeState Evaluate()
    {
        // Calculate avoidance force using AIAvoidance
        Steering avoidanceSteering = aiAvoidance.GetSteering(agent);

        // Apply avoidance force and torque directly within Evaluate()
        

        return BehaviorNodeState.SUCCESS;
    }
}
