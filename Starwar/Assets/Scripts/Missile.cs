using UnityEngine;
public class Missile : MonoBehaviour
{
    [SerializeField] private float DamageRadius, DamageAmount;
    [SerializeField] private GameObject ExplotionPrefeb;
    public float InitialLauchForce;
    private GameObject target;
    public GameObject Target
    {
        get { return target; }
        set
        {
            target = value;
            seek.Target = Target;
            lookAtTarget.Target = Target;
        }
    }

    private Seek seek;
    private LookAtTarget lookAtTarget;
    private void Awake()
    {
        seek = GetComponent<Seek>();
        lookAtTarget = GetComponent<LookAtTarget>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject explosionObject = Instantiate(ExplotionPrefeb, transform.position, transform.rotation);
        Destroy(explosionObject, 5.0f);
        Destroy(gameObject, 0.1f);
    }
}
