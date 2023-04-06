using UnityEngine;
public class Health : MonoBehaviour
{
    public float MaxHealth;
    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private bool isAlive;

    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if (value <= 0)
            {
                currentHealth = 0;
                isAlive = false;
                Ship.UnregisterShip(GetComponent<Ship>());
            }
            else { currentHealth = value; }
        }
    }
    public bool IsAlive { get { return isAlive; } }

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

