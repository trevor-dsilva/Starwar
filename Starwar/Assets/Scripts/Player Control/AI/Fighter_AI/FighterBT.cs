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
    public float viewRange;
    // Will cast a ray toward enemy ship, ray should be hit obstacles and enemy ship, but not ally ship.
    public UnityEngine.LayerMask viewMask;
    public BehaviorNode _root;
    private void Start()
    {
        _root = SetupTree();
    }
    private void Update()
    {
        if (_root != null)
        {
            _root.Evaluate();
        }
    }
    protected override BehaviorNode SetupTree()
    {
        BehaviorNode root =
            new Selector(new List<BehaviorNode>()
            {
                new Sequence(new List<BehaviorNode>
                {
                    new AmIAlive(GetComponent<Health>()),
                    new SpotEnemyInFOV(transform, viewRange, GetComponent<Ship>().ShipBelong, viewMask),
                    new Selector(new List<BehaviorNode>
                    {
                        new Sequence(new List<BehaviorNode>()
                        {
                            new IsPatrolState(this),
                            new Selector(new List<BehaviorNode>()
                            {
                                new Sequence(new List<BehaviorNode>()
                                {
                                    // If there is spotted enemy
                                    // Set State of steering agent to Assault
                                    new ExistSpottedEnemy(GetComponent<Ship>().ShipBelong),
                                    new SetStateAssault(this, GetComponent<SteeringAgent>()),
                                }),
                                new Sequence(new List<BehaviorNode>()
                                {
                                    new IsPatrolTargetNull(GetComponent<Patrol>()),
                                    new SetPatrolNewTarget(GetComponent<Patrol>())
                                }),
                                new Sequence(new List<BehaviorNode>()
                                {
                                    new IsAtPatrolTarget(GetComponent<Patrol>()),
                                    new SetPatrolNextTarget(GetComponent<Patrol>())
                                }),
                                // Return running 
                                // No enenmy & on my way to the next waypoint
                                new Running()
                            })
                        }),
                        new Sequence(new List<BehaviorNode>()
                        {
                            new IsAssaultState(this),
                            new Selector(new List<BehaviorNode>()
                            {
                                new Sequence(new List<BehaviorNode>()
                                {
                                    new IsAssaultTargetNullOrDead(GetComponent<Assault>()),new Message(),
                                    new Selector(new List<BehaviorNode>()
                                    {
                                        new Sequence(new List<BehaviorNode>()
                                        {
                                            new ExistSpottedEnemy(GetComponent<Ship>().ShipBelong),
                                            new SetAssaultTarget(GetComponent<Assault>(), GetComponent<Ship>().ShipBelong)
                                        }),
                                        new SetStatePatrol(this, GetComponent<SteeringAgent>()),
                                    }),
                                }),

                                new Running()
                            }),
                            new Running()
                        })
                    }),
                }),
                new IsDeadState(this),
                new SetStateDead(this, GetComponent<SteeringAgent>())
            });

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

public class AmIHealthy : BehaviorNode
{
    public Health myHealth;
    public float criticalPercentage;
    public AmIHealthy(Health health, float criticalPercentage)
    {
        myHealth = health;
        this.criticalPercentage = criticalPercentage;
    }
    public override BehaviorNodeState Evaluate()
    {
        float percentage = myHealth.CurrentHealth / myHealth.MaxHealth;
        if (percentage < criticalPercentage)
        {
            return BehaviorNodeState.FAILURE;
        }
        else
        {
            return BehaviorNodeState.SUCCESS;
        }
    }
}

public class AmIAlive : BehaviorNode
{
    public Health myHealth;
    public AmIAlive(Health health)
    { myHealth = health; }
    public override BehaviorNodeState Evaluate()
    {
        return myHealth.IsAlive ? BehaviorNodeState.SUCCESS : BehaviorNodeState.FAILURE;
    }
}

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

public class SpotEnemyInFOV : BehaviorNode
{
    public Transform self;
    public float viewRange;
    public Ship.Belong shipBelong;
    // Will cast a ray toward enemy ship, ray should be hit obstacles and enemy ship, but not ally ship.
    public LayerMask viewMask;
    public SpotEnemyInFOV(Transform transform, float viewRange, Ship.Belong shipBelong, LayerMask viewMask)
    {
        this.self = transform;
        this.viewRange = viewRange;
        this.shipBelong = shipBelong;
        this.viewMask = viewMask;
    }
    public override BehaviorNodeState Evaluate()
    {
        Ship.Belong enenmyBelong = Ship.Belong.Blue;
        if (shipBelong == Ship.Belong.Blue)
        {
            enenmyBelong = Ship.Belong.Red;
        }
        foreach (Ship ship in Ship.Ships(enenmyBelong))
        {
            if (ship.IsSpotted)
            {
                continue;
            }
            Vector3 direction = ship.transform.position - self.position;
            if (Physics.Raycast(self.position, direction, out RaycastHit hitInfo, viewRange, viewMask))
            {
                if (hitInfo.collider.gameObject == ship.gameObject)
                {
                    ship.Spotted();
                }
            }
            Debug.DrawRay(self.position, direction * viewRange, Color.red, 1);
        }
        return BehaviorNodeState.SUCCESS;
    }
}

