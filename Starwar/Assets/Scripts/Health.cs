using UnityEngine;
public class Health : MonoBehaviour
{
    public float MaxHealth;

    private float currentHealth;
    private bool isAlive;

    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            Debug.Log("Take Damage " + value);
            if (value <= 0)
            {
                currentHealth = 0;
                isAlive = false;
                Debug.Log("Die");
            }
            else { currentHealth = value; }
        }
    }

    private void Start()
    {
        currentHealth = MaxHealth;
        isAlive = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            float damage = collision.relativeVelocity.magnitude * collision.rigidbody.mass;
            CurrentHealth -= damage;
        }
    }

}
