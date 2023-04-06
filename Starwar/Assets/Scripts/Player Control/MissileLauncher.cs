using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    public Vector3 velocity;
    [SerializeField]
    private GameObject MissilePrefab;
    private bool isLoaded = true;
    public bool IsLoaded
    {
        get { return isLoaded; }
        set
        {
            isLoaded = value;
            if (isLoaded) { GetComponent<Renderer>().enabled = true; }
            else { GetComponent<Renderer>().enabled = false; }
        }
    }
    public GameObject Target;

    public void Launch()
    {
        if (Target != null && IsLoaded)
        {
            GameObject missleObject = Instantiate(MissilePrefab, transform.position - 2 * transform.up, transform.rotation);
            Missile missle = missleObject.GetComponent<Missile>();
            missle.InitialVelocity = velocity;
            missle.Target = Target;
            IsLoaded = false;
        }
    }
}