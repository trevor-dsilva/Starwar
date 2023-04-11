using UnityEngine;
using System.Collections.Generic;
public class MissileLauncherManager : MonoBehaviour
{
    [SerializeField] private List<MissileLauncher> missileLaunchers;
    [SerializeField] float MaxLockOnAngle, MaxLockOnDistance;
    [SerializeField] private LayerMask EnemyMask;
    [SerializeField] private GameObject Target;
    [SerializeField] private SoundController soundController;
    public float ReloadInterval;


    private int missileLaunchersCount;
    private float lastReloadTime = 0;

    private void Start()
    {

        missileLaunchersCount = 0;
        foreach (MissileLauncher launcher in missileLaunchers)
        {
            if (launcher.IsLoaded)
            {
                missileLaunchersCount++;
            }
        }
    }
    public bool LockOn(GameObject target)
    {
        float targetDistance = Vector3.Distance(target.transform.position, transform.position);
        if (targetDistance > MaxLockOnDistance)
        {
            return false;
        }
        Vector3 targetDirection = target.transform.position - transform.position;
        float targetAngle = Vector3.Angle(targetDirection, transform.forward);
        if (targetAngle > MaxLockOnAngle) { Debug.Log("b"); return false; }
        Target = target;
        return true;
    }
    public void LockOn()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, MaxLockOnDistance, EnemyMask);
        Collider targetCollider = null;
        float targetAngle = 0;
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == gameObject) { continue; }
            Vector3 colliderDirection = collider.transform.position - transform.position;
            float colliderAngle = Vector3.Angle(transform.forward, colliderDirection);
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
                missileLauncher.velocity = GetComponent<Rigidbody>().velocity;
                missileLauncher.Launch();
                soundController.playMissile();
                missileLaunchersCount--;
                return;
            }
        }
        Target = null;
    }
    public void Reload(bool all = false)
    {
        if (missileLaunchersCount < missileLaunchers.Count)
        {
            if (lastReloadTime + ReloadInterval <= Time.fixedTime)
            {
                lastReloadTime = Time.fixedTime;
                foreach (MissileLauncher missileLauncher in missileLaunchers)
                {
                    if (!missileLauncher.IsLoaded)
                    {
                        missileLauncher.IsLoaded = true;
                        missileLaunchersCount++;
                        if (!all) { return; }
                    }
                }
            }
        }
    }
}



