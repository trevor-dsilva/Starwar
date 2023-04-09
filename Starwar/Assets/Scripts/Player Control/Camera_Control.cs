using UnityEngine;

public class Camera_Control : MonoBehaviour
{
    [SerializeField] private float MaxRotation, Scale;
    private Rigidbody ParentBody;
    [SerializeField] private Transform ParentTransform;
    private Vector3 previousAngularVelocity;

    private void Start()
    {
        previousAngularVelocity = Vector3.zero;
        ParentBody = ParentTransform.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 newCameraPosition = ParentTransform.position - 14.0f * ParentTransform.forward + 3.0f * ParentTransform.up;
        transform.position = Scale * transform.position + (1 - Scale) * newCameraPosition;

        transform.LookAt(ParentTransform.position + ParentTransform.forward * 5.0f, ParentTransform.up);
    }
}
