using UnityEngine;

public class Camera_Control : MonoBehaviour
{
    [SerializeField] private float MaxRotation, Scale;
    [SerializeField] private Rigidbody ParentBody;
    [SerializeField] private Transform ParentTransform;
    private Vector3 previousAngularVelocity;

    private void Start()
    {
        previousAngularVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        Vector3 newCameraPosition = ParentTransform.position - 10.0f * ParentTransform.forward + 5.0f * ParentTransform.up;
        transform.position = Scale * transform.position + (1 - Scale) * newCameraPosition;

        transform.LookAt(ParentTransform.position + ParentTransform.forward * 5.0f, ParentTransform.up);
        //Debug.Log(ParentBody.angularVelocity);
        //Vector3 angularVelocityDifference = ParentBody.angularVelocity - previousAngularVelocity;
        //transform.Rotate(ParentTransform.right, angularVelocityDifference.x * Scale);
        //transform.Rotate(ParentTransform.up, angularVelocityDifference.y * Scale);
        //transform.Rotate(ParentTransform.forward, angularVelocityDifference.z * Scale);

        //previousAngularVelocity = ParentBody.angularVelocity;
    }
}
