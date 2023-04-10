using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviorTree;

public class EnemyAIFighterBT : BehaviorTree.BehaviorTree
{
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float waypointReachedDistance = 1.0f;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float obstacleAvoidanceRadius = 5.0f;
    [SerializeField] private LayerMask obstacleMask;

    public int currentWaypointIndex = 0;
    public Rigidbody rb;

    protected override BehaviorNode SetupTree()
    {
        rb = GetComponent<Rigidbody>();

        Sequence patrolSequence = new Sequence(new List<BehaviorNode>
        {
            new PatrolAction(this, waypoints, waypointReachedDistance, speed),
            new AvoidObstacleAction(this, obstacleAvoidanceRadius, obstacleMask, speed) // Pass the speed variable
        });

        return patrolSequence;
    }
}

public class PatrolAction : BehaviorNode
{
    private EnemyAIFighterBT bt;
    private List<Transform> waypoints;
    private float waypointReachedDistance;
    private float speed;
    private Rigidbody rb;

    public PatrolAction(EnemyAIFighterBT bt, List<Transform> waypoints, float waypointReachedDistance, float speed)
    {
        this.bt = bt;
        this.waypoints = waypoints;
        this.waypointReachedDistance = waypointReachedDistance;
        this.speed = speed;
        this.rb = bt.GetComponent<Rigidbody>();
    }

    public override BehaviorNodeState Evaluate()
    {
        if (waypoints.Count == 0)
        {
            return BehaviorNodeState.FAILURE;
        }

        Transform targetWaypoint = waypoints[bt.currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - bt.transform.position).normalized;
        rb.velocity = direction * speed;

        if (Vector3.Distance(bt.transform.position, targetWaypoint.position) <= waypointReachedDistance)
        {
            bt.currentWaypointIndex = (bt.currentWaypointIndex + 1) % waypoints.Count;
        }

        return BehaviorNodeState.SUCCESS;
    }
}

public class AvoidObstacleAction : BehaviorNode
{
    private EnemyAIFighterBT bt;
    private float obstacleAvoidanceRadius;
    private LayerMask obstacleMask;
    private Rigidbody rb;
    private float speed; // Add the speed variable

    public AvoidObstacleAction(EnemyAIFighterBT bt, float obstacleAvoidanceRadius, LayerMask obstacleMask, float speed) // Pass the speed variable through the constructor
    {
        this.bt = bt;
        this.obstacleAvoidanceRadius = obstacleAvoidanceRadius;
        this.obstacleMask = obstacleMask;
        this.rb = bt.GetComponent<Rigidbody>();
        this.speed = speed; // Assign the speed variable
    }

    public override BehaviorNodeState Evaluate()
    {
        RaycastHit hit;
        if (Physics.SphereCast(bt.transform.position, obstacleAvoidanceRadius, bt.transform.forward, out hit, 100.0f, obstacleMask))
        {
            Vector3 avoidanceDirection = (hit.point - bt.transform.position).normalized;
            Vector3 desiredVelocity = bt.transform.forward * speed;
            rb.velocity = Vector3.Lerp(rb.velocity, desiredVelocity + avoidanceDirection, Time.deltaTime);
            return BehaviorNodeState.SUCCESS;
        }
        return BehaviorNodeState.FAILURE;
    }
}
