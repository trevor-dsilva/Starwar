using UnityEngine;

public class AIPlaneObstacleAvoidance : MonoBehaviour
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

    void FixedUpdate()
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
                    // Draw a red line when the ray hits an obstacle
                    Debug.DrawLine(transform.position, hit.point, Color.red);

                    if (hit.distance < closestHitDistance)
                    {
                        closestHitDistance = hit.distance;
                        Vector3 targetDirection = (hit.point - transform.position).normalized;
                        avoidanceForce = (transform.right * maxAvoidanceForce) * (1.0f - hit.distance / detectionDistance);
                    }
                }
                else
                {
                    // Draw a green line when there's no obstacle detected
                    Debug.DrawLine(transform.position, transform.position + rayDirection * detectionDistance, Color.green);
                }
            }
        }

        rb.AddForce(avoidanceForce, ForceMode.Force);
    }
}
