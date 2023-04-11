using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthIndicator : MonoBehaviour
{
    public Health health;
    private Image healthBarImage;

    private void Start()
    {
        healthBarImage = GetComponent<Image>();
    }

    private void Update()
    {
        float healthPercentage = health.CurrentHealth / health.MaxHealth;
        healthBarImage.fillAmount = healthPercentage;
    }
}
