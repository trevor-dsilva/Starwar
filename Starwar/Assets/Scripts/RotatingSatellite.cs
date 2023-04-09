using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSatellite : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0.1f, 0f);
    }
}
