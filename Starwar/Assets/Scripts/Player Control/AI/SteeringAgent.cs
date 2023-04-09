using System.Collections.Generic;
using UnityEngine;
public class SteeringAgent : MonoBehaviour
{
    public List<SteeringMovement> SteeringMovements;
    public Assault assault;
    public Patrol patrol;
    public Retreat retreat;

    // Add AIAvoidance reference
    public AIAvoidance aiAvoidance;

    public float
        Forward_Force = 3.0f,
        TorqueX_Force = 0.1f,
        TorqueY_Force = 0.1f,
        TorqueZ_Force = 0.01f;

    private Rigidbody Rigidbody;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        assault = GetComponent<Assault>();
        patrol = GetComponent<Patrol>();
        retreat = GetComponent<Retreat>();

        // Add GetComponent<AIAvoidance>() line in Start() method
        aiAvoidance = GetComponent<AIAvoidance>();
        if (patrol != null)
        {
            Patrol();
        }
    }

    private Steering GetSteeringSum()
    {
        Steering ret = new Steering(0, 0, 0, 0);
        if (aiAvoidance != null)
        {
            ret.Add(aiAvoidance.GetSteering(this), aiAvoidance.weight);
            foreach (SteeringMovement steeringMovement in SteeringMovements)
            {
                ret.Add(steeringMovement.GetSteering(this), 1 - aiAvoidance.weight);
            }
        }
        else
        {
            foreach (SteeringMovement steeringMovement in SteeringMovements)
            {
                ret.Add(steeringMovement.GetSteering(this));
            }
        }

        ret.Clamp(TorqueX_Force, TorqueY_Force, TorqueZ_Force, Forward_Force);

        return ret;
    }

    private void FixedUpdate()
    {
        //LocalAngularVelocity = transform.InverseTransformDirection(Rigidbody.angularVelocity);
        Steering finalSteering = GetSteeringSum();
        if (finalSteering.ForwardLinear > 0)
        {
            Rigidbody.AddRelativeForce(Vector3.forward * finalSteering.ForwardLinear * Forward_Force, ForceMode.Force);
        }
        if (!Mathf.Approximately(finalSteering.TorqueX, 0f))
        {
            Rigidbody.AddRelativeTorque(Vector3.left * finalSteering.TorqueX * TorqueX_Force, ForceMode.Force);
        }
        if (!Mathf.Approximately(finalSteering.TorqueY, 0f))
        {
            Rigidbody.AddRelativeTorque(Vector3.up * finalSteering.TorqueY * TorqueY_Force, ForceMode.Force);
        }
        if (!Mathf.Approximately(finalSteering.TorqueZ, 0f))
        {
            Rigidbody.AddRelativeTorque(Vector3.forward * finalSteering.TorqueZ * TorqueZ_Force, ForceMode.Force);
        }
    }

    public void Assault()
    {
        SteeringMovements.Clear();
        SteeringMovements.Add(assault);
    }

    public void Patrol()
    {
        patrol.Target = null;
        SteeringMovements.Clear();
        SteeringMovements.Add(patrol);
    }

    public void Dead()
    {
        SteeringMovements.Clear();
        aiAvoidance = null;
    }

    public void Retreat()
    {
        SteeringMovements.Clear();
        SteeringMovements.Add(retreat);
    }
}
