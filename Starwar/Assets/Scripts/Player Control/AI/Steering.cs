using UnityEngine;
public class Steering
{
    public float TorqueX { get; set; }
    public float TorqueY { get; set; }
    public float TorqueZ { get; set; }
    public float ForwardLinear { get; set; }
    public Steering(float torqueX, float torqueY, float torqueZ, float forwardLinear)
    {
        TorqueX = torqueX;
        TorqueY = torqueY;
        TorqueZ = torqueZ;
        ForwardLinear = forwardLinear;
    }
    public void Add(Steering steering, float weight = 1.0f)
    {
        TorqueX += steering.TorqueX * weight;
        TorqueY += steering.TorqueY * weight;
        TorqueZ += steering.TorqueZ * weight;
        ForwardLinear += steering.ForwardLinear * weight;
    }
    
    public void Clamp(float maxTorqueX, float maxTorqueY, float maxTorqueZ, float maxForwardLinear)
    {
        TorqueX = Mathf.Clamp(TorqueX, -maxTorqueX, maxTorqueX);
        TorqueY = Mathf.Clamp(TorqueY, -maxTorqueY, maxTorqueY);
        TorqueZ = Mathf.Clamp(TorqueZ, -maxTorqueZ, maxTorqueZ);
        ForwardLinear = Mathf.Clamp(ForwardLinear, 0, maxForwardLinear);
    }
}
