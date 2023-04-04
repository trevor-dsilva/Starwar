using UnityEngine;
public class Bullet : MonoBehaviour
{
    public float Force, Lifetime;
    public Vector3 Direction, ParentVelocity;

    private Rigidbody _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        //_rigidbody.velocity = ParentVelocity;
    }
    private void Start()
    {
        _rigidbody.AddRelativeForce(Direction * Force, ForceMode.Impulse);
        Destroy(gameObject, Lifetime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject, 0.1f);
    }
}
