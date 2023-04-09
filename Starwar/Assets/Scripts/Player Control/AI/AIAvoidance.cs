using UnityEngine;

public class AIAvoidance : SteeringMovement
{
    public float maxAvoidanceForce = 50f;
    public float detectionDistance = 100f;
    public LayerMask obstacleLayer;
    public int horizontalRays = 8;
    public int verticalRays = 4;
    public float horizontalAngle = 45f;
    public float verticalAngle = 30f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Override GetSteering method to return avoidance force as a Steering object
    public override Steering GetSteering(SteeringAgent agent)
    {
        Vector3 avoidanceForce = CalculateAvoidanceForce();
        // Return avoidance force as Steering with torque values set to zero
        return new Steering(0, 0, 0, avoidanceForce.magnitude);
    }

    private Vector3 CalculateAvoidanceForce()
    {
        Vector3 avoidanceForce = Vector3.zero;
        float closestHitDistance = float.MaxValue;

        for (int i = 0; i < horizontalRays; i++)
        {
            for (int j = 0; j < verticalRays; j++)
            {
                float horizontalAngleStep = horizontalAngle / (horizontalRays - 1);
                float verticalAngleStep = verticalAngle / (verticalRays - 1);

                float currentHorizontalAngle = -horizontalAngle / 2 + horizontalAngleStep * i;
                float currentVerticalAngle = -verticalAngle / 2 + verticalAngleStep * j;

                Quaternion horizontalRotation = Quaternion.AngleAxis(currentHorizontalAngle, transform.up);
                Quaternion verticalRotation = Quaternion.AngleAxis(currentVerticalAngle, transform.right);

                Vector3 rayDirection = verticalRotation * horizontalRotation * transform.forward;

                RaycastHit hit;
                if (Physics.Raycast(transform.position, rayDirection, out hit, detectionDistance, obstacleLayer))
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red);

                    if (hit.distance < closestHitDistance)
                    {
                        closestHitDistance = hit.distance;
                        avoidanceForce = (transform.right * maxAvoidanceForce) * (1.0f - hit.distance / detectionDistance);
                    }
                }
                else
                {
                    Debug.DrawLine(transform.position, transform.position + rayDirection * detectionDistance, Color.green);
                }
            }
        }
        return avoidanceForce;
    }
}
