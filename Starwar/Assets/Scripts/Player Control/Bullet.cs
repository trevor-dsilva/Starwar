using UnityEngine;
public class Bullet : MonoBehaviour
{
    public float Force, Lifetime;
    public Vector3 Direction, InitialVelocity;

    private Rigidbody _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        _rigidbody.velocity = InitialVelocity;
        _rigidbody.AddRelativeForce(Direction * Force, ForceMode.Impulse);
        Destroy(gameObject, Lifetime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
