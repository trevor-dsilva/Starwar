using UnityEngine;
using System.Collections.Generic;
public class MissileLauncherManager : MonoBehaviour
{
    [SerializeField] private List<MissileLauncher> missileLaunchers;
    [SerializeField] private GameObject ParentObject;
    [SerializeField] float MaxLockOnAngle, MaxLockOnDistance;
    [SerializeField] private LayerMask EnemyMask;
    [SerializeField] private GameObject Target;

    public void LockOn()
    {
        Collider[] colliders = Physics.OverlapSphere(ParentObject.transform.position, MaxLockOnDistance, EnemyMask);
        Collider targetCollider = null;
        float targetAngle = 0;
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == ParentObject) { continue; }
            Vector3 colliderDirection = collider.transform.position - ParentObject.transform.position;
            float colliderAngle = Vector3.Angle(ParentObject.transform.forward, colliderDirection);
            if (colliderAngle <= MaxLockOnAngle)
            {
                if (targetCollider == null)
                {
                    targetAngle = colliderAngle;
                    targetCollider = collider;
                    continue;
                }
                else
                {
                    if (targetAngle < colliderAngle) { continue; }
                    else
                    {
                        targetAngle = colliderAngle;
                        targetCollider = collider;
                        continue;
                    }
                }
            }
        }
        if (targetCollider != null) { Target = targetCollider.gameObject; }
    }
    public void Fire()
    {
        if (Target == null) { return; }
        foreach (MissileLauncher missileLauncher in missileLaunchers)
        {
            if (missileLauncher.IsLoaded)
            {
                missileLauncher.Target = Target;
                missileLauncher.Launch();
                return;
            }
        }
    }
}
