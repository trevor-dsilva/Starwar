using UnityEngine;
public class MachineGun : MonoBehaviour
{
    [SerializeField] float MaxAngle, Force, Interval, BulletLifetime;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private Rigidbody ParentRigidBody;

    private float lastFire;
    private void Start()
    {
        lastFire = Time.fixedTime;
    }
    private void FixedUpdate()
    {
    }
    public void Fire()
    {
        if (lastFire + Interval > Time.fixedTime) { return; }

        Vector3 direction = new Vector3(Random.Range(-MaxAngle, MaxAngle), Random.Range(-MaxAngle, MaxAngle), 1.0f).normalized;
        GameObject bulletObject = Instantiate(BulletPrefab, transform.position, transform.rotation);


        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.Force = Force;
        bullet.Direction = direction;
        bullet.ParentVelocity = ParentRigidBody.velocity;
        bullet.Lifetime = BulletLifetime;

        lastFire = Time.fixedTime;
    }


}
