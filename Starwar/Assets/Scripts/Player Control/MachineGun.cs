using UnityEngine;
public class MachineGun : MonoBehaviour
{
    [SerializeField] float MaxAngle, Force, BulletLifetime;
    [SerializeField] private GameObject BulletPrefab;
    public Vector3 velocity;
    public void Fire()
    {
        Vector3 direction = new Vector3(Random.Range(-MaxAngle, MaxAngle), Random.Range(-MaxAngle, MaxAngle), 1.0f).normalized;
        GameObject bulletObject = Instantiate(BulletPrefab, transform.position, transform.rotation);

        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.Force = Force;
        bullet.Direction = direction;
        bullet.Lifetime = BulletLifetime;
        bullet.InitialVelocity = velocity;
    }
}
