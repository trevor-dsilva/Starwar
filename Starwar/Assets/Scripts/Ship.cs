using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class Ship : MonoBehaviour
{
    public bool IsSpotted;
    // After a ship disappear from field of view, it will still be spotted for x seconds.
    // Then it goes unspotted.
    public float SpotTime = 3.0f;

    private float spotTimeCountDown = 0;
    public void Spotted()
    {
        IsSpotted = true;
        spotTimeCountDown = SpotTime;
    }
    public void Unspotted()
    {
        IsSpotted = false;
    }

    private void FixedUpdate()
    {
        if (IsSpotted && spotTimeCountDown <= 0)
        { Unspotted(); }
        else
        { spotTimeCountDown -= Time.fixedDeltaTime; }

    }
}
