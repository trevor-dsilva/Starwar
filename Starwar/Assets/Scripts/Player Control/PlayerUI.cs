using System;

using UnityEngine;
using UnityEngine.UI;


public class PlayerUI: MonoBehaviour
{
    GameObject aircraft;

    GameObject healthText;

    // Start is called before the first frame update
    void Start()
    {
        aircraft = GameObject.Find("Aircraft");

        healthText = GameObject.Find("Health");

    }


    // Update is called once per frame
    void Update()
    {
        //Health
        string healthString = aircraft.GetComponent<Health>().CurrentHealth.ToString("F0");
        healthText.GetComponent<Text>().text = "Health: " + healthString;

     



    }
}
