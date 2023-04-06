using BehaviorTree;
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

public class AmIHealthy : BehaviorNode
{
    public Health myHealth;
    public float criticalPercentage;
    public AmIHealthy(Health health, float criticalPercentage)
    {
        myHealth = health;
        this.criticalPercentage = criticalPercentage;
    }

    public override BehaviorNodeState Evaluate()
    {
        float percentage = myHealth.CurrentHealth / myHealth.MaxHealth;
        if (percentage < criticalPercentage)
        {
            return BehaviorNodeState.FAILURE;
        }
        else
        {
            return BehaviorNodeState.SUCCESS;
        }
    }
}

public class AmIAlive : BehaviorNode
{
    public Health myHealth;
    public AmIAlive(Health health)
    { myHealth = health; }

    public override BehaviorNodeState Evaluate()
    {
        return myHealth.IsAlive ? BehaviorNodeState.SUCCESS : BehaviorNodeState.FAILURE;
    }
}


