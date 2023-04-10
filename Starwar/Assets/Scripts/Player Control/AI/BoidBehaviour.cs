using UnityEngine;
using System.Collections;

public class BoidBehaviour : MonoBehaviour
{
    // Reference to the controller.
    public BoidController controller;

    // Options for animation playback.
    public float animationSpeedVariation = 0.2f;
    private GameObject[] waypointNodes; 

    // Random seed.
    float noiseOffset;
    public GameObject currWaypoint;
    Vector3 waypointVec;
    [SerializeField] private float _rotationSpeed = 5f;
    private Vector3 _previousPosition;

    // Caluculates the separation vector with a target.
    Vector3 GetSeparationVector(Transform target)
    {
        var diff = transform.position - target.transform.position;
        var diffLen = diff.magnitude;
        var scaler = Mathf.Clamp01(1.0f - diffLen / controller.neighborDist);
        return diff * (scaler / diffLen);
    }

    void Start()
    {
        _previousPosition = transform.position;
        noiseOffset = Random.value * 10.0f;

        currWaypoint = controller.currWayNode;

        var animator = GetComponent<Animator>();
        if (animator)
            animator.speed = Random.Range(-1.0f, 1.0f) * animationSpeedVariation + 1.0f;



    }

    void Update()
    {
        currWaypoint = controller.currWayNode;
        var currentPosition = transform.position;
        var currentRotation = transform.rotation;

        // Current velocity randomized with noise.
        var noise = Mathf.PerlinNoise(Time.time, noiseOffset) * 2.0f - 1.0f;
        var velocity = controller.velocity * (1.0f + noise * controller.velocityVariation);

        // Initializes the vectors.
        var separation = Vector3.zero;
        var alignment = controller.transform.forward;
        var cohesion = controller.transform.position;

        // Looks up nearby boids.
        var nearbyBoids = Physics.OverlapSphere(currentPosition, controller.neighborDist, controller.searchLayer);

        // Accumulates the vectors.
        foreach (var boid in nearbyBoids)
        {
            if (boid.gameObject == gameObject) continue;
            var t = boid.transform;
            separation += GetSeparationVector(t);
            alignment += t.forward;
            cohesion += t.position;
        }

        var avg = 1.0f / nearbyBoids.Length;
        alignment *= avg;
        cohesion *= avg;
        cohesion = (cohesion - currentPosition).normalized;

        // Calculates a rotation from the vectors.
        var direction = separation + alignment + cohesion;
       

       

        waypointVec = MoveToWaypoint(currWaypoint);
        // Moves forawrd. 
        
        

        transform.position += waypointVec.normalized * (velocity * Time.deltaTime);

        Vector3 lookDirection = transform.position - _previousPosition;

        if (lookDirection.magnitude > 0.01f)
        {
            // Calculate the target rotation based on the direction of movement
            Quaternion targetRotation = Quaternion.LookRotation(-lookDirection, Vector3.up);

            // Rotate towards the target rotation using Slerp for smooth rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        _previousPosition = transform.position;
    }


    Vector3 MoveToWaypoint(GameObject waypoint){
        Vector3 retVec = (waypoint.transform.position - transform.position);
        return retVec;
    }
}