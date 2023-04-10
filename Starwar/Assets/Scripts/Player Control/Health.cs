using UnityEngine;
public class Health : MonoBehaviour
{
    public float MaxHealth;
    [SerializeField]
    public float currentHealth;
    [SerializeField]
    private bool isAlive;
    public float HealInterval;

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

    private float lastHealTime = 0;
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

    public void Heal(float amount = 1)
    {
        if (CurrentHealth < MaxHealth)
        {
            if (lastHealTime + HealInterval <= Time.fixedTime)
            {
                lastHealTime = Time.fixedTime;
                CurrentHealth += amount;
                if (CurrentHealth > MaxHealth) { CurrentHealth = MaxHealth; }
            }
        }
    }
}

