using UnityEngine;
public class Bullet : MonoBehaviour
{
    public float Force, Lifetime;
    public Vector3 Direction, ParentVelocity;


    //private float colliderDelayCountDown;
    private Rigidbody _rigidbody;
    private bool activated = false;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        //_rigidbody.velocity = ParentVelocity;
        _rigidbody.AddRelativeForce(Direction * Force, ForceMode.Impulse);
        //colliderDelayCountDown = ColliderDelay;
        Destroy(gameObject, Lifetime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
    private void Update()
    {
        /*
        if (colliderDelayCountDown > 0)
        {
            colliderDelayCountDown -= Time.deltaTime;
        }
        else if (!activated)
        {
            //GetComponent<Collider>().enabled = true;
            activated = true;
        }*/
    }
}
