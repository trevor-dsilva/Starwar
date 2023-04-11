using System;

using UnityEngine;
using UnityEngine.UI;


public class PlayerUI: MonoBehaviour
{
    
    public GameObject aircraft;

    public GameObject healthText;


    
    // Update is called once per frame
    void Update()
    {
        //Health
        string healthString = aircraft.GetComponent<Health>().CurrentHealth.ToString("F0");
        healthText.GetComponent<Text>().text = "Health: " + healthString;
    }
}
