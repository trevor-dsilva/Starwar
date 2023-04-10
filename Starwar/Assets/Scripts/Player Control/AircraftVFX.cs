using UnityEngine;

public class AircraftVFX : MonoBehaviour
{
    [SerializeField] private GameObject whiteSmoke;
    [SerializeField] private GameObject darkSmoke;
    [SerializeField] private GameObject fireSmoke;
    [SerializeField] private GameObject explosion;

    private Health aircraftHealth;

    private void Start()
    {
        aircraftHealth = GetComponent<Health>();
        explosion.SetActive(false);
        if (!aircraftHealth)
        {
            Debug.LogError("No Health script attached to the GameObject!");
            return;
        }
    }

    private void Update()
    {
        if (!aircraftHealth)
        {
            return;
        }

        UpdateVFX();
    }

    private void UpdateVFX()
    {
        float healthPercentage = aircraftHealth.currentHealth / aircraftHealth.MaxHealth * 100f;

        whiteSmoke.SetActive(healthPercentage >= 60f && healthPercentage <= 80f);
        darkSmoke.SetActive(healthPercentage >= 30f && healthPercentage < 60f);
        fireSmoke.SetActive(healthPercentage > 0f && healthPercentage < 30f);

        if (aircraftHealth.currentHealth == 0f)
        {
            explosion.SetActive(true);
            Debug.Log("test");
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
