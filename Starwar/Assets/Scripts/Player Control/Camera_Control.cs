using UnityEngine;
using System.Collections.Generic;

public class Camera_Control : MonoBehaviour
{
    [SerializeField] private float MaxRotation, Scale;
    private Transform ParentTransform;
    [SerializeField] private List<Transform> Parents;
    private int index = 0;

    private void Start()
    {
        ParentTransform = Parents[index];
    }

    private void FixedUpdate()
    {
        Vector3 newCameraPosition = ParentTransform.position - 14.0f * ParentTransform.forward + 3.0f * ParentTransform.up;
        transform.position = Scale * transform.position + (1 - Scale) * newCameraPosition;

        transform.LookAt(ParentTransform.position + ParentTransform.forward * 5.0f, ParentTransform.up);
    }
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            index++;
            if (index >= Parents.Count)
            {
                index = 0;
            }
            ParentTransform = Parents[index];
            Debug.Log("Viewing " + ParentTransform.name);
        }
    }
}
