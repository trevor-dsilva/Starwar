using BehaviorTree;
using System.Collections.Generic;

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
            });

        return root;
    }
}
