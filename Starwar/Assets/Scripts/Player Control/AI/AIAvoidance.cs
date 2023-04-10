using UnityEngine;

public class AIAvoidance : SteeringMovement
{
    public float detectionDistanceRatio = 10f;
    public LayerMask obstacleLayer;
    public int horizontalRays = 8;
    public int verticalRays = 4;
    public float
        horizontalAngle = 45f,
        verticalAngle = 30f,
        fleeAngle;
    public Vector3 Kp, Ki, Kd;
    public float weight, minimumSpeed, minimumDistanceRatio;

    private Rigidbody rb;
    private Avoid avoid;
    private float detectionDistance;
    private Vector3 P, I, D, PreviousError;

    [SerializeField]
    private GameObject target;
    public GameObject Target
    {
        get { return target; }
        set
        {
            avoid.target = value;
            target = value;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        avoid = new Avoid()
        {
            Kp = Kp,
            Ki = Ki,
            Kd = Kd
        };
    }

    // Override GetSteering method to return avoidance force as a Steering object
    public override Steering GetSteering(SteeringAgent agent)
    {
        Steering ret = base.GetSteering(agent);

        SetDetectionDistance();
        Target = Detect(out float hitDistance);
        SetWeight(hitDistance);
        if (Target == null) { return ret; }

        ret.Add(avoid.GetSteering(agent));
        return ret;
    }
    private void SetDetectionDistance()
    {
        detectionDistance = 20 + detectionDistanceRatio * rb.velocity.magnitude;
    }
    private void SetWeight(float hitDistance)
    {
        if (Target == null) { weight = 0; }
        else
        {
            weight = ((detectionDistance - hitDistance) / (detectionDistance * minimumDistanceRatio));
            if (weight > 1) { weight = 1; }
        }
    }

    private GameObject Detect(out float closestHitDistance)
    {
        GameObject ret = null;
        closestHitDistance = float.MaxValue;
        Vector3 velocityDirection = Vector3.Normalize(rb.velocity);
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

                Vector3 rayDirection = verticalRotation * horizontalRotation * velocityDirection;

                if (Physics.Raycast(transform.position, rayDirection, out RaycastHit raycastHit, detectionDistance, obstacleLayer))
                {
                    Debug.DrawLine(transform.position, raycastHit.point, Color.red);

                    if (raycastHit.distance < closestHitDistance && ret != raycastHit.transform.gameObject)
                    {
                        closestHitDistance = raycastHit.distance;
                        ret = raycastHit.transform.gameObject;
                    }
                }
                else
                {
                    Debug.DrawLine(transform.position, transform.position + rayDirection * detectionDistance, Color.green);
                }
            }
        }
        return ret;
    }
}
