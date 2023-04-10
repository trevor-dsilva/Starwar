using UnityEngine;
public class Avoid : SteeringMovement
{
    public GameObject target;
    public Vector3 Kp, Ki, Kd, PreviousError;
    private Vector3 P, I, D;
    public override Steering GetSteering(SteeringAgent agent)
    {
        Steering ret = base.GetSteering(agent);
        int layerMask = 0;
        //        layermask += 1 << StateLayerMap[CharacterState.ActiveEvader];
        layerMask += 1 << target.layer;
        bool hasHit = Physics.Raycast(agent.transform.position, agent.transform.forward, out RaycastHit hit, float.MaxValue, layerMask);
        Debug.DrawRay(agent.transform.position, 100 * agent.transform.forward, Color.blue);

        if (hasHit && (hit.collider.gameObject == target))
        {
            Vector3 targetDirection = agent.transform.position - target.transform.position;
            float angleFromUpToTargetDirection = Vector3.Angle(agent.transform.up, targetDirection);
            float angleFromDownToTargetDirection = Vector3.Angle(-agent.transform.up, targetDirection);

            float angleFromLeftToTargetDirection = Vector3.Angle(-agent.transform.right, targetDirection);
            float angleFromRightToTargetDirection = Vector3.Angle(agent.transform.right, targetDirection);

            // With PID
            float currentError = (angleFromDownToTargetDirection - angleFromUpToTargetDirection) / 180;
            //float currentError = (angleFromDownToTargetDirection - angleFromUpToTargetDirection);
            P.x = currentError;
            I.x += P.x * Time.deltaTime;
            D.x = (P.x - PreviousError.x) / Time.deltaTime;
            PreviousError.x = currentError;
            float torqueX = P.x * Kp.x + I.x * Ki.x + D.x * Kd.x;
            torqueX = Mathf.Clamp(torqueX, -1.0f, 1.0f);

            currentError = (angleFromLeftToTargetDirection - angleFromRightToTargetDirection) / 180;
            //currentError = (angleFromLeftToTargetDirection - angleFromRightToTargetDirection);
            P.y = currentError;
            I.y += P.y * Time.deltaTime;
            D.y = (P.y - PreviousError.y) / Time.deltaTime;
            PreviousError.y = currentError;
            float torqueY = P.y * Kp.y + I.y * Ki.y + D.y * Kd.y;
            torqueY = Mathf.Clamp(torqueY, -1.0f, 1.0f);

            currentError = -currentError;
            P.z = currentError;
            I.z += P.z * Time.deltaTime;
            D.z = (P.z - PreviousError.z) / Time.deltaTime;
            PreviousError.z = currentError;
            float torqueZ = P.z * Kp.z + I.z * Ki.z + D.z * Kd.z;
            torqueZ = Mathf.Clamp(torqueZ, -1.0f, 1.0f);

            ret.TorqueX = torqueX;
            ret.TorqueY = torqueY;
            ret.TorqueZ = torqueZ;
            return ret;
        }
        else
        {
            ret.ForwardLinear = 1;
            return ret;
        }
    }
}
