using UnityEngine;
public abstract class SteeringMovement : MonoBehaviour
{
    public virtual Steering GetSteering(SteeringAgent agent)
    {
        return new Steering(0, 0, 0, 0);
    }
}
