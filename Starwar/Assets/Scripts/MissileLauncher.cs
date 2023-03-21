using UnityEngine;
public class MissileLauncher : MonoBehaviour
{
    [SerializeField] float InitialLauchForce;
    [SerializeField]
    private GameObject MissilePrefab;
    private bool isLoaded = true;
    public bool IsLoaded
    {
        get { return isLoaded; }
        set
        {
            isLoaded = value;
            if (isLoaded) { gameObject.SetActive(true); }
            else { gameObject.SetActive(false); }
        }
    }
    public GameObject Target;

    public void Launch()
    {
        if (Target != null && IsLoaded)
        {
            GameObject missleObject = Instantiate(MissilePrefab, transform.position - 2 * transform.up, transform.rotation);
            Missile missle = missleObject.GetComponent<Missile>();
            missle.InitialLauchForce = InitialLauchForce;
            missle.Target = Target;
            IsLoaded = false;
        }
    }
}
